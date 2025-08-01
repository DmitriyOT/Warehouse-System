
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Warehouse.Contracts.Api.Response;
using Warehouse.Infrastructure.Db;
using Warehouse.Contracts.Application;
using Warehouse.Application.Services;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Infrastructure.Db.Repository;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Api;

/// <summary>
/// Главный класс программы
/// </summary>
public class Program
{
    /// <summary>
    /// Точка входа в программу
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            var xmlFileDomain = $"{Assembly.GetAssembly(typeof(BaseEntityWithId))?.GetName().Name}.xml";
            string locationDomain = Path.GetDirectoryName( Assembly.GetAssembly(typeof(BaseEntityWithId))?.Location ) ?? "";
            if (locationDomain != null)
            {
                var xmlPathDomain = Path.Combine(locationDomain, xmlFileDomain);
                c.IncludeXmlComments(xmlPathDomain);
            }
            c.IncludeXmlComments(xmlPath);
            var contractAssembly = Assembly.GetAssembly(typeof(ResponseDto<>));
            if(contractAssembly != null)
            {
                var xmlFileContract = contractAssembly.GetName().Name + ".xml";
                var xmlPathContract = Path.Combine(Path.GetDirectoryName(contractAssembly.Location) ?? "", xmlFileContract);
                c.IncludeXmlComments(xmlPathContract);
            }
        });

        builder.Services.AddDbContextPool<PostgresDbContext>((service, options) =>
        {
            var _configuration = service.GetRequiredService<IConfiguration>();
            var connectionString = _configuration.GetSection("ConnectionString");
            options.UseNpgsql(connectionString.Value);
        });

        builder.Services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
        builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));

        var app = builder.Build();

        ILogger<Program>? logger = null;
        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        }

        //Configure DB
        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<PostgresDbContext>();
            context.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseExceptionHandler(handle =>
        {
            handle.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;

                var exHandler = context.Features.Get<IExceptionHandlerPathFeature>();

                Console.WriteLine(exHandler?.Error?.ToString() ?? "Internal Error");
                logger.LogError(exHandler?.Error?.ToString() ?? "Internal Error");

                await context.Response.WriteAsJsonAsync(
                    new ErrorResponseDto(exHandler?.Error ?? new Exception("Internal not handled exception") )
                    );
            });
        });

        app.Run();
    }
}

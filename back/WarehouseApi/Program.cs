using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Warehouse.Application.Services;
using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;
using Warehouse.Domain.Models.Base;
using Warehouse.Infrastructure.Db;
using Warehouse.Infrastructure.Db.Repository;
using Warehouse.Infrastructure.Db.Repository.Base;

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
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var state = context.ModelState;
                return new JsonResult(new ErrorResponseDto(new Exception("Заполнены не все поля.")));
            };
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Warehouse API",
                Description = "Реализация api для склада в рамках тестового задания."
            });
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
        builder.Services.AddScoped(typeof(IArchiveCrudService<>), typeof(ArchiveCrudService<>));
        builder.Services.AddScoped(typeof(IArchiveCrudRepository<>), typeof(ArchiveCrudRepository<>));

        builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
        builder.Services.AddScoped<IncomeService>();

        builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
        builder.Services.AddScoped<ShipmentService>();

        builder.Services.AddCors(options =>
         {
             options.AddPolicy(name: MyAllowSpecificOrigins,
                               policy =>
                               {
                                   policy.WithOrigins("http://localhost:5173", "https://warehouse.dimonogen.ru")
                                       .AllowAnyHeader()
                                       .WithMethods("GET", "POST", "PUT", "DELETE")
                                       .AllowCredentials();
                               });
         });

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

        app.UseCors(MyAllowSpecificOrigins);

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

                if (exHandler?.Error.GetType() == typeof(UserException))
                {
                    await context.Response.WriteAsJsonAsync(
                                        new ErrorResponseDto(exHandler?.Error ?? new Exception("Ошибка системы."))
                                        );
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(
                                        new ErrorResponseDto(new Exception("Ошибка системы."))
                                        );
                }

            });
        });

        app.Run();
    }
}

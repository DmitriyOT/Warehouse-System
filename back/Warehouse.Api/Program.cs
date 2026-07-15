using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Warehouse.Application;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Exceptions;
using Warehouse.Domain.Models;
using Warehouse.Domain.Models.Base;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Db;

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

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            // Обработка ошибки модели: 400 Bad Request с перечнем ошибок валидации
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                    .ToList();

                var message = errors.Count > 0
                    ? string.Join("; ", errors)
                    : "Заполнены не все поля.";

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new JsonResult(new ErrorResponseDto(message));
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
                Description = "Реализация api для склада."
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            // Так как несколько проектов из которых нужно вытащить информация в swagger, то все их нужно подключить
            var xmlFileDomain = $"{Assembly.GetAssembly(typeof(BaseEntityWithId))?.GetName().Name}.xml";
            string locationDomain = Path.GetDirectoryName( Assembly.GetAssembly(typeof(BaseEntityWithId))?.Location ) ?? "";
            if (locationDomain != null)
            {
                var xmlPathDomain = Path.Combine(locationDomain, xmlFileDomain);
                c.IncludeXmlComments(xmlPathDomain);
            }
            
            var contractAssembly = Assembly.GetAssembly(typeof(ResponseDto<>));
            if(contractAssembly != null)
            {
                var xmlFileContract = contractAssembly.GetName().Name + ".xml";
                var xmlPathContract = Path.Combine(Path.GetDirectoryName(contractAssembly.Location) ?? "", xmlFileContract);
                c.IncludeXmlComments(xmlPathContract);
            }
        });

        // Инфраструктурные сервисы: БД, репозитории, единица работы, health checks
        builder.Services.AddWarehouseInfrastructure(builder.Configuration);

        // Прикладные бизнес-сервисы
        builder.Services.AddWarehouseApplication();

        // CORS добавляем
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

        //Configure DB и автоматическая миграция БД
        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<PostgresDbContext>();
            context.Database.Migrate();
        }

        // Только в режиме отладки включаем сваггер, так как сваггер дырявый
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //Обработка ошибок глобальная по всей системе
        app.UseExceptionHandler(handle =>
        {
            handle.Run(async context =>
            {
                var exHandler = context.Features.Get<IExceptionHandlerPathFeature>();
                var error = exHandler?.Error;

                Log.Error(error, "Unhandled exception occurred");

                if (error is UserException userEx)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new ErrorResponseDto(userEx.Message));
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    var message = app.Environment.IsDevelopment()
                        ? error?.ToString() ?? "Internal Error"
                        : "Ошибка системы.";
                    await context.Response.WriteAsJsonAsync(new ErrorResponseDto(message ?? "Ошибка системы."));
                }
            });
        });

        app.UseCors(MyAllowSpecificOrigins);

        app.UseHttpsRedirection();

        app.UseSerilogRequestLogging();

        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        });

        app.MapControllers();

        app.Run();
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

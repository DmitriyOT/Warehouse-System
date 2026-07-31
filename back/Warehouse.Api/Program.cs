using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using Warehouse.Application;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Application;
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

            // Кнопка Authorize в Swagger: ввод JWT-токена (Bearer)
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT-токен, полученный через /Auth/login"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Инфраструктурные сервисы: БД, репозитории, единица работы, health checks
        builder.Services.AddWarehouseInfrastructure(builder.Configuration);

        // Прикладные бизнес-сервисы
        builder.Services.AddWarehouseApplication();

        // JWT-аутентификация. Ключ подписи задаётся в конфигурации (Jwt:Key),
        // в проде переопределяется переменной окружения Jwt__Key
        var jwtKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "Ключ подписи JWT не задан. Укажите 'Jwt:Key' в конфигурации " +
                "или переменную окружения 'Jwt__Key'.");
        }

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
        builder.Services.AddAuthorization();

        // CORS добавляем
        builder.Services.AddCors(options =>
         {
             options.AddPolicy(name: MyAllowSpecificOrigins,
                               policy =>
                               {
                                   if (builder.Environment.IsDevelopment())
                                   {
                                       // В разработке разрешаем любой порт localhost: vite может занять не 5173,
                                       // а следующий свободный (5174, 5175...), если порт уже занят
                                       policy.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
                                           .AllowAnyHeader()
                                           .WithMethods("GET", "POST", "PUT", "DELETE")
                                           .AllowCredentials();
                                   }
                                   else
                                   {
                                       policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
                                           .AllowAnyHeader()
                                           .WithMethods("GET", "POST", "PUT", "DELETE")
                                           .AllowCredentials();
                                   }
                               });
         });

        var app = builder.Build();

        //Configure DB и автоматическая миграция БД
        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<PostgresDbContext>();
            context.Database.Migrate();

            // Первый пользователь (администратор) создаётся из конфигурации AdminUser,
            // если в системе нет ни одного пользователя. Пароль в проде задаётся
            // переменной окружения AdminUser__Password
            var adminLogin = app.Configuration["AdminUser:Login"];
            var adminPassword = app.Configuration["AdminUser:Password"];
            if (!string.IsNullOrWhiteSpace(adminLogin) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                var authService = serviceScope.ServiceProvider.GetRequiredService<IAuthService>();
                authService.SeedAdmin(adminLogin, adminPassword).GetAwaiter().GetResult();
                Log.Information("Проверен seed администратора '{Login}'", adminLogin);
            }
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

                switch (error)
                {
                    // Пользовательские ошибки — ожидаемая ситуация, логируем как Warning, без стектрейса в ответе
                    case NotFoundException notFoundEx:
                        Log.Warning(notFoundEx, "Объект не найден");
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsJsonAsync(new ErrorResponseDto(notFoundEx.Message));
                        break;
                    case UserException userEx:
                        Log.Warning(userEx, "Ошибка пользовательского запроса");
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new ErrorResponseDto(userEx.Message));
                        break;
                    // Конфликт при сохранении в БД: гонка номера документа, дубль баланса,
                    // несуществующие связанные Id — отдаём 409 с понятным сообщением
                    case DbUpdateException dbEx:
                        Log.Warning(dbEx, "Конфликт при сохранении данных в БД");
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        await context.Response.WriteAsJsonAsync(new ErrorResponseDto(GetDbConflictMessage(dbEx)));
                        break;
                    default:
                        Log.Error(error, "Unhandled exception occurred");
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        var message = app.Environment.IsDevelopment()
                            ? error?.ToString() ?? "Internal Error"
                            : "Ошибка системы.";
                        await context.Response.WriteAsJsonAsync(new ErrorResponseDto(message ?? "Ошибка системы."));
                        break;
                }
            });
        });

        app.UseCors(MyAllowSpecificOrigins);

        // В Development фронт ходит по http (5189) — редирект на https ломает запросы
        // из-за недоверенного dev-сертификата. В Production редирект остаётся.
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseSerilogRequestLogging();

        app.UseAuthentication();
        app.UseAuthorization();

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

    /// <summary>
    /// Понятное сообщение для 409 Conflict по коду ошибки PostgreSQL
    /// </summary>
    private static string GetDbConflictMessage(DbUpdateException dbEx)
    {
        if (dbEx.InnerException is Npgsql.PostgresException pgEx)
        {
            return pgEx.SqlState switch
            {
                // Нарушение уникальности: дубль номера документа, дубль строки баланса
                Npgsql.PostgresErrorCodes.UniqueViolation =>
                    "Ошибка. Запись с такими значениями уже существует.",
                // Нарушение внешнего ключа: несуществующие ResourceId/UnitId и т.п.
                Npgsql.PostgresErrorCodes.ForeignKeyViolation =>
                    "Ошибка. Нарушение связей данных: связанный объект не существует или используется.",
                _ => "Ошибка. Конфликт при сохранении данных.",
            };
        }
        return "Ошибка. Конфликт при сохранении данных.";
    }
}

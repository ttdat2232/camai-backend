using System.Reflection;
using System.Text.Json.Serialization;
using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Implements;
using Core.Domain.Events;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Configurations;
using Core.Domain.Services;
using Host.CamAI.API.BackgroundServices;
using Host.CamAI.API.Events;
using Host.CamAI.API.Sockets;
using Infrastructure.Blob;
using Infrastructure.Files;
using Microsoft.OpenApi.Models;

namespace Host.CamAI.API;

public static class ApiDependencyInjection
{
    private static IServiceCollection AddObserverPattern(this IServiceCollection services)
    {
        //Subject
        services.AddSingleton<AccountNotificationSubject>();
        services.AddSingleton<IncidentSubject>();
        services.AddSingleton<BulkUpsertProgressSubject>();

        //Observer
        services.AddSingleton<NotificationSocketManager>();
        services.AddSingleton<IncidentSocketManager>();
        return services;
    }

    public static IServiceCollection AddEventListener(this IServiceCollection services)
    {
        services.AddSingleton<IApplicationDelayEventListener, ApplicationDelayEventListener>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IShopService, ShopService>();
        services.AddScoped<ICameraService, CameraService>();
        services.AddScoped<IIncidentService, IncidentService>();
        services.AddScoped<IEdgeBoxService, EdgeBoxService>();
        services.AddScoped<IEdgeBoxInstallService, EdgeBoxInstallService>();
        services.AddScoped<IEdgeBoxModelService, EdgeBoxModelService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IEdgeBoxInstallService, EdgeBoxInstallService>();
        services.AddScoped<ISupervisorAssignmentService, SupervisorAssignmentService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddSingleton<IBulkTaskService, BulkTaskService>();
        services.AddSingleton<EventManager>();
        services.AddSingleton<EventManager>().AddSingleton<HumanCountSubject>();

        services.AddObserverPattern();
        services.AddReadFile();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var imgConfig =
            configuration.GetRequiredSection("ImageConfiguration").Get<ImageConfiguration>()
            ?? throw new ServiceUnavailableException("Cannot get image configuration");
        services.AddSingleton(imgConfig);
        services.AddSingleton(configuration.GetSection("HealthCheckConfiguration").Get<HealthCheckConfiguration>()!);
        services.AddBlobService();
        return services;
    }

    public static IServiceCollection AddBackgroundService(this IServiceCollection services)
    {
        services.AddHostedService<EdgeBoxHealthCheckService>();
        services.AddHostedService<AutoAssignSupervisorService>();
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "CamAI API", Version = "v1" });
            option.SupportNonNullableReferenceTypes();
            option.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                }
            );
            option.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                }
            );
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            option.IncludeXmlComments(xmlPath);
        });
        return services;
    }
}

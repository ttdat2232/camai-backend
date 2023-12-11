using Core.Domain.Models;
using Host.CamAI.API.Middlewares;
using Infrastructure.Jwt;
using Infrastructure.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appConfig = builder.Configuration.Get<AppConfiguration>();
if (appConfig != null)
{
    appConfig.ConnectionString = builder.Configuration.GetConnectionString("Default") ?? throw new ArgumentException("Not found database connection string");
    appConfig.Issuer = builder.Configuration["Jwt:Issuer"] ?? throw new ArgumentException("Not found issuer");
    appConfig.Audience = builder.Configuration["Jwt:Audience"] ?? throw new ArgumentException("Not found Audience");
    appConfig.JwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new ArgumentException("Not found Jwt secret");
    appConfig.Expired = int.Parse(builder.Configuration["Jwt:Expired"] ?? throw new ArgumentException("Not found expired"));
    builder.Services.AddSingleton(appConfig);
    builder.Services.RepositoryDependencyInjection(appConfig.ConnectionString);
    builder.Services.JwtDependencyInjection();
}

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

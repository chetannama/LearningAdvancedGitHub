using EmailSpamDetectionService.Helpers;
using EmailSpamDetectionService.Middlewares;
using EmailSpamDetectionService.Services.Interfaces;
using EmailSpamDetectionService.Services.Services;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IEmailSpamService, EmailSpamService>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmailSpamDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure the HTTP request pipeline.
// To Configure Retry Policy
builder.Services.AddHttpClient("EmailSpamService", client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:8002");
})
    .AddPolicyHandler(RetryPolicyHelper.GetRetryPolicy());

// Configure Serilog
Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .WriteTo.Console(
                        outputTemplate:
                                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} CorrelationId:{CorrelationId} {NewLine}{Exception}")
                //.WriteTo.Seq("URL")
                .WriteTo.File(
                                "logs/log-.txt",
                                rollingInterval: RollingInterval.Day,
                                retainedFileCountLimit: 30)
                .CreateLogger();


builder.Services.AddControllers();
builder.Host.UseSerilog();
var app = builder.Build();

app.UseMiddleware<CorrelationMiddleware>();


//Enable Request Logging Middleware
//Now every API call logs automatically.
app.UseSerilogRequestLogging();

//You can trace the same request across multiple services.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

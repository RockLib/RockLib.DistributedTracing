using Microsoft.AspNetCore.Builder;
using Middleware.NET60;
using RockLib.DistributedTracing.AspNetCore;
using System;
using System.Linq;
using System.Security.Cryptography;

#pragma warning disable CA1812

var builder = WebApplication.CreateBuilder(args);

// Use this to change the name of the correlation id header
//builder.Services.Configure<CorrelationIdMiddlewareOptions>(options => options.HeaderName = "Custom-Id-Header");

var app = builder.Build();

app.UseCorrelationIdMiddleware();

app.MapGet("/", () =>
{
   return Enumerable.Range(1, 5).Select(index => new WeatherForecast
   {
      Date = DateTime.Now.AddDays(index),
      TemperatureC = RandomNumberGenerator.GetInt32(-20, 55),
      Summary = WeatherForecast.Summaries[RandomNumberGenerator.GetInt32(WeatherForecast.Summaries.Length)]
   }).ToArray();
});

app.Run();
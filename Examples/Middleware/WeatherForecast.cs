using System;

namespace Middleware.NET60;

public sealed class WeatherForecast
{
   public static readonly string[] Summaries = new[]
   {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
   };

   public DateTime Date { get; init; }

   public int TemperatureC { get; init; }

   public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

   public string? Summary { get; init; }
}

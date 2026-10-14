using System.Text.Json.Serialization;

namespace MeteoWeatherApi_ObserverPattern_And_MVVM.Models;

public class CurrentWeather
{
    public string? Time { get; set; }
    
    [JsonPropertyName("temperature_2m")]
    public double? Temperature2M { get; set; }
    
    [JsonPropertyName("wind_speed_10m")]
    public double? WindSpeed10M { get; set; }
}
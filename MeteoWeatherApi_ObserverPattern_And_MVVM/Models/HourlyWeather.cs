using System.Text.Json.Serialization;

namespace MeteoWeatherApi_ObserverPattern_And_MVVM.Models;

public class HourlyWeather
{
    [JsonPropertyName("time")]
    public List<string>? Time { get; set; }
    
    [JsonPropertyName("temperature_2m")]
    public List<double>? Temperature2M{ get; set; }
    
    [JsonPropertyName("relative_humidity_2m")]
    public List<int>? RelativeHumidity2M { get; set; }
    
    [JsonPropertyName("wind_speed_10m")]
    public List<double>? WindSpeed10M { get; set; }
}
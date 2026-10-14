using System.Text.Json;
using MeteoWeatherApi_ObserverPattern_And_MVVM.Models;

namespace MeteoWeatherApi_ObserverPattern_And_MVVM.MeteoApiClient;

public class MeteoApiClient
{
    private HttpClient? _client;

    public MeteoApiClient()
    {
        _client = new HttpClient();
    }
    
    public async Task<WeatherResponse?> GetWeatherData()
    {
        string url = "https://api.open-meteo.com/v1/forecast" +
                     "?latitude=52.52" +
                     "&longitude=13.41" +
                     "&current=temperature_2m,wind_speed_10m" +
                     "&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m";
        WeatherResponse? weather =  new WeatherResponse();

        if (_client is not null)
        {
            var response = await _client.GetStringAsync(url);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            weather = JsonSerializer.Deserialize<WeatherResponse>(response, options);

            /*Console.WriteLine($"Temperatur: {weather.Current.Temperature2M} °C");
            Console.WriteLine($"Wind: {weather.Current.WindSpeed10M} km/h");*/
        }
        return weather;
    }
} 
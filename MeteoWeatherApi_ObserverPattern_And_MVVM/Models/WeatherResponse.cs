namespace MeteoWeatherApi_ObserverPattern_And_MVVM.Models;

public class WeatherResponse
{
    public CurrentWeather? Current { get; set; }
    public HourlyWeather? Hourly { get; set; }
}
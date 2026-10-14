using MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Observer;
using MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Subject;

namespace MeteoWeatherApi_ObserverPattern_And_MVVM.Subjects.CurrentConditions;

public class CurrentConditionsData : ISubject
{
    private MeteoApiClient.MeteoApiClient _client;

    private List<IObserver> _observers;

    public string? Temperature { get; set; } = "0.0";
    
    public string? WindSpeed10M { get; set; } = "0.0";

    public CurrentConditionsData(MeteoApiClient.MeteoApiClient client)
    {
        _client = client;
        _observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        _observers.Add((IObserver)observer);
    }
    
    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove((IObserver)observer);
    }
    
    public void NotifyObserver()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }

    public void CallLoadData()
    {
        LoadData().GetAwaiter().GetResult();
    }
    
    private async Task LoadData()
    {
        var result = await _client.GetWeatherData();
        Temperature = $"{result?.Current?.Temperature2M:F1} °C";
        WindSpeed10M = $"{result?.Current?.WindSpeed10M:F1} km/h";
        NotifyObserver();
    }
}
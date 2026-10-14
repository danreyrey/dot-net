using MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Observer;

namespace MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Subject;

public interface ISubject
{
    public void RegisterObserver(IObserver observer);
    public void RemoveObserver(IObserver observer);
    public void NotifyObserver();
}
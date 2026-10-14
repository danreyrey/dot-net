using MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Subject;

namespace MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Observer;

public interface IObserver
{
    public void Update(ISubject subject);
}
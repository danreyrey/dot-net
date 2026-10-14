using MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Observer;
using MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Subject;
using MeteoWeatherApi_ObserverPattern_And_MVVM.Subjects.CurrentConditions;

namespace MeteoWeatherApi_ObserverPattern_And_MVVM;

public partial class MainPage : ContentPage, IObserver
{
    public string? Temperature { get; set; }
    public string? WindSpeed10M { get; set; }


    public MainPage()
    {
        InitializeComponent();
        Temperature = "0";
        WindSpeed10M = "0";
        LoadData();
    }

    private void LoadData()
    {
        /*Task.Delay(1000);
        MeteoApiClient.MeteoApiClient client = new MeteoApiClient.MeteoApiClient();
        CurrentConditionsData data = new CurrentConditionsData(client);
        data.RegisterObserver(this);
        data.CallLoadData();*/
    }
    
    public void Update(ISubject subject)
    {
        if ((subject as CurrentConditionsData)?.Temperature is string tmpTemperature && 
            (subject as CurrentConditionsData)?.WindSpeed10M is string tmpWindSpeed10M)
        {
            Temperature = tmpTemperature;
            WindSpeed10M = tmpWindSpeed10M;
        }
    }
}
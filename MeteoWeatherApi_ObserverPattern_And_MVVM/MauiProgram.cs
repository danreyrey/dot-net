using MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Observer;
using MeteoWeatherApi_ObserverPattern_And_MVVM.Interfaces.Subject;
using MeteoWeatherApi_ObserverPattern_And_MVVM.Subjects.CurrentConditions;
using Microsoft.Extensions.Logging;

namespace MeteoWeatherApi_ObserverPattern_And_MVVM;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        
        return builder.Build();
    }
}
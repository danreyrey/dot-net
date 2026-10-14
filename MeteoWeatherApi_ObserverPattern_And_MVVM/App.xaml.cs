using Microsoft.Extensions.DependencyInjection;

namespace MeteoWeatherApi_ObserverPattern_And_MVVM;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
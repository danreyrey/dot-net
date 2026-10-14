global using CommunityToolkit.Maui;
using ListenApp.ViewModel;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using DevExpress.Maui;

namespace ListenApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // the builder design-pattern by using method chaining for configuration results in instantiating objects of type
        // App, TodoListsDatabase, MainPage, MainViewModel, DetailPage and DetailViewmodel
        var builder = MauiApp.CreateBuilder(); 
        builder
            .UseDevExpress(useLocalization: false)
            .UseDevExpressCollectionView()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                // fonts.AddFont("completedTask.ttf", "ct");
                // fonts.AddFont("notCompletedTask.ttf", "nCT");
            })
            .UseLocalNotification();
        
        builder.Services.AddSingleton<TodoListsDatabase>(); // registration: passing the dependency injection container a type of TodoListsDatabase
        
        builder.Services.AddSingleton<MainPage>();      // registration: passing the dependency injection container a type of MainPage (View)
        builder.Services.AddSingleton<MainViewModel>(); // registration: passing the dependency injection container a type of MainViewModel
        
        builder.Services.AddTransient<DetailPage>();      // registration: passing the dependency injection container a type of DetailPage (View)
        builder.Services.AddTransient<DetailViewModel>(); // registration: passing the dependency injection container a type of DetailViewModel
        
#if DEBUG
        
        builder.Logging.AddDebug();
     
#endif

        return builder.Build();
        
    }
}
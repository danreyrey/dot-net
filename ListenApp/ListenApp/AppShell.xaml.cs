namespace ListenApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage)); // navigating to DetailPage
    }
}
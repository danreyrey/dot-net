namespace Restschuldrechner;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        // Databindings im XAML-Markup mit dem ViewModel verbinden
        this.BindingContext = new RestschuldViewModel();
    }

    private double _width;
    private double _height;
    // Diese Methode wird während des Layoutzyklus aufgerufen,
    // der zum Beispiel auftritt, wenn die Ausrichtung des Geräts geändert wird (Hoch- oder Querformat)^
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (width != _width || height != _height) // Prüfen, ob sich die Größe geändert hat
        {
            _width = width;
            _height = height;

            if (width > height) // Breite > Höhe = Querformat
            {
                OuterLayout.Orientation = StackOrientation.Horizontal;

                // Breite der Kindelemente proportional anpassen
                foreach (View view in OuterLayout.Children)
                {
                    view.WidthRequest = width / OuterLayout.Children.Count;
                }
            }
            else
            {
                OuterLayout.Orientation = StackOrientation.Vertical;

                // Im Hochformat haben die Kindelemente die gleiche Breite wie das OuterLayout
                foreach (View view in OuterLayout.Children)
                {
                    view.WidthRequest = OuterLayout.Width;
                }
            }
        }
    }

}
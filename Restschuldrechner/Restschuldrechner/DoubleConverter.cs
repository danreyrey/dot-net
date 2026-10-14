namespace Restschuldrechner
{
    // Die Klasse DoubleConverter ermöglicht die Eingabe lokalisierter Dezimaltrennzeichen wie z. B. des Kommas.
    // Implementierung des im XAML-Markup genutzten DoubleConverters
    // Dieser implementiert die Schnittstelle IValueConverter aus dem Namensraum Microsoft.Maui.Controls.
    public class DoubleConverter : IValueConverter
    {
        // Convert wird aufgerufen, um den Wert des ViewModels in einen String zu konvertieren,
        // der anschließend durch die Datenbindung in das Eingabefeld der Oberfläche geschrieben wird. 
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return value.ToString();
            }

            return value;
        }

        // ConvertBack dient für den Rückweg, also für die Übertragung der Be- nutzereingabe in das ViewModel.
        // Der Code der Methode prüft lediglich, ob der eingegebene Wert in einen Double geparst werden kann und gibt diesen dann zurück.
        // Kann der Wert nicht geparst werden, wird der unveränderte Rohwert zurückgegeben.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (double.TryParse(value as string, out double dbl))
            {
                return dbl;
            }

            return value;
        }

    }
}
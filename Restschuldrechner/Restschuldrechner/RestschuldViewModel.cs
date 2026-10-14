// ViewModel kapselt Daten und Logik für die Bildschirmmaske;
// ViewModel muss zur Unterstützung der Zwei-Wege-Datenbindung
// Die Schnittstelle INotifyPropertyChanged implementieren;
// Import der zusätzlichen Namensräume System.ComponentModel und System.Runtime.CompileServices
// durch das Schlüsselwort using;
// Mit der using-Anweisung kann man in einem Namespace definierte Typen verwenden,
// ohne den vollqualifizierten Namespace für diese anzugeben (Console.WriteLine("Hello World!"); anstelle von
// System.Console.WriteLine("Hello World!"););
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Restschuldrechner;

// Markieren der Klasse RestschuldViewModel für die Implementierung der Schnittstelle INotifyPropertyChanged;
public class RestschuldViewModel : INotifyPropertyChanged
{
    // INotifyPropertyChanged definiert ein Event vom Typ PropertyChangedEventHandler mit dem Namen PropertyChanged.
    // Dieses können Sie innerhalb der Klasse über ein einfaches öffentliches Feld implementieren;
    public event PropertyChangedEventHandler PropertyChanged;

    // Eigenschaftsdefinitionen für die Eingabewerte/-felder der Bildschirmmaske der Restschuldrechner-App
    private double _kreditSumme = 150000;
    private double _tilgungsSatz = 1.5f;
    private int _zinsbindung = 15;
    private double _zinsfuss = 1.5f;

    // Eigenschaftsdefinitionen für die Ausgabefelder der Bildschirmmaske der Restschuldrechner-App
    private double _zahlungen;
    private double _rsk;
    private double _tilgung;
    private double _zinszahlungen;
    private double _anteilRestschuld;


    /// <summary>
    /// Kreditsumme (S)
    /// </summary>
    public double KreditSumme
    {
        // get-Anweisung gibt den Wert eines privaten Feldes zurück
        get => _kreditSumme;
        // set-Anweisung prüft, ob ein geänderter Wert übergeben wurde;
        // Falls nicht, wird die Set-Anweisung sofort verlassen. Wurde ein geänderter Wert übergeben,
        // dann wird dieser an das zugrunde liegende private Feld zugwiesen.
        // Anschließend wird das Ereignis PropertyChanged ausgelöst und die Methode Calculate aufgerufen.
        // Die Methode Calculate sorgt dafür, dass bei jeder Benutzereingabe sofort eine Berechnung ausgeführt wird,
        // ohne dass der Benutzer dazu eine spezielle Schaltfläche drücken muss.
        set
        {
            if (_kreditSumme == value) { return; }
            _kreditSumme = value;
            OnPropertyChanged();
            Calculate();
        }
    }

    /// <summary>
    /// Zinsfuß (p)
    /// </summary>
    public double Zinsfuss
    {
        get => _zinsfuss;
        set
        {
            if (_zinsfuss == value) { return; }
            _zinsfuss = value;
            OnPropertyChanged();
            Calculate();
        }
    }


    /// <summary>
    /// Tilgungsssatz (t)
    /// </summary>
    public double TilgungsSatz
    {
        get => _tilgungsSatz;
        set
        {
            if (_tilgungsSatz == value) { return; }
            _tilgungsSatz = value;
            OnPropertyChanged();
            Calculate();
        }
    }

    /// <summary>
    /// Zinsbindung in Jahren (k)
    /// </summary>
    public int Zinsbindung
    {
        get => _zinsbindung;
        set
        {
            if (_zinsbindung == value) { return; }
            _zinsbindung = value;
            OnPropertyChanged();
            Calculate();
        }
    }

    /// <summary>
    /// Anteil der Restschuld an der Gesamtschuld
    /// </summary>
    // Da für diese Eigenschaften keine Zwei-Wege-Datenbindung benötigt wird,
    // fehlt der Teil, in dem geprüft wird, ob sich der übergebene Wert geändert hat.
    public double AnteilRestschuld
    {
        get => _anteilRestschuld;
        set
        {
            _anteilRestschuld = Math.Max(0, value);
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Summe der Zinszahlungen während der Zinsbindung
    /// </summary>
    public double Zinszahlungen
    {
        get => _zinszahlungen;
        set
        {
            _zinszahlungen = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Höhe der Tilgung während der Zinsbindung
    /// </summary>
    public double Tilgung
    {
        get => _tilgung;
        set
        {
            _tilgung = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///  Restschuld am Ende der Laufzeit
    /// </summary>
    public double Restschuld
    {
        get => _rsk;
        set
        {
            _rsk = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gesamtzahlungen während der Laufzeit
    /// </summary>
    public double Zahlungen
    {
        get => _zahlungen;
        set
        {
            _zahlungen = value;
            OnPropertyChanged();
        }
    }

    // Die Methode Calculate berechnet die Restschuld, Tilgung,
    // gezahlte Zinsen sowie den Anteil der Restschuld an der Gesamtschuld.
    private void Calculate()
    {
        // Zinssatz pro Jahr (i)
        var i = Zinsfuss / 100;

        // Zinssatz pro Monat (im)
        var im = i / 12;

        // Zinsfaktor pro Monat (qm)
        var qm = im + 1;

        // Zinsbindung in Monaten
        var km = Zinsbindung * 12;

        // jährliche Annuität (A)
        var annuitaet = KreditSumme * (i + TilgungsSatz / 100);

        // monatliche Annuität (a)
        var a = annuitaet / 12;

        // Gesamtzahlungen während der Laufzeit
        Zahlungen = annuitaet * Zinsbindung;

        // Restschuld am Ende der Laufzeit / Jahr k (RSk)
        Restschuld = KreditSumme - (Math.Pow(qm, km) - 1) / im * (a - im * KreditSumme);

        // Tilgung 
        Tilgung = KreditSumme - Restschuld;
        Zinszahlungen = Zahlungen - Tilgung;

        AnteilRestschuld = 250d / KreditSumme * Restschuld;

    }
    
    // Für eine korrekte Zwei-Wege-Datenbindung muss die ViewModel-Klasse das
    // Ereignis ("public event PropertyChangedEventHandler PropertyChanged;" line 19)
    // bei jeder Eigenschaftsänderung auslösen.
    // Zu diesem Zweck fügen wir folgende Hilfsmethode in die Klasse RestschuldViewModel ein:
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}

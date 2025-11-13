using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiteDB;

namespace CGEasy.Core.Models;

public class BilancioContabile : INotifyPropertyChanged
{
    [BsonId]
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public int Mese { get; set; } // 1-12
    public int Anno { get; set; }
    public string? DescrizioneBilancio { get; set; } // Descrizione del bilancio
    public string TipoBilancio { get; set; } = "CE"; // CE = Conto Economico, SP = Stato Patrimoniale
    public string CodiceMastrino { get; set; } = string.Empty;
    public string DescrizioneMastrino { get; set; } = string.Empty;
    public decimal Importo { get; set; }
    public string? Note { get; set; }
    public DateTime DataImport { get; set; }
    public int ImportedBy { get; set; }
    public string ImportedByName { get; set; } = string.Empty;
    
    // Proprietà per checkbox selezione
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }

    // Proprietà calcolate per UI
    public string PeriodoDisplay => $"{Mese:00}/{Anno}";
    public string ImportoFormatted => Importo.ToString("C2");
    public string MeseNome => Mese switch
    {
        1 => "Gen",
        2 => "Feb",
        3 => "Mar",
        4 => "Apr",
        5 => "Mag",
        6 => "Giu",
        7 => "Lug",
        8 => "Ago",
        9 => "Set",
        10 => "Ott",
        11 => "Nov",
        12 => "Dic",
        _ => ""
    };
    public string PeriodoCompleto => $"{MeseNome} {Anno}";
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}



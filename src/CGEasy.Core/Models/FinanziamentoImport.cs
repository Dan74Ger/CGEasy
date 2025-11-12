using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CGEasy.Core.Models;

public class FinanziamentoImport : INotifyPropertyChanged
{
    private int _id;
    private string _nomeFinanziamento = string.Empty;
    private DateTime _dataInizio;
    private DateTime _dataFine;
    private decimal _importo;
    private int _bancaId;
    private string? _nomeFornitore;
    private string? _numeroFattura;
    private DateTime? _dataFattura;
    private int _incassoId;
    private int _pagamentoId;
    private DateTime _dataCreazione;
    private int _utenteId;

    public int Id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
    }

    public string NomeFinanziamento
    {
        get => _nomeFinanziamento;
        set { _nomeFinanziamento = value; OnPropertyChanged(); }
    }

    public DateTime DataInizio
    {
        get => _dataInizio;
        set { _dataInizio = value; OnPropertyChanged(); }
    }

    public DateTime DataFine
    {
        get => _dataFine;
        set { _dataFine = value; OnPropertyChanged(); }
    }

    public decimal Importo
    {
        get => _importo;
        set { _importo = value; OnPropertyChanged(); }
    }

    public int BancaId
    {
        get => _bancaId;
        set { _bancaId = value; OnPropertyChanged(); }
    }

    public string? NomeFornitore
    {
        get => _nomeFornitore;
        set { _nomeFornitore = value; OnPropertyChanged(); }
    }

    public string? NumeroFattura
    {
        get => _numeroFattura;
        set { _numeroFattura = value; OnPropertyChanged(); }
    }

    public DateTime? DataFattura
    {
        get => _dataFattura;
        set { _dataFattura = value; OnPropertyChanged(); }
    }

    public int IncassoId
    {
        get => _incassoId;
        set { _incassoId = value; OnPropertyChanged(); }
    }

    public int PagamentoId
    {
        get => _pagamentoId;
        set { _pagamentoId = value; OnPropertyChanged(); }
    }

    public DateTime DataCreazione
    {
        get => _dataCreazione;
        set { _dataCreazione = value; OnPropertyChanged(); }
    }

    public int UtenteId
    {
        get => _utenteId;
        set { _utenteId = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


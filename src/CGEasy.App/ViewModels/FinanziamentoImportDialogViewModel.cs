using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CGEasy.Core.Models;
using CGEasy.Core.Repositories;
using CGEasy.Core.Services;
using System;
using System.Windows;

namespace CGEasy.App.ViewModels;

public partial class FinanziamentoImportDialogViewModel : ObservableObject
{
    private readonly FinanziamentoImportRepository _finanziamentoRepository;
    private readonly BancaIncassoRepository _incassoRepository;
    private readonly BancaPagamentoRepository _pagamentoRepository;
    private readonly int _bancaId;
    private readonly string _nomeBanca;

    [ObservableProperty]
    private string _nomeFinanziamento = string.Empty;

    [ObservableProperty]
    private DateTime _dataInizio = DateTime.Today;

    [ObservableProperty]
    private DateTime _dataFine = DateTime.Today.AddMonths(3);

    [ObservableProperty]
    private decimal _importo;

    [ObservableProperty]
    private string? _nomeFornitore;

    [ObservableProperty]
    private string? _numeroFattura;

    [ObservableProperty]
    private DateTime? _dataFattura;

    public bool DialogResult { get; private set; }

    public FinanziamentoImportDialogViewModel(
        FinanziamentoImportRepository finanziamentoRepository,
        BancaIncassoRepository incassoRepository,
        BancaPagamentoRepository pagamentoRepository,
        int bancaId,
        string nomeBanca)
    {
        _finanziamentoRepository = finanziamentoRepository;
        _incassoRepository = incassoRepository;
        _pagamentoRepository = pagamentoRepository;
        _bancaId = bancaId;
        _nomeBanca = nomeBanca;
    }

    [RelayCommand]
    private void Salva()
    {
        // Validazione
        if (string.IsNullOrWhiteSpace(NomeFinanziamento))
        {
            MessageBox.Show("Inserire il nome del finanziamento", "Validazione", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (Importo <= 0)
        {
            MessageBox.Show("L'importo deve essere maggiore di zero", "Validazione", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (DataFine <= DataInizio)
        {
            MessageBox.Show("La data fine deve essere successiva alla data inizio", "Validazione", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var currentUser = SessionManager.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("Utente non autenticato", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 1. Crea INCASSO (Accensione finanziamento alla Data Inizio)
            var incasso = new BancaIncasso
            {
                BancaId = _bancaId,
                DataScadenza = DataInizio,
                Importo = Importo,
                NomeCliente = "Finanziamento Import",
                Incassato = false,
                Note = $"Accensione Fin. Import: {NomeFinanziamento}",
                Anno = DataInizio.Year,
                Mese = DataInizio.Month,
                DataCreazione = DateTime.Now,
                DataUltimaModifica = DateTime.Now
            };
            var incassoId = _incassoRepository.Insert(incasso);

            // Verifica se ci sono dati fattura (almeno uno compilato)
            bool hasDatiFattura = !string.IsNullOrWhiteSpace(NomeFornitore) || 
                                  !string.IsNullOrWhiteSpace(NumeroFattura) || 
                                  DataFattura.HasValue;

            int? pagamentoFatturaId = null;

            // 2. Se ci sono dati fattura: crea PAGAMENTO FATTURA alla Data Inizio (stesso giorno)
            if (hasDatiFattura)
            {
                var pagamentoFattura = new BancaPagamento
                {
                    BancaId = _bancaId,
                    DataScadenza = DataInizio, // STESSO GIORNO dell'incasso
                    Importo = Importo,
                    NomeFornitore = !string.IsNullOrWhiteSpace(NomeFornitore) ? NomeFornitore : "Fornitore Estero",
                    Pagato = false,
                    NumeroFatturaFornitore = !string.IsNullOrWhiteSpace(NumeroFattura) ? NumeroFattura : null,
                    DataFatturaFornitore = DataFattura,
                    Note = $"Pagamento Fattura per Fin. Import: {NomeFinanziamento}",
                    Anno = DataInizio.Year,
                    Mese = DataInizio.Month,
                    DataCreazione = DateTime.Now,
                    DataUltimaModifica = DateTime.Now
                };
                pagamentoFatturaId = _pagamentoRepository.Insert(pagamentoFattura);
            }

            // 3. Crea PAGAMENTO (Chiusura finanziamento alla Data Fine)
            var pagamentoChiusura = new BancaPagamento
            {
                BancaId = _bancaId,
                DataScadenza = DataFine,
                Importo = Importo,
                NomeFornitore = "Banca - Chiusura Finanziamento",
                Pagato = false,
                Note = $"Chiusura Fin. Import: {NomeFinanziamento}",
                Anno = DataFine.Year,
                Mese = DataFine.Month,
                DataCreazione = DateTime.Now,
                DataUltimaModifica = DateTime.Now
            };
            var pagamentoChiusuraId = _pagamentoRepository.Insert(pagamentoChiusura);

            // 4. Crea FINANZIAMENTO
            var finanziamento = new FinanziamentoImport
            {
                NomeFinanziamento = NomeFinanziamento,
                DataInizio = DataInizio,
                DataFine = DataFine,
                Importo = Importo,
                BancaId = _bancaId,
                NomeFornitore = !string.IsNullOrWhiteSpace(NomeFornitore) ? NomeFornitore : null,
                NumeroFattura = !string.IsNullOrWhiteSpace(NumeroFattura) ? NumeroFattura : null,
                DataFattura = DataFattura,
                IncassoId = incassoId,
                PagamentoId = pagamentoChiusuraId, // Riferimento al pagamento di chiusura
                UtenteId = currentUser.Id
            };
            _finanziamentoRepository.Insert(finanziamento);

            // Chiudi immediatamente per aggiornamento automatico
            DialogResult = true;
            CloseDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Errore durante il salvataggio: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Annulla()
    {
        DialogResult = false;
        CloseDialog();
    }

    public event EventHandler? RequestClose;

    private void CloseDialog()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}


using CGEasy.Core.Data;
using CGEasy.Core.Models;
using CGEasy.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Services;

/// <summary>
/// Service per logica business della gestione Banche
/// Calcoli saldo, fido, interessi, alert, scadenziari
/// </summary>
public class BancaService
{
    private readonly BancaRepository _bancaRepo;
    private readonly BancaIncassoRepository _incassoRepo;
    private readonly BancaPagamentoRepository _pagamentoRepo;
    private readonly BancaUtilizzoAnticipoRepository _anticipoRepo;
    private readonly BancaSaldoGiornalieroRepository _saldoRepo;

    public BancaService(LiteDbContext context)
    {
        _bancaRepo = new BancaRepository(context);
        _incassoRepo = new BancaIncassoRepository(context);
        _pagamentoRepo = new BancaPagamentoRepository(context);
        _anticipoRepo = new BancaUtilizzoAnticipoRepository(context);
        _saldoRepo = new BancaSaldoGiornalieroRepository(context);
    }

    #region CALCOLI SALDO

    /// <summary>
    /// Calcola il saldo corrente di una banca
    /// (usa il campo SaldoDelGiorno della banca)
    /// </summary>
    public decimal GetSaldoCorrente(int bancaId)
    {
        var banca = _bancaRepo.GetById(bancaId);
        return banca?.SaldoDelGiorno ?? 0;
    }

    /// <summary>
    /// Calcola il saldo totale di tutte le banche al primo giorno del mese
    /// (per calcolo automatico A0 nel Margine Tesoreria)
    /// </summary>
    public decimal GetSaldoTotaleAllaData(DateTime data)
    {
        var banche = _bancaRepo.GetAll();
        decimal totale = 0;

        foreach (var banca in banche)
        {
            // Usa il saldo storico se disponibile, altrimenti usa SaldoDelGiorno
            var saldoStorico = _saldoRepo.GetAllaData(banca.Id, data);
            if (saldoStorico != null)
            {
                totale += saldoStorico.Saldo;
            }
            else
            {
                totale += banca.SaldoDelGiorno;
            }
        }

        return totale;
    }

    /// <summary>
    /// Calcola il saldo previsto per una banca ad una certa data
    /// considerando incassi e pagamenti previsti
    /// </summary>
    public decimal GetSaldoPrevisto(int bancaId, DateTime dataPrevisione)
    {
        var saldoAttuale = GetSaldoCorrente(bancaId);

        // Aggiungi incassi previsti fino alla data
        var incassiPrevisti = _incassoRepo.GetByBancaId(bancaId)
            .Where(i => !i.Incassato && i.DataScadenza <= dataPrevisione)
            .Sum(i => i.Importo);

        // Sottrai pagamenti previsti fino alla data
        var pagamentiPrevisti = _pagamentoRepo.GetByBancaId(bancaId)
            .Where(p => !p.Pagato && p.DataScadenza <= dataPrevisione)
            .Sum(p => p.Importo);

        return saldoAttuale + incassiPrevisti - pagamentiPrevisti;
    }

    /// <summary>
    /// Calcola il saldo mensile (somma saldi di tutte le banche per un mese)
    /// </summary>
    public decimal GetSaldoMensile(int anno, int mese)
    {
        var dataInizioMese = new DateTime(anno, mese, 1);
        return GetSaldoTotaleAllaData(dataInizioMese);
    }

    #endregion

    #region CALCOLI FIDO

    /// <summary>
    /// Calcola il fido residuo disponibile per una banca
    /// Fido Residuo = Fido Accordato - Utilizzo Attivo
    /// </summary>
    public decimal GetFidoResiduo(int bancaId)
    {
        var banca = _bancaRepo.GetById(bancaId);
        if (banca == null) return 0;

        var utilizzoTotale = _anticipoRepo.GetTotaleUtilizziAttivi(bancaId);
        return banca.FidoCCAccordato - utilizzoTotale;
    }

    /// <summary>
    /// Verifica se c'è un superamento del fido per una banca
    /// </summary>
    public bool IsFidoSuperato(int bancaId)
    {
        return GetFidoResiduo(bancaId) < 0;
    }

    /// <summary>
    /// Calcola la percentuale di utilizzo del fido
    /// </summary>
    public decimal GetPercentualeUtilizzoFido(int bancaId)
    {
        var banca = _bancaRepo.GetById(bancaId);
        if (banca == null || banca.FidoCCAccordato == 0) return 0;

        var utilizzoTotale = _anticipoRepo.GetTotaleUtilizziAttivi(bancaId);
        return (utilizzoTotale / banca.FidoCCAccordato) * 100;
    }

    /// <summary>
    /// Verifica se è possibile utilizzare un certo importo di anticipo
    /// senza superare il massimale
    /// </summary>
    public bool PuoUtilizzareAnticipo(int bancaId, decimal importo)
    {
        var banca = _bancaRepo.GetById(bancaId);
        if (banca == null) return false;

        var utilizzoAttuale = _anticipoRepo.GetTotaleUtilizziAttivi(bancaId);
        return (utilizzoAttuale + importo) <= banca.AnticipoFattureMassimo;
    }

    /// <summary>
    /// Calcola l'anticipo fatture residuo disponibile
    /// </summary>
    public decimal GetAnticipoResiduoDisponibile(int bancaId)
    {
        var banca = _bancaRepo.GetById(bancaId);
        if (banca == null) return 0;

        var utilizzoAttuale = _anticipoRepo.GetTotaleUtilizziAttivi(bancaId);
        return banca.AnticipoFattureMassimo - utilizzoAttuale;
    }

    #endregion

    #region CALCOLI INTERESSI

    /// <summary>
    /// Calcola gli interessi maturati per un utilizzo anticipo
    /// Interessi = Importo * (Tasso / 100) * (Giorni / 365)
    /// </summary>
    public decimal CalcolaInteressiAnticipo(int bancaId, decimal importo, DateTime dataInizio, DateTime dataFine)
    {
        var banca = _bancaRepo.GetById(bancaId);
        if (banca == null) return 0;

        var giorni = (dataFine - dataInizio).Days;
        if (giorni <= 0) return 0;

        return importo * (banca.InteresseAnticipoFatture / 100) * (giorni / 365m);
    }

    /// <summary>
    /// Calcola gli interessi maturati su un utilizzo anticipo specifico
    /// </summary>
    public decimal CalcolaInteressiUtilizzo(int utilizzoId)
    {
        var utilizzo = _anticipoRepo.GetById(utilizzoId);
        if (utilizzo == null) return 0;

        var dataFine = utilizzo.Rimborsato && utilizzo.DataRimborsoEffettivo.HasValue
            ? utilizzo.DataRimborsoEffettivo.Value
            : DateTime.Now;

        return CalcolaInteressiAnticipo(
            utilizzo.BancaId,
            utilizzo.ImportoUtilizzo,
            utilizzo.DataInizioUtilizzo,
            dataFine
        );
    }

    /// <summary>
    /// Calcola il totale interessi maturati per tutti gli anticipi attivi di una banca
    /// </summary>
    public decimal GetTotaleInteressiAttivi(int bancaId)
    {
        var anticipiAttivi = _anticipoRepo.GetAttivi(bancaId);
        return anticipiAttivi.Sum(a => CalcolaInteressiUtilizzo(a.Id));
    }

    #endregion

    #region SCADENZIARI

    /// <summary>
    /// Recupera lo scadenziario completo di una banca
    /// (incassi + pagamenti in scadenza)
    /// </summary>
    public ScadenziarioBanca GetScadenziario(int bancaId, DateTime? dataInizio = null, DateTime? dataFine = null)
    {
        dataInizio ??= DateTime.Now;
        dataFine ??= DateTime.Now.AddMonths(3); // Default: prossimi 3 mesi

        var incassi = _incassoRepo.GetByBancaId(bancaId)
            .Where(i => !i.Incassato && i.DataScadenza >= dataInizio && i.DataScadenza <= dataFine)
            .OrderBy(i => i.DataScadenza)
            .ToList();

        var pagamenti = _pagamentoRepo.GetByBancaId(bancaId)
            .Where(p => !p.Pagato && p.DataScadenza >= dataInizio && p.DataScadenza <= dataFine)
            .OrderBy(p => p.DataScadenza)
            .ToList();

        return new ScadenziarioBanca
        {
            BancaId = bancaId,
            DataInizio = dataInizio.Value,
            DataFine = dataFine.Value,
            Incassi = incassi,
            Pagamenti = pagamenti,
            TotaleIncassi = incassi.Sum(i => i.Importo),
            TotalePagamenti = pagamenti.Sum(p => p.Importo)
        };
    }

    /// <summary>
    /// Recupera lo scadenziario consolidato di tutte le banche
    /// </summary>
    public ScadenziarioConsolidato GetScadenziarioConsolidato(DateTime? dataInizio = null, DateTime? dataFine = null)
    {
        var banche = _bancaRepo.GetAll();
        var scadenziari = new List<ScadenziarioBanca>();

        foreach (var banca in banche)
        {
            scadenziari.Add(GetScadenziario(banca.Id, dataInizio, dataFine));
        }

        return new ScadenziarioConsolidato
        {
            DataInizio = dataInizio ?? DateTime.Now,
            DataFine = dataFine ?? DateTime.Now.AddMonths(3),
            ScadenziariBanche = scadenziari,
            TotaleIncassiConsolidato = scadenziari.Sum(s => s.TotaleIncassi),
            TotalePagamentiConsolidato = scadenziari.Sum(s => s.TotalePagamenti)
        };
    }

    /// <summary>
    /// Recupera incassi in scadenza nei prossimi N giorni
    /// </summary>
    public List<BancaIncasso> GetIncassiInScadenza(int bancaId, int giorniLimite = 7)
    {
        var dataLimite = DateTime.Now.AddDays(giorniLimite);
        return _incassoRepo.GetInScadenzaEntro(bancaId, dataLimite);
    }

    /// <summary>
    /// Recupera pagamenti in scadenza nei prossimi N giorni
    /// </summary>
    public List<BancaPagamento> GetPagamentiInScadenza(int bancaId, int giorniLimite = 7)
    {
        var dataLimite = DateTime.Now.AddDays(giorniLimite);
        return _pagamentoRepo.GetInScadenzaEntro(bancaId, dataLimite);
    }

    #endregion

    #region ALERT E NOTIFICHE

    /// <summary>
    /// Recupera tutti gli alert per una banca
    /// </summary>
    public List<AlertBanca> GetAlerts(int bancaId)
    {
        var alerts = new List<AlertBanca>();
        var banca = _bancaRepo.GetById(bancaId);
        if (banca == null) return alerts;

        // Alert superamento fido
        if (IsFidoSuperato(bancaId))
        {
            var fidoResiduo = GetFidoResiduo(bancaId);
            alerts.Add(new AlertBanca
            {
                Tipo = TipoAlertBanca.SuperamentoFido,
                Gravita = GravitaAlert.Alta,
                Messaggio = $"⚠️ FIDO SUPERATO! Scoperto di {Math.Abs(fidoResiduo):C}",
                BancaId = bancaId,
                NomeBanca = banca.NomeBanca
            });
        }
        else
        {
            // Alert fido in esaurimento (> 90%)
            var percUtilizzo = GetPercentualeUtilizzoFido(bancaId);
            if (percUtilizzo > 90)
            {
                alerts.Add(new AlertBanca
                {
                    Tipo = TipoAlertBanca.FidoInEsaurimento,
                    Gravita = GravitaAlert.Media,
                    Messaggio = $"⚠️ Fido utilizzato al {percUtilizzo:F1}%",
                    BancaId = bancaId,
                    NomeBanca = banca.NomeBanca
                });
            }
        }

        // Alert saldo negativo
        if (banca.SaldoDelGiorno < 0)
        {
            alerts.Add(new AlertBanca
            {
                Tipo = TipoAlertBanca.SaldoNegativo,
                Gravita = GravitaAlert.Alta,
                Messaggio = $"⚠️ Saldo negativo: {banca.SaldoDelGiorno:C}",
                BancaId = bancaId,
                NomeBanca = banca.NomeBanca
            });
        }

        // Alert incassi in scadenza (prossimi 3 giorni)
        var incassiInScadenza = GetIncassiInScadenza(bancaId, 3);
        if (incassiInScadenza.Count > 0)
        {
            var totale = incassiInScadenza.Sum(i => i.Importo);
            alerts.Add(new AlertBanca
            {
                Tipo = TipoAlertBanca.IncassiInScadenza,
                Gravita = GravitaAlert.Bassa,
                Messaggio = $"📥 {incassiInScadenza.Count} incassi in scadenza (tot. {totale:C})",
                BancaId = bancaId,
                NomeBanca = banca.NomeBanca
            });
        }

        // Alert pagamenti in scadenza (prossimi 3 giorni)
        var pagamentiInScadenza = GetPagamentiInScadenza(bancaId, 3);
        if (pagamentiInScadenza.Count > 0)
        {
            var totale = pagamentiInScadenza.Sum(p => p.Importo);
            alerts.Add(new AlertBanca
            {
                Tipo = TipoAlertBanca.PagamentiInScadenza,
                Gravita = GravitaAlert.Media,
                Messaggio = $"📤 {pagamentiInScadenza.Count} pagamenti in scadenza (tot. {totale:C})",
                BancaId = bancaId,
                NomeBanca = banca.NomeBanca
            });
        }

        // Alert anticipi in scadenza (prossimi 7 giorni)
        var anticipiInScadenza = _anticipoRepo.GetInScadenzaEntro(bancaId, DateTime.Now.AddDays(7));
        if (anticipiInScadenza.Count > 0)
        {
            var totale = anticipiInScadenza.Sum(a => a.ImportoUtilizzo);
            alerts.Add(new AlertBanca
            {
                Tipo = TipoAlertBanca.AnticipiInScadenza,
                Gravita = GravitaAlert.Media,
                Messaggio = $"💳 {anticipiInScadenza.Count} anticipi in scadenza (tot. {totale:C})",
                BancaId = bancaId,
                NomeBanca = banca.NomeBanca
            });
        }

        return alerts;
    }

    /// <summary>
    /// Recupera tutti gli alert per tutte le banche
    /// </summary>
    public List<AlertBanca> GetAllAlerts()
    {
        var banche = _bancaRepo.GetAll();
        var alertsTotali = new List<AlertBanca>();

        foreach (var banca in banche)
        {
            alertsTotali.AddRange(GetAlerts(banca.Id));
        }

        return alertsTotali.OrderByDescending(a => a.Gravita).ToList();
    }

    #endregion

    #region RIEPILOGO TOTALI

    /// <summary>
    /// Recupera il riepilogo totale di tutte le banche
    /// </summary>
    public RiepilogoBanche GetRiepilogoTotale()
    {
        var banche = _bancaRepo.GetAll();

        return new RiepilogoBanche
        {
            SaldoTotale = banche.Sum(b => b.SaldoDelGiorno),
            FidoTotaleAccordato = banche.Sum(b => b.FidoCCAccordato),
            FidoTotaleUtilizzato = banche.Sum(b => _anticipoRepo.GetTotaleUtilizziAttivi(b.Id)),
            AnticipoTotaleMassimo = banche.Sum(b => b.AnticipoFattureMassimo),
            AnticipoTotaleUtilizzato = banche.Sum(b => _anticipoRepo.GetTotaleUtilizziAttivi(b.Id)),
            InteressiTotaliMaturati = banche.Sum(b => GetTotaleInteressiAttivi(b.Id)),
            NumeroBanche = banche.Count
        };
    }

    #endregion
}

#region CLASSI DI SUPPORTO

/// <summary>
/// Rappresenta lo scadenziario di una singola banca
/// </summary>
public class ScadenziarioBanca
{
    public int BancaId { get; set; }
    public DateTime DataInizio { get; set; }
    public DateTime DataFine { get; set; }
    public List<BancaIncasso> Incassi { get; set; } = new();
    public List<BancaPagamento> Pagamenti { get; set; } = new();
    public decimal TotaleIncassi { get; set; }
    public decimal TotalePagamenti { get; set; }
    public decimal SaldoNetto => TotaleIncassi - TotalePagamenti;
}

/// <summary>
/// Rappresenta lo scadenziario consolidato di tutte le banche
/// </summary>
public class ScadenziarioConsolidato
{
    public DateTime DataInizio { get; set; }
    public DateTime DataFine { get; set; }
    public List<ScadenziarioBanca> ScadenziariBanche { get; set; } = new();
    public decimal TotaleIncassiConsolidato { get; set; }
    public decimal TotalePagamentiConsolidato { get; set; }
    public decimal SaldoNettoConsolidato => TotaleIncassiConsolidato - TotalePagamentiConsolidato;
}

/// <summary>
/// Rappresenta un alert/notifica per una banca
/// </summary>
public class AlertBanca
{
    public TipoAlertBanca Tipo { get; set; }
    public GravitaAlert Gravita { get; set; }
    public string Messaggio { get; set; } = string.Empty;
    public int BancaId { get; set; }
    public string NomeBanca { get; set; } = string.Empty;
}

/// <summary>
/// Tipo di alert
/// </summary>
public enum TipoAlertBanca
{
    SuperamentoFido,
    FidoInEsaurimento,
    SaldoNegativo,
    IncassiInScadenza,
    PagamentiInScadenza,
    AnticipiInScadenza
}

/// <summary>
/// Gravità dell'alert
/// </summary>
public enum GravitaAlert
{
    Bassa = 1,
    Media = 2,
    Alta = 3
}

/// <summary>
/// Riepilogo totale di tutte le banche
/// </summary>
public class RiepilogoBanche
{
    public decimal SaldoTotale { get; set; }
    public decimal FidoTotaleAccordato { get; set; }
    public decimal FidoTotaleUtilizzato { get; set; }
    public decimal AnticipoTotaleMassimo { get; set; }
    public decimal AnticipoTotaleUtilizzato { get; set; }
    public decimal InteressiTotaliMaturati { get; set; }
    public int NumeroBanche { get; set; }
    
    public decimal FidoTotaleResiduo => FidoTotaleAccordato - FidoTotaleUtilizzato;
    public decimal AnticipoTotaleResiduo => AnticipoTotaleMassimo - AnticipoTotaleUtilizzato;
    public decimal PercentualeUtilizzoFido => FidoTotaleAccordato > 0 
        ? (FidoTotaleUtilizzato / FidoTotaleAccordato) * 100 
        : 0;
}

#endregion


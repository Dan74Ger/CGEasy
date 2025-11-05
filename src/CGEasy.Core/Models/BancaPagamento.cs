using System;

namespace CGEasy.Core.Models;

/// <summary>
/// Rappresenta un pagamento a fornitore per una banca
/// </summary>
public class BancaPagamento
{
    public int Id { get; set; }
    
    /// <summary>
    /// ID della banca di riferimento
    /// </summary>
    public int BancaId { get; set; }
    
    /// <summary>
    /// Nome del fornitore/destinatario
    /// </summary>
    public string NomeFornitore { get; set; } = string.Empty;
    
    /// <summary>
    /// Anno di riferimento
    /// </summary>
    public int Anno { get; set; }
    
    /// <summary>
    /// Mese di riferimento (1-12)
    /// </summary>
    public int Mese { get; set; }
    
    /// <summary>
    /// Importo del pagamento (fattura)
    /// </summary>
    public decimal Importo { get; set; }
    
    /// <summary>
    /// Percentuale di anticipo richiesta (0-100)
    /// </summary>
    public decimal PercentualeAnticipo { get; set; }
    
    /// <summary>
    /// Importo anticipato calcolato (Importo * PercentualeAnticipo / 100)
    /// </summary>
    public decimal ImportoAnticipato => Importo * (PercentualeAnticipo / 100);
    
    /// <summary>
    /// Data inizio anticipo
    /// </summary>
    public DateTime? DataInizioAnticipo { get; set; }
    
    /// <summary>
    /// Data scadenza anticipo
    /// </summary>
    public DateTime? DataScadenzaAnticipo { get; set; }
    
    /// <summary>
    /// Importo fattura a scadenza (Importo - ImportoAnticipato)
    /// </summary>
    public decimal ImportoFatturaScadenza => Importo - ImportoAnticipato;
    
    /// <summary>
    /// Flag che indica se il pagamento è stato effettuato
    /// </summary>
    public bool Pagato { get; set; }
    
    /// <summary>
    /// Data di scadenza prevista per il pagamento (fattura)
    /// </summary>
    public DateTime DataScadenza { get; set; }
    
    /// <summary>
    /// Data effettiva di pagamento (se avvenuto)
    /// </summary>
    public DateTime? DataPagamentoEffettivo { get; set; }
    
    /// <summary>
    /// Numero fattura fornitore (opzionale)
    /// </summary>
    public string? NumeroFatturaFornitore { get; set; }
    
    /// <summary>
    /// Data fattura fornitore (opzionale)
    /// </summary>
    public DateTime? DataFatturaFornitore { get; set; }
    
    /// <summary>
    /// Note aggiuntive
    /// </summary>
    public string? Note { get; set; }
    
    /// <summary>
    /// Data creazione record
    /// </summary>
    public DateTime DataCreazione { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Data ultima modifica
    /// </summary>
    public DateTime DataUltimaModifica { get; set; } = DateTime.Now;
}


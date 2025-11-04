using System;

namespace CGEasy.Core.Models;

/// <summary>
/// Rappresenta un utilizzo di anticipo fatture/SBF per una banca
/// </summary>
public class BancaUtilizzoAnticipo
{
    public int Id { get; set; }
    
    /// <summary>
    /// ID della banca di riferimento
    /// </summary>
    public int BancaId { get; set; }
    
    /// <summary>
    /// Importo dell'anticipo utilizzato
    /// </summary>
    public decimal ImportoUtilizzo { get; set; }
    
    /// <summary>
    /// Data inizio utilizzo anticipo
    /// </summary>
    public DateTime DataInizioUtilizzo { get; set; }
    
    /// <summary>
    /// Data scadenza/rimborso anticipo
    /// </summary>
    public DateTime DataScadenzaUtilizzo { get; set; }
    
    /// <summary>
    /// Flag che indica se l'anticipo è stato rimborsato
    /// </summary>
    public bool Rimborsato { get; set; }
    
    /// <summary>
    /// Data effettiva di rimborso (se avvenuto)
    /// </summary>
    public DateTime? DataRimborsoEffettivo { get; set; }
    
    /// <summary>
    /// Interessi maturati sul periodo (calcolato)
    /// </summary>
    public decimal InteressiMaturati { get; set; }
    
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


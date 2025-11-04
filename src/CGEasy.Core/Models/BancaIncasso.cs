using System;

namespace CGEasy.Core.Models;

/// <summary>
/// Rappresenta un incasso da cliente per una banca
/// </summary>
public class BancaIncasso
{
    public int Id { get; set; }
    
    /// <summary>
    /// ID della banca di riferimento
    /// </summary>
    public int BancaId { get; set; }
    
    /// <summary>
    /// Nome del cliente
    /// </summary>
    public string NomeCliente { get; set; } = string.Empty;
    
    /// <summary>
    /// Anno di riferimento
    /// </summary>
    public int Anno { get; set; }
    
    /// <summary>
    /// Mese di riferimento (1-12)
    /// </summary>
    public int Mese { get; set; }
    
    /// <summary>
    /// Importo dell'incasso
    /// </summary>
    public decimal Importo { get; set; }
    
    /// <summary>
    /// Flag che indica se l'importo è stato incassato
    /// </summary>
    public bool Incassato { get; set; }
    
    /// <summary>
    /// Data di scadenza prevista per l'incasso
    /// </summary>
    public DateTime DataScadenza { get; set; }
    
    /// <summary>
    /// Data effettiva di incasso (se avvenuto)
    /// </summary>
    public DateTime? DataIncassoEffettivo { get; set; }
    
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


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
    /// Importo del pagamento
    /// </summary>
    public decimal Importo { get; set; }
    
    /// <summary>
    /// Flag che indica se il pagamento è stato effettuato
    /// </summary>
    public bool Pagato { get; set; }
    
    /// <summary>
    /// Data di scadenza prevista per il pagamento
    /// </summary>
    public DateTime DataScadenza { get; set; }
    
    /// <summary>
    /// Data effettiva di pagamento (se avvenuto)
    /// </summary>
    public DateTime? DataPagamentoEffettivo { get; set; }
    
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


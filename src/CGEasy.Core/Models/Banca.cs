using System;

namespace CGEasy.Core.Models;

/// <summary>
/// Rappresenta una banca gestita nel sistema
/// </summary>
public class Banca
{
    public int Id { get; set; }
    
    /// <summary>
    /// Nome della banca
    /// </summary>
    public string NomeBanca { get; set; } = string.Empty;
    
    /// <summary>
    /// Codice identificativo della banca (opzionale)
    /// </summary>
    public string? CodiceIdentificativo { get; set; }
    
    /// <summary>
    /// IBAN del conto corrente (opzionale)
    /// </summary>
    public string? IBAN { get; set; }
    
    /// <summary>
    /// Note aggiuntive (opzionale)
    /// </summary>
    public string? Note { get; set; }
    
    /// <summary>
    /// Saldo corrente del conto
    /// </summary>
    public decimal SaldoDelGiorno { get; set; }
    
    /// <summary>
    /// Fido C/C accordato (massimale)
    /// </summary>
    public decimal FidoCCAccordato { get; set; }
    
    /// <summary>
    /// Anticipo fatture/SBF massimo accordato
    /// </summary>
    public decimal AnticipoFattureMassimo { get; set; }
    
    /// <summary>
    /// Interesse percentuale per anticipo fatture
    /// </summary>
    public decimal InteresseAnticipoFatture { get; set; }
    
    /// <summary>
    /// Data creazione record
    /// </summary>
    public DateTime DataCreazione { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Data ultima modifica
    /// </summary>
    public DateTime DataUltimaModifica { get; set; } = DateTime.Now;
}


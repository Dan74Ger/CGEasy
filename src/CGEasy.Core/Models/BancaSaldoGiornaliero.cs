using System;

namespace CGEasy.Core.Models;

/// <summary>
/// Rappresenta lo storico del saldo giornaliero di una banca
/// </summary>
public class BancaSaldoGiornaliero
{
    public int Id { get; set; }
    
    /// <summary>
    /// ID della banca di riferimento
    /// </summary>
    public int BancaId { get; set; }
    
    /// <summary>
    /// Data del saldo
    /// </summary>
    public DateTime Data { get; set; }
    
    /// <summary>
    /// Saldo alla data specificata
    /// </summary>
    public decimal Saldo { get; set; }
    
    /// <summary>
    /// Note aggiuntive
    /// </summary>
    public string? Note { get; set; }
    
    /// <summary>
    /// Data creazione record
    /// </summary>
    public DateTime DataCreazione { get; set; } = DateTime.Now;
}


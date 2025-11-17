using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Models;

/// <summary>
/// Rappresenta un indice di bilancio calcolato per uno o più periodi
/// </summary>
public class IndiceCalcolato
{
    public string NomeIndice { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty; // "Liquidità", "Solidità", "Redditività", "Efficienza", "Personalizzato"
    public string Formula { get; set; } = string.Empty;
    public string UnitaMisura { get; set; } = string.Empty; // "%", "€", "volte", "giorni", etc.
    
    /// <summary>
    /// Valori calcolati per periodo (chiave: "Mese Anno", es: "Gen 2024")
    /// </summary>
    public Dictionary<string, decimal> ValoriPerPeriodo { get; set; } = new();
    
    /// <summary>
    /// Indica se l'indice è standard (pre-configurato) o personalizzato
    /// </summary>
    public bool IsPersonalizzato { get; set; }
    
    /// <summary>
    /// ID dell'indice personalizzato salvato (se applicabile)
    /// </summary>
    public int? IndicePersonalizzatoId { get; set; }
    
    /// <summary>
    /// Restituisce il primo valore disponibile (per visualizzazione singola)
    /// </summary>
    public decimal? ValorePrimoPeriodo => ValoriPerPeriodo.Values.FirstOrDefault();
    
    /// <summary>
    /// Restituisce il valore formattato con unità di misura
    /// </summary>
    public string ValoreFormattato
    {
        get
        {
            if (ValoriPerPeriodo.Count == 0) return "-";
            var valore = ValoriPerPeriodo.Values.First();
            return UnitaMisura == "%" 
                ? $"{valore:N2} {UnitaMisura}" 
                : $"{valore:N2} {UnitaMisura}";
        }
    }
}


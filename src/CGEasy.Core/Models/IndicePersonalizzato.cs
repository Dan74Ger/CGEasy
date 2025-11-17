using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CGEasy.Core.Models;

/// <summary>
/// Rappresenta un indice personalizzato creato dall'utente e salvato nel database
/// </summary>
public class IndicePersonalizzato : INotifyPropertyChanged
{
    [BsonId(true)]
    public int Id { get; set; }
    
    public int ClienteId { get; set; }
    public string NomeIndice { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    
    /// <summary>
    /// Operatore matematico: "divisione", "moltiplicazione", "somma", "sottrazione"
    /// </summary>
    public string Operatore { get; set; } = "divisione";
    
    /// <summary>
    /// Moltiplicatore finale (es: 100 per percentuali)
    /// </summary>
    public decimal Moltiplicatore { get; set; } = 1;
    
    /// <summary>
    /// Unità di misura del risultato
    /// </summary>
    public string UnitaMisura { get; set; } = string.Empty;
    
    /// <summary>
    /// Codici delle voci del numeratore (da CE o SP)
    /// Es: ["FATTURATO", "TOTALE ATTIVITA"]
    /// </summary>
    public List<string> CodiciNumeratore { get; set; } = new();
    
    /// <summary>
    /// Codici delle voci del denominatore (da CE o SP)
    /// Es: ["PATRIMONIO NETTO", "DEBITI CORRENTI"]
    /// </summary>
    public List<string> CodiciDenominatore { get; set; } = new();
    
    /// <summary>
    /// Indica se sommare o moltiplicare le voci del numeratore
    /// "somma" o "prodotto"
    /// </summary>
    public string OperazioneNumeratore { get; set; } = "somma";
    
    /// <summary>
    /// Indica se sommare o moltiplicare le voci del denominatore
    /// "somma" o "prodotto"
    /// </summary>
    public string OperazioneDenominatore { get; set; } = "somma";
    
    public DateTime DataCreazione { get; set; } = DateTime.Now;
    public DateTime? DataUltimaModifica { get; set; }
    
    /// <summary>
    /// Se true, l'indice è attivo e verrà mostrato
    /// </summary>
    public bool Attivo { get; set; } = true;
    
    private bool _abilitato = true;
    /// <summary>
    /// Se true, l'indice è abilitato per il calcolo
    /// </summary>
    [BsonField("Abilitato")]
    public bool Abilitato
    {
        get => _abilitato;
        set
        {
            if (_abilitato != value)
            {
                _abilitato = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


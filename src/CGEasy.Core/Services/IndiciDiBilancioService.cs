using CGEasy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Services;

/// <summary>
/// Servizio per calcolo indici di bilancio standard e personalizzati
/// </summary>
public class IndiciDiBilancioService
{
    /// <summary>
    /// Definizioni degli indici standard pre-configurati (accessibili pubblicamente)
    /// </summary>
    public static readonly List<IndiceStandardDefinition> IndiciStandardDefinitions = new()
    {
        // ========== INDICI DI LIQUIDITÀ ==========
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "LIQ_CORRENTE",
            Nome = "Liquidità Corrente",
            Categoria = "Liquidità",
            Formula = "Attività Correnti / Passività Correnti",
            UnitaMisura = "volte",
            CodiciNumeratore = new List<string> { "TOTALE ATTIVITA CORRENTI", "ATTIVO CORRENTE" },
            CodiciDenominatore = new List<string> { "TOTALE PASSIVITA CORRENTI", "PASSIVO CORRENTE", "DEBITI A BREVE" },
            Descrizione = "Misura la capacità dell'azienda di far fronte ai debiti a breve termine. Valore ottimale: 1.5-2"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "LIQ_IMMEDIATA",
            Nome = "Liquidità Immediata",
            Categoria = "Liquidità",
            Formula = "(Liq. Immediate + Liq. Differite) / Passività Correnti",
            UnitaMisura = "volte",
            CodiciNumeratore = new List<string> { "LIQUIDITA IMMEDIATE", "LIQUIDITA DIFFERITE", "DISPONIBILITA LIQUIDE" },
            CodiciDenominatore = new List<string> { "TOTALE PASSIVITA CORRENTI", "PASSIVO CORRENTE", "DEBITI A BREVE" },
            Descrizione = "Capacità di pagare i debiti immediati senza vendere le rimanenze. Valore ottimale: 0.8-1"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "LIQ_SECCA",
            Nome = "Liquidità Secca (Acid Test)",
            Categoria = "Liquidità",
            Formula = "(Attività Correnti - Rimanenze) / Passività Correnti",
            UnitaMisura = "volte",
            CodiciNumeratore = new List<string> { "TOTALE ATTIVITA CORRENTI", "ATTIVO CORRENTE" },
            CodiciDenominatore = new List<string> { "TOTALE PASSIVITA CORRENTI", "PASSIVO CORRENTE", "DEBITI A BREVE" },
            SottraiDaNumeratore = new List<string> { "RIMANENZE", "MAGAZZINO" },
            Descrizione = "Test di liquidità escludendo le rimanenze (meno liquide). Valore ottimale: 1"
        },

        // ========== INDICI DI SOLIDITÀ ==========
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "IND_FINANZIARIA",
            Nome = "Indipendenza Finanziaria",
            Categoria = "Solidità",
            Formula = "(Patrimonio Netto / Totale Attivo) × 100",
            UnitaMisura = "%",
            CodiciNumeratore = new List<string> { "PATRIMONIO NETTO", "CAPITALE PROPRIO" },
            CodiciDenominatore = new List<string> { "TOTALE ATTIVITA", "TOTALE ATTIVO" },
            Moltiplicatore = 100,
            Descrizione = "Percentuale di attività finanziate con capitale proprio. Valore ottimale: > 30%"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "RAP_INDEBITAMENTO",
            Nome = "Rapporto di Indebitamento",
            Categoria = "Solidità",
            Formula = "Totale Debiti / Patrimonio Netto",
            UnitaMisura = "volte",
            CodiciNumeratore = new List<string> { "TOTALE DEBITI", "TOTALE PASSIVITA" },
            CodiciDenominatore = new List<string> { "PATRIMONIO NETTO", "CAPITALE PROPRIO" },
            Descrizione = "Rapporto tra debiti e capitale proprio. Valore ottimale: < 2"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "COP_IMMOBILIZZI",
            Nome = "Copertura Immobilizzi",
            Categoria = "Solidità",
            Formula = "Patrimonio Netto / Immobilizzazioni",
            UnitaMisura = "volte",
            CodiciNumeratore = new List<string> { "PATRIMONIO NETTO", "CAPITALE PROPRIO" },
            CodiciDenominatore = new List<string> { "IMMOBILIZZAZIONI", "ATTIVO FISSO", "ATTIVITA IMMOBILIZZATE" },
            Descrizione = "Capacità del patrimonio di coprire gli investimenti fissi. Valore ottimale: > 1"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "LEVERAGE",
            Nome = "Leverage",
            Categoria = "Solidità",
            Formula = "Totale Attivo / Patrimonio Netto",
            UnitaMisura = "volte",
            CodiciNumeratore = new List<string> { "TOTALE ATTIVITA", "TOTALE ATTIVO" },
            CodiciDenominatore = new List<string> { "PATRIMONIO NETTO", "CAPITALE PROPRIO" },
            Descrizione = "Leva finanziaria: quante volte il patrimonio 'moltiplica' gli investimenti. Valore ottimale: 2-3"
        },

        // ========== INDICI DI REDDITIVITÀ ==========
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "ROE",
            Nome = "ROE (Return on Equity)",
            Categoria = "Redditività",
            Formula = "(Utile Netto / Patrimonio Netto) × 100",
            UnitaMisura = "%",
            CodiciNumeratore = new List<string> { "UTILE NETTO", "RISULTATO D'ESERCIZIO", "UTILE (PERDITA)" },
            CodiciDenominatore = new List<string> { "PATRIMONIO NETTO", "CAPITALE PROPRIO" },
            Moltiplicatore = 100,
            Descrizione = "Redditività del capitale proprio. Valore ottimale: > 10%"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "ROI",
            Nome = "ROI (Return on Investment)",
            Categoria = "Redditività",
            Formula = "(Utile Operativo / Capitale Investito) × 100",
            UnitaMisura = "%",
            CodiciNumeratore = new List<string> { "RISULTATO OPERATIVO", "EBIT", "REDDITO OPERATIVO" },
            CodiciDenominatore = new List<string> { "TOTALE ATTIVITA", "TOTALE ATTIVO", "CAPITALE INVESTITO" },
            Moltiplicatore = 100,
            Descrizione = "Redditività operativa degli investimenti. Valore ottimale: > 7%"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "ROS",
            Nome = "ROS (Return on Sales)",
            Categoria = "Redditività",
            Formula = "(Utile Operativo / Fatturato) × 100",
            UnitaMisura = "%",
            CodiciNumeratore = new List<string> { "RISULTATO OPERATIVO", "EBIT", "REDDITO OPERATIVO" },
            CodiciDenominatore = new List<string> { "TOTALE FATTURATO", "RICAVI VENDITE", "FATTURATO" },
            Moltiplicatore = 100,
            Descrizione = "Margine operativo sulle vendite. Valore ottimale: > 5%"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "ROA",
            Nome = "ROA (Return on Assets)",
            Categoria = "Redditività",
            Formula = "(Utile Netto / Totale Attivo) × 100",
            UnitaMisura = "%",
            CodiciNumeratore = new List<string> { "UTILE NETTO", "RISULTATO D'ESERCIZIO", "UTILE (PERDITA)" },
            CodiciDenominatore = new List<string> { "TOTALE ATTIVITA", "TOTALE ATTIVO" },
            Moltiplicatore = 100,
            Descrizione = "Redditività degli investimenti totali. Valore ottimale: > 5%"
        },

        // ========== INDICI DI EFFICIENZA ==========
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "ROT_ATTIVO",
            Nome = "Rotazione Attivo",
            Categoria = "Efficienza",
            Formula = "Fatturato / Totale Attivo",
            UnitaMisura = "volte",
            CodiciNumeratore = new List<string> { "TOTALE FATTURATO", "RICAVI VENDITE", "FATTURATO" },
            CodiciDenominatore = new List<string> { "TOTALE ATTIVITA", "TOTALE ATTIVO" },
            Descrizione = "Quante volte le attività 'ruotano' in un anno. Valore ottimale: > 1"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "ROT_CREDITI",
            Nome = "Rotazione Crediti",
            Categoria = "Efficienza",
            Formula = "Fatturato / Crediti Commerciali",
            UnitaMisura = "volte",
            CodiciNumeratore = new List<string> { "TOTALE FATTURATO", "RICAVI VENDITE", "FATTURATO" },
            CodiciDenominatore = new List<string> { "CREDITI COMMERCIALI", "CREDITI VERSO CLIENTI", "CREDITI" },
            Descrizione = "Velocità di incasso dei crediti. Valore più alto = meglio"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "DURATA_CREDITI",
            Nome = "Durata Media Crediti",
            Categoria = "Efficienza",
            Formula = "(Crediti / Fatturato) × 365",
            UnitaMisura = "giorni",
            CodiciNumeratore = new List<string> { "CREDITI COMMERCIALI", "CREDITI VERSO CLIENTI", "CREDITI" },
            CodiciDenominatore = new List<string> { "TOTALE FATTURATO", "RICAVI VENDITE", "FATTURATO" },
            Moltiplicatore = 365,
            Descrizione = "Tempo medio di incasso crediti. Valore ottimale: 60-90 giorni"
        },
        new IndiceStandardDefinition
        {
            CodiceIdentificativo = "ROT_MAGAZZINO",
            Nome = "Rotazione Magazzino",
            Categoria = "Efficienza",
            Formula = "Costo del Venduto / Rimanenze",
            UnitaMisura = "volte",
            CodiciNumeratore = new List<string> { "COSTI OPERATIVI", "COSTO DEL VENDUTO", "COSTI PRODUZIONE" },
            CodiciDenominatore = new List<string> { "RIMANENZE", "MAGAZZINO" },
            Descrizione = "Velocità di rotazione del magazzino. Valore più alto = meglio"
        }
    };

    /// <summary>
    /// Calcola tutti gli indici standard per le statistiche selezionate
    /// </summary>
    public List<IndiceCalcolato> CalcolaIndiciStandard(
        List<StatisticaCESalvata> statisticheCE,
        List<StatisticaSPSalvata> statisticheSP)
    {
        var risultati = new List<IndiceCalcolato>();

        foreach (var defIndice in IndiciStandardDefinitions)
        {
            var indiceCalcolato = new IndiceCalcolato
            {
                NomeIndice = defIndice.Nome,
                Categoria = defIndice.Categoria,
                Formula = defIndice.Formula,
                UnitaMisura = defIndice.UnitaMisura,
                IsPersonalizzato = false
            };

            // Calcola per ogni periodo disponibile
            var periodiDisponibili = OttieniPeriodiComuni(statisticheCE, statisticheSP);
            
            foreach (var periodo in periodiDisponibili)
            {
                var valore = CalcolaIndiceStandard(defIndice, statisticheCE, statisticheSP, periodo);
                if (valore.HasValue)
                {
                    indiceCalcolato.ValoriPerPeriodo[periodo] = valore.Value;
                }
            }

            // Aggiungi solo se ha almeno un valore calcolato
            if (indiceCalcolato.ValoriPerPeriodo.Count > 0)
            {
                risultati.Add(indiceCalcolato);
            }
        }

        return risultati;
    }

    /// <summary>
    /// Calcola un singolo indice standard per un periodo specifico
    /// </summary>
    private decimal? CalcolaIndiceStandard(
        IndiceStandardDefinition def,
        List<StatisticaCESalvata> statisticheCE,
        List<StatisticaSPSalvata> statisticheSP,
        string periodo)
    {
        try
        {
            // Trova i dati per questo periodo
            var datiCE = new List<BilancioStatisticaMultiPeriodo>();
            foreach (var statCE in statisticheCE)
            {
                try
                {
                    if (!string.IsNullOrEmpty(statCE.DatiStatisticheJson))
                    {
                        var deserializzati = System.Text.Json.JsonSerializer.Deserialize<List<BilancioStatisticaMultiPeriodo>>(
                            statCE.DatiStatisticheJson,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );
                        if (deserializzati != null)
                        {
                            datiCE.AddRange(deserializzati);
                        }
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[INDICI] Errore deserializzazione CE: {ex.Message}");
                    continue;
                }
            }

            var datiSP = new List<BilancioStatisticaMultiPeriodo>();
            foreach (var statSP in statisticheSP)
            {
                try
                {
                    if (!string.IsNullOrEmpty(statSP.DatiStatisticheJson))
                    {
                        var deserializzati = System.Text.Json.JsonSerializer.Deserialize<List<BilancioStatisticaMultiPeriodo>>(
                            statSP.DatiStatisticheJson,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );
                        if (deserializzati != null)
                        {
                            datiSP.AddRange(deserializzati);
                        }
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[INDICI] Errore deserializzazione SP: {ex.Message}");
                    continue;
                }
            }

            if (datiCE.Count == 0 && datiSP.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine($"[INDICI] Nessun dato disponibile per calcolo indice {def.Nome}");
                return null;
            }

            // Calcola numeratore
            decimal numeratore = 0;
            foreach (var codice in def.CodiciNumeratore)
            {
                var voce = TrovaVocePerCodice(datiCE, datiSP, codice, periodo);
                if (voce.HasValue)
                {
                    numeratore += voce.Value;
                }
            }

            // Sottrai dal numeratore (es: rimanenze per liquidità secca)
            if (def.SottraiDaNumeratore != null)
            {
                foreach (var codice in def.SottraiDaNumeratore)
                {
                    var voce = TrovaVocePerCodice(datiCE, datiSP, codice, periodo);
                    if (voce.HasValue)
                    {
                        numeratore -= voce.Value;
                    }
                }
            }

            // Calcola denominatore
            decimal denominatore = 0;
            foreach (var codice in def.CodiciDenominatore)
            {
                var voce = TrovaVocePerCodice(datiCE, datiSP, codice, periodo);
                if (voce.HasValue)
                {
                    denominatore += voce.Value;
                }
            }

            // Evita divisione per zero
            if (denominatore == 0)
            {
                return null;
            }

            decimal risultato = (numeratore / denominatore) * def.Moltiplicatore;
            return Math.Round(risultato, 2);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Trova il valore di una voce per chiave univoca (CODICE|DESCRIZIONE) in CE o SP
    /// </summary>
    private decimal? TrovaVocePerCodice(
        List<BilancioStatisticaMultiPeriodo> datiCE,
        List<BilancioStatisticaMultiPeriodo> datiSP,
        string chiaveUnica,
        string periodo)
    {
        // La chiaveUnica è nel formato "CODICE|DESCRIZIONE"
        string codice;
        string? descrizione = null;

        if (chiaveUnica.Contains('|'))
        {
            var parti = chiaveUnica.Split('|', 2);
            codice = parti[0];
            descrizione = parti.Length > 1 ? parti[1] : null;
        }
        else
        {
            codice = chiaveUnica;
        }

        // Cerca in CE
        var voceCE = descrizione != null
            ? datiCE.FirstOrDefault(v => 
                v.Codice.Equals(codice, StringComparison.OrdinalIgnoreCase) &&
                v.Descrizione.Equals(descrizione, StringComparison.OrdinalIgnoreCase))
            : datiCE.FirstOrDefault(v => 
                v.Codice.Equals(codice, StringComparison.OrdinalIgnoreCase));
        
        if (voceCE != null && voceCE.DatiPerPeriodo.Count > 0)
        {
            // SOLUZIONE: Prendi il PRIMO valore disponibile, indipendentemente dalla chiave
            var primoValore = voceCE.DatiPerPeriodo.First().Value;
            return primoValore.Importo;
        }

        // Cerca in SP
        var voceSP = descrizione != null
            ? datiSP.FirstOrDefault(v =>
                v.Codice.Equals(codice, StringComparison.OrdinalIgnoreCase) &&
                v.Descrizione.Equals(descrizione, StringComparison.OrdinalIgnoreCase))
            : datiSP.FirstOrDefault(v =>
                v.Codice.Equals(codice, StringComparison.OrdinalIgnoreCase));

        if (voceSP != null && voceSP.DatiPerPeriodo.Count > 0)
        {
            // SOLUZIONE: Prendi il PRIMO valore disponibile
            var primoValore = voceSP.DatiPerPeriodo.First().Value;
            return primoValore.Importo;
        }

        return null;
    }

    /// <summary>
    /// Ottiene i periodi comuni tra CE e SP
    /// </summary>
    private List<string> OttieniPeriodiComuni(
        List<StatisticaCESalvata> statisticheCE,
        List<StatisticaSPSalvata> statisticheSP)
    {
        var periodiCE = new HashSet<string>();
        var periodiSP = new HashSet<string>();

        System.Diagnostics.Debug.WriteLine($"[INDICI] === OTTENGO PERIODI COMUNI ===");
        System.Diagnostics.Debug.WriteLine($"[INDICI] Statistiche CE: {statisticheCE.Count}");
        System.Diagnostics.Debug.WriteLine($"[INDICI] Statistiche SP: {statisticheSP.Count}");

        foreach (var stat in statisticheCE)
        {
            System.Diagnostics.Debug.WriteLine($"[INDICI] CE - PeriodiJson: {stat.PeriodiJson}");
            try
            {
                // Prova prima come List<string>
                var periodiStr = System.Text.Json.JsonSerializer.Deserialize<List<string>>(stat.PeriodiJson);
                if (periodiStr != null && periodiStr.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[INDICI] CE - Periodi come stringhe: {periodiStr.Count} -> [{string.Join(", ", periodiStr)}]");
                    foreach (var periodo in periodiStr)
                    {
                        periodiCE.Add(periodo);
                    }
                    continue;
                }
            }
            catch { }

            try
            {
                // Altrimenti prova come List<oggetti con Mese/Anno>
                var periodiObj = System.Text.Json.JsonSerializer.Deserialize<List<System.Text.Json.JsonElement>>(stat.PeriodiJson);
                if (periodiObj != null)
                {
                    foreach (var obj in periodiObj)
                    {
                        if (obj.TryGetProperty("Mese", out var mese) && obj.TryGetProperty("Anno", out var anno))
                        {
                            var meseNum = mese.GetInt32();
                            var annoNum = anno.GetInt32();
                            var periodoStr = ConvertMeseAnnoToString(meseNum, annoNum);
                            System.Diagnostics.Debug.WriteLine($"[INDICI] CE - Convertito: Mese={meseNum}, Anno={annoNum} -> '{periodoStr}'");
                            periodiCE.Add(periodoStr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[INDICI] ❌ ERRORE deserializzazione CE: {ex.Message}");
            }
        }

        foreach (var stat in statisticheSP)
        {
            System.Diagnostics.Debug.WriteLine($"[INDICI] SP - PeriodiJson: {stat.PeriodiJson}");
            try
            {
                // Prova prima come List<string>
                var periodiStr = System.Text.Json.JsonSerializer.Deserialize<List<string>>(stat.PeriodiJson);
                if (periodiStr != null && periodiStr.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[INDICI] SP - Periodi come stringhe: {periodiStr.Count} -> [{string.Join(", ", periodiStr)}]");
                    foreach (var periodo in periodiStr)
                    {
                        periodiSP.Add(periodo);
                    }
                    continue;
                }
            }
            catch { }

            try
            {
                // Altrimenti prova come List<oggetti con Mese/Anno>
                var periodiObj = System.Text.Json.JsonSerializer.Deserialize<List<System.Text.Json.JsonElement>>(stat.PeriodiJson);
                if (periodiObj != null)
                {
                    foreach (var obj in periodiObj)
                    {
                        if (obj.TryGetProperty("Mese", out var mese) && obj.TryGetProperty("Anno", out var anno))
                        {
                            var meseNum = mese.GetInt32();
                            var annoNum = anno.GetInt32();
                            var periodoStr = ConvertMeseAnnoToString(meseNum, annoNum);
                            System.Diagnostics.Debug.WriteLine($"[INDICI] SP - Convertito: Mese={meseNum}, Anno={annoNum} -> '{periodoStr}'");
                            periodiSP.Add(periodoStr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[INDICI] ❌ ERRORE deserializzazione SP: {ex.Message}");
            }
        }

        System.Diagnostics.Debug.WriteLine($"[INDICI] Periodi CE totali: {periodiCE.Count}");
        System.Diagnostics.Debug.WriteLine($"[INDICI] Periodi SP totali: {periodiSP.Count}");

        // Intersezione dei periodi
        var comuni = periodiCE.Intersect(periodiSP)
            .OrderBy(p => ConvertPeriodoToDate(p))
            .ToList();

        System.Diagnostics.Debug.WriteLine($"[INDICI] Periodi COMUNI: {comuni.Count} -> [{string.Join(", ", comuni)}]");
        return comuni;
    }

    private string ConvertMeseAnnoToString(int mese, int anno)
    {
        var mesiIta = new[] { "", "gen", "feb", "mar", "apr", "mag", "giu", "lug", "ago", "set", "ott", "nov", "dic" };
        return $"{mesiIta[mese]} {anno}";
    }

    private DateTime ConvertPeriodoToDate(string periodo)
    {
        try
        {
            var parts = periodo.Split(' ');
            if (parts.Length != 2) return DateTime.MinValue;

            var mesi = new Dictionary<string, int>
            {
                {"Gen", 1}, {"Feb", 2}, {"Mar", 3}, {"Apr", 4},
                {"Mag", 5}, {"Giu", 6}, {"Lug", 7}, {"Ago", 8},
                {"Set", 9}, {"Ott", 10}, {"Nov", 11}, {"Dic", 12}
            };

            if (int.TryParse(parts[1], out int anno) && mesi.TryGetValue(parts[0], out int mese))
            {
                return new DateTime(anno, mese, 1);
            }
        }
        catch { }
        
        return DateTime.MinValue;
    }

    /// <summary>
    /// Calcola un indice personalizzato
    /// </summary>
    public IndiceCalcolato? CalcolaIndicePersonalizzato(
        IndicePersonalizzato indice,
        List<StatisticaCESalvata> statisticheCE,
        List<StatisticaSPSalvata> statisticheSP)
    {
        var risultato = new IndiceCalcolato
        {
            NomeIndice = indice.NomeIndice,
            Categoria = "Personalizzato",
            Formula = $"({string.Join(" + ", indice.CodiciNumeratore)}) {indice.Operatore} ({string.Join(" + ", indice.CodiciDenominatore)})",
            UnitaMisura = indice.UnitaMisura,
            IsPersonalizzato = true,
            IndicePersonalizzatoId = indice.Id
        };

        var periodiDisponibili = OttieniPeriodiComuni(statisticheCE, statisticheSP);
        
        foreach (var periodo in periodiDisponibili)
        {
            var valore = CalcolaValorePersonalizzato(indice, statisticheCE, statisticheSP, periodo);
            if (valore.HasValue)
            {
                risultato.ValoriPerPeriodo[periodo] = valore.Value;
            }
        }

        return risultato.ValoriPerPeriodo.Count > 0 ? risultato : null;
    }

    private decimal? CalcolaValorePersonalizzato(
        IndicePersonalizzato indice,
        List<StatisticaCESalvata> statisticheCE,
        List<StatisticaSPSalvata> statisticheSP,
        string periodo)
    {
        try
        {
            var datiCE = new List<BilancioStatisticaMultiPeriodo>();
            foreach (var statCE in statisticheCE)
            {
                try
                {
                    if (!string.IsNullOrEmpty(statCE.DatiStatisticheJson))
                    {
                        var deserializzati = System.Text.Json.JsonSerializer.Deserialize<List<BilancioStatisticaMultiPeriodo>>(
                            statCE.DatiStatisticheJson,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );
                        if (deserializzati != null) datiCE.AddRange(deserializzati);
                    }
                }
                catch { continue; }
            }

            var datiSP = new List<BilancioStatisticaMultiPeriodo>();
            foreach (var statSP in statisticheSP)
            {
                try
                {
                    if (!string.IsNullOrEmpty(statSP.DatiStatisticheJson))
                    {
                        var deserializzati = System.Text.Json.JsonSerializer.Deserialize<List<BilancioStatisticaMultiPeriodo>>(
                            statSP.DatiStatisticheJson,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );
                        if (deserializzati != null) datiSP.AddRange(deserializzati);
                    }
                }
                catch { continue; }
            }

            // Calcola numeratore
            decimal numeratore = CalcolaEspressionePersonalizzata(
                indice.CodiciNumeratore,
                indice.OperazioneNumeratore,
                datiCE,
                datiSP,
                periodo);

            // Calcola denominatore
            decimal denominatore = CalcolaEspressionePersonalizzata(
                indice.CodiciDenominatore,
                indice.OperazioneDenominatore,
                datiCE,
                datiSP,
                periodo);

            if (denominatore == 0) return null;

            decimal risultato = 0;
            switch (indice.Operatore.ToLower())
            {
                case "divisione":
                    risultato = numeratore / denominatore;
                    break;
                case "moltiplicazione":
                    risultato = numeratore * denominatore;
                    break;
                case "somma":
                    risultato = numeratore + denominatore;
                    break;
                case "sottrazione":
                    risultato = numeratore - denominatore;
                    break;
            }

            risultato *= indice.Moltiplicatore;
            return Math.Round(risultato, 2);
        }
        catch
        {
            return null;
        }
    }

    private decimal CalcolaEspressionePersonalizzata(
        List<string> codici,
        string operazione,
        List<BilancioStatisticaMultiPeriodo> datiCE,
        List<BilancioStatisticaMultiPeriodo> datiSP,
        string periodo)
    {
        decimal risultato = operazione == "somma" ? 0 : 1;

        foreach (var codice in codici)
        {
            var valore = TrovaVocePerCodice(datiCE, datiSP, codice, periodo);
            if (valore.HasValue)
            {
                if (operazione == "somma")
                    risultato += valore.Value;
                else
                    risultato *= valore.Value;
            }
        }

        return risultato;
    }
}

/// <summary>
/// Definizione di un indice standard pre-configurato
/// </summary>
public class IndiceStandardDefinition
{
    public string CodiceIdentificativo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Formula { get; set; } = string.Empty;
    public string UnitaMisura { get; set; } = string.Empty;
    public List<string> CodiciNumeratore { get; set; } = new();
    public List<string> CodiciDenominatore { get; set; } = new();
    public List<string>? SottraiDaNumeratore { get; set; }
    public decimal Moltiplicatore { get; set; } = 1;
    public string Descrizione { get; set; } = string.Empty;
}


# 📊 MODULO CONTROLLO DI GESTIONE - SPECIFICHE COMPLETE

## 🎯 OBIETTIVO
Creare un modulo completo per la gestione del **Margine di Tesoreria** con monitoraggio dei flussi di cassa mensili, previsioni budget, gestione conti correnti bancari e controllo delle scadenze.

---

## 📐 ARCHITETTURA DEL MODULO

Il modulo è composto da **3 SEZIONI PRINCIPALI**:

```
CONTROLLO DI GESTIONE
├── 1. GESTIONE BANCHE (Conti correnti multipli)
├── 2. MARGINE DI TESORERIA (Dashboard principale)
└── 3. DETTAGLIO VOCI (15 pagine di input)
```

---

## 🏦 SEZIONE 1: GESTIONE BANCHE

### 📋 OVERVIEW
Sistema completo per gestire **multipli conti correnti bancari** con:
- Incassi e pagamenti mensili
- Fidi e anticipi fatture
- Scadenziario
- Alert superamento fido
- Calcolo interessi

---

### 🗂️ MODELLI DATABASE

#### **1. Banca (Anagrafica)**
```csharp
public class Banca
{
    public int Id { get; set; }
    public string Nome { get; set; }                    // es: "Intesa Sanpaolo"
    public string? Codice { get; set; }                 // Codice identificativo
    public string? IBAN { get; set; }
    public string? Note { get; set; }
    public bool Attiva { get; set; }                    // true = in uso
    
    // DATI FIDO
    public decimal FidoCCAccordato { get; set; }        // Massimale fido C/C
    public decimal AnticipoFattureMassimo { get; set; } // Massimale SBF/anticipo
    public decimal InteresseAnticipo { get; set; }      // % interesse annuo
    
    public DateTime DataCreazione { get; set; }
    public int UtenteId { get; set; }
}
```

#### **2. BancaSaldoGiornaliero**
```csharp
public class BancaSaldoGiornaliero
{
    public int Id { get; set; }
    public int BancaId { get; set; }
    public DateTime Data { get; set; }
    public decimal Saldo { get; set; }                  // Saldo del giorno
    public string? Note { get; set; }
    public DateTime DataInserimento { get; set; }
    public int UtenteId { get; set; }
}
```

#### **3. BancaIncasso**
```csharp
public class BancaIncasso
{
    public int Id { get; set; }
    public int BancaId { get; set; }
    public int Anno { get; set; }
    public string NomeCliente { get; set; }
    public string? Descrizione { get; set; }
    
    // IMPORTI MENSILI (12 mesi)
    public decimal ImportoGennaio { get; set; }
    public decimal ImportoFebbraio { get; set; }
    public decimal ImportoMarzo { get; set; }
    public decimal ImportoAprile { get; set; }
    public decimal ImportoMaggio { get; set; }
    public decimal ImportoGiugno { get; set; }
    public decimal ImportoLuglio { get; set; }
    public decimal ImportoAgosto { get; set; }
    public decimal ImportoSettembre { get; set; }
    public decimal ImportoOttobre { get; set; }
    public decimal ImportoNovembre { get; set; }
    public decimal ImportoDicembre { get; set; }
    
    // FLAG INCASSO (12 mesi)
    public bool IncassatoGennaio { get; set; }
    public bool IncassatoFebbraio { get; set; }
    public bool IncassatoMarzo { get; set; }
    public bool IncassatoAprile { get; set; }
    public bool IncassatoMaggio { get; set; }
    public bool IncassatoGiugno { get; set; }
    public bool IncassatoLuglio { get; set; }
    public bool IncassatoAgosto { get; set; }
    public bool IncassatoSettembre { get; set; }
    public bool IncassatoOttobre { get; set; }
    public bool IncassatoNovembre { get; set; }
    public bool IncassatoDicembre { get; set; }
    
    // DATE SCADENZA (12 mesi)
    public DateTime? ScadenzaGennaio { get; set; }
    public DateTime? ScadenzaFebbraio { get; set; }
    public DateTime? ScadenzaMarzo { get; set; }
    public DateTime? ScadenzaAprile { get; set; }
    public DateTime? ScadenzaMaggio { get; set; }
    public DateTime? ScadenzaGiugno { get; set; }
    public DateTime? ScadenzaLuglio { get; set; }
    public DateTime? ScadenzaAgosto { get; set; }
    public DateTime? ScadenzaSettembre { get; set; }
    public DateTime? ScadenzaOttobre { get; set; }
    public DateTime? ScadenzaNovembre { get; set; }
    public DateTime? ScadenzaDicembre { get; set; }
    
    public DateTime DataCreazione { get; set; }
    public int UtenteId { get; set; }
}
```

#### **4. BancaPagamento**
```csharp
public class BancaPagamento
{
    public int Id { get; set; }
    public int BancaId { get; set; }
    public int Anno { get; set; }
    public string NomeFornitore { get; set; }
    public string? Descrizione { get; set; }
    
    // IMPORTI MENSILI (12 mesi)
    public decimal ImportoGennaio { get; set; }
    public decimal ImportoFebbraio { get; set; }
    public decimal ImportoMarzo { get; set; }
    public decimal ImportoAprile { get; set; }
    public decimal ImportoMaggio { get; set; }
    public decimal ImportoGiugno { get; set; }
    public decimal ImportoLuglio { get; set; }
    public decimal ImportoAgosto { get; set; }
    public decimal ImportoSettembre { get; set; }
    public decimal ImportoOttobre { get; set; }
    public decimal ImportoNovembre { get; set; }
    public decimal ImportoDicembre { get; set; }
    
    // FLAG PAGAMENTO (12 mesi)
    public bool PagatoGennaio { get; set; }
    public bool PagatoFebbraio { get; set; }
    public bool PagatoMarzo { get; set; }
    public bool PagatoAprile { get; set; }
    public bool PagatoMaggio { get; set; }
    public bool PagatoGiugno { get; set; }
    public bool PagatoLuglio { get; set; }
    public bool PagatoAgosto { get; set; }
    public bool PagatoSettembre { get; set; }
    public bool PagatoOttobre { get; set; }
    public bool PagatoNovembre { get; set; }
    public bool PagatoDicembre { get; set; }
    
    // DATE SCADENZA (12 mesi)
    public DateTime? ScadenzaGennaio { get; set; }
    public DateTime? ScadenzaFebbraio { get; set; }
    public DateTime? ScadenzaMarzo { get; set; }
    public DateTime? ScadenzaAprile { get; set; }
    public DateTime? ScadenzaMaggio { get; set; }
    public DateTime? ScadenzaGiugno { get; set; }
    public DateTime? ScadenzaLuglio { get; set; }
    public DateTime? ScadenzaAgosto { get; set; }
    public DateTime? ScadenzaSettembre { get; set; }
    public DateTime? ScadenzaOttobre { get; set; }
    public DateTime? ScadenzaNovembre { get; set; }
    public DateTime? ScadenzaDicembre { get; set; }
    
    public DateTime DataCreazione { get; set; }
    public int UtenteId { get; set; }
}
```

#### **5. BancaUtilizzoAnticipo**
```csharp
public class BancaUtilizzoAnticipo
{
    public int Id { get; set; }
    public int BancaId { get; set; }
    public decimal ImportoUtilizzo { get; set; }
    public DateTime DataInizio { get; set; }
    public DateTime DataScadenza { get; set; }
    public decimal InteresseApplicato { get; set; }     // % interesse
    public bool Chiuso { get; set; }                    // true = rimborsato
    public DateTime? DataChiusura { get; set; }
    public string? Note { get; set; }
    public DateTime DataCreazione { get; set; }
    public int UtenteId { get; set; }
}
```

---

### 🔧 FUNZIONALITÀ BANCHE

#### **1. ANAGRAFICA BANCHE**
- CRUD completo banche
- Gestione dati fido e anticipi
- Attivazione/Disattivazione banca

#### **2. GESTIONE INCASSI**
- Inserimento incassi previsti da clienti
- Importi mensili (Gen-Dic)
- Flag incasso effettuato (SI/NO)
- Data scadenza per ogni mese
- CRUD completo

#### **3. GESTIONE PAGAMENTI**
- Inserimento pagamenti previsti a fornitori
- Importi mensili (Gen-Dic)
- Flag pagamento effettuato (SI/NO)
- Data scadenza per ogni mese
- CRUD completo

#### **4. SALDI GIORNALIERI**
- Inserimento saldo del giorno
- Storico saldi per data
- Visualizzazione grafico andamento

#### **5. UTILIZZO ANTICIPI/SBF**
- Registrazione utilizzo anticipo fatture
- Periodo utilizzo (data inizio/fine)
- **Calcolo automatico**:
  - Residuo disponibile = Fido massimo - Utilizzo corrente
  - Interessi maturati
  - **ALERT** se superamento fido
  
**Esempio calcolo:**
```
Fido massimo anticipo: 100.000 €
Utilizzo corrente:      50.000 € (dal 01/01 al 28/02)
Residuo disponibile:    50.000 €
Interessi (5% annuo):      416 € (2 mesi)
```

#### **6. SCADENZIARIO BANCA**
Vista consolidata per banca:
- Incassi in scadenza (prossimi 30/60/90 giorni)
- Pagamenti in scadenza
- Ordinamento per data
- Filtro per stato (incassato/pagato SI/NO)

#### **7. RIEPILOGO TOTALE BANCHE**
Vista consolidata di **tutte le banche**:
- Saldo totale (somma saldi tutte le banche)
- Fido totale utilizzato
- Scadenziario consolidato
- Utilizzo anticipi totale

---

### 🎨 UI BANCHE

#### **Pagina 1: Lista Banche**
```
┌─────────────────────────────────────────────────┐
│  🏦 GESTIONE BANCHE                             │
├─────────────────────────────────────────────────┤
│  [+ Nuova Banca]                                │
│                                                  │
│  📊 Riepilogo Totale:                           │
│  Saldo totale:     150.000 €                    │
│  Fido utilizzato:   75.000 €                    │
│  [📊 Vedi Riepilogo Completo]                   │
│                                                  │
│  ┌────────────────────────────────────────────┐│
│  │ Banca              Saldo    Fido    Azioni ││
│  ├────────────────────────────────────────────┤│
│  │ Intesa Sanpaolo   50.000€  30.000€ [Apri] ││
│  │ UniCredit         80.000€  45.000€ [Apri] ││
│  │ Banco BPM         20.000€       0€ [Apri] ││
│  └────────────────────────────────────────────┘│
└─────────────────────────────────────────────────┘
```

#### **Pagina 2: Dettaglio Banca (TabControl)**
```
┌─────────────────────────────────────────────────┐
│  🏦 Intesa Sanpaolo                             │
├─────────────────────────────────────────────────┤
│  [Dati Generali] [Incassi] [Pagamenti]         │
│  [Utilizzo Anticipi] [Scadenziario] [Saldi]    │
│                                                  │
│  TAB ATTIVO: Dati Generali                      │
│  ┌────────────────────────────────────────────┐│
│  │ Nome:          [Intesa Sanpaolo]           ││
│  │ IBAN:          [IT60X0542811101...]        ││
│  │ Fido C/C:      [100.000] €                 ││
│  │ Anticipo Max:  [50.000] €                  ││
│  │ Interesse:     [5,5] %                     ││
│  │                                             ││
│  │ Saldo corrente: 50.000 €                   ││
│  │ Fido utilizzato: 30.000 €                  ││
│  │ Residuo: 20.000 € ✅                       ││
│  │                                             ││
│  │ [Salva] [Annulla]                          ││
│  └────────────────────────────────────────────┘│
└─────────────────────────────────────────────────┘
```

#### **Tab: Incassi/Pagamenti**
```
┌─────────────────────────────────────────────────┐
│  📥 INCASSI DA CLIENTI - Anno 2025              │
├─────────────────────────────────────────────────┤
│  [+ Nuovo Incasso]                              │
│                                                  │
│  DataGrid con colonne:                          │
│  Cliente | Gen | Feb | Mar | ... | Dic | Azioni│
│  ────────────────────────────────────────────── │
│  ACME    |✅ 10k|⏳ 5k|✅ 8k| ... |⏳ 12k|[Edit] │
│           |01/01|15/02|05/03|     |20/12|[Del] │
│  ────────────────────────────────────────────── │
│  Beta    |⏳ 3k |✅ 4k|⏳ 2k | ... |✅ 6k |[Edit] │
│           |10/01|28/02|15/03|     |30/12|[Del] │
│                                                  │
│  Legenda: ✅=Incassato ⏳=In attesa              │
└─────────────────────────────────────────────────┘
```

---

## 💰 SEZIONE 2: MARGINE DI TESORERIA

### 📋 OVERVIEW
Dashboard principale con calcolo automatico del margine di tesoreria mensile.

---

### 🗂️ MODELLI DATABASE

#### **1. MargineTesoreríaPeriodo**
```csharp
public class MargineTesoreríaPeriodo
{
    public int Id { get; set; }
    public int Anno { get; set; }
    public int Mese { get; set; }                   // 1-12
    public int ClienteId { get; set; }              // Riferimento cliente (multi-azienda)
    
    // ===== SALDO BANCA =====
    public bool SaldoBancaManuale { get; set; }     // true=manuale, false=da banche
    public decimal SaldoBancaValore { get; set; }   // Saldo iniziale (A0)
    
    // ===== ENTRATE =====
    public decimal ClientiContabili { get; set; }   // A
    public decimal ClientiBudget { get; set; }      // B
    public decimal AltriCrediti { get; set; }       // C
    // TotaleEntrata = A0 + A + B + C (calcolato)
    
    // ===== USCITE =====
    public decimal FornitoriContabili { get; set; } // E
    public decimal FornitoriBudget { get; set; }    // F
    public decimal DebitoDipendenti { get; set; }   // G
    public decimal DebitoTasse { get; set; }        // H
    public decimal DebitoIVA { get; set; }          // I
    public decimal AltriDebiti { get; set; }        // L
    // TotaleUscita = E + F + G + H + I + L (calcolato)
    
    // ===== ARRETRATI =====
    public decimal FornitoriArretrati { get; set; } // P
    public decimal DebitiTributariArretrati { get; set; } // Q
    public decimal AltriDebitiArretrati { get; set; } // R
    // TotaleArretrati = P + Q + R (calcolato)
    
    // ===== INVESTIMENTI =====
    public decimal Investimenti { get; set; }       // INV0
    public decimal Finanziamenti { get; set; }      // FIN0
    public decimal Leasing { get; set; }            // LEA0
    // TotaleInvestimenti = INV0 - FIN0 - LEA0 (calcolato)
    
    // ===== CALCOLI =====
    public decimal FlussoRiportoPrecedente { get; set; } // N0 (dal mese prec.)
    
    public DateTime DataCreazione { get; set; }
    public DateTime? DataModifica { get; set; }
    public int UtenteId { get; set; }
}
```

---

### 📐 FORMULE DI CALCOLO

#### **SEZIONE ENTRATE**
```
D (Totale Entrata) = A0 + A + B + C
```

#### **SEZIONE USCITE**
```
M (Totale Uscita) = E + F + G + H + I + L
```

#### **FLUSSO OPERATIVO**
```
N (Flusso Operativo) = D - M
O (Flusso Cumulato) = N0 + N
```

#### **ARRETRATI**
```
S (Totale Arretrati) = P + Q + R
T (Flusso Operativo Netto) = O - S
```

#### **INVESTIMENTI**
```
INV1 (Totale Investimenti) = INV0 - FIN0 - LEA0
```

#### **RISULTATO FINALE**
```
Z (Margine Finale) = N + N0 - S - INV1
```

---

### 🎨 UI MARGINE TESORERIA

```
┌───────────────────────────────────────────────────────────────┐
│  📊 MARGINE DI TESORERIA - Anno 2025                          │
├───────────────────────────────────────────────────────────────┤
│  Cliente: [Seleziona Cliente ▼]    Anno: [2025 ▼]            │
│                                                                │
│  ┌──────────────────────────────────────────────────────────┐│
│  │  🏦 SALDO BANCA INIZIALE (A0)                            ││
│  │  ┌────────────────────────────────────────────────────┐ ││
│  │  │ Modalità: ⚪ Manuale  🔘 Automatico (da Banche)   │ ││
│  │  │                                                     │ ││
│  │  │ Gennaio: 50.000 € (calcolato da 3 banche)         │ ││
│  │  │ [📊 Vedi dettaglio banche]                         │ ││
│  │  └────────────────────────────────────────────────────┘ ││
│  └──────────────────────────────────────────────────────────┘│
│                                                                │
│  TABELLA MENSILE:                                             │
│  ┌────────────────────────────────────────────────────────┐  │
│  │Voce         │Gen│Feb│Mar│Apr│Mag│Giu│...│TOTALI│       │  │
│  ├────────────────────────────────────────────────────────┤  │
│  │ENTRATE      │   │   │   │   │   │   │   │      │       │  │
│  │ Saldo Banca │50k│   │   │   │   │   │   │  50k │[Man] │  │
│  │ Cli.Cont.   │150│50 │10 │   │   │   │   │ 210k │[Det] │  │
│  │ Cli.Budget  │20 │90 │130│90 │100│150│   │1380k │[Det] │  │
│  │ Alt.Crediti │   │   │45 │   │   │50 │   │  95k │[Det] │  │
│  │ TOT ENTRATA │220│140│185│90 │100│200│   │1685k │      │  │
│  ├────────────────────────────────────────────────────────┤  │
│  │USCITE       │   │   │   │   │   │   │   │      │       │  │
│  │ Forn.Cont.  │120│45 │25 │   │   │   │   │      │[Det] │  │
│  │ Forn.Budget │10 │90 │120│140│140│140│   │1490k │[Det] │  │
│  │ ...         │   │   │   │   │   │   │   │      │       │  │
│  │ TOT USCITA  │60 │40 │40 │40 │40 │85 │   │ 560k │      │  │
│  ├────────────────────────────────────────────────────────┤  │
│  │RISULTATO    │   │   │   │   │   │   │   │      │       │  │
│  │ Flusso Op.  │160│100│145│50 │60 │115│   │1125k │      │  │
│  │ Margine Net │830│595│605│520│445│560│   │1085k │      │  │
│  └────────────────────────────────────────────────────────┘  │
│                                                                │
│  [📊 Export Excel] [📄 Stampa Report]                        │
└───────────────────────────────────────────────────────────────┘
```

---

### 🔧 FUNZIONALITÀ SALDO BANCA

#### **MODALITÀ MANUALE**
```
┌────────────────────────────────────────┐
│ Modalità: 🔘 Manuale                   │
│                                         │
│ Gennaio:  [___50.000___] €             │
│ Febbraio: [___55.000___] €             │
│ Marzo:    [___60.000___] €             │
│ ...                                     │
│                                         │
│ ⚠️ Modalità manuale attiva              │
│ Il saldo non è sincronizzato           │
│ con le banche                           │
└────────────────────────────────────────┘
```

#### **MODALITÀ AUTOMATICA**
```
┌────────────────────────────────────────┐
│ Modalità: 🔘 Automatico (da Banche)    │
│                                         │
│ Gennaio:  50.000 € (readonly)          │
│           Calcolato da 3 banche        │
│           📊 [Vedi dettaglio]          │
│                                         │
│ ✅ Sincronizzazione attiva              │
│ Il saldo è aggiornato automaticamente  │
│ dalla sezione Banche                   │
└────────────────────────────────────────┘
```

#### **Dialog "Vedi Dettaglio Banche"**
```
┌─────────────────────────────────────────┐
│  🏦 DETTAGLIO SALDO BANCHE              │
│  Periodo: Gennaio 2025                  │
├─────────────────────────────────────────┤
│  Intesa Sanpaolo:       25.000 €        │
│  UniCredit:             15.000 €        │
│  Banco BPM:             10.000 €        │
│  ────────────────────────────────────   │
│  TOTALE:                50.000 €        │
│                                         │
│  Ultimo aggiornamento: 03/11/2025      │
│                                         │
│  [🏦 Vai a Gestione Banche] [Chiudi]   │
└─────────────────────────────────────────┘
```

---

## 📋 SEZIONE 3: DETTAGLIO VOCI (15 PAGINE)

### 📐 STRUTTURA COMUNE

Tutte le pagine di dettaglio seguono lo **stesso template base** con variazioni minime.

---

### 🗂️ MODELLI DATABASE

#### **Template Base: VoceBudget**
```csharp
public class VoceBudget
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int Anno { get; set; }
    public string TipoVoce { get; set; }        // "ClientiBudget", "FornitoriBudget", etc.
    
    // DATI GENERALI
    public string NomeDestinatario { get; set; } // Cliente/Fornitore/Altro
    public DateTime? DataFattura { get; set; }
    public string? NumeroFattura { get; set; }
    public decimal ImportoTotale { get; set; }
    public string? Descrizione { get; set; }
    
    // SCADENZE (percentuali)
    public decimal Percentuale30gg { get; set; }
    public decimal Percentuale60gg { get; set; }
    public decimal Percentuale90gg { get; set; }
    public decimal Percentuale120gg { get; set; }
    public decimal Percentuale150gg { get; set; }
    public decimal Percentuale180gg { get; set; }
    
    // CALCOLI AUTOMATICI (derivati)
    // Importi per scadenza = ImportoTotale * Percentuale / 100
    
    public DateTime DataCreazione { get; set; }
    public int UtenteId { get; set; }
}
```

---

### 🎨 UI DETTAGLIO VOCE (Template)

#### **Esempio: Pagina "Clienti Budget"**

```
┌───────────────────────────────────────────────────────────┐
│  📊 CLIENTI BUDGET - Anno 2025                            │
├───────────────────────────────────────────────────────────┤
│  Cliente: [Seleziona Cliente ▼]                           │
│  [+ Nuovo Cliente Budget]                                 │
│                                                            │
│  ┌──────────────────────────────────────────────────────┐│
│  │ LISTA VOCI BUDGET                                    ││
│  │                                                       ││
│  │ Cliente    │Importo│30gg│60gg│90gg│...│Azioni      ││
│  │────────────────────────────────────────────────────  ││
│  │ ACME Spa   │10.000 │30% │40% │30% │... │[Edit][Del]││
│  │ Beta Srl   │ 5.000 │50% │50% │ 0% │... │[Edit][Del]││
│  │ ...        │       │    │    │    │... │           ││
│  └──────────────────────────────────────────────────────┘│
│                                                            │
│  ┌──────────────────────────────────────────────────────┐│
│  │ TABELLA COMPETENZA MENSILE                           ││
│  │ (Fatturato per data competenza)                      ││
│  │                                                       ││
│  │ Cliente│Gen│Feb│Mar│Apr│Mag│Giu│...│Dic│TOTALE     ││
│  │─────────────────────────────────────────────────────  ││
│  │ ACME   │ 3k│ 0 │ 4k│ 3k│ 0 │ 0 │...│ 0 │ 10.000 €  ││
│  │ Beta   │ 0 │ 5k│ 0 │ 0 │ 0 │ 0 │...│ 0 │  5.000 €  ││
│  │─────────────────────────────────────────────────────  ││
│  │ TOTALE │ 3k│ 5k│ 4k│ 3k│ 0 │ 0 │...│ 0 │ 15.000 €  ││
│  └──────────────────────────────────────────────────────┘│
│                                                            │
│  ┌──────────────────────────────────────────────────────┐│
│  │ TABELLA SCADENZE MENSILI                             ││
│  │ (Incassi per data scadenza)                          ││
│  │                                                       ││
│  │ Cliente│Gen│Feb│Mar│Apr│Mag│Giu│...│Dic│TOTALE     ││
│  │─────────────────────────────────────────────────────  ││
│  │ ACME   │ 1k│ 2k│ 3k│ 2k│ 1k│ 1k│...│ 0 │ 10.000 €  ││
│  │ Beta   │ 2k│ 2k│ 1k│ 0 │ 0 │ 0 │...│ 0 │  5.000 €  ││
│  │─────────────────────────────────────────────────────  ││
│  │ TOTALE │ 3k│ 4k│ 4k│ 2k│ 1k│ 1k│...│ 0 │ 15.000 €  ││
│  └──────────────────────────────────────────────────────┘│
│                                                            │
│  ℹ️ I totali mensili vengono automaticamente riportati   │
│     nella pagina Margine di Tesoreria                     │
│                                                            │
│  [💾 Salva Tutto] [📊 Export Excel]                      │
└───────────────────────────────────────────────────────────┘
```

#### **Dialog Inserimento/Modifica Voce**

```
┌─────────────────────────────────────────┐
│  ✏️ MODIFICA VOCE BUDGET                │
├─────────────────────────────────────────┤
│  Nome Cliente:   [ACME Spa______]       │
│  Data Fattura:   [15/01/2025]           │
│  Num. Fattura:   [FT-001_______]        │
│  Importo Tot:    [10.000____] €         │
│  Descrizione:    [Servizi annuali...]   │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │ RIPARTIZIONE SCADENZE             │ │
│  │                                    │ │
│  │ ⏱️  30 giorni:  [30__] %  = 3.000€ │ │
│  │ ⏱️  60 giorni:  [40__] %  = 4.000€ │ │
│  │ ⏱️  90 giorni:  [30__] %  = 3.000€ │ │
│  │ ⏱️ 120 giorni:  [ 0__] %  =     0€ │ │
│  │ ⏱️ 150 giorni:  [ 0__] %  =     0€ │ │
│  │ ⏱️ 180 giorni:  [ 0__] %  =     0€ │ │
│  │                  ─────────────────  │ │
│  │ TOTALE:         100 % ✅ = 10.000€ │ │
│  └───────────────────────────────────┘ │
│                                         │
│  [💾 Salva] [❌ Annulla]                │
└─────────────────────────────────────────┘
```

---

### 📋 ELENCO COMPLETO 15 PAGINE

| # | Nome Pagina | Tipo | Note |
|---|-------------|------|------|
| 1 | **Clienti Contabili** | Import | Da contabilità esistente |
| 2 | **Clienti Budget** | Template | TEMPLATE BASE |
| 3 | **Altri Crediti** | Template | Come Clienti Budget |
| 4 | **Fornitori Contabili** | Import | Come Clienti Contabili (uscite) |
| 5 | **Fornitori Budget** | Template | Come Clienti Budget (uscite) |
| 6 | **Debito Dipendenti** | Template | Come Clienti Budget (stipendi) |
| 7 | **Debito Tasse** | Template | Come Clienti Budget (imposte) |
| 8 | **Debito IVA** | Template | Come Clienti Budget (IVA) |
| 9 | **Altri Debiti** | Template | Come Clienti Budget (altri) |
| 10 | **Fornitori Arretrati** | Template | Come Clienti Budget (arretrati) |
| 11 | **Debiti Tributari Arretrati** | Template | Come Clienti Budget |
| 12 | **Altri Debiti Arretrati** | Template | Come Clienti Budget |
| 13 | **Investimenti** | Template | Come Clienti Budget |
| 14 | **Finanziamenti** | Template | Come Clienti Budget |
| 15 | **Leasing** | Template | Come Clienti Budget |

---

## 🔧 FUNZIONALITÀ COMUNI

### ✅ CRUD COMPLETO
- **Create**: Inserimento nuove voci
- **Read**: Visualizzazione griglia
- **Update**: Modifica voci esistenti
- **Delete**: Cancellazione voci

### 📊 CALCOLI AUTOMATICI
- Ripartizione importi su scadenze (%)
- Somme mensili automatiche
- Export automatico a Margine Tesoreria
- Validazione: totale percentuali = 100%

### 📈 REPORT
- Tabella competenza mensile
- Tabella scadenze mensili
- Export Excel
- Stampa PDF

### 🔄 RIPORTO ANNO SUCCESSIVO
- Possibilità di copiare budget anno precedente
- Riporto automatico scadenze non incassate

---

## 🗺️ NAVIGAZIONE DEL MODULO

```
MENU PRINCIPALE
└── 📊 Controllo di Gestione
    ├── 🏦 Gestione Banche
    │   ├── Lista Banche
    │   ├── Dettaglio Banca (TabControl)
    │   └── Riepilogo Totale Banche
    │
    ├── 💰 Margine di Tesoreria
    │   └── Dashboard Principale (con Saldo Banca)
    │
    └── 📋 Dettaglio Voci
        ├── 1. Clienti Contabili
        ├── 2. Clienti Budget
        ├── 3. Altri Crediti
        ├── 4. Fornitori Contabili
        ├── 5. Fornitori Budget
        ├── 6. Debito Dipendenti
        ├── 7. Debito Tasse
        ├── 8. Debito IVA
        ├── 9. Altri Debiti
        ├── 10. Fornitori Arretrati
        ├── 11. Debiti Tributari Arretrati
        ├── 12. Altri Debiti Arretrati
        ├── 13. Investimenti
        ├── 14. Finanziamenti
        └── 15. Leasing
```

---

## 🎯 PATTERN ARCHITETTURALI

### 🔗 PATTERN DA SEGUIRE
- **ConnectionType.Shared**: Multi-client con database condiviso
- **Singleton LiteDbContext**: Istanza globale, mai chiusa
- **Repository Pattern**: Repository per ogni entità
- **Service Layer**: Business logic nei Service
- **View-ViewModel Pattern**: MVVM con CommunityToolkit
- **Materializzazione query**: `.ToList()` prima di iterare
- **Batch operations**: `DeleteMany` per eliminazioni multiple
- **Audit logging**: Tracciamento modifiche utente

### 📦 STRUTTURA FILE

```
src/
├── CGEasy.Core/
│   ├── Models/
│   │   ├── Banca.cs
│   │   ├── BancaSaldoGiornaliero.cs
│   │   ├── BancaIncasso.cs
│   │   ├── BancaPagamento.cs
│   │   ├── BancaUtilizzoAnticipo.cs
│   │   ├── MargineTesoreríaPeriodo.cs
│   │   └── VoceBudget.cs
│   │
│   ├── Repositories/
│   │   ├── BancaRepository.cs
│   │   ├── BancaSaldoRepository.cs
│   │   ├── BancaIncassoRepository.cs
│   │   ├── BancaPagamentoRepository.cs
│   │   ├── BancaUtilizzoAnticipoRepository.cs
│   │   ├── MargineTesoreríaRepository.cs
│   │   └── VoceBudgetRepository.cs
│   │
│   └── Services/
│       ├── BancaService.cs
│       ├── MargineTesoreríaService.cs
│       └── VoceBudgetService.cs
│
└── CGEasy.App/
    ├── ViewModels/
    │   ├── GestioneBancheViewModel.cs
    │   ├── BancaDettaglioViewModel.cs
    │   ├── MargineTesoreríaViewModel.cs
    │   └── VoceBudgetViewModel.cs (template per 15 pagine)
    │
    └── Views/
        ├── GestioneBancheView.xaml
        ├── BancaDettaglioView.xaml
        ├── MargineTesoreríaView.xaml
        └── VoceBudgetView.xaml (template per 15 pagine)
```

---

## 📋 TODO LIST COMPLETA

### ✅ FASE 1: MODELLI E DATABASE (Core)

#### Banche:
- [ ] 1. Creare modello `Banca`
- [ ] 2. Creare modello `BancaSaldoGiornaliero`
- [ ] 3. Creare modello `BancaIncasso`
- [ ] 4. Creare modello `BancaPagamento`
- [ ] 5. Creare modello `BancaUtilizzoAnticipo`

#### Margine Tesoreria:
- [ ] 6. Creare modello `MargineTesoreríaPeriodo`

#### Voci Budget:
- [ ] 7. Creare modello `VoceBudget`

#### Database:
- [ ] 8. Aggiornare `LiteDbContext` con nuove collections
- [ ] 9. Creare indici per performance
- [ ] 10. Configurare BsonMapper

---

### ✅ FASE 2: REPOSITORY (Core)

- [ ] 11. Implementare `BancaRepository`
- [ ] 12. Implementare `BancaSaldoRepository`
- [ ] 13. Implementare `BancaIncassoRepository`
- [ ] 14. Implementare `BancaPagamentoRepository`
- [ ] 15. Implementare `BancaUtilizzoAnticipoRepository`
- [ ] 16. Implementare `MargineTesoreríaRepository`
- [ ] 17. Implementare `VoceBudgetRepository`

---

### ✅ FASE 3: SERVICE LAYER (Core)

#### BancaService:
- [ ] 18. Metodo `GetSaldoTotaleAllaData(DateTime data)`
- [ ] 19. Metodo `CalcolaFidoResiduo(int bancaId)`
- [ ] 20. Metodo `CalcolaInteressiAnticipo(int utilizzoId)`
- [ ] 21. Metodo `VerificaSuperamentoFido(int bancaId)` → ALERT
- [ ] 22. Metodo `GetScadenziarioBanca(int bancaId, int mesi)`
- [ ] 23. Metodo `GetRiepilogoTutteBanche()`

#### MargineTesoreríaService:
- [ ] 24. Metodo `GetSaldoBancaIniziale(anno, mese)` → gestisce manuale/automatico
- [ ] 25. Metodo `CalcolaTotaleEntrate(periodo)`
- [ ] 26. Metodo `CalcolaTotaleUscite(periodo)`
- [ ] 27. Metodo `CalcolaFlussoOperativo(periodo)`
- [ ] 28. Metodo `CalcolaMargineFinalePeriodo(periodo)`
- [ ] 29. Metodo `GetMargineTesoreríaAnnoCompleto(anno)`

#### VoceBudgetService:
- [ ] 30. Metodo `CalcolaRipartizioneScadenze(voce)`
- [ ] 31. Metodo `GetCompetenzaMensile(tipoVoce, anno)`
- [ ] 32. Metodo `GetScadenzeMensili(tipoVoce, anno)`
- [ ] 33. Metodo `ValidaPercentuali(voce)` → totale = 100%
- [ ] 34. Metodo `EsportaVersMargineTes(tipoVoce, anno, mese)`

---

### ✅ FASE 4: UI - GESTIONE BANCHE (App)

#### Lista Banche:
- [ ] 35. Creare `GestioneBancheViewModel`
- [ ] 36. Creare `GestioneBancheView.xaml`
- [ ] 37. Implementare CRUD banche (lista + dialog)
- [ ] 38. Visualizzare riepilogo totale (saldo, fido)
- [ ] 39. Pulsante "Apri" per dettaglio banca

#### Dettaglio Banca (TabControl):
- [ ] 40. Creare `BancaDettaglioViewModel`
- [ ] 41. Creare `BancaDettaglioView.xaml` con TabControl
- [ ] 42. Tab "Dati Generali": form con dati banca + fido
- [ ] 43. Tab "Incassi": DataGrid con CRUD incassi clienti
- [ ] 44. Tab "Pagamenti": DataGrid con CRUD pagamenti fornitori
- [ ] 45. Tab "Utilizzo Anticipi": DataGrid + calcolo fido residuo + ALERT
- [ ] 46. Tab "Scadenziario": Lista incassi/pagamenti in scadenza
- [ ] 47. Tab "Saldi": Grafico + lista storico saldi giornalieri

#### Riepilogo Totale Banche:
- [ ] 48. Creare `RiepilogoBancheViewModel`
- [ ] 49. Creare `RiepilogoBancheView.xaml`
- [ ] 50. Visualizzare saldo totale tutte le banche
- [ ] 51. Visualizzare fido totale utilizzato
- [ ] 52. Scadenziario consolidato

---

### ✅ FASE 5: UI - MARGINE TESORERIA (App)

- [ ] 53. Creare `MargineTesoreríaViewModel`
- [ ] 54. Creare `MargineTesoreríaView.xaml`
- [ ] 55. Implementare toggle "Saldo Banca Manuale/Automatico"
- [ ] 56. Campo saldo manuale (editabile se manuale)
- [ ] 57. Campo saldo automatico (readonly con calcolo da banche)
- [ ] 58. Dialog "Vedi dettaglio banche" con lista saldi
- [ ] 59. Tabella mensile con tutte le voci (Gen-Dic + Totali)
- [ ] 60. Pulsanti "Dettaglio" per ogni voce → apre pagina specifica
- [ ] 61. Calcoli automatici delle formule (D, M, N, O, S, T, Z)
- [ ] 62. Export Excel del margine
- [ ] 63. Stampa Report PDF

---

### ✅ FASE 6: UI - DETTAGLIO VOCI (App)

#### Template Base (riutilizzabile):
- [ ] 64. Creare `VoceBudgetViewModel` (template generico)
- [ ] 65. Creare `VoceBudgetView.xaml` (template generico)
- [ ] 66. DataGrid lista voci con CRUD
- [ ] 67. Dialog inserimento/modifica voce con ripartizione scadenze
- [ ] 68. Validazione: totale percentuali = 100%
- [ ] 69. Tabella competenza mensile (per data fattura)
- [ ] 70. Tabella scadenze mensili (per data scadenza)
- [ ] 71. Export automatico totali mensili a Margine Tesoreria

#### Istanziare per 15 pagine:
- [ ] 72. Pagina 1: Clienti Contabili (import da contabilità)
- [ ] 73. Pagina 2: Clienti Budget
- [ ] 74. Pagina 3: Altri Crediti
- [ ] 75. Pagina 4: Fornitori Contabili (import)
- [ ] 76. Pagina 5: Fornitori Budget
- [ ] 77. Pagina 6: Debito Dipendenti
- [ ] 78. Pagina 7: Debito Tasse
- [ ] 79. Pagina 8: Debito IVA
- [ ] 80. Pagina 9: Altri Debiti
- [ ] 81. Pagina 10: Fornitori Arretrati
- [ ] 82. Pagina 11: Debiti Tributari Arretrati
- [ ] 83. Pagina 12: Altri Debiti Arretrati
- [ ] 84. Pagina 13: Investimenti
- [ ] 85. Pagina 14: Finanziamenti
- [ ] 86. Pagina 15: Leasing

---

### ✅ FASE 7: INTEGRAZIONE (App)

- [ ] 87. Aggiornare `MainViewModel` con navigazione modulo
- [ ] 88. Creare menu "Controllo di Gestione" in MainWindow
- [ ] 89. Collegare Saldo Banche → Margine Tesoreria
- [ ] 90. Collegare Voci Budget → Margine Tesoreria (sync real-time)
- [ ] 91. Implementare Audit Log per tutte le operazioni
- [ ] 92. Integrare con sistema Licenze (modulo "ControlloGestione")
- [ ] 93. Implementare permission check (solo utenti autorizzati)

---

### ✅ FASE 8: TESTING E OTTIMIZZAZIONE

- [ ] 94. Test CRUD completo tutte le entità
- [ ] 95. Test calcoli margine tesoreria
- [ ] 96. Test alert superamento fido
- [ ] 97. Test export Excel
- [ ] 98. Test sincronizzazione Banche ↔ Margine
- [ ] 99. Test modalità multi-client (ConnectionType.Shared)
- [ ] 100. Ottimizzazione query con `.ToList()` e `DeleteMany`

---

## 📊 STIMA COMPLESSITÀ

| Componente | Complessità | Tempo Stimato |
|------------|-------------|---------------|
| **Modelli** | Media | 2-3 ore |
| **Repository** | Bassa | 2-3 ore |
| **Service** | Alta | 5-7 ore |
| **UI Banche** | Alta | 8-10 ore |
| **UI Margine Tes.** | Media | 4-5 ore |
| **UI Voci (x15)** | Alta | 10-12 ore |
| **Integrazione** | Media | 3-4 ore |
| **Testing** | Media | 3-4 ore |
| **TOTALE** | - | **37-48 ore** |

---

## 🔐 SICUREZZA E PERMESSI

- **Licenza richiesta**: Modulo "ControlloGestione"
- **Permessi utente**: Solo utenti con ruolo adeguato
- **Audit log**: Tracciamento completo di tutte le modifiche
- **Multi-azienda**: Ogni cliente vede solo i propri dati

---

## 📄 EXPORT E REPORT

### Export Excel
- Margine Tesoreria completo (12 mesi)
- Dettaglio singola voce
- Riepilogo banche
- Scadenziario

### Stampa PDF
- Report margine tesoreria
- Scheda singola banca
- Report scadenze

---

## 🎨 DESIGN GUIDELINES

- **Colori**:
  - Entrate: Verde (#4CAF50)
  - Uscite: Rosso (#F44336)
  - Neutro: Blu (#2196F3)
  - Alert: Arancione (#FF9800)

- **Icone**:
  - 🏦 Banche
  - 💰 Margine Tesoreria
  - 📥 Entrate
  - 📤 Uscite
  - ⚠️ Alert
  - 📊 Report

- **UX**:
  - Navigazione intuitiva con breadcrumb
  - Tooltip informativi su calcoli
  - Conferme per operazioni critiche
  - Progress indicator per operazioni lunghe

---

## 📝 NOTE IMPLEMENTATIVE

### ⚠️ ATTENZIONI SPECIALI

1. **Calcolo Saldo Banca**: Sempre verificare se modalità manuale o automatica
2. **Percentuali scadenze**: Validare totale = 100% prima di salvare
3. **Alert Fido**: Notifica real-time quando si supera il fido
4. **Riporto anno**: Gestire correttamente il riporto dei dati anno precedente
5. **Performance**: Materializzare query con `.ToList()` prima di iterazioni

### 🔄 SINCRONIZZAZIONE

- **Banche → Margine Tes.**: Aggiornamento real-time saldo iniziale (se modalità automatica)
- **Voci Budget → Margine Tes.**: Export automatico totali mensili
- **Multi-client**: Usare sempre `ConnectionType.Shared`

### 🐛 GESTIONE ERRORI

- Try-catch su tutte le operazioni database
- Messaggi utente chiari e localizzati
- Log errori per debug
- Rollback transazioni in caso di errore

---

## 🚀 PRIORITÀ IMPLEMENTAZIONE

### 🔥 PRIORITÀ ALTA (MVP)
1. Modelli + Database
2. Repository base
3. Service base (calcoli essenziali)
4. UI Margine Tesoreria (dashboard)
5. UI Clienti Budget (template)

### 🔸 PRIORITÀ MEDIA
6. UI Gestione Banche (completa)
7. Altre pagine voci budget (clone template)
8. Export Excel
9. Alert fido

### 🔹 PRIORITÀ BASSA
10. Grafici avanzati
11. Report PDF
12. Statistiche avanzate
13. Dashboard analytics

---

## 📚 RIFERIMENTI

- **File Excel originale**: `C:\devcg-group\modulo controllo gestione.xlsx`
- **Pattern applicazione**: Come modulo TODO e Circolari
- **Database**: LiteDB con ConnectionType.Shared
- **UI Framework**: WPF + ModernWPF + MaterialDesign

---

**DOCUMENTO CREATO**: 03/11/2025  
**VERSIONE**: 1.0  
**AUTORE**: AI Assistant per CGEasy Project  

---

**PRONTO PER L'IMPLEMENTAZIONE! 🚀**



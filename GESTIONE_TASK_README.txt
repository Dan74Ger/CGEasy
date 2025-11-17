╔════════════════════════════════════════════════════════════════════════╗
║                  GUIDA SCRIPT GESTIONE_TASK.BAT                        ║
║                   Commit e Push Automatico GitHub                      ║
╚════════════════════════════════════════════════════════════════════════╝

📋 DESCRIZIONE
---------------
Script batch automatico per committare e pushare TUTTI i file del progetto
CGEasy su GitHub, inclusi file nuovi, modificati ed eliminati.

🔧 COSA FA LO SCRIPT
--------------------
[1] Verifica lo stato del repository Git
[2] Aggiunge TUTTI i file in 3 passaggi:
    ✓ git add -u      → File modificati già tracciati
    ✓ git add .       → Tutti i file nuovi nella directory corrente
    ✓ git add -f src/ → Aggiunge forzatamente file in src/CGEasy.Core e src/CGEasy.App
[3] Mostra i file che verranno committati
[3.1] Mostra eventuali file non tracciati rimasti
[4] Crea commit con timestamp automatico: "Update GG/MM/AAAA HH:MM"
[5] Push su GitHub (repository Dan74Ger/CGEasy)

✅ VANTAGGI DELLO SCRIPT MIGLIORATO
------------------------------------
✓ Aggiunge TUTTI i file (anche nuovi ViewModels e Views)
✓ Usa 3 comandi git add diversi per massima copertura
✓ Mostra file non tracciati per controllo manuale
✓ Elenca i file committati al termine
✓ Messaggi di errore più chiari
✓ Verifica forzata su cartelle critiche (src/)

⚙️ COME USARE
--------------
1. **PRIMA CONFIGURAZIONE** (una sola volta):
   a) Crea un file "git_token.txt" nella root del progetto
   b) Incolla il tuo token GitHub personale dentro al file (solo il token, niente altro)
   c) Salva e chiudi (il file è protetto da .gitignore e non verrà mai committato)
   
   OPPURE
   
   a) Imposta la variabile d'ambiente GIT_TOKEN nel sistema Windows
   b) Lo script la userà automaticamente

2. **UTILIZZO NORMALE**:
   a) Doppio click su "gestione_task.bat"
   b) Lo script mostra tutti i file da committare
   c) Verifica che non ci siano file non tracciati indesiderati
   d) Conferma con INVIO per continuare
   e) Attendi il completamento del push

⚠️ ATTENZIONE
--------------
- Lo script usa un token GitHub personale da file "git_token.txt" (protetto da .gitignore)
- Se il token scade, aggiornare il file git_token.txt con il nuovo token
- NON condividere il file git_token.txt con nessuno (contiene credenziali personali)
- I file in .gitignore NON verranno mai committati (es: bin/, obj/, git_token.txt)
- I file non tracciati vengono mostrati ma vanno aggiunti manualmente se voluti

🔍 RISOLUZIONE PROBLEMI
------------------------
PROBLEMA: "File non tracciati rimasti" mostra file che voglio committare
SOLUZIONE: Assicurati che non siano in .gitignore, poi rimuovili dal .gitignore

PROBLEMA: "ERRORE durante git add"
SOLUZIONE: Verifica di avere Git installato e di essere nella directory corretta

PROBLEMA: "ERRORE durante il push"
SOLUZIONE: Verifica connessione internet, permessi e validità del token GitHub

PROBLEMA: Lo script dice "NESSUNA MODIFICA DA COMMITTARE"
SOLUZIONE: Controlla se ci sono file non tracciati sopra (sezione [3.1])

📁 FILE COMMITTATI
-------------------
Lo script committa automaticamente:
✓ Tutti i file .cs (C# source code)
✓ Tutti i file .xaml e .xaml.cs (UI WPF)
✓ Tutti i file .csproj (progetti)
✓ Tutti i file .json, .txt, .md, .bat, .ps1
✓ Tutti i file di configurazione

ESCLUSI automaticamente da .gitignore:
✗ bin/ e obj/ (output compilazione)
✗ .vs/ (cache Visual Studio)
✗ packages/ (NuGet cache)
✗ *.user, *.suo (impostazioni personali)

🎯 ESEMPIO OUTPUT RIUSCITO
----------------------------
========================================
  COMPLETATO CON SUCCESSO!
========================================

Commit: Update 17/11/2025 12:30
Repository: https://github.com/Dan74Ger/CGEasy

File committati e pushati:
src/CGEasy.App/ViewModels/IndiciDiBilancioViewModel.cs
src/CGEasy.App/Views/IndiciDiBilancioView.xaml
src/CGEasy.Core/Services/IndiciDiBilancioService.cs
...

========================================

📞 SUPPORTO
-----------
Per problemi o domande, verificare sempre:
1. La sezione [3.1] per file non tracciati
2. Il messaggio di errore completo
3. Lo stato di git status --short manualmente

═══════════════════════════════════════════════════════════════════════
Ultimo aggiornamento: 17/11/2025
Versione script: 2.0 (Multi-add strategy)
═══════════════════════════════════════════════════════════════════════


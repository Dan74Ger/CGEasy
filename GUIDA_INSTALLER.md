# 🚀 Guida Creazione Installer CGEasy

## 📋 Prerequisiti

### 1. Inno Setup
**Download**: https://jrsoftware.org/isdl.php

- ✅ Versione consigliata: **6.2.2** o superiore
- ✅ Gratuito e Open Source
- ✅ Supporta Windows 7/8/10/11

**Installazione**:
1. Scarica `innosetup-6.2.2.exe`
2. Esegui l'installer
3. Segui il wizard (Next, Next, Install)
4. ✅ Installazione completata in `C:\Program Files (x86)\Inno Setup 6\`

### 2. .NET 8.0 SDK
Già installato per lo sviluppo.

### 3. File Necessari
Tutti i file sono già pronti nel progetto:
- ✅ `installer.iss` - Script Inno Setup
- ✅ `LICENSE.txt` - Licenza software
- ✅ `INSTALLAZIONE_DA_ZERO.md` - Guida installazione
- ✅ Binari compilati in `src\CGEasy.App\bin\Release\net8.0-windows\`

---

## 🔨 Procedura di Creazione Installer

### **Step 1: Compila l'applicazione in Release**

```powershell
cd C:\devcg-group\appcg_easy_project
dotnet build --configuration Release
```

**Output atteso**:
```
Build succeeded.
    0 Error(s)
    [warnings] Warning(s)
```

**Verifica binari**:
```powershell
Get-ChildItem "src\CGEasy.App\bin\Release\net8.0-windows" | Select-Object Name, Length
```

Dovresti vedere:
- `CGEasy.App.exe`
- `CGEasy.Core.dll`
- Varie altre DLL (.NET runtime, LiteDB, ClosedXML, ecc.)

---

### **Step 2: Crea l'icona (opzionale)**

Se non esiste `src\CGEasy.App\Assets\app_icon.ico`, puoi:

**Opzione A**: Creare un'icona semplice
```powershell
# Usa un'icona di default o crea la tua con un tool online
# Es: https://favicon.io/ o https://www.icoconverter.com/
```

**Opzione B**: Disabilitare l'icona nel file `installer.iss`
Commenta la riga:
```iss
; SetupIconFile=src\CGEasy.App\Assets\app_icon.ico
```

---

### **Step 3: Compila l'installer con Inno Setup**

#### Metodo A: Interfaccia Grafica (Consigliato per prima volta)

1. **Apri Inno Setup Compiler**
   - Menu Start → Inno Setup → Inno Setup Compiler

2. **Carica lo script**
   - File → Open
   - Seleziona: `C:\devcg-group\appcg_easy_project\installer.iss`

3. **Compila**
   - Build → Compile (oppure F9)

4. **Attendi la compilazione**
   ```
   Compiling script...
   Creating setup files...
   Setup successfully compiled.
   ```

5. **Output**
   L'installer verrà creato in:
   ```
   C:\devcg-group\appcg_easy_project\installer_output\CGEasy_Setup_v1.0.0.exe
   ```

#### Metodo B: Linea di Comando (Automatico)

```powershell
cd C:\devcg-group\appcg_easy_project

# Compila lo script
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer.iss
```

**Output atteso**:
```
Inno Setup 6 Command-Line Compiler

Compiling installer.iss...
Preprocessor: [lines processed]
Compiler: [files processed]
Setup successfully compiled.
Output: installer_output\CGEasy_Setup_v1.0.0.exe
```

---

### **Step 4: Testa l'installer**

1. **Esegui l'installer**
   ```powershell
   .\installer_output\CGEasy_Setup_v1.0.0.exe
   ```

2. **Wizard di installazione**
   - ✅ Welcome page
   - ✅ License agreement (accetta)
   - ✅ Selezione cartella installazione (default: `C:\Program Files\CGEasy\`)
   - ✅ Configurazione database (default: database vuoto)
   - ✅ Configurazione rete (opzionale)
   - ✅ Task aggiuntivi (icona desktop, avvio automatico)
   - ✅ Installazione in corso...
   - ✅ Completamento

3. **Verifica installazione**
   ```powershell
   # Verifica file applicazione
   Get-ChildItem "C:\Program Files\CGEasy"

   # Verifica database
   Get-ChildItem "C:\db_CGEASY"
   ```

4. **Avvia CGEasy**
   - Dal menu Start: CGEasy
   - Oppure dall'icona desktop
   - Login: `admin1` / `123123`

---

## 📦 Distribuzione Installer

### **Opzione 1: Distribuzione Locale**

Copia il file `CGEasy_Setup_v1.0.0.exe` su:
- ✅ Chiavetta USB
- ✅ Cartella condivisa di rete
- ✅ Server FTP interno

### **Opzione 2: Distribuzione Online**

1. **GitHub Releases** (consigliato)
   ```powershell
   # Carica su GitHub come release
   gh release create v1.0.0 installer_output\CGEasy_Setup_v1.0.0.exe --title "CGEasy v1.0.0" --notes "Prima release stabile"
   ```

2. **Server Web**
   - Carica su un server web aziendale
   - Condividi il link: `https://tuoserver.it/downloads/CGEasy_Setup_v1.0.0.exe`

3. **Cloud Storage**
   - Google Drive, Dropbox, OneDrive
   - Condividi link pubblico o privato

---

## 🔄 Aggiornamento Versione

Per creare una nuova versione:

1. **Modifica `installer.iss`**
   ```iss
   #define MyAppVersion "1.0.1"  ; <- Cambia qui
   ```

2. **Ricompila applicazione**
   ```powershell
   dotnet build --configuration Release
   ```

3. **Ricompila installer**
   ```powershell
   "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer.iss
   ```

4. **Output**
   ```
   installer_output\CGEasy_Setup_v1.0.1.exe
   ```

---

## 🎨 Personalizzazione Installer

### Cambiare Icona
```iss
SetupIconFile=percorso\alla\tua\icona.ico
```

### Aggiungere File Aggiuntivi
```iss
[Files]
Source: "MioFile.pdf"; DestDir: "{app}\Docs"; Flags: ignoreversion
```

### Modificare Percorso Database
```iss
[Dirs]
Name: "D:\CGEasyDB"; Permissions: users-full  ; <- Cambia percorso
```

### Aggiungere Step Personalizzati
Modifica la sezione `[Code]` in `installer.iss`.

---

## 🐛 Risoluzione Problemi

### Errore: "ISCC.exe non trovato"
**Causa**: Inno Setup non installato o percorso errato
**Soluzione**: 
```powershell
# Verifica installazione
Test-Path "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
```

### Errore: "File not found: src\CGEasy.App\bin\Release\..."
**Causa**: Applicazione non compilata in Release
**Soluzione**:
```powershell
dotnet build --configuration Release
```

### Errore: "Cannot create output file"
**Causa**: Installer già aperto o in uso
**Soluzione**:
```powershell
# Chiudi processi in uso
taskkill /F /IM CGEasy_Setup_v1.0.0.exe
```

### Warning: "Icon file not found"
**Causa**: File `app_icon.ico` non esiste
**Soluzione**: Commenta la riga in `installer.iss`:
```iss
; SetupIconFile=src\CGEasy.App\Assets\app_icon.ico
```

---

## 📝 Checklist Pre-Release

Prima di distribuire l'installer, verifica:

- [ ] ✅ Applicazione compilata in **Release** (non Debug)
- [ ] ✅ Versione aggiornata in `installer.iss`
- [ ] ✅ File `LICENSE.txt` presente
- [ ] ✅ File documentazione aggiornati
- [ ] ✅ Database di test funzionante
- [ ] ✅ Installer testato su PC pulito
- [ ] ✅ Credenziali di default corrette (admin1/123123)
- [ ] ✅ Password database corretta (Woodstockac@74)
- [ ] ✅ Nessun file di sviluppo incluso

---

## 🎯 Script Automatico per Build Completa

Crea `build_installer.ps1`:

```powershell
# Build completo: applicazione + installer

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  BUILD INSTALLER CGEASY" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 1. Pulisci build precedenti
Write-Host "`n[1] Pulizia build precedenti..." -ForegroundColor Yellow
dotnet clean --configuration Release

# 2. Compila applicazione
Write-Host "`n[2] Compilazione applicazione Release..." -ForegroundColor Yellow
dotnet build --configuration Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nERRORE durante la compilazione!" -ForegroundColor Red
    exit 1
}

# 3. Compila installer
Write-Host "`n[3] Compilazione installer..." -ForegroundColor Yellow
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer.iss

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nERRORE durante la creazione installer!" -ForegroundColor Red
    exit 1
}

# 4. Riepilogo
Write-Host "`n========================================" -ForegroundColor Green
Write-Host "  BUILD COMPLETATO CON SUCCESSO!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green

$installerPath = "installer_output\CGEasy_Setup_v1.0.0.exe"
$installerSize = (Get-Item $installerPath).Length / 1MB
Write-Host "`nInstaller creato:" -ForegroundColor Cyan
Write-Host "  Percorso: $installerPath" -ForegroundColor White
Write-Host "  Dimensione: $($installerSize.ToString('0.00')) MB" -ForegroundColor White

Write-Host "`nPremi un tasto per uscire..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
```

**Uso**:
```powershell
.\build_installer.ps1
```

---

## 📞 Supporto

Per problemi con l'installer:
- Controlla i log in `installer_output\`
- Consulta la documentazione Inno Setup: https://jrsoftware.org/ishelp/
- Apri issue su GitHub: https://github.com/Dan74Ger/CGEasy/issues

---

**Versione documento**: 1.0  
**Data**: 07/11/2025  
**Autore**: AI Assistant per Dott. Geron Daniele

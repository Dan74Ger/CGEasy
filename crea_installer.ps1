# Script PowerShell per Build + Creazione Installer CGEasy
# Versione: 1.0.0

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   CGEasy - Build & Installer Creator   " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Parametri
$Configuration = "Release"  # Cambia in "Debug" se vuoi testare
$ProjectPath = "src\CGEasy.App\CGEasy.App.csproj"
$InnoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
$InnoScriptPath = "CGEasy_Installer.iss"

# Step 1: Pulisci build precedenti
Write-Host "[1/4] Pulizia build precedenti..." -ForegroundColor Yellow
dotnet clean CGEasy.sln --configuration $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Host "Errore durante la pulizia!" -ForegroundColor Red
    exit 1
}
Write-Host "      Completato!" -ForegroundColor Green
Write-Host ""

# Step 2: Build del progetto
Write-Host "[2/4] Build del progetto in modalita $Configuration..." -ForegroundColor Yellow
dotnet build CGEasy.sln --configuration $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Host "Errore durante la build!" -ForegroundColor Red
    exit 1
}
Write-Host "      Completato!" -ForegroundColor Green
Write-Host ""

# Step 3: Verifica che Inno Setup sia installato
Write-Host "[3/4] Verifica Inno Setup..." -ForegroundColor Yellow
if (-Not (Test-Path $InnoSetupPath)) {
    Write-Host "      ATTENZIONE: Inno Setup non trovato!" -ForegroundColor Red
    Write-Host "      Percorso cercato: $InnoSetupPath" -ForegroundColor Red
    Write-Host ""
    Write-Host "      Scarica Inno Setup da: https://jrsoftware.org/isdl.php" -ForegroundColor Cyan
    Write-Host "      Oppure aggiorna la variabile `$InnoSetupPath nello script" -ForegroundColor Cyan
    exit 1
}
Write-Host "      Inno Setup trovato!" -ForegroundColor Green
Write-Host ""

# Step 4: Crea installer con Inno Setup
Write-Host "[4/4] Creazione installer..." -ForegroundColor Yellow
& $InnoSetupPath $InnoScriptPath
if ($LASTEXITCODE -ne 0) {
    Write-Host "      Errore durante la creazione dell'installer!" -ForegroundColor Red
    exit 1
}
Write-Host "      Completato!" -ForegroundColor Green
Write-Host ""

# Risultato finale
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   INSTALLER CREATO CON SUCCESSO!      " -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Mostra percorso file di output
$OutputDir = "installer_output"
if (Test-Path $OutputDir) {
    $InstallerFiles = Get-ChildItem -Path $OutputDir -Filter "*.exe"
    if ($InstallerFiles.Count -gt 0) {
        Write-Host "File creato:" -ForegroundColor Cyan
        foreach ($file in $InstallerFiles) {
            $sizeInMB = [math]::Round($file.Length / 1MB, 2)
            Write-Host "  - $($file.Name) ($sizeInMB MB)" -ForegroundColor White
            Write-Host "    Percorso: $($file.FullName)" -ForegroundColor Gray
        }
        Write-Host ""
    } else {
        Write-Host "Nessun file .exe trovato nella cartella $OutputDir" -ForegroundColor Yellow
    }
} else {
    Write-Host "Cartella $OutputDir non trovata!" -ForegroundColor Red
}

Write-Host ""
Write-Host "Installazione completata!" -ForegroundColor Green








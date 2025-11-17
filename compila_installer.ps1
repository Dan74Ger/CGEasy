# ========================================
# Script per Compilare Installer CGEasy
# Con Database di Sviluppo Incluso
# ========================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  COMPILAZIONE INSTALLER CGEASY v2.0" -ForegroundColor Cyan
Write-Host "  Con opzione Database Sviluppo" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Percorsi
$ProjectRoot = "C:\devcg-group\appcg_easy_project"
$DbPath = "C:\db_CGEASY"
$InnoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
$InstallerScript = "$ProjectRoot\CGEasy_Installer.iss"
$OutputDir = "$ProjectRoot\installer_output"

# ========================================
# STEP 1: Verifica prerequisiti
# ========================================
Write-Host "[1/5] Verifica prerequisiti..." -ForegroundColor Yellow

# Verifica Inno Setup installato
if (-not (Test-Path $InnoSetupPath)) {
    Write-Host "❌ ERRORE: Inno Setup non trovato!" -ForegroundColor Red
    Write-Host "   Percorso atteso: $InnoSetupPath" -ForegroundColor Red
    Write-Host "   Scarica da: https://jrsoftware.org/isdl.php" -ForegroundColor Yellow
    pause
    exit 1
}
Write-Host "   ✅ Inno Setup trovato" -ForegroundColor Green

# Verifica database di sviluppo
$DbFile = "$DbPath\cgeasy.db"
$DbKeyFile = "$DbPath\db.key"

if (-not (Test-Path $DbFile)) {
    Write-Host "   ⚠️  WARNING: Database sviluppo non trovato!" -ForegroundColor Yellow
    Write-Host "      Percorso: $DbFile" -ForegroundColor Yellow
    Write-Host ""
    $risposta = Read-Host "      Continuare con installer solo DB vuoto? (S/N)"
    if ($risposta -ne "S") {
        exit 0
    }
} else {
    $DbSize = (Get-Item $DbFile).Length / 1MB
    Write-Host "   ✅ Database trovato ($([math]::Round($DbSize, 2)) MB)" -ForegroundColor Green
}

if (-not (Test-Path $DbKeyFile)) {
    Write-Host "   ⚠️  WARNING: db.key non trovato!" -ForegroundColor Yellow
    Write-Host "      Senza questo file, il DB non si aprirà!" -ForegroundColor Yellow
} else {
    Write-Host "   ✅ db.key trovato" -ForegroundColor Green
}

Write-Host ""

# ========================================
# STEP 2: Build Release dell'applicazione
# ========================================
Write-Host "[2/5] Build Release applicazione..." -ForegroundColor Yellow

cd $ProjectRoot
$buildOutput = dotnet build src/CGEasy.App/CGEasy.App.csproj -c Release --no-incremental 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Host "   ❌ ERRORE durante la build!" -ForegroundColor Red
    Write-Host $buildOutput
    pause
    exit 1
}

# Verifica file exe
$ExePath = "$ProjectRoot\src\CGEasy.App\bin\Release\net8.0-windows\CGEasy.App.exe"
if (-not (Test-Path $ExePath)) {
    Write-Host "   ❌ ERRORE: CGEasy.App.exe non trovato!" -ForegroundColor Red
    Write-Host "      Percorso atteso: $ExePath" -ForegroundColor Red
    pause
    exit 1
}

$ExeSize = (Get-Item $ExePath).Length / 1MB
Write-Host "   ✅ Build completata ($([math]::Round($ExeSize, 2)) MB)" -ForegroundColor Green
Write-Host ""

# ========================================
# STEP 3: Verifica script Inno Setup
# ========================================
Write-Host "[3/5] Verifica script Inno Setup..." -ForegroundColor Yellow

if (-not (Test-Path $InstallerScript)) {
    Write-Host "   ❌ ERRORE: CGEasy_Installer.iss non trovato!" -ForegroundColor Red
    pause
    exit 1
}

Write-Host "   ✅ Script Inno Setup trovato" -ForegroundColor Green
Write-Host ""

# ========================================
# STEP 4: Compilazione Installer
# ========================================
Write-Host "[4/5] Compilazione installer..." -ForegroundColor Yellow
Write-Host "   Attendere (può richiedere 30-60 secondi)..." -ForegroundColor Gray

# Esegui compilazione
& $InnoSetupPath $InstallerScript

if ($LASTEXITCODE -ne 0) {
    Write-Host "   ❌ ERRORE durante la compilazione!" -ForegroundColor Red
    pause
    exit 1
}

Write-Host "   ✅ Compilazione completata" -ForegroundColor Green
Write-Host ""

# ========================================
# STEP 5: Verifica output
# ========================================
Write-Host "[5/5] Verifica installer creato..." -ForegroundColor Yellow

$InstallerFile = "$OutputDir\CGEasy_Setup_v2.0.exe"

if (-not (Test-Path $InstallerFile)) {
    Write-Host "   ❌ ERRORE: Installer non creato!" -ForegroundColor Red
    Write-Host "      Percorso atteso: $InstallerFile" -ForegroundColor Red
    pause
    exit 1
}

$InstallerSize = (Get-Item $InstallerFile).Length / 1MB
Write-Host "   ✅ Installer creato con successo!" -ForegroundColor Green
Write-Host ""

# ========================================
# RIEPILOGO
# ========================================
Write-Host "========================================" -ForegroundColor Green
Write-Host "  COMPILAZIONE COMPLETATA!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "📦 File Installer:" -ForegroundColor Cyan
Write-Host "   $InstallerFile" -ForegroundColor White
Write-Host ""
Write-Host "📊 Dimensioni:" -ForegroundColor Cyan
Write-Host "   $([math]::Round($InstallerSize, 2)) MB" -ForegroundColor White
Write-Host ""

if (Test-Path $DbFile) {
    Write-Host "✅ Include Database Sviluppo:" -ForegroundColor Cyan
    Write-Host "   - Database: cgeasy.db ($([math]::Round($DbSize, 2)) MB)" -ForegroundColor White
    if (Test-Path $DbKeyFile) {
        Write-Host "   - Password: db.key" -ForegroundColor White
    }
    Write-Host ""
    Write-Host "   Gli utenti potranno scegliere:" -ForegroundColor Yellow
    Write-Host "   1. Database VUOTO (produzione)" -ForegroundColor Gray
    Write-Host "   2. Database con DATI DI ESEMPIO (test/demo)" -ForegroundColor Gray
} else {
    Write-Host "ℹ️  Database Sviluppo NON incluso" -ForegroundColor Yellow
    Write-Host "   Solo opzione: Database vuoto" -ForegroundColor Gray
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Chiedi se aprire la cartella
$risposta = Read-Host "Vuoi aprire la cartella dell'installer? (S/N)"
if ($risposta -eq "S") {
    explorer $OutputDir
}

Write-Host ""
Write-Host "Premi un tasto per chiudere..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")


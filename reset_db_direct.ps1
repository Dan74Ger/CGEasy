# Script diretto per reset database - NESSUNA CONFERMA
$ErrorActionPreference = "Stop"

Write-Host "=== RESET DATABASE CGEASY ===" -ForegroundColor Cyan

$dbPath = "C:\devcg-group\dbtest_prova\cgeasy.db"
$exePath = "C:\devcg-group\appcg_easy_project\ResetDB\bin\Debug\net9.0\ResetDB.exe"

if (-not (Test-Path $dbPath)) {
    Write-Host "[X] Database non trovato: $dbPath" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $exePath)) {
    Write-Host "[!] Eseguibile non trovato, compilo il progetto..." -ForegroundColor Yellow
    Set-Location "C:\devcg-group\appcg_easy_project"
    dotnet build ResetDB\ResetDB.csproj -c Debug
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[X] Errore compilazione" -ForegroundColor Red
        exit 1
    }
}

Write-Host "[*] Eseguo reset database..." -ForegroundColor Cyan
& $exePath $dbPath

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "[OK] DATABASE RESETTATO!" -ForegroundColor Green
    Write-Host "Credenziali: admin / 123456" -ForegroundColor White
} else {
    Write-Host "[X] Errore durante il reset" -ForegroundColor Red
}


# Script per resettare il database CGEasy
# Mantiene solo utente admin con password 123456
# Elimina tutti gli altri dati

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "   RESET DATABASE CGEASY" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$dbPath = "C:\devcg-group\dbtest_prova\cgeasy.db"

if (-not (Test-Path $dbPath)) {
    Write-Host "[X] Database non trovato: $dbPath" -ForegroundColor Red
    exit 1
}

Write-Host "[OK] Database trovato: $dbPath" -ForegroundColor Green
Write-Host ""
Write-Host "ATTENZIONE!" -ForegroundColor Yellow
Write-Host "Questo script eliminera TUTTI i dati eccetto:" -ForegroundColor Yellow
Write-Host "  - Utente admin (password: 123456)" -ForegroundColor Yellow
Write-Host ""

$confirm = Read-Host "Vuoi continuare? (S/N)"
if ($confirm -ne "S" -and $confirm -ne "s") {
    Write-Host "[X] Operazione annullata" -ForegroundColor Red
    exit 0
}

Write-Host ""
Write-Host "[*] Compilazione progetto ResetDatabase..." -ForegroundColor Cyan

# Compila il progetto C# per il reset
dotnet build ResetDatabase.csproj -c Release -v quiet

if ($LASTEXITCODE -ne 0) {
    Write-Host "[X] Errore durante la compilazione" -ForegroundColor Red
    exit 1
}

Write-Host "[OK] Compilazione completata" -ForegroundColor Green
Write-Host ""
Write-Host "[*] Esecuzione reset database..." -ForegroundColor Cyan

# Esegui il reset
dotnet run --project ResetDatabase.csproj --no-build -c Release -- "$dbPath"

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "=====================================" -ForegroundColor Green
    Write-Host "[OK] DATABASE RESETTATO CON SUCCESSO!" -ForegroundColor Green
    Write-Host "=====================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Credenziali admin:" -ForegroundColor Cyan
    Write-Host "  Username: admin" -ForegroundColor White
    Write-Host "  Password: 123456" -ForegroundColor White
} else {
    Write-Host ""
    Write-Host "[X] Errore durante il reset del database" -ForegroundColor Red
}

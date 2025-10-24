# Script PowerShell per verificare le licenze nel database usando C#
$scriptContent = @'
using System;
using System.Linq;

var dbPath = System.IO.Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
    "CGEasy",
    "cgeasy.db"
);

Console.WriteLine("=== VERIFICA LICENZE DATABASE ===");
Console.WriteLine($"Database: {dbPath}");
Console.WriteLine();

if (!System.IO.File.Exists(dbPath))
{
    Console.WriteLine("ERRORE: Database non trovato!");
    return;
}

var connectionString = $"Filename={dbPath};Connection=shared";
using var db = new LiteDB.LiteDatabase(connectionString);

var keys = db.GetCollection<dynamic>("license_keys");
var allKeys = keys.FindAll().ToList();

Console.WriteLine($"Totale licenze nel database: {allKeys.Count}");
Console.WriteLine();

foreach (var key in allKeys)
{
    var moduleName = key.ModuleName ?? "N/A";
    var fullKey = key.FullKey ?? "N/A";
    var isActive = key.IsActive;
    var scadenza = key.DataScadenza;
    
    Console.WriteLine($"Modulo: {moduleName}");
    Console.WriteLine($"  Chiave: {fullKey}");
    Console.WriteLine($"  Attiva: {isActive}");
    Console.WriteLine($"  Scadenza: {(scadenza != null ? scadenza.ToString() : "Perpetua")}");
    Console.WriteLine();
}
'@

$scriptContent | Out-File -FilePath "temp_verifica.csx" -Encoding UTF8

# Esegui con dotnet-script (se installato) o usa LiteDB.Studio
if (Get-Command dotnet-script -ErrorAction SilentlyContinue) {
    dotnet-script temp_verifica.csx
} else {
    # Prova con dotnet fsi (F# interactive)
    Write-Host "Usando metodo alternativo..." -ForegroundColor Yellow
    
    # Leggi il database path
    $dbPath = "$env:CommonApplicationData\CGEasy\cgeasy.db"
    Write-Host "Database: $dbPath"
    
    if (Test-Path $dbPath) {
        Write-Host "Database trovato! Usa LiteDB Studio per visualizzarlo." -ForegroundColor Green
        Write-Host "Path: $dbPath" -ForegroundColor Cyan
        
        # Avvia LiteDB Studio se disponibile
        $liteDbStudio = ".\tools\LiteDB.Studio.exe"
        if (Test-Path $liteDbStudio) {
            Start-Process $liteDbStudio $dbPath
        }
    } else {
        Write-Host "Database non trovato!" -ForegroundColor Red
    }
}

Remove-Item "temp_verifica.csx" -ErrorAction SilentlyContinue


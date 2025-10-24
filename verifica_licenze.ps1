# Script per verificare licenze nel database
$dbPath = "$env:ProgramData\CGEasy\cgeasy.db"

Write-Host "=== VERIFICA LICENZE NEL DATABASE ===" -ForegroundColor Cyan
Write-Host "Database: $dbPath" -ForegroundColor Yellow
Write-Host ""

if (-not (Test-Path $dbPath)) {
    Write-Host "ERRORE: Database non trovato!" -ForegroundColor Red
    exit
}

# Usa LiteDB.Studio per leggere (se disponibile), altrimenti usa script C#
$scriptCs = @"
using System;
using System.IO;
using LiteDB;

var dbPath = @"$dbPath";
var connectionString = `$"Filename={dbPath};Connection=shared";

using var db = new LiteDatabase(connectionString);
var keys = db.GetCollection("license_keys");

Console.WriteLine("=== LICENZE NEL DATABASE ===");
Console.WriteLine();

var allKeys = keys.FindAll();
var count = 0;

foreach (var key in allKeys)
{
    count++;
    Console.WriteLine(`$"Licenza #{count}");
    Console.WriteLine(`$"  Modulo: {key["ModuleName"]}");
    Console.WriteLine(`$"  Chiave: {key["FullKey"]}");
    Console.WriteLine(`$"  Attiva: {key["IsActive"]}");
    
    var scadenza = key["DataScadenza"];
    if (scadenza != null && scadenza.AsDateTime != DateTime.MinValue)
    {
        Console.WriteLine(`$"  Scadenza: {scadenza.AsDateTime:dd/MM/yyyy}");
    }
    else
    {
        Console.WriteLine(`$"  Scadenza: Perpetua");
    }
    Console.WriteLine();
}

if (count == 0)
{
    Console.WriteLine("Nessuna licenza trovata nel database!");
}
else
{
    Console.WriteLine(`$"Totale licenze: {count}");
}
"@

# Salva script C#
$scriptCs | Out-File -FilePath "temp_check_licenses.cs" -Encoding UTF8

# Esegui script
Write-Host "Lettura database..." -ForegroundColor Yellow
dotnet script temp_check_licenses.cs

# Pulisci
Remove-Item "temp_check_licenses.cs" -ErrorAction SilentlyContinue


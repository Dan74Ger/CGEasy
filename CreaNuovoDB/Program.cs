using System;
using System.IO;
using LiteDB;

// Percorso database CON password master
// NOTA: Il database viene SEMPRE criptato con la password master Woodstockac@74
// La password che l'utente imposta (es: Danger1974) viene salvata solo nel file db.key
var dbPath = @"C:\devcg-group\dbtest_prova\cgeasy_con_password.db";
var masterPassword = "Woodstockac@74";  // Password master FISSA

// Elimina se esiste
if (File.Exists(dbPath))
{
    File.Delete(dbPath);
    Console.WriteLine("[*] Database vecchio eliminato");
}

Console.WriteLine($"[*] Creazione nuovo database: {dbPath}");
Console.WriteLine($"[*] Password master (fissa): {masterPassword}");

// Crea database CON password master
var connectionString = $"Filename={dbPath};Password={masterPassword}";
using var db = new LiteDatabase(connectionString);

// Crea utente admin
Console.WriteLine("[*] Creazione utente admin...");
var utentiCol = db.GetCollection<BsonDocument>("utenti");

var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");
Console.WriteLine($"[*] Password hash generato: {passwordHash.Substring(0, 20)}...");

var admin = new BsonDocument
{
    ["_id"] = 1,
    ["username"] = "admin",
    ["email"] = "admin@cgeasy.local",
    ["password_hash"] = passwordHash,  // Usa il nome corretto del campo
    ["nome"] = "Amministratore",
    ["cognome"] = "Sistema",
    ["ruolo"] = 1,  // Administrator = 1
    ["attivo"] = true,
    ["data_creazione"] = DateTime.UtcNow,
    ["data_modifica"] = DateTime.UtcNow
};

utentiCol.Insert(admin);
utentiCol.EnsureIndex("Username", unique: true);

// Crea permessi admin
Console.WriteLine("[*] Creazione permessi admin...");
var permissionsCol = db.GetCollection<BsonDocument>("user_permissions");

var permissions = new BsonDocument
{
    ["_id"] = 1,
    ["id_utente"] = 1,
    ["modulo_todo"] = true,
    ["modulo_bilanci"] = true,
    ["modulo_circolari"] = true,
    ["modulo_controllo_gestione"] = true,
    ["clienti_create"] = true,
    ["clienti_read"] = true,
    ["clienti_update"] = true,
    ["clienti_delete"] = true,
    ["professionisti_create"] = true,
    ["professionisti_read"] = true,
    ["professionisti_update"] = true,
    ["professionisti_delete"] = true,
    ["utenti_manage"] = true,
    ["data_creazione"] = DateTime.UtcNow,
    ["data_modifica"] = DateTime.UtcNow
};

permissionsCol.Insert(permissions);
permissionsCol.EnsureIndex("IdUtente", unique: true);

// Checkpoint
db.Checkpoint();

Console.WriteLine("");
Console.WriteLine("=====================================");
Console.WriteLine("[OK] DATABASE CREATO CON SUCCESSO!");
Console.WriteLine("=====================================");
Console.WriteLine("");
Console.WriteLine("Database: " + dbPath);
Console.WriteLine("Password DB: " + masterPassword + " (MASTER PASSWORD - FISSA)");
Console.WriteLine("");
Console.WriteLine("Credenziali admin:");
Console.WriteLine("  Username: admin");
Console.WriteLine("  Password: 123456");
Console.WriteLine("");
Console.WriteLine($"Statistiche:");
Console.WriteLine($"  Utenti: {utentiCol.Count()}");
Console.WriteLine($"  Permessi: {permissionsCol.Count()}");

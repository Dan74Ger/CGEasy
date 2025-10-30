using System;
using System.IO;
using LiteDB;

// Percorso nuovo database SENZA password
var dbPath = @"C:\devcg-group\dbtest_prova\cgeasy_nuovo.db";

// Elimina se esiste
if (File.Exists(dbPath))
{
    File.Delete(dbPath);
    Console.WriteLine("[*] Database vecchio eliminato");
}

Console.WriteLine($"[*] Creazione nuovo database: {dbPath}");

// Crea database SENZA password
using var db = new LiteDatabase(dbPath);

// Crea utente admin
Console.WriteLine("[*] Creazione utente admin...");
var utentiCol = db.GetCollection<BsonDocument>("utenti");

var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");
Console.WriteLine($"[*] Password hash generato: {passwordHash.Substring(0, 20)}...");

var admin = new BsonDocument
{
    ["_id"] = 1,
    ["Username"] = "admin",
    ["Email"] = "admin@cgeasy.local",
    ["PasswordHash"] = passwordHash,
    ["Nome"] = "Amministratore",
    ["Cognome"] = "Sistema",
    ["Ruolo"] = "Administrator",
    ["Attivo"] = true,
    ["DataCreazione"] = DateTime.UtcNow,
    ["DataModifica"] = DateTime.UtcNow
};

utentiCol.Insert(admin);
utentiCol.EnsureIndex("Username", unique: true);

// Crea permessi admin
Console.WriteLine("[*] Creazione permessi admin...");
var permissionsCol = db.GetCollection<BsonDocument>("user_permissions");

var permissions = new BsonDocument
{
    ["_id"] = 1,
    ["IdUtente"] = 1,
    ["ModuloTodo"] = true,
    ["ModuloBilanci"] = true,
    ["ModuloCircolari"] = true,
    ["ModuloControlloGestione"] = true,
    ["ClientiCreate"] = true,
    ["ClientiRead"] = true,
    ["ClientiUpdate"] = true,
    ["ClientiDelete"] = true,
    ["ProfessionistiCreate"] = true,
    ["ProfessionistiRead"] = true,
    ["ProfessionistiUpdate"] = true,
    ["ProfessionistiDelete"] = true,
    ["UtentiManage"] = true,
    ["DataCreazione"] = DateTime.UtcNow,
    ["DataModifica"] = DateTime.UtcNow
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
Console.WriteLine("Password DB: NESSUNA (non criptato)");
Console.WriteLine("");
Console.WriteLine("Credenziali admin:");
Console.WriteLine("  Username: admin");
Console.WriteLine("  Password: 123456");
Console.WriteLine("");
Console.WriteLine($"Statistiche:");
Console.WriteLine($"  Utenti: {utentiCol.Count()}");
Console.WriteLine($"  Permessi: {permissionsCol.Count()}");


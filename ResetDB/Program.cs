using System;
using System.IO;
using LiteDB;

var dbPath = @"C:\devcg-group\dbtest_prova\cgeasy.db";

if (!File.Exists(dbPath))
{
    Console.WriteLine($"[X] Database non trovato: {dbPath}");
    return;
}

Console.WriteLine($"[*] Apertura database: {dbPath}");

// Usa la password master per aprire il database criptato
var masterPassword = "Woodstockac@74";
var connectionString = $"Filename={dbPath};Password={masterPassword}";
using var db = new LiteDatabase(connectionString);

// Elimina tutte le collections
Console.WriteLine("[*] Eliminazione clienti...");
db.DropCollection("clienti");

Console.WriteLine("[*] Eliminazione professionisti...");
db.DropCollection("professionisti");

Console.WriteLine("[*] Eliminazione tipo pratiche...");
db.DropCollection("tipo_pratiche");

Console.WriteLine("[*] Eliminazione audit logs...");
db.DropCollection("audit_logs");

Console.WriteLine("[*] Eliminazione TODO Studio...");
db.DropCollection("todoStudio");

Console.WriteLine("[*] Eliminazione bilanci...");
db.DropCollection("bilancio_contabile");
db.DropCollection("bilancio_template");

Console.WriteLine("[*] Eliminazione associazioni mastrini...");
db.DropCollection("associazioni_mastrini");
db.DropCollection("associazioni_mastrini_dettagli");

Console.WriteLine("[*] Eliminazione licenze...");
db.DropCollection("license_clients");
db.DropCollection("license_keys");

Console.WriteLine("[*] Eliminazione permessi utenti...");
db.DropCollection("user_permissions");

// Ricrea collection utenti con solo admin
Console.WriteLine("[*] Creazione utente admin...");
var utentiCol = db.GetCollection<BsonDocument>("utenti");
utentiCol.DeleteAll();

// Configura autoId per la collection
var mapper = BsonMapper.Global;

var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");

var admin = new BsonDocument
{
    ["_id"] = 1,  // Usa _id come campo ID in LiteDB
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
    ["_id"] = 1,  // Usa _id come campo ID in LiteDB
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

// Checkpoint finale
db.Checkpoint();

Console.WriteLine("");
Console.WriteLine("=====================================");
Console.WriteLine("[OK] DATABASE RESETTATO CON SUCCESSO!");
Console.WriteLine("=====================================");
Console.WriteLine("");
Console.WriteLine("Credenziali admin:");
Console.WriteLine("  Username: admin");
Console.WriteLine("  Password: 123456");
Console.WriteLine("");
Console.WriteLine($"Statistiche finali:");
Console.WriteLine($"  Utenti: {utentiCol.Count()}");
Console.WriteLine($"  Permessi: {permissionsCol.Count()}");
Console.WriteLine($"  Clienti: 0");
Console.WriteLine($"  Professionisti: 0");
Console.WriteLine($"  Licenze: 0");

// Elimina anche il file licenses.json se esiste
var licensesJson = Path.Combine(Path.GetDirectoryName(dbPath)!, "licenses.json");
if (File.Exists(licensesJson))
{
    File.Delete(licensesJson);
    Console.WriteLine($"[*] File licenses.json eliminato");
}

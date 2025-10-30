using LiteDB;
using System;

var dbPath = @"C:\devcg-group\dbtest_prova\cgeasy.db";
var masterPassword = "Woodstockac@74";

Console.WriteLine($"Creazione database: {dbPath}");

// Crea connection string con password
var connString = new ConnectionString
{
    Filename = dbPath,
    Password = masterPassword,
    Connection = ConnectionType.Direct
};

using (var db = new LiteDatabase(connString))
{
    // Collection utenti
    var utenti = db.GetCollection("utenti");
    utenti.DeleteAll();
    
    // Admin nascosto (developer)
    var admin = new BsonDocument
    {
        ["_id"] = 1,
        ["id_utente"] = 1,
        ["username"] = "admin",
        ["email"] = "admin@cgeasy.local",
        ["password_hash"] = BCrypt.Net.BCrypt.HashPassword("123456"),
        ["nome"] = "Amministratore",
        ["cognome"] = "Sistema",
        ["ruolo"] = "Administrator",
        ["attivo"] = true,
        ["data_creazione"] = DateTime.UtcNow,
        ["data_modifica"] = DateTime.UtcNow
    };
    utenti.Insert(admin);
    
    // Admin1 (cliente)
    var admin1 = new BsonDocument
    {
        ["_id"] = 2,
        ["id_utente"] = 2,
        ["username"] = "admin1",
        ["email"] = "admin1@cgeasy.local",
        ["password_hash"] = BCrypt.Net.BCrypt.HashPassword("123123"),
        ["nome"] = "Amministratore",
        ["cognome"] = "Cliente",
        ["ruolo"] = "Administrator",
        ["attivo"] = true,
        ["data_creazione"] = DateTime.UtcNow,
        ["data_modifica"] = DateTime.UtcNow
    };
    utenti.Insert(admin1);
    
    // Permessi
    var permissions = db.GetCollection("user_permissions");
    permissions.DeleteAll();
    
    for (int i = 1; i <= 2; i++)
    {
        var perm = new BsonDocument
        {
            ["_id"] = i,
            ["id_utente"] = i,
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
        permissions.Insert(perm);
    }
    
    db.Checkpoint();
}

Console.WriteLine("✅ Database creato con successo!");
Console.WriteLine("   - admin / 123456");
Console.WriteLine("   - admin1 / 123123");

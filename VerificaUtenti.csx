using System;
using System.IO;
using LiteDB;

var dbPath = @"C:\devcg-group\dbtest_prova\cgeasy.db";
var masterPassword = "Woodstockac@74";
var connectionString = $"Filename={dbPath};Password={masterPassword}";

using var db = new LiteDatabase(connectionString);

var utentiCol = db.GetCollection<BsonDocument>("utenti");
var users = utentiCol.FindAll();

Console.WriteLine("=== VERIFICA UTENTI ===");
Console.WriteLine($"Totale utenti: {utentiCol.Count()}");
Console.WriteLine();

foreach (var user in users)
{
    Console.WriteLine($"ID: {user["_id"]}");
    Console.WriteLine($"Username: {user["Username"]}");
    Console.WriteLine($"Email: {user["Email"]}");
    Console.WriteLine($"Attivo: {user["Attivo"]}");
    
    var passwordHash = user["PasswordHash"].AsString;
    Console.WriteLine($"PasswordHash presente: {!string.IsNullOrEmpty(passwordHash)}");
    Console.WriteLine($"PasswordHash length: {passwordHash?.Length ?? 0}");
    
    if (!string.IsNullOrEmpty(passwordHash))
    {
        // Verifica password
        try 
        {
            bool isValid = BCrypt.Net.BCrypt.Verify("123456", passwordHash);
            Console.WriteLine($"Password '123456' valida: {isValid}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore verifica: {ex.Message}");
        }
    }
    Console.WriteLine();
}


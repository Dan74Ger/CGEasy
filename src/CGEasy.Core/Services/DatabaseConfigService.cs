using System;
using System.IO;
using System.Text.Json;

namespace CGEasy.Core.Services
{
    /// <summary>
    /// Servizio per gestire la configurazione del percorso del database
    /// Permette di salvare/caricare il percorso del database (locale o UNC)
    /// </summary>
    public class DatabaseConfigService
    {
        private static readonly string ConfigFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CGEasy");

        private static readonly string ConfigFile = Path.Combine(ConfigFolder, "database.config");

        /// <summary>
        /// Modello configurazione database
        /// </summary>
        public class DatabaseConfig
        {
            public string DatabasePath { get; set; } = string.Empty;
            public DateTime LastModified { get; set; }
            public bool IsNetworkPath { get; set; }
        }

        /// <summary>
        /// Ottiene il percorso configurato del database (se esiste)
        /// </summary>
        /// <returns>Percorso configurato o null se non esiste configurazione</returns>
        public static string? GetConfiguredPath()
        {
            try
            {
                if (!File.Exists(ConfigFile))
                    return null;

                var json = File.ReadAllText(ConfigFile);
                var config = JsonSerializer.Deserialize<DatabaseConfig>(json);
                
                return config?.DatabasePath;
            }
            catch
            {
                // Se c'è un errore, ritorna null (usa default)
                return null;
            }
        }

        /// <summary>
        /// Salva il percorso del database nella configurazione
        /// </summary>
        public static void SaveDatabasePath(string databasePath)
        {
            try
            {
                // Crea cartella se non esiste
                if (!Directory.Exists(ConfigFolder))
                {
                    Directory.CreateDirectory(ConfigFolder);
                }

                var config = new DatabaseConfig
                {
                    DatabasePath = databasePath,
                    LastModified = DateTime.Now,
                    IsNetworkPath = databasePath.StartsWith(@"\\") || databasePath.StartsWith("//")
                };

                var json = JsonSerializer.Serialize(config, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });

                File.WriteAllText(ConfigFile, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Impossibile salvare configurazione: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Ottiene la configurazione completa
        /// </summary>
        public static DatabaseConfig? GetConfiguration()
        {
            try
            {
                if (!File.Exists(ConfigFile))
                    return null;

                var json = File.ReadAllText(ConfigFile);
                return JsonSerializer.Deserialize<DatabaseConfig>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Elimina la configurazione (torna al percorso di default)
        /// </summary>
        public static void DeleteConfiguration()
        {
            try
            {
                if (File.Exists(ConfigFile))
                {
                    File.Delete(ConfigFile);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Impossibile eliminare configurazione: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica se un percorso database è raggiungibile
        /// </summary>
        public static bool TestDatabasePath(string databasePath, string? password = null)
        {
            try
            {
                // Verifica che la directory esista o sia creabile
                var directory = Path.GetDirectoryName(databasePath);
                if (string.IsNullOrEmpty(directory))
                    return false;

                // Se è un percorso di rete, verifica che sia raggiungibile
                if (databasePath.StartsWith(@"\\") || databasePath.StartsWith("//"))
                {
                    // Prova ad accedere alla directory
                    if (!Directory.Exists(directory))
                    {
                        return false;
                    }
                }
                else
                {
                    // Per percorsi locali, crea la directory se non esiste
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }

                // Se il file esiste, prova ad aprirlo
                if (File.Exists(databasePath))
                {
                    // Prova ad aprire il database in modalità Shared (compatibile multi-client)
                    using (var db = new LiteDB.LiteDatabase(new LiteDB.ConnectionString
                    {
                        Filename = databasePath,
                        Connection = LiteDB.ConnectionType.Shared,
                        ReadOnly = true,  // Usa ReadOnly per test non invasivo
                        Password = password
                    }))
                    {
                        // Prova a leggere qualcosa per verificare che sia accessibile
                        var collections = db.GetCollectionNames().ToList();
                        return true;
                    }
                }
                else
                {
                    // Se il file non esiste, verifica solo che si possa scrivere nella directory
                    var testFile = Path.Combine(directory, $"test_{Guid.NewGuid()}.tmp");
                    File.WriteAllText(testFile, "test");
                    File.Delete(testFile);
                    return true;
                }
            }
            catch (LiteDB.LiteException ex) when (ex.Message.Contains("password") || ex.Message.Contains("encrypt"))
            {
                // Password errata o problema di crittografia - rilancia per gestire sopra
                throw;
            }
            catch (Exception)
            {
                // Altri errori (permessi, percorso invalido, ecc)
                return false;
            }
        }

        /// <summary>
        /// Ottiene informazioni sulla configurazione corrente
        /// </summary>
        public static string GetConfigurationInfo()
        {
            var config = GetConfiguration();
            if (config == null)
            {
                return "Nessuna configurazione personalizzata (uso percorso di default)";
            }

            var pathType = config.IsNetworkPath ? "Percorso di rete" : "Percorso locale";
            return $"{pathType}\nConfigurato il: {config.LastModified:dd/MM/yyyy HH:mm:ss}";
        }
    }
}







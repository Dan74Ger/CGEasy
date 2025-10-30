# Verifica utente admin nel database
$dbPath = "C:\devcg-group\dbtest_prova\cgeasy.db"
$masterPassword = "Woodstockac@74"

Add-Type -Path "C:\devcg-group\appcg_easy_project\ResetDB\bin\Debug\net9.0\LiteDB.dll"
Add-Type -Path "C:\devcg-group\appcg_easy_project\ResetDB\bin\Debug\net9.0\BCrypt.Net-Next.dll"

$connectionString = "Filename=$dbPath;Password=$masterPassword"
$db = New-Object LiteDB.LiteDatabase($connectionString)

$utentiCol = $db.GetCollection("utenti")
$users = $utentiCol.FindAll()

Write-Host "=== UTENTI NEL DATABASE ===" -ForegroundColor Cyan
foreach ($user in $users) {
    Write-Host ""
    Write-Host "ID: $($user['Id'])" -ForegroundColor Green
    Write-Host "Username: $($user['Username'])"
    Write-Host "Email: $($user['Email'])"
    Write-Host "Nome: $($user['Nome'])"
    Write-Host "Cognome: $($user['Cognome'])"
    Write-Host "Ruolo: $($user['Ruolo'])"
    Write-Host "Attivo: $($user['Attivo'])"
    Write-Host "PasswordHash: $($user['PasswordHash'])"
    
    # Verifica password
    $passwordToTest = "123456"
    $isValid = [BCrypt.Net.BCrypt]::Verify($passwordToTest, $user['PasswordHash'])
    Write-Host "Password '123456' valida: $isValid" -ForegroundColor $(if($isValid){"Green"}else{"Red"})
}

$db.Dispose()
Write-Host ""
Write-Host "Totale utenti: $($users.Count)" -ForegroundColor Cyan


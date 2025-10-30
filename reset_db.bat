@echo off
echo ========================================
echo    RESET DATABASE CGEASY
echo ========================================
echo.

set DB_PATH=C:\devcg-group\dbtest_prova\cgeasy.db
set EXE_PATH=C:\devcg-group\appcg_easy_project\ResetDB\bin\Debug\net9.0\ResetDB.exe

if not exist "%DB_PATH%" (
    echo [X] Database non trovato: %DB_PATH%
    pause
    exit /b 1
)

if not exist "%EXE_PATH%" (
    echo [!] Eseguibile non trovato, compilo...
    cd /d C:\devcg-group\appcg_easy_project
    dotnet build ResetDB\ResetDB.csproj -c Debug -v minimal
    if errorlevel 1 (
        echo [X] Errore compilazione
        pause
        exit /b 1
    )
)

echo [*] Eseguo reset database...
echo.
"%EXE_PATH%" "%DB_PATH%"

if errorlevel 1 (
    echo.
    echo [X] Errore durante il reset
    pause
    exit /b 1
) else (
    echo.
    echo ========================================
    echo [OK] DATABASE RESETTATO CON SUCCESSO!
    echo ========================================
    echo.
    echo Credenziali login:
    echo   Username: admin
    echo   Password: 123456
    echo.
)

pause


; CGEasy - Script Installer Inno Setup
; Versione: 1.0.0
; Data: 2025-10-27

#define MyAppName "CGEasy"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "CG Group - Dott. Geron Daniele Commercialista e Revisore legale dei Conti"
#define MyAppURL "https://www.cg-group.it"
#define MyAppExeName "CGEasy.App.exe"

[Setup]
; Informazioni di base
AppId={{B8F3D9A1-2C4E-4F5A-9B7D-1E3C5A7B9D2F}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

; Cartella di installazione di default
DefaultDirName={autopf}\CGEasy
DefaultGroupName={#MyAppName}

; File di output
OutputDir=installer_output
OutputBaseFilename=CGEasy_Setup_v{#MyAppVersion}

; Compressione
Compression=lzma2/max
SolidCompression=yes

; Privilegi richiesti (admin per installare in Program Files)
PrivilegesRequired=admin

; Icona installer
SetupIconFile=src\CGEasy.App\Images\logo.ico

; Immagine wizard (opzionale)
;WizardImageFile=installer_resources\wizard.bmp
;WizardSmallImageFile=installer_resources\wizard_small.bmp

; Lingua
ShowLanguageDialog=no

; Disinstallazione
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Crea un'icona sul &Desktop"; GroupDescription: "Icone aggiuntive:"; Flags: unchecked
Name: "quicklaunchicon"; Description: "Crea un'icona nella &Barra di avvio veloce"; GroupDescription: "Icone aggiuntive:"; Flags: unchecked; OnlyBelowVersion: 6.1; Check: not IsAdminInstallMode

[Files]
; File principale (eseguibile)
Source: "src\CGEasy.App\bin\Release\net8.0-windows\CGEasy.App.exe"; DestDir: "{app}"; Flags: ignoreversion

; DLL del progetto
Source: "src\CGEasy.App\bin\Release\net8.0-windows\CGEasy.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "src\CGEasy.App\bin\Release\net8.0-windows\CGEasy.BilanciModule.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "src\CGEasy.App\bin\Release\net8.0-windows\CGEasy.CircolariModule.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "src\CGEasy.App\bin\Release\net8.0-windows\CGEasy.ControlloModule.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "src\CGEasy.App\bin\Release\net8.0-windows\CGEasy.TodoModule.dll"; DestDir: "{app}"; Flags: ignoreversion

; File di configurazione
Source: "src\CGEasy.App\bin\Release\net8.0-windows\CGEasy.App.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "src\CGEasy.App\bin\Release\net8.0-windows\CGEasy.App.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion

; DLL di terze parti
Source: "src\CGEasy.App\bin\Release\net8.0-windows\*.dll"; DestDir: "{app}"; Flags: ignoreversion

; Cartelle di risorse (lingue, etc.) - OPZIONALE
; Se esistono cartelle con nome pattern xx-XX (es: it-IT, en-US), le copia
Source: "src\CGEasy.App\bin\Release\net8.0-windows\it-IT\*"; DestDir: "{app}\it-IT"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "src\CGEasy.App\bin\Release\net8.0-windows\en-US\*"; DestDir: "{app}\en-US"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "src\CGEasy.App\bin\Release\net8.0-windows\de-DE\*"; DestDir: "{app}\de-DE"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "src\CGEasy.App\bin\Release\net8.0-windows\fr-FR\*"; DestDir: "{app}\fr-FR"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "src\CGEasy.App\bin\Release\net8.0-windows\es-ES\*"; DestDir: "{app}\es-ES"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist

; Database VUOTO iniziale (se non esiste, verrà creato dall'app)
; IMPORTANTE: Il database verrà creato in C:\devcg-group\dbtest_prova al primo avvio
; Le licenze verranno salvate nella stessa cartella (licenses.json)

[Dirs]
; Crea cartella per database e file licenze
Name: "C:\devcg-group\dbtest_prova"; Permissions: users-modify
Name: "{commonappdata}\CGEasy"; Permissions: users-modify

[Icons]
; Menu Start
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#MyAppExeName}"; IconIndex: 0
Name: "{group}\Disinstalla {#MyAppName}"; Filename: "{uninstallexe}"

; Desktop (opzionale, se selezionato) - FORZA l'icona dall'eseguibile
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; IconFilename: "{app}\{#MyAppExeName}"; IconIndex: 0

; Quick Launch (opzionale)
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon; IconFilename: "{app}\{#MyAppExeName}"; IconIndex: 0

[Run]
; Esegui l'applicazione dopo l'installazione (opzionale)
Filename: "{app}\{#MyAppExeName}"; Description: "Avvia {#MyAppName}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Elimina file di log e temporanei durante la disinstallazione (opzionale)
Type: filesandordirs; Name: "{localappdata}\CGEasy\logs"

[Code]
// Nota: Il controllo .NET è stato rimosso perché potrebbe dare falsi negativi
// L'installer installerà comunque il programma
// Se .NET 8.0 manca, Windows mostrerà un errore all'avvio del programma


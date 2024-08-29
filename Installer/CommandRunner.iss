; CommandRunner installer configuration
#define MyAppName "CommandRunner"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Adam Samec"
#define MyAppExecutable MyAppName + ".exe"

[CustomMessages]
en.MyDescription=Utility for running console commands with screen reader accessible output
en.LaunchAfterInstall=Start {#MyAppName} after finishing installation

[Setup]
OutputBaseFilename={#MyAppName}-{#MyAppVersion}-win32-setup
AppVersion={#MyAppVersion}
AppName={#MyAppName}
AppId={#MyAppName}
AppPublisher={#MyAppPublisher}
;PrivilegesRequired=lowest
DisableProgramGroupPage=yes
WizardStyle=modern
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
VersionInfoDescription={#MyAppName} Setup
VersionInfoProductName={#MyAppName}
; Uncomment the following line to disable the "Select Setup Language"
; dialog and have it rely solely on auto-detection.
;ShowLanguageDialog=no

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"

[Messages]
en.BeveledLabel=English

[Files]
Source: "..\{#MyAppName}\bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExecutable}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExecutable}"

[Run]
Filename: {app}\{#MyAppExecutable}; Description: {cm:LaunchAfterInstall,{#MyAppName}}; Flags: nowait postinstall skipifsilent

[Setup]
#define MainProg "../BorderlessGaming/bin/Release/net8.0-windows/win-x64/publish/BorderlessGaming.exe"
#define Major
#define Minor
#define Rev
#define Build
#define Version ParseVersion(MainProg, Major, Minor, Rev, Build)
#define AppVersion Str(Major)+"."+Str(Minor)+(Rev > 0 ? "."+Str(Rev) : "")
AppName=Borderless Gaming
AppPublisher=Andrew Sampson
AppCopyright=Copyright (C) 2014-2018 Andrew Sampson
DefaultDirName={pf}\Borderless Gaming
DefaultGroupName=Borderless Gaming
OutputDir=./
DisableReadyMemo=yes
DisableReadyPage=yes
SetupIconFile=../BorderlessGaming_new.ico
Compression=lzma/ultra64
SolidCompression=yes
LicenseFile=../LICENSE
Uninstallable=yes
; Needed to modify %AppData%
PrivilegesRequired=admin
DisableProgramGroupPage=yes
DirExistsWarning=no

; Shown as installed version (Programs & Features) as well as product version ('Details' tab when right-clicking setup program and choosing 'Properties')
AppVersion={#AppVersion}
; Stored in the version info for the setup program itself ('Details' tab when right-clicking setup program and choosing 'Properties')
VersionInfoVersion={#Version}
; Other version info
OutputBaseFilename=BorderlessGaming{#AppVersion}_admin_setup


; Shown in the setup program during install only
AppVerName=Borderless Gaming v{#AppVersion}

; Shown only in Programs & Features
AppContact=Borderless Gaming on Github
AppComments=Play your favorite games in a borderless window; no more time-consuming Alt-Tabs!
AppPublisherURL=https://github.com/Codeusa/Borderless-Gaming
AppSupportURL=https://github.com/Codeusa/Borderless-Gaming/issues
AppUpdatesURL=https://github.com/Codeusa/Borderless-Gaming/releases/latest
UninstallDisplayName=Borderless Gaming
; 691 KB as initial install
UninstallDisplaySize=929008
UninstallDisplayIcon={app}\BorderlessGaming.exe


[Messages]
BeveledLabel=Borderless Gaming {#AppVersion} Setup

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Files]
Source: ../BorderlessGaming/bin/Release/net8.0-windows/win-x64/publish/*; DestDir: {app}; Flags: ignoreversion recursesubdirs

Source: ../LICENSE; DestName: License.txt; DestDir: {app}
Source: ../README.md; DestName: Read Me.txt; DestDir: {app}
Source: ./uninstall.ico; DestDir: {app}

[Tasks]
;Name: quicklaunchicon; Description: {cm:CreateQuickLaunchIcon}; GroupDescription: {cm:AdditionalIcons}
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons};

[Icons]
Name: {commondesktop}\Borderless Gaming; Filename: {app}\BorderlessGaming.exe; WorkingDir: {app}; Tasks: desktopicon
Name: {group}\Borderless Gaming; Filename: {app}\BorderlessGaming.exe; WorkingDir: {app}
Name: {group}\Uninstall Borderless Gaming; Filename: {uninstallexe}; IconFileName: {app}\uninstall.ico
Name: {group}\License Agreement; Filename: {app}\License.txt
Name: {group}\Read Me; Filename: {app}\Read Me.txt

[Run]
Description: Start Borderless Gaming; Filename: {app}\BorderlessGaming.exe; Flags: nowait postinstall skipifsilent shellexec

[UninstallDelete]
Type: files; Name: {app}\License.txt
Type: files; Name: {app}\Read Me.txt
Type: files; Name: {app}\BorderlessGaming.exe
Type: files; Name: {app}\uninstall.ico
Type: files; Name: {app}\Languages.zip
Type: files; Name: {app}\BorderlessGaming.pdb
Type: files; Name: {app}\Facepunch.Steamworks.Win64.pdb
Type: files; Name: {app}\Facepunch.Steamworks.Win64.xml
Type: files; Name: {app}\steam_appid.txt

[Code]
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then begin
    if MsgBox('Do you want to delete your Borderless Gaming settings and preferences as well?', mbConfirmation, MB_YESNO) = IDYES 
    then begin
      DelTree(ExpandConstant('{userappdata}\borderless-gaming'), True, True, True)
    end;
  end;
end;

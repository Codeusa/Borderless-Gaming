$ErrorActionPreference = "Stop"
$root = (get-item $PSScriptRoot ).parent.FullName
$csproj = "$root\BorderlessGaming\BorderlessGaming.csproj"

$projectXml = [xml](Get-Content $csproj)
$versionXml = [xml](Get-Content "$root\version.xml")
$projectXml.Project.PropertyGroup[0].AssemblyVersion = "$($versionXml.borderlessgaming.version)"
$projectXml.Project.PropertyGroup[0].FileVersion = "$($versionXml.borderlessgaming.version)"
$projectXml.Project.PropertyGroup[0].Version = "$($versionXml.borderlessgaming.version)"
$projectXml.Save($csproj)

& bebopc
& dotnet restore "$cjproj"
& dotnet publish "$csproj" -c Release -r win-x64
& iscc "$root\Installers\BorderlessGaming_Standalone_Admin.iss"
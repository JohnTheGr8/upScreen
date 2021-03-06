# Define Variables

$Name       = "upScreen"
$ProjectDir = (Resolve-Path .\).Path + "\upScreen\"
$TargetDir  = $ProjectDir + "bin\Release\"

# Load built assembly and get the version

$TargetPath = "$TargetDir$Name.exe"

$assembly = [System.Reflection.Assembly]::LoadFile($TargetPath)
$v = $assembly.GetName().Version;
$version = [string]::Format("{0}.{1}.{2}",$v.Major, $v.Minor, $v.Build)

# NuGet pack

$nuspec = $ProjectDir + "upScreen.nuspec"
Write-Host "Path to nuspec file: " $nuspec

nuget pack $nuspec -Version $version -Properties Configuration=Release -OutputDirectory $TargetDir -BasePath $TargetDir

# Squirrel Releasify

$icon = $ProjectDir + "upscreen.ico"
Write-Host "Path to ico file: " $icon

$nupkg = "$TargetDir$Name.$version.nupkg"
Write-Host "Path to generated nupkg: " $nupkg

New-Alias squirrel $ProjectDir\packages\squirrel.windows*\tools\Squirrel.exe -Force

squirrel --releasify $nupkg --setupIcon $icon --icon $icon --no-msi | Write-Output

# Rename setup file

$setup = "Releases\Setup.exe"
$newSetup = "Releases\upScreen-$version-Setup.exe"

Move-Item $setup $newSetup -Force
Write-Host "Setup path: " $newSetup
# The following parameters should be passed from Visual Studio

param (
    [string]$Name,
    [string]$ProjectDir,
    [string]$TargetDir,
    [string]$TargetPath
)

# Load built assembly and get the version

$assembly = [System.Reflection.Assembly]::LoadFile($TargetPath)
$v = $assembly.GetName().Version;
$version = [string]::Format("{0}.{1}.{2}",$v.Major, $v.Minor, $v.Build)

# NuGet pack

$nuspec = "$ProjectDir$Name.nuspec"
Write-Host "Path to nuspec file: " $nuspec

nuget pack $nuspec -Version $version -Properties Configuration=Release -OutputDirectory $TargetDir -BasePath $TargetDir

# Squirrel Releasify

$icon = $ProjectDir + "upscreen.ico"
Write-Host "Path to ico file: " $icon

$nupkg = "upScreen.$version.nupkg"
Write-Host "Path to generated nupkg: " $nupkg

New-Alias squirrel $ProjectDir\packages\squirrel.windows*\tools\Squirrel.exe -Force

squirrel --releasify $nupkg --setupIcon $icon --icon $icon --no-msi | Write-Output
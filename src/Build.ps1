
# Global Variables
$currentDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptsDirectory = Join-Path $currentDirectory "scripts"

$projectDirectory = Split-Path -Parent $currentDirectory
$buildOutputDirectory = Join-Path $projectDirectory "buildoutput"

# Imports
Import-Module (Join-Path $scriptsDirectory "build-utility-functions.ps1")

# Build
$solutionPath = Join-Path $currentDirectory "SignalKo.SystemMonitor.sln"
Build-Solution -solutionPath $solutionPath -buildOutputFolder $buildOutputDirectory
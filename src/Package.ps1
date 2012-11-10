
# Global Variables
$currentDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptsDirectory = Join-Path $currentDirectory "scripts"
$toolsDirectory = Join-Path $currentDirectory "tools"

$nuDeployExecutablePath = Join-Path $toolsDirectory "NuDeploy\NuDeploy.exe"
$projectDirectory = Split-Path -Parent $currentDirectory
$publishLocation = Join-Path $projectDirectory "packages"

# Imports
Import-Module (Join-Path $scriptsDirectory "build-utility-functions.ps1")

# Package the Agent
$solutionPath = Join-Path $currentDirectory "SignalKo.SystemMonitor.Agent.sln"
Package-Solution -nuDeployExePath $nuDeployExecutablePath -solutionPath $solutionPath -publishLocation $publishLocation

# Package the Web Monitor
$solutionPath = Join-Path $currentDirectory "SignalKo.SystemMonitor.Web.sln"
Package-Solution -nuDeployExePath $nuDeployExecutablePath -solutionPath $solutionPath -publishLocation $publishLocation

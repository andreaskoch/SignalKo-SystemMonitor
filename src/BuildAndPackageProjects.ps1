
# Global Variables
$currentDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$toolsDirectory = Join-Path $currentDirectory "tools"
$projectDirectory = Split-Path -Parent $currentDirectory

$solutionPath = Join-Path $currentDirectory "SignalKo.SystemMonitor.Agent.sln"

$nuDeployDirectory = Join-Path $toolsDirectory "NuDeploy"
$nuDeployExecutablePath = Join-Path $nuDeployDirectory "NuDeploy.exe"
$nuDeployPackageCommand = "$nuDeployExecutablePath packagesolution `"-SolutionPath=$solutionPath`" `"-BuildConfiguration=Release`""

set-location $nuDeployDirectory
Invoke-Expression $nuDeployPackageCommand
$packageSucceeded = ($LASTEXITCODE -eq 0)
set-location $currentDirectory

if ($packageSucceeded -eq $false)
{
    Throw "Packaging failed. Executed command: $nuDeployPackageCommand"
}
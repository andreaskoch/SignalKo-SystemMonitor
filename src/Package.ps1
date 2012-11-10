
# Global Variables
$currentDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$toolsDirectory = Join-Path $currentDirectory "tools"
$nuDeployExecutablePath = Join-Path $toolsDirectory "NuDeploy\NuDeploy.exe"
$projectDirectory = Split-Path -Parent $currentDirectory

# Functions
Function Package-Solution
{
   [CmdletBinding()]
    Param(
        [Parameter(Position=0, Mandatory=$True, ValueFromPipeline=$True)]
        [string]$nuDeployExePath,    
    
        [Parameter(Position=1, Mandatory=$True, ValueFromPipeline=$True)]
        [string]$solutionPath
    )
	
	if ((Test-Path -Path $nuDeployExePath) -eq $false) {
		Throw "NuDeploy was not found at `"$nuDeployExePath`""
	}
	
	if ((Test-Path -Path $solutionPath) -eq $false) {
		Throw "No solution file found at `"$solutionPath`""
	}
	
	$solutionFilename = (Get-Item $solutionPath).FullName
	Write-Host "Packaging solution `"$solutionFilename`"."
	
	$previousLocation = get-location
	$nuDeployDirectory = Split-Path -Parent $nuDeployExePath
	$nuDeployPackageCommand = "$nuDeployExecutablePath packagesolution `"-SolutionPath=$solutionPath`" `"-BuildConfiguration=Release`""

	# Switch to the NuDeploy folder
	set-location $nuDeployDirectory
	
	# Run NuDeploy
	Invoke-Expression $nuDeployPackageCommand
	$packageSucceeded = ($LASTEXITCODE -eq 0)
	
	# Switch back to the previous location
	set-location $previousLocation

	if ($packageSucceeded -eq $false)
	{
		Throw "Packaging the solution `"$solutionPath`" failed (Command: $nuDeployPackageCommand)."
	}
}

# Package the Agent
$solutionPath = Join-Path $currentDirectory "SignalKo.SystemMonitor.Agent.sln"
Package-Solution -nuDeployExePath $nuDeployExecutablePath -solutionPath $solutionPath

# Package the Web Monitor
$solutionPath = Join-Path $currentDirectory "SignalKo.SystemMonitor.Web.sln"
Package-Solution -nuDeployExePath $nuDeployExecutablePath -solutionPath $solutionPath

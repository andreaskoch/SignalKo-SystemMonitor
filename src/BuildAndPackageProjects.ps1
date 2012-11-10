
# Global Variables
$currentDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptsDirectory = Join-Path $currentDirectory "scripts"
$toolsDirectory = Join-Path $currentDirectory "tools"
$deploymentPackageAdditionsDirectory = Join-Path $currentDirectory "deployment"
$projectDirectory = Split-Path -Parent $currentDirectory

# Imports
Import-Module (Join-Path $scriptsDirectory "Build.ps1")


################################################################################################


#################
# Build - Agent #
#################
$buildOutputDirectory = Join-Path $projectDirectory "build"
$publishedWebsitesDirectory = Join-Path $buildOutputDirectory "_PublishedWebsites"
$publishedApplicationsDirectory = Join-Path $buildOutputDirectory "_PublishedApplications"
$solutionPath = Join-Path $currentDirectory "SignalKo.SystemMonitor.Agent.sln"

Build-Solution -solutionPath $solutionPath -buildOutputFolder $buildOutputDirectory -buildConfiguration "Release" -targetPlatform "Any CPU"


#################
# Merge - Agent #
#################
$ilMergeExecutablePath = Join-Path $toolsDirectory "ILMerge\ILMerge.exe"
$buildResultAgentDirecotry = Join-Path $publishedApplicationsDirectory "Agent"
$targetFilename = "SystemMonitorAgent"

$sourceFiles = Get-ChildItem $buildResultAgentDirecotry\*.* -include *.exe,*.dll,$targetFilename | Foreach { $_.Name } | Where { $_ -notlike "$targetFilename*" }

$exeSourceFiles = $sourceFiles | Where { $_ -like "*.exe" }
$otherSourceFiles = $sourceFiles | Where { $_ -notlike "*.exe" }

$sourceFilesParameter = $exeSourceFiles -join " "
$sourceFilesParameter += " " + $otherSourceFiles -join " "

$netFrameworkFolder = "$($env:ProgramFiles)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"
if ((Test-Path $netFrameworkFolder) -eq $false){
	$netFrameworkFolder = "${env:ProgramFiles(x86)}\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"
}

# Execute merge
set-location $buildResultAgentDirecotry

$mergeCommand = "$ilMergeExecutablePath /targetplatform:`"v4,$netFrameworkFolder`" /internalize:`"ilmerge.internalize.ignore.txt`" /target:exe /out:`"$targetFilename.exe`" /allowDup $sourceFilesParameter"
Invoke-Expression $mergeCommand
$mergeSucceeded = ($LASTEXITCODE -eq 0)

set-location $currentDirectory

if ($mergeSucceeded -eq $false)
{
    Throw "Merge failed. Executed command: $mergeCommand"
}

# Rename the config file
Get-ChildItem -Path $buildResultAgentDirecotry -Filter *.config | Rename-Item -NewName "$targetFilename.exe.config"

# Delete all source files that have been used for the merge
Get-ChildItem -Path $buildResultAgentDirecotry | Where { ($_ -notlike "$targetFilename*") } | Remove-Item


###################
# Package - Agent #
###################
$packageDirectory = Join-Path $projectDirectory "packages"
$nuDeployDirectory = Join-Path $toolsDirectory "NuDeploy"
$nuDeployExecutablePath = Join-Path $nuDeployDirectory "NuDeploy.exe"
$buildOutputDeploymentPackageAdditions = Join-Path $buildOutputDirectory "deploymentpackageadditions"
$agentDeploymentPackageAdditions =  Join-Path $deploymentPackageAdditionsDirectory "SignalKo.SystemMonitor.Agent"

# Add Deployment Package Additions to build result folder
Copy-Item $agentDeploymentPackageAdditions -Destination $buildOutputDeploymentPackageAdditions -Recurse

# Package the build output
set-location $nuDeployDirectory

$nuDeployPackageCommand = "$nuDeployExecutablePath packagebuildoutput `"-BuildOutputPath=$buildOutputDirectory`""
Invoke-Expression $nuDeployPackageCommand
$packageSucceeded = ($LASTEXITCODE -eq 0)

set-location $currentDirectory

if ($packageSucceeded -eq $false)
{
    Throw "Packaging failed. Executed command: $nuDeployPackageCommand"
}
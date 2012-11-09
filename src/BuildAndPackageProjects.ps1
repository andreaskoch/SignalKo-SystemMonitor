
# Global Variables
$currentDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptsDirectory = Join-Path $currentDirectory "scripts"
$toolsDirectory = Join-Path $currentDirectory "tools"
$projectDirectory = Split-Path -Parent $currentDirectory

$binariesDirectory = Join-Path $projectDirectory "binaries"
$packageDirectory = Join-Path $projectDirectory "packages"

$buildOutputDirectory = Join-Path $projectDirectory "build"
$publishedWebsitesDirectory = Join-Path $buildOutputDirectory "_PublishedWebsites"
$publishedApplicationsDirectory = Join-Path $buildOutputDirectory "_PublishedApplications"

$prepackagingDirectory = Join-Path $projectDirectory "prepackaging"
$prepackagingContentDirectory = Join-Path $prepackagingDirectory "content"

# Imports
Import-Module (Join-Path $scriptsDirectory "Build.ps1")

# Build
$solutionPath = Join-Path $currentDirectory "SignalKo.SystemMonitor.Agent.sln"
Build-Solution -solutionPath $solutionPath -buildOutputFolder $buildOutputDirectory -buildConfiguration "Release" -targetPlatform "Any CPU"

# Merge Agent
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

$mergeCommand = "$ilMergeExecutablePath /targetplatform:`"v4,$netFrameworkFolder`" /internalize:`"ilmerge.internalize.ignore.txt`" /target:exe /out:`"$targetFilename.exe`" /allowDup $sourceFilesParameter"

# Merge
$previousLocation = get-location
set-location $buildResultAgentDirecotry

Invoke-Expression $mergeCommand
$mergeSucceeded = ($LASTEXITCODE -eq 0)

set-location $previousLocation

if ($mergeSucceeded -eq $false)
{
    Throw "Merge failed. Executed command: $mergeCommand"
}

# Rename the config file
Get-ChildItem -Path $buildResultAgentDirecotry -Filter *.config | Rename-Item -NewName "$targetFilename.exe.config"

# Delete all source files that have been used for the merge
Get-ChildItem -Path $buildResultAgentDirecotry | Where { ($_ -notlike "$targetFilename*") } | Remove-Item

# Package SignalKo.SystemMonitor.Agent
$agentBinariesDirectory = Join-Path $binariesDirectory "SignalKo.SystemMonitor.Agent"

# copy package content to prepackaging folder
# Get-ChildItem -Path "$agentBinariesDirectory\*.*" -include *.exe | Copy-Item -Destination $prepackagingContentDirectory
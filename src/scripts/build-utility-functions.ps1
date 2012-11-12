
Function Prepare-Directory
{
	[CmdletBinding()]
	Param(
		[Parameter(Position=0, Mandatory=$True, ValueFromPipeline=$True)]
		[string]$path
	)

	if ((Test-Path $path) -eq $false)
	{
		New-Item $path -type directory
	}
	else
	{
		if ((Get-ChildItem $path).Count -gt 0)
		{
			Remove-Item -Recurse -Force $path
			if ($? -eq $false) {
				Throw "Could not cleanup the directory `"$path`""
			}
		}
	}
	
}

Function Build-Solution
{
	[CmdletBinding()]
	Param(
		[Parameter(Position=0, Mandatory=$True, ValueFromPipeline=$True)]
		[string]$solutionPath,
		
		[Parameter(Position=1, Mandatory=$True, ValueFromPipeline=$True)]
		[string]$buildOutputFolder,		
		
		[Parameter(Position=2, Mandatory=$False, ValueFromPipeline=$True)]
		[string]$buildConfiguration="Release",
		
		[Parameter(Position=3, Mandatory=$False, ValueFromPipeline=$True)]
		[string]$targetPlatform="Any CPU"
	)
	
	if ((Test-Path $solutionPath) -eq $false)
	{
		Throw "The soltuion file `"$solutionPath`" was not found."
	}
	
	Prepare-Directory -path $buildOutputFolder

	$buildCommand = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild `"$solutionPath`" /p:Configuration=`"$buildConfiguration`" /p:Platform=`"$targetPlatform`" /p:OutputPath=`"$buildOutputFolder`" /t:Rebuild"
	Invoke-Expression $buildCommand
	$buildSucceeded = ($LASTEXITCODE -eq 0)

	if ($buildSucceeded -eq 0)
	{
		Throw "Building solution `"$solutionPath`" failed."
	}
}

Function Package-Solution
{
	[CmdletBinding()]
	Param(
		[Parameter(Position=0, Mandatory=$True, ValueFromPipeline=$True)]
		[string]$nuDeployExePath,
	
		[Parameter(Position=1, Mandatory=$True, ValueFromPipeline=$True)]
		[string]$solutionPath,
		
		[Parameter(Position=2, Mandatory=$False, ValueFromPipeline=$True)]
		[string]$publishLocation
	)
	
	if ((Test-Path -Path $nuDeployExePath) -eq $false)
	{
		Throw "NuDeploy was not found at `"$nuDeployExePath`""
	}
	
	if ((Test-Path -Path $solutionPath) -eq $false)
	{
		Throw "No solution file found at `"$solutionPath`""
	}
	
	$solutionFilename = (Get-Item $solutionPath).FullName
	Write-Host "Packaging solution `"$solutionFilename`"."
	
	$previousLocation = get-location
	$nuDeployDirectory = Split-Path -Parent $nuDeployExePath
	$nuDeployPackageCommand = "$nuDeployExecutablePath packagesolution `"-SolutionPath=$solutionPath`" `"-BuildConfiguration=Release`""
	
	# Register Publishing Profile
	if ($publishLocation -ne $null)
	{
		if ((Test-Path -Path $publishLocation) -eq $false)
		{
			New-Item $publishLocation -type directory
		}
	
		$publishConfigurationName = "Publish"
		$ensurePublishConfigurationExistsCommand = "$nuDeployExecutablePath targets `"-Action=add`" `"-Name=$publishConfigurationName`" `"-Location=$publishLocation`""
		set-location $nuDeployDirectory	
		Invoke-Expression $ensurePublishConfigurationExistsCommand
		$addPublishConfigurationSucceeded = ($LASTEXITCODE -eq 0)
		
		if ($addPublishConfigurationSucceeded -eq $true)
		{
			$nuDeployPackageCommand += " `"-PublishingConfiguration=$publishConfigurationName`""
		}
	}	

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

Function Install-Package
{
	[CmdletBinding()]
	Param(
		[Parameter(Position=0, Mandatory=$True, ValueFromPipeline=$True)]
		[string]$nuDeployExePath,
		
		[Parameter(Position=1, Mandatory=$True, ValueFromPipeline=$True)]
		[string]$packageId,    
	
		[Parameter(Position=2, Mandatory=$True, ValueFromPipeline=$True)]
		[string]$sourceRepositoryPath
	)
	
	if ((Test-Path -Path $nuDeployExePath) -eq $false)
	{
		Throw "NuDeploy was not found at `"$nuDeployExePath`""
	}
	
	if ((Test-Path -Path $sourceRepositoryPath) -eq $false)
	{
		Throw "The source repository `"$sourceRepositoryPath`" does not exist."
	}
	
	Write-Host "Installing package `"$packageId`""
	
	$nuDeployDirectory = Split-Path -Parent $nuDeployExePath
	$previousLocation = get-location
	
	# Register Source Repository
	$sourceRepositoryName = "Source"
	$registerSourceRepositoryCommand = "$nuDeployExePath sources `"-Action=add`" `"-Name=$sourceRepositoryName`" `"-Url=$sourceRepositoryPath`""
	
	set-location $nuDeployDirectory
	Invoke-Expression $registerSourceRepositoryCommand
	$registerSourceRepositorySucceeded = ($LASTEXITCODE -eq 0)
	set-location $previousLocation
	
	if ($registerSourceRepositorySucceeded -eq $false)
	{
		Throw "Could not register the path `"$sourceRepositoryPath`" as a source repository."
	}
	
	# Install Package
	$installPackageCommand = "$nuDeployExePath install `"-NugetPackageId=$packageId`" `"-DeploymentType=Full`""
	
	set-location $nuDeployDirectory
	Invoke-Expression $installPackageCommand
	$installationSucceeded = ($LASTEXITCODE -eq 0)
	set-location $previousLocation
	
	if ($installationSucceeded -eq $false)
	{
		Throw "Installation of the package `"$packageId`" failed."
	}	
}
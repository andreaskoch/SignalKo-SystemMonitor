
Function Build-Solution {

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
	
	if ((Test-Path $solutionPath) -eq $false) {
		Throw "The soltuion file `"$solutionPath`" was not found."
	}
	
	if ((Get-ChildItem $buildOutputFolder).Count -gt 0)
	{
		Remove-Item -Recurse -Force $buildOutputFolder
        if ($? -eq $false) {
            Throw "Could not cleanup the build output folder `"$buildOutputFolder`""
        }
	}

	$buildCommand = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild `"$solutionPath`" /p:Configuration=`"$buildConfiguration`" /p:Platform=`"$targetPlatform`" /p:OutputPath=`"$buildOutputFolder`" /t:Rebuild"
	Invoke-Expression $buildCommand
	$buildSucceeded = ($LASTEXITCODE -eq 0)

	if ($buildSucceeded -eq 0) {
		Throw "Building solution `"$solutionPath`" failed."
	}
}

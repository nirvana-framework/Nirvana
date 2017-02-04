function package(){
param($versionNumber)
	$scriptblocks = @()
	$nuspecs= dir *.nuspec -Recurse -ErrorAction SilentlyContinue |   Where-Object {
		$_.FullName -NotLike "*obj*" -and $_.FullName -NotLike  "*\.bower\cache*" -and $_.FullName -NotLike  "*npm*"-and $_.FullName -NotLike  "*bower_components*" -and $_.FullName -NotLike  "*test*"
	}
	$exe = [string](Resolve-Path "$env:ChocolateyInstall\lib\NuGet.CommandLine\tools\nuget.exe" )
	
	if( (($env:PACKAGE_PARALLEL) -eq $null) -or (($env:PACKAGE_PARALLEL) -ne "true" ) ){
		"running serial package creation" | out-host	
		foreach($nuspec in $nuspecs){	
			"packaging $($nuspec.FullName)" | out-host
			Push-Location $nuspec.Directory
				& $exe pack  $($nuspec.FullName) -Version $versionNumber -NoPackageAnalysis -OutputDirectory ..\  | Out-File "$TestAssetPath\packaging.log" -Append	
			Pop-Location
		}	
	}
	else{	
		"running parallel package creation" | out-host	
		foreach($nuspec in $nuspecs){
			$scriptblocks +=  [scriptblock]::Create({
		    "packaging $($nuspec.FullName)" | out-host
			Push-Location $nuspec.Directory
				cd $($nuspec.Directory)
				& $exe pack  $($nuspec.FullName) -Version $versionNumber -NoPackageAnalysis -OutputDirectory ..\  | Out-File "$TestAssetPath\packaging.log" -Append
			})
			Pop-Location
		}
		foreach($block in $scriptblocks){
			Start-Job -ScriptBlock $block 
		}
		wait_for_jobs_to_finish "packaging.log"
	}
}

filter Where-NotMatch($Selector,[String[]]$Like,[String[]]$Regex) {

    if ($Selector -is [String]) { $Value = $_.$Selector }
    elseif ($Selector -is [ScriptBlock]) { $Value = &$Selector }
    else { throw 'Selector must be a ScriptBlock or property name' }
    if ($Like.Length) {
        foreach ($Pattern in $Like) {
            if ($Value -like $Pattern) { return }
        }
    }

    if ($Regex.Length) {
        foreach ($Pattern in $Regex) {
            if ($Value -match $Pattern) { return }
        }
    }
    return $_
}



function fail-on-missed-packaging(){
param($searchString, $pattern, $logName)
	$result = Select-String -Path $searchString -Pattern $pattern
	if($result -eq $null){
		$message = "could not find $pattern when searching for $logName packaging"
		throw $message
	}
	else
	{
		$result  | foreach { $_ | Out-File "$TestAssetPath\packaging.log" -Append}
	}
}
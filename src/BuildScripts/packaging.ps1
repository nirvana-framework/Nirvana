function package(){
param($versionNumber)
	$scriptblocks = @()
	$nuspecs= dir *.nuspec -Recurse -ErrorAction SilentlyContinue |   Where-Object {
		$_.FullName -NotLike "*obj*" -and $_.FullName -NotLike  "*\.bower\cache*" -and $_.FullName -NotLike  "*npm*"-and $_.FullName -NotLike  "*bower_components*" -and $_.FullName -NotLike  "*test*"
	}
	$exe = [string](Resolve-Path "$env:ChocolateyInstall\lib\NuGet.CommandLine\tools\nuget.exe" )
		
	foreach($nuspec in $nuspecs){	
		"packaging $($nuspec.FullName)" | out-host
		& $exe pack  $($nuspec.FullName) -Version $versionNumber -NoPackageAnalysis -OutputDirectory buildoutput  | Out-File "buildoutput\packaging.log" -Append	
		
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
param($searchFileName, $pattern, $logName)
	$result = Select-String -Path $searchFileName -Pattern $pattern
	if($result -eq $null){
		$message = "could not find $pattern when searching for $logName packaging"
		throw $message
	}
	else
	{
		$result  | foreach { $_ | Out-File "buildoutput\packaging.log" -Append}
	}
}
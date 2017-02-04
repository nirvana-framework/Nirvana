#$framework = '4.0x64'
Framework "4.6.1"
properties {
	$branchname='master'
}


function initialize-build(){
	get-job|remove-job
	$path= Resolve-Path ./
	$scripts =@()
	$ElapsedTime = [System.Diagnostics.Stopwatch]::StartNew()			
	remove-item *.log	
	Get-ChildItem (Join-Path ($path) \BuildScripts\*.ps1)  | ForEach { 		$scripts += $_.FullName	} 
	Get-ChildItem (Join-Path ($path) \SolutionScripts\*.ps1)  | ForEach { 		$scripts += $_.FullName	} 
	$scripts | ForEach { . $_} 
	$source =[string] $path
	ensure-build-number
	$TestAssetPath = get-test-asset-path($source)
	log "Test Asset Path: $source\..\"
	log "Runtime Directory: $([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory())"
	Log "Complete : $($ElapsedTime.Elapsed)"
}

task compile -depends ShowMsBuildVersion {
	. initialize-build	
	restore-packages $source $TestAssetPath
	clean-and-build #buildutilities.ps1		
}

task rebuild  -depends ShowMsBuildVersion{	
	. initialize-build	
	restore-packages $source $TestAssetPath
	rebuild-without-clean	
}

task incrementalbuild  -depends ShowMsBuildVersion{	
	. initialize-build	
	restore-packages $source $TestAssetPath
	incremental-build-without-clean		
}

task buildAndPackage  -depends ShowMsBuildVersion{	
	#build and package
	. initialize-build	
	restore-packages $source $TestAssetPath
	build-and-package #buildutilities.ps1		
}

task ShowMsBuildVersion {
  msbuild /version
}









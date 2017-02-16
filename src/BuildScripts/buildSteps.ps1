

function clean-build-logs-and-test-data()
{
	log "Resetting  logs"	
	Remove-Item "buildoutput\*.log"  | out-null
	Remove-Item "buildoutput\*.nupkg"  | out-null
	Remove-Item "buildoutput\*.coverage"  | out-null
	Remove-Item "buildoutput\*.coverage.raw"  | out-null
	Get-ChildItem "buildoutput\*.xml" | foreach ($_){remove-item $_.FullName } 
}

function restore-packages(){
	param($source,$TestAssetPath)
	log "Restoring Packages"
	cmd.exe /c "restoreSolutionPackages.bat" | Out-File "buildoutput\restore.log" 
}
function  clean-and-build(){

	log "Cleaning solution and removing old build packages"
	clean-build-logs-and-test-data

	exec { msbuild Nirvana.sln /m /t:clean /v:n /nr:false | out-file "buildoutput\build.log" -Append	}

	log "Building Solution"
	exec { msbuild Nirvana.sln /m  /t:Rebuild /v:n /nr:false | out-file "buildoutput\build.log" -Append }
}

function  rebuild-without-clean(){
	exec { msbuild Nirvana.sln /m /t:rebuild /v:n /nr:false | out-file "buildoutput\build.log" -Append }
}

function  incremental-build-without-clean(){
	log "Building Solution"
	exec { msbuild Nirvana.sln /m /t:build /v:n /nr:false | out-file "buildoutput\build.log" -Append }
}


function build-and-package(){
	clean-and-build 
	package_all_projects 
	validate-packaging
}

function package_all_projects(){		
	log "Building Packages"	
	package($env:BUILD_NUMBER) 
}

function validate-packaging (){
}



function run_analysis(){
param($sourcePath)
	Write-Host "Generating Coverage report"		
	$outputPath = resolve-path ".\buildoutput"
	$chocoPath = "$env:ChocolateyInstall\lib"
	$xunitLocation = "packages\xunit.runner.console.2.1.0\tools\xunit.console.exe"	
	$testLocation="TechFu.Nirvana.Tests\bin\Debug\TechFu.Nirvana.Tests.dll"
	$target="-target:`"$xunitLocation`""
	$targetArgs="-targetargs:`"$testLocation`""
	$searchdirs="-searchdirs:`"TechFu.Nirvana\bin\Debug`""
	$register="-register:user"
	$outputFile="-output:$outputPath\coverage.xml"

	$openCverLocation ="packages\OpenCover.4.6.519\tools\OpenCover.Console.exe"
	$paramList="$target","$targetArgs","$searchdirs","$register","$outputFile"
	& $openCverLocation $paramList
	if($env:COVERALLS_REPO_TOKEN -ne $null){	
		Write-Host "Uploading to Coveralls"		
		$coverallsLocation ="packages\coveralls.io.1.3.4\tools\coveralls.net.exe"
		$caparams="--opencover","$outputPath\coverage.xml"
		& $coverallsLocation $caparams 
	}
	else{
		Write-Host "Building Report"		
		$gen="packages\ReportGenerator.2.5.2\tools\ReportGenerator.exe"
		$rparams= "-reports:`"$outputPath\coverage.xml`"","-targetdir:`"$outputPath\OpenCover\UnitTestReport`""
		& $gen $rparams
		Start-Process "chrome.exe" "$outputPath\OpenCover\UnitTestReport\index.htm"		
	}




}

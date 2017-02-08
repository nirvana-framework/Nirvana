

function clean-build-logs-and-test-data()
{

	log "Resetting  logs"
	
	Remove-Item "$TestAssetPath\*.log"  | out-null
	Remove-Item "$TestAssetPath\*.nupkg"  | out-null
	Remove-Item "$TestAssetPath\*.coverage"  | out-null
	Remove-Item "$TestAssetPath\*.coverage.raw"  | out-null
	Get-ChildItem "$TestAssetPath\*.xml" | foreach ($_){remove-item $_.FullName } 
}

function restore-packages(){
	param($source,$TestAssetPath)
	log "Restoring Packages"
	cmd.exe /c "$source\restoreSolutionPackages.bat" | Out-File "$TestAssetPath\restore.log" 
}
function  clean-and-build(){

	log "Cleaning solution and removing old build packages"
	clean-build-logs-and-test-data

	exec { msbuild TechFu.Nirvana.sln /m /t:clean /v:n /nr:false | out-file "$TestAssetPath\build.log" -Append	}

	log "Building Solution"
	exec { msbuild TechFu.Nirvana.sln /m  /t:Rebuild /v:n /nr:false | out-file "$TestAssetPath\build.log" -Append }
}

function  rebuild-without-clean(){
	exec { msbuild TechFu.Nirvana.sln /m /t:rebuild /v:n /nr:false | out-file "$TestAssetPath\build.log" -Append }
}

function  incremental-build-without-clean(){
	log "Building Solution"
	exec { msbuild TechFu.Nirvana.sln /m /t:build /v:n /nr:false | out-file "$TestAssetPath\build.log" -Append }
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
	
	$chocoPath = "$env:ChocolateyInstall\lib"
	$xunitLocation = "packages\xunit.runner.console.2.1.0\tools\xunit.console.exe"
	
	$testLocation="TechFu.Nirvana.Tests\bin\Debug\TechFu.Nirvana.Tests.dll"
	$target="-target:`"$xunitLocation`""
	$targetArgs="-targetargs:`"$testLocation`""
	$searchdirs="-searchdirs:`"TechFu.Nirvana\bin\Debug`""
	$register="-register:user"
	$outputFile="-output:$sourcePath\coverage.xml"

	$openCverLocation ="$sourcePath\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe"
	$paramList="$target","$targetArgs","$searchdirs","$register","$outputFile"
	& $openCverLocation $paramList

	
	if($env:COVERALLS_REPO_TOKEN -ne $null){	
		$coverallsLocation ="$sourcePath\packages\coveralls.io.1.3.4\tools\coveralls.net.exe"
		$caparams="--opencover","coverage.xml"
		& $coverallsLocation $caparams 
	}
	else{
		Write-Host "Building Report"		
		$gen="packages\ReportGenerator.2.5.2\tools\ReportGenerator.exe"
		$rparams= "-reports:`"$sourcePath\coverage.xml`"","-targetdir:`"$sourcePath\..\OpenCover\UnitTestReport`""
		& $gen $rparams
		Start-Process "chrome.exe" "$sourcePath\..\OpenCover\UnitTestReport\index.htm"		
	}




}

function dummy(){


 %ChocolateyInstall%\lib\opencover.portable\tools\OpenCover.Console.exe -target:"%ChocolateyInstall%\lib\XUnit\tools\xunit\xunit.console.exe" ^
 -targetargs:"TechFu.Nirvana.Tests\bin\Debug\TechFu.Nirvana.Tests.dll TechFu.Nirvana.Tests\bin\Debug\TechFu.Nirvana.Tests.dll" ^
 -searchdirs:"QS.Core\bin\Debug;packages\CsvHelper.2.2.2\lib\net40-client;packages\structuremap.4.1.1.372\lib\dotnet" ^
 -output:"..\unittest.coverage.raw.xml" ^
 -filter:"+[*]* -[UnitTests]* -[StructureMap]* -[CsvHelper]* -[Octokit]* " ^
 -register:user




 $a = "$sourcePath\generatemetrics.bat" 
	 & $a | Out-Null

	$reportPath = "$sourcePath\..\unittest.coverage.raw.xml"
	$uploaderPath = "$sourcePath\..\CodecovUploader.sh"
	$arguments = "-f $reportPath -t fd2ea931-1b48-4597-a850-fbe58c58020e -X gcov"
	
	if ($env:BUILD_NUMBER -ne "1.0.0.0") {	
		Write-Host "uploading to codecov"				
		Invoke-WebRequest -Uri "https://codecov.io/bash" -OutFile $uploaderPath
		Start-Process $uploaderPath $arguments -Wait
		#Invoke-Expression "$uploaderPath -f $reportPath -t fd2ea931-1b48-4597-a850-fbe58c58020e -X gcov"	
	}
	else{	
		Write-Host "Building Report"		
		$gen="$packages\reportgenerator.portable\tools\ReportGenerator.exe"
		& $gen "-reports:$sourcePath\..\unittest.coverage.raw.xml" "-targetdir:$sourcePath\..\OpenCover\UnitTestReport"
		Start-Process "chrome.exe" "$sourcePath\..\OpenCover\UnitTestReport\index.htm"		
	}
}



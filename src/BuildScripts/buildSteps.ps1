

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

	exec { msbuild MultiDashboard.sln /m /t:clean /v:n /nr:false | out-file "$TestAssetPath\build.log" -Append	}

	log "Building Solution"
	exec { msbuild MultiDashboard.sln /m  /t:Rebuild /v:n /nr:false | out-file "$TestAssetPath\build.log" -Append }
}

function  rebuild-without-clean(){
	exec { msbuild MultiDashboard.sln /m /t:rebuild /v:n /nr:false | out-file "$TestAssetPath\build.log" -Append }
}

function  incremental-build-without-clean(){
	log "Building Solution"
	exec { msbuild MultiDashboard.sln /m /t:build /v:n /nr:false | out-file "$TestAssetPath\build.log" -Append }
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


$global:appCmdExe = 'C:\Program Files (x86)\IIS Express\appcmd.exe' 

function global:Configure-IIS()
{
	Stop-Process -ProcessName iisexpress*

	#Configure-Application-IIS "TechFu.LiveOnPage.Api" "local-api.lop.mean.software" 24165 54403
	#Configure-Application-IIS "TechFu.LiveOnPage.UI" "local-UI.lop.mean.software" 24166 54404
	#Configure-Application-IIS "TechFu.Nirvana.WebApi.Sample" "local-nirvanaAPI.mean.software" 24167 54405
}


function global:Configure-UI-IIS()
{
	param([string] $folderName, [string] $siteName, [int] $httpPort, [int] $httpsPort)

	$webFolder  = Resolve-Path ".\\$folderName"

	remove-host $siteName
	add-host "127.0.0.1" $siteName
	add-host "::1" $siteName


	remove-iis-applicationhost -applicationName $siteName -physicalPath $webFolder
	add-iis-applicationhost -applicationName $siteName $httpPort $httpsPort $webFolder

	Write-Host $parent

}
function global:Configure-Application-IIS([string] $application, [string] $siteName, [int] $httpPort, [int] $httpsPort, [string] $physicalPath)
{
	remove-iis-applicationhost $application $physicalPath
	add-iis-applicationhost $application $siteName $httpPort $httpsPort $physicalPath
	remove-host $siteName
	add-host "127.0.0.1" $siteName
	add-host "::1" $siteName
	remove-sslcertificate $siteName $httpsPort
	install-sslcertificate $siteName $httpsPort
}



function global:add-iis-applicationhost([string] $applicationName, [string] $hostName, [string] $httpPort, [string] $httpsPort, [string] $physicalPath)
{
	if (-not $physicalPath) {
		$physicalPath = Resolve-Path $applicationName;
	}
	Write-Host "Adding sites in applicationhost.config file for $physicalPath"

	$bindings =
		"http/*:" + $httpPort + ":" + $hostName +
		",https/*:" + $httpsPort + ":" + $hostName +
		",http/*:" + $httpPort + ":localhost" +
		",https/*:" + $httpsPort + ":localhost";

	& $appCmdExe add site /name:$applicationName /bindings:$bindings
	& $appCmdExe add app /site.name:$applicationName /path:"/" /physicalpath:$physicalPath /applicationPool:"Clr4IntegratedAppPool"
}

function global:remove-iis-applicationhost([string] $applicationName, [string] $physicalPath)
{
	if (-not $physicalPath) {
		$physicalPath = Resolve-Path $applicationName
	}
	Write-Host "Deleting sites in applicationhost.config file that use path $physicalPath"

	& $appCmdExe list vdir /physicalPath:$physicalPath /xml | & $appCmdExe list app /xml /in | & $appCmdExe list site /xml /in | & $appCmdExe delete site /in
}




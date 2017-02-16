$global:appCmdExe = 'C:\Program Files (x86)\IIS Express\appcmd.exe' 

function global:Configure-IIS()
{
	Stop-Process -ProcessName iisexpress*

	Configure-Application-IIS "Nirvana.WebApi.Sample" "Samples\Simple WebApi\Nirvana.WebApi.Sample" "local-samplenirvanaapi.mean.software" 24167 54405 
	Configure-Application-IIS "Nirvana.EventStoreSample.WebAPI.Commands" "Samples\EventStore\Nirvana.EventStoreSample.WebAPI.Commands" "local-commandAPI.mean.software" 24168 54406
	Configure-Application-IIS "Nirvana.EventStoreSample.UI" "Samples\EventStore\Nirvana.EventStoreSample.UI" "local-eventsourcesample.mean.software" 24169 54407
	Configure-Application-IIS "Nirvana.EventStoreSample.WebAPI.CommandProcessor" "Samples\EventStore\Nirvana.EventStoreSample.WebAPI.CommandProcessor" "local-commandProcessor.mean.software" 24170 54408
	Configure-Application-IIS "Nirvana.EventStoreSample.WebAPI.Notifications" "Samples\EventStore\Nirvana.EventStoreSample.WebAPI.Notifications" "local-uinotifications.mean.software" 24171 54409
	Configure-Application-IIS "Nirvana.EventStoreSample.WebAPI.Queries" "Samples\EventStore\Nirvana.EventStoreSample.WebAPI.Queries" "local-queryapi.mean.software" 24172 54410
}

function global:Configure-Application-IIS([string] $application,[string]$physicalPath , [string] $siteName, [int] $httpPort, [int] $httpsPort)
{

	$physicalPath = Resolve-Path ".\\$physicalPath"

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




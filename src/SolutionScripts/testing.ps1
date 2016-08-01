

function global:Debug-iis-Express()
{
	$DTE.Debugger.LocalProcesses | where-object -FilterScript {$_.Name -like "*iisexpress.exe*" } | ForEach-Object {Write-Host $_.Attach()}
}

#start iis with the console window up, so we can see trace messages from the website while were doing front end development.
function global:start-iis(){
param($site,$url)
	&start 'C:\Program Files\IIS Express\iisexpress.exe' "/trace:none /site:$site /apppool:Clr4IntegratedAppPool"
	if($url -ne $null)
	{
		Start-Process "chrome.exe" $url
	}
}

function global:stop-iis(){
    Stop-Process -ProcessName iisexpress*
}

function global:Run-Apps([string[]]$apps,[switch] $Secure=$false)
{	
	stop-iis
	foreach ($appShortCut in $apps) {
		$expression =""
		if ($Secure)
		{
			$expression= "Start-$appShortCut -Secure" 
		}	
		else
		{
			$expression= "Start-$appShortCut" 
		}	
		Invoke-Expression  $expression
	}	
}


function global:start-sample()
{
	param( [switch] $Secure)
	if($secure)
	{
		start-iis "TechFu.Nirvana.WebApi.Sample" "https://local-nirvanaAPI.mean.software:54405/api/Infrastructure/GetVersion"
	}
	else{
		start-iis "TechFu.Nirvana.WebApi.Sample" "http://local-nirvanaAPI.mean.software:24167/api/Infrastructure/GetVersion"
	}
}
function global:run-sample()
{
	stop-iis
	param( [switch] $Secure)
	if($secure)
	{
		start-sample -$Secure
	}
	else{
		start-sample 
	}
}

function global:start-es-sample()
{
	param( [switch] $Secure)

	start-iis "TechFu.Nirvana.EventStoreSample.WebAPI.Commands"
	start-iis "TechFu.Nirvana.EventStoreSample.WebAPI.CommandProcessor" 
	start-iis "TechFu.Nirvana.EventStoreSample.WebAPI.Notifications"
	start-iis "TechFu.Nirvana.EventStoreSample.WebAPI.Queries"
	
	 $path = Resolve-Path ".\samples\eventstore" 
	 Start-Process "$path\TechFu.Nirvana.EventStoreSample.QueueCommandProcessor\bin\Debug\TechFu.Nirvana.EventStoreSample.QueueCommandProcessor.exe"

	if($secure)
	{		
		start-iis "TechFu.Nirvana.EventStoreSample.UI" "https://local-eventsourcesample.mean.software:54407/index.html"
	}
	else{	
		start-iis "TechFu.Nirvana.EventStoreSample.UI" "http://local-eventsourcesample.mean.software:24169/index.html"	
	}

	& "C:\Program Files\MongoDB\Server\3.2\bin\mongod.exe" --dbpath "c:\temp\mongoDb" 



	 #Write-Host $path
}
function global:global:run-es-sample()
{
	param( [switch] $Secure)
	stop-iis
	if($secure)
	{
		start-es-sample -Secure
	}
	else{
		start-es-sample 
	}
}

function global:kill-es-sample(){
	stop-iis
	Stop-Process -processname "TechFu.Nirvana.EventStoreSample*"
}

function global:debug-es-sample()
{
	kill-es-sample
	run-es-sample
	Debug-iis-Express
}

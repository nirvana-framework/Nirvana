

function global:Debug-iis-Express()
{
	$DTE.Debugger.LocalProcesses | where-object -FilterScript {$_.Name -like "*iisexpress.exe*" } | ForEach-Object {Write-Host $_.Attach()}
}
function global:Debug-Processor()
{
	$DTE.Debugger.LocalProcesses | where-object -FilterScript {$_.Name -like "*QueueCommandProcessor*" } | ForEach-Object {Write-Host $_.Attach()}
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
		start-iis "Nirvana.WebApi.Sample" "https://local-nirvanaAPI.mean.software:54405/api/Infrastructure/GetVersion"
	}
	else{
		start-iis "Nirvana.WebApi.Sample" "http://local-nirvanaAPI.mean.software:24167/api/Infrastructure/GetVersion"
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

	start-iis "Nirvana.EventStoreSample.WebAPI.Commands"
	start-iis "Nirvana.EventStoreSample.WebAPI.CommandProcessor" 
	start-iis "Nirvana.EventStoreSample.WebAPI.Notifications"
	start-iis "Nirvana.EventStoreSample.WebAPI.Queries"
	Start-Queue-Emulator

	$path = Resolve-Path ".\samples\eventstore" 
	Start-Process "$path\Nirvana.EventStoreSample.QueueCommandProcessor\bin\Debug\Nirvana.EventStoreSample.QueueCommandProcessor.exe"

	if($secure)
	{		
		start-iis "Nirvana.EventStoreSample.UI" "https://local-eventsourcesample.mean.software:54407/index.html"
	}
	else{	
		start-iis "Nirvana.EventStoreSample.UI" "http://local-eventsourcesample.mean.software:24169/index.html"	
	}





	 #Write-Host $path
}
function global:run-es-sample()
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
	Stop-Process -processname "Nirvana.EventStoreSample*"
	Stop-Queue-Emulator
}

function global:debug-es-sample()
{
	Debug-iis-Express
	Debug-Processor
}

function global:Start-Queue-Emulator(){
 $status = & "${Env:ProgramFiles(x86)}\microsoft sdks\azure\storage emulator\AzureStorageEmulator.exe" "status"
 if ($status -contains 'IsRunning: False') {
  Write-Host "starting queue"
  & "${Env:ProgramFiles(x86)}\microsoft sdks\azure\storage emulator\AzureStorageEmulator.exe" "start" 
  }
}
function global:Stop-Queue-Emulator(){
 
 $status = & "${Env:ProgramFiles(x86)}\microsoft sdks\azure\storage emulator\AzureStorageEmulator.exe" "status" 
 if ($status -contains 'IsRunning: True') {
  Write-Host "starting queue"
  & "${Env:ProgramFiles(x86)}\microsoft sdks\azure\storage emulator\AzureStorageEmulator.exe" "stop" 
  }
}

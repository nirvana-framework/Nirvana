

function global:Debug-iis-Express()
{
	$DTE.Debugger.LocalProcesses | where-object -FilterScript {$_.Name -like "*iisexpress.exe*" } | ForEach-Object {Write-Host $_.Attach()}
}

#start iis with the console window up, so we can see trace messages from the website while were doing front end development.
function global:start-iis(){
param($site,$url)
	&start 'C:\Program Files\IIS Express\iisexpress.exe' "/trace:none /site:$site /apppool:Clr4IntegratedAppPool"
	Start-Process "chrome.exe" $url
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
		start-iis "TechFu.Nirvana.WebApi.Sample" "https://local-nirvanaAPI.mean.software:54404/api/Infrastructure/GetVersion"
	}
	else{
		start-iis "TechFu.Nirvana.WebApi.Sample" "http://local-nirvanaAPI.mean.software:24166/api/Infrastructure/GetVersion"
	}
}
function global:run-sample()
{
	stop-iis
	param( [switch] $Secure)
	if($secure)
	{
		start-iis "TechFu.Nirvana.WebApi.Sample" "https://local-nirvanaAPI.mean.software:54404/api/Infrastructure/GetVersion"
	}
	else{
		start-iis "TechFu.Nirvana.WebApi.Sample" "http://local-nirvanaAPI.mean.software:24166/api/Infrastructure/GetVersion"
	}
}

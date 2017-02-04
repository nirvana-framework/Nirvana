
function ensure-build-number(){
	if($env:BUILD_NUMBER -eq $null -or $env:BUILD_NUMBER -eq ""){
		$env:BUILD_NUMBER="1.0.0.0"
		log "Setting Build Number to $env:BUILD_NUMBER"
	}	
}

function show-timing(){
	get-job | select -property name,@{name="runtime" ;expression={($_.psendtime-$_.psbegintime).TotalSeconds}}|sort runtime -Descending | log	
}

function log(){
	param($message)
	$ElapsedTime.Elapsed.TotalSeconds.ToString("000.0") + ": " + $message
}



function wait_for_jobs_to_finish($logFileName){

	log "Waiting for background jobs"
	Get-Job | Wait-Job  | Out-Null
	get-job | select -property name,@{name="runtime" ;expression={($_.psendtime-$_.psbegintime).TotalSeconds}}|sort runtime -Descending | Out-File "$TestAssetPath\$logFileName"
	Get-Job | Receive-Job -ErrorAction SilentlyContinue | ForEach-Object { Write-Job-Log $_ "$TestAssetPath\$logFileName" }
	log "All jobs finished!"
}

function Write-Job-Log(){
param([string]$data,$logFile)
    log "write job log $data"
	if([string]::IsNullOrEmpty($data) )
	{
		$data="Complete"
	}
	$data | Out-File $logFile -Append
}



function stop-running-prcoess(){
param($processName)
	$serviceRef = Get-Process $processName -ErrorAction SilentlyContinue
	if ($serviceRef) {
		$serviceRef | Stop-Process -Force
		log "$processName not Stopped"
	}
	else
	{
		log "$processName not running"
	}
	Remove-Variable serviceRef
}
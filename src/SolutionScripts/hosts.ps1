$global:hostsFile = "$env:SystemRoot\System32\drivers\etc\hosts"
 
function global:add-host([string]$ip, [string]$hostname) {
	$ip + "`t`t" + $hostname | Out-File -encoding ASCII -append $hostsFile
}
 
function global:remove-host([string]$hostname) {
	$c = Get-Content $hostsFile
	$newLines = @()
	
	foreach ($line in $c) {
		$bits = [regex]::Split($line, "\s+|\t+")
		if ($bits.count -eq 2) {
			if ($bits[1] -ne $hostname) {
			
				$newLines += $line
			}
		} else {
			$newLines += $line
		}
	}
	
	# Write file
	Clear-Content $hostsFile
	foreach ($line in $newLines) {
	
		$line | Out-File -encoding ASCII -append $hostsFile
	}
}
 
function global:print-hosts() {
	Write-Host "Outputing contents of hosts file..."
	$c = Get-Content $hostsFile
	
	foreach ($line in $c) {
		$bits = [regex]::Split($line, "\t+")
		if ($bits.count -eq 2) {
			Write-Host $bits[0] `t`t $bits[1]
		}
	}
}

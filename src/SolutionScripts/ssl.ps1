##########################################################
# PLEASE READ:
# 
# This script will be loaded by Visual Studio when the
# solution is loaded.  To force a refresh of this script,
# issue the Update-SolutionScripts command in Package
# Manager Console.
##########################################################

function global:Install-SslCertificate([Parameter(Mandatory=$true)] [string] $domain, [Parameter(Mandatory=$true)] [string] $port)
{
	$dir = $env:TEMP
	$certFilename = "$dir\$domain.cer"

	Write-Host "Creating certificate for $domain at $certFilename"

	$name = New-Object -ComObject X509Enrollment.CX500DistinguishedName.1
	$name.Encode("CN=" + $domain, 0)

	$key = New-Object -ComObject X509Enrollment.CX509PrivateKey.1
	$key.ProviderName = "Microsoft RSA SChannel Cryptographic Provider"
	$key.KeySpec = 1
	$key.Length = 2048
	$key.SecurityDescriptor = "D:PAI(A;;0xd01f01ff;;;SY)(A;;0xd01f01ff;;;BA)(A;;0x80120089;;;NS)"
	$key.MachineContext = 1
	$key.Create()

	$serverauthoid = New-Object -ComObject X509Enrollment.CObjectId.1
	$serverauthoid.InitializeFromValue("1.3.6.1.5.5.7.3.1")
	$ekuoids = New-Object -ComObject X509Enrollment.CObjectIds.1
	$ekuoids.Add($serverauthoid)
	$ekuext = New-Object -ComObject X509Enrollment.CX509ExtensionEnhancedKeyUsage.1
	$ekuext.InitializeEncode($ekuoids)

	# Set the hash algorithm to sha256 instead of the default sha1
	$hashAlgorithmObject = New-Object -ComObject X509Enrollment.CObjectId.1
	$hashAlgorithmObject.InitializeFromAlgorithmName(1, 0, 0, "SHA256")

	$cert = New-Object -ComObject X509Enrollment.CX509CertificateRequestCertificate.1
	$cert.InitializeFromPrivateKey(2, $key, "")
	$cert.Subject = $name
	$cert.Issuer = $cert.Subject
	$cert.HashAlgorithm = $hashAlgorithmObject
	$cert.NotBefore = Get-Date
	$cert.NotAfter = $cert.NotBefore.AddDays(900)
	$cert.X509Extensions.Add($ekuext)
	$cert.Encode()

	$enrollment = New-Object -ComObject X509Enrollment.CX509Enrollment.1
	$enrollment.InitializeFromRequest($cert)
	$certdata = $enrollment.CreateRequest(0)
	$enrollment.InstallResponse(2, $certdata, 0, "")

	$thumbprint = (Dir cert:\LocalMachine\My -recurse | Where { $_.Subject -match "CN=" + $domain } | Select-Object -Last 1).thumbprint

	Write-Host "Trusting cert:\LocalMachine\My\$thumbprint"
	$newCert = Get-ChildItem cert:\LocalMachine\My\$thumbprint
	$store = New-Object System.Security.Cryptography.X509certificates.X509Store 'Root', 'LocalMachine'
	$store.Open('ReadWrite')
	$store.Add($newCert)
	$store.Close() 

	Write-Host "Exporting cert:\LocalMachine\My\$thumbprint to $certfilename ... "
	Export-Certificate -FilePath $certFilename -Cert cert:\LocalMachine\My\$thumbprint
	
	Write-Host "Enabling SSL cert on port $port"
	$guid = [guid]::NewGuid().ToString().ToUpper()
	& netsh http add sslcert ipport=0.0.0.0:$port appid="{$guid}" certhash="$thumbprint"
}

function global:Remove-SslCertificate([Parameter(Mandatory=$true)] [string] $domain, [Parameter(Mandatory=$true)] [string] $port)
{
	& netsh http delete sslcert ipport=0.0.0.0:$port

	$certs = @(Dir -recurse cert:\LocalMachine\My | Where { $_.Subject -match "CN=" + $domain })
	foreach ($cert in $certs)
	{
		$thumbprint = $cert.Thumbprint 
		if (Test-Path cert:\LocalMachine\My\$thumbprint)
		{
			Write-Host "Deleting cert:\LocalMachine\My\$thumbprint"
			Remove-Item cert:\LocalMachine\My\$thumbprint -DeleteKey
		}
	}

	$certs = @(Dir -recurse cert:\LocalMachine\Root | Where { $_.Subject -match "CN=" + $domain })
	foreach ($cert in $certs)
	{
		$thumbprint = $cert.Thumbprint 
		if (Test-Path cert:\LocalMachine\Root\$thumbprint)
		{
			Write-Host "Deleting cert:\LocalMachine\Root\$thumbprint"
			Remove-Item cert:\LocalMachine\Root\$thumbprint
		}
	}
}

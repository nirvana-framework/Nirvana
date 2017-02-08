param (
    [string]$branchname = "master",
    [string]$taskName ="build"
 )

function Create-package-object() {
param($name,$version,$useNuget)
	$object = @{}
	$object.Add('Name', $name)
	$object.Add('Version', $version)
	$object.Add('UseNuget', $useNuget)
	return $object 
}

$packages=@()
$packages+=(Create-package-object 'Psake' "4.5.0" $true)
$packages+=(Create-package-object 'Nuget.CommandLine' $null)
$packages+=(Create-package-object 'Xunit' $null)
$packages+=(Create-package-object 'azurestorageemulator' $null)
$packages+=(Create-package-object 'opencover.portable' '4.6.519' $false)
$packages+=(Create-package-object 'coveralls.io' '1.3.4' $true)
$packages+=(Create-package-object 'reportgenerator.portable' '2.5.0.0' $false)
$packs=(choco list -lo)

function Exec([scriptblock]$cmd, [string]$errorMessage = "Error executing command: " + $cmd) { 
  & $cmd 
$lastcode = $LastExitCode
  if ($lastcode -ne 0) {
	  if($errorMessage -ne $null){
			throw $errorMessage 
		}
	  throw $lastcode
  } 
}



function format-chocolatey-packages() {
    $xml = "<?xml version=`"1.0`" encoding=`"utf-8`"?>`n"
    $xml += "<packages>`n"
    foreach ($program in $packs) {
    $name, $version, $shouldBeEmpty = $program.Split(" ")
	    if (!$shouldBeEmpty) {
		    $xml += ("  <package id=`"{0}`" version=`"{1}`"/>`n" -f $name,$version)
	    }
    }
    $xml += "</packages>`n"
    return $xml
}

function has-Package() {
param($packageName, $packageVersion)
   $temp=(format-chocolatey-packages)
   $packageData= [xml]$temp;
   $returnTemp=$false

    $packageData.packages.package | foreach {
        if ($_.id -eq $packageName ) 
        {
            if($packageVersion -eq $null){
                $returnTemp = $true
            }
            else{
                if($packageVersion -eq $_.version){
                $returnTemp = $true
                }
            }
        }
    }
    return $returnTemp
}

function ensure-package() {
param($package)

	if ((has-Package $package.Name $package.Version) -eq $false) {
		if ($package.UseNuget -eq $true) {
			$sourceString= "https://nuget.org/api/v2/;https://chocolatey.org/api/v2/"
		}
		else {
			$sourceString= "https://chocolatey.org/api/v2/"
		}

		if ($package.Version -eq $null) {
			exec { choco install $package.Name --force --confirm -source $sourceString --ignoreDependencies --yes }
		}
		else {
			exec { choco install $package.Name -version $package.Version  --force --confirm -source $sourceString --ignoreDependencies --yes }
		}

	}
	else {
		Write-Host $package.Name "installed"
	}
		
}

if ($env:ChocolateyInstall -eq $null -or $env:ChocolateyInstall -eq "") {
	iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))
}
else {
	Write-Host "Chocolatey installed"
}

Write-Host "Checking for packages"
$packages | foreach	{ ensure-package $_ }

Import-Module "$env:ChocolateyInstall\lib\psake\tools\psake.psm1" | Out-null
Write-Host "Running $taskName on branch $branchname "

Invoke-psake .\default.ps1 "$taskName" -Framework "4.6.1x64"  -properties  @{ "branchname"=$branchname; } 
if(!$psake.build_success){
	throw "Build Failed"
}

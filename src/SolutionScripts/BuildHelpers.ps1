function global:psake-clean-and-rebuild {
	.\psakebootstrapper.ps1 master rebuild
}
function global:psake-fastBuild {
	.\psakebootstrapper.ps1 master buildantunittest
}
function global:psake-package {
	.\psakebootstrapper.ps1 master buildAndPackage
}
function global:psake-analyze {
	.\psakebootstrapper.ps1 master analyze
}
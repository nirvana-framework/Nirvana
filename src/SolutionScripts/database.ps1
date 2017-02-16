function global:migration()
{
    param($name)
    Add-Migration -Name $name -ConfigurationTypeName Nirvana.EventStoreSample.Infrastructure.Migrations.Configuration -ProjectName Nirvana.EventStoreSample.Infrastructure -StartUpProjectName Nirvana.EventStoreSample.WebAPI.CommandProcessor
}


function global:ReScaffold-Last-Migration()
{
	$lastMigration = Get-ChildItem -Path '.\Samples\EventStore\Nirvana.EventStoreSample.Infrastructure\Migrations' -Include '*_*.cs' -Exclude '*_*.Designer.cs' -Name | Sort -Descending | Select-Object -First 1 | % { $_.Trim('.cs') }
	Add-Migration -Verbose -Name $lastMigration -ProjectName Nirvana.EventStoreSample.Infrastructure -StartUpProjectName Nirvana.EventStoreSample.WebAPI.CommandProcessor -ConfigurationTypeName Infrastructure.Migrations.Configuration
}



function global:Migrate-Database()
{
 param($dbname='es_sample',$migrationName=$null)

	if($migrationName -eq $null)
	{
		Update-Database   -ProjectName Nirvana.EventStoreSample.Infrastructure -StartUpProjectName Nirvana.EventStoreSample.WebAPI.CommandProcessor -ConfigurationTypeName Nirvana.EventStoreSample.Infrastructure.Migrations.Configuration
	}
	else{
		Update-Database  –TargetMigration: $migrationName  -ProjectName Nirvana.EventStoreSample.Infrastructure -StartUpProjectName Nirvana.EventStoreSample.WebAPI.CommandProcessor -ConfigurationTypeName Nirvana.EventStoreSample.Infrastructure.Migrations.Configuration
	}

}
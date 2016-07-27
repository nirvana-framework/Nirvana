function global:migration()
{
    param($name)
    Add-Migration -Name $name -ConfigurationTypeName TechFu.Nirvana.EventStoreSample.Infrastructure.Migrations.Configuration -ProjectName TechFu.Nirvana.EventStoreSample.Infrastructure -StartUpProjectName TechFu.Nirvana.EventStoreSample.WebAPI.CommandProcessor
}


function global:ReScaffold-Last-Migration()
{
	$lastMigration = Get-ChildItem -Path '.\Samples\EventStore\TechFu.Nirvana.EventStoreSample.Infrastructure\Migrations' -Include '*_*.cs' -Exclude '*_*.Designer.cs' -Name | Sort -Descending | Select-Object -First 1 | % { $_.Trim('.cs') }
	Add-Migration -Verbose -Name $lastMigration -ProjectName TechFu.Nirvana.EventStoreSample.Infrastructure -StartUpProjectName TechFu.Nirvana.EventStoreSample.WebAPI.CommandProcessor -ConfigurationTypeName TechFu.Infrastructure.Migrations.Configuration
}



function global:Migrate-Database()
{
 param($dbname='es_sample',$migrationName=$null)

	if($migrationName -eq $null)
	{
		Update-Database   -ProjectName TechFu.Nirvana.EventStoreSample.Infrastructure -StartUpProjectName TechFu.Nirvana.EventStoreSample.WebAPI.CommandProcessor -ConfigurationTypeName TechFu.Nirvana.EventStoreSample.Infrastructure.Migrations.Configuration
	}
	else{
		Update-Database  –TargetMigration: $migrationName  -ProjectName TechFu.Nirvana.EventStoreSample.Infrastructure -StartUpProjectName TechFu.Nirvana.EventStoreSample.WebAPI.CommandProcessor -ConfigurationTypeName TechFu.Nirvana.EventStoreSample.Infrastructure.Migrations.Configuration
	}

}
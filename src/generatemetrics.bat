
 %ChocolateyInstall%\lib\opencover.portable\tools\OpenCover.Console.exe ^
 -target:"%ChocolateyInstall%\lib\XUnit\tools\xunit\xunit.console.exe" ^
 -targetargs:"UnitTests\bin\debug\UnitTests.dll UnitTests\bin\debug\UnitTests.dll.Config" ^
 -searchdirs:"QS.Core\bin\Debug;packages\CsvHelper.2.2.2\lib\net40-client;packages\structuremap.4.1.1.372\lib\dotnet" ^
 -output:"..\unittest.coverage.raw.xml" ^
 -filter:"+[*]* -[UnitTests]* -[StructureMap]* -[CsvHelper]* -[Octokit]* " ^
 -register:user


rem %ChocolateyInstall%\lib\opencover.portable\tools\OpenCover.Console.exe ^
rem -target:"%ChocolateyInstall%\lib\XUnit\tools\xunit\xunit.console.exe" ^
rem -threshold:1 ^
rem -skipautoprops ^
rem -targetargs:"FunctionalTests\bin\Debug\QS.FunctionalTests.dll FunctionalTests\bin\Debug\QS.FunctionalTests.dll.config" ^
rem -coverbytest:"QS.FunctionalTests.dll"^
rem -searchdirs:"QS.Core\bin\Debug;packages\CsvHelper.2.2.2\lib\net40-client;packages\structuremap.4.1.1.372\lib\dotnet" ^
rem -output:"..\functionalTests.coverage.raw.xml" ^
rem -filter:"+[*]* -[QS.FunctionalTests]* -[UnitTests]* -[StructureMap]* -[CsvHelper]* -[Octokit]* -[mscorlib]* -[xunit*]* -[EntityFramework*]* -[Exceptionless*]* -[Intuit*]* -[Loggly*]* -[Accord*]* -[AForge*]* -[ApprovalTests*]* [Bytescout*]* -[Common.Logging*]* -[Dapper*]* [DotNetOpenAuth*]* -[FluentValidation*]* -[System*]*" ^
rem -register:user


rem md ..\OpenCover
rem del ..\OpenCover\*.* /s
rem md ..\OpenCover\UnitTestReport
rem md ..\OpenCover\AcceptanceTestReport
rem  
rem %ChocolateyInstall%\lib\reportgenerator.portable\tools\ReportGenerator.exe ^
rem -reports:"..\unittest.coverage.raw.xml" ^
rem -targetdir:"..\OpenCover\UnitTestReport"
rem 
rem %ChocolateyInstall%\lib\reportgenerator.portable\tools\ReportGenerator.exe ^
rem -reports:"..\functionalTests.coverage.raw.xml" ^
rem -targetdir:"..\OpenCover\AcceptanceTestReport"




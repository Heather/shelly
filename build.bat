@echo off
SET EnableNuGetPackageRestore=true
cls
if not exist "tools\FAKE\tools\Fake.exe" (
	"tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
	)
"tools\FAKE\tools\Fake.exe" build.fsx
pause
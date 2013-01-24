@echo off
SET EnableNuGetPackageRestore=true
cls
if not exist "tools\FAKE\tools\Fake.exe" (
	"tools\nuget\nuget.exe" "install" "FAKE" "-Version" "2.1.28-alpha" "-OutputDirectory" "tools" "-ExcludeVersion"
	)
if not exist "Heather.dll" (
	set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
	::4.5:
	::%MSBuild% src\Heather.fsproj /tv:4.0 /p:TargetFrameworkVersion=v4.5;TargetFrameworkProfile="";Configuration=Release
	::4.0
	%MSBuild% src\Heather.fsproj /tv:4.0 /p:TargetFrameworkVersion=v4.0;TargetFrameworkProfile="";Configuration=Release
	cp "src\bin\Release\Heather.dll" "Heather.dll"
	)
"tools\FAKE\tools\Fake.exe" build.fsx
pause
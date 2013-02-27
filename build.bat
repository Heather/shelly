@echo off
SET EnableNuGetPackageRestore=true
if %PROCESSOR_ARCHITECTURE%==x86 (
	set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
) else (
	set MSBUILD=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe
)
::4.5:
::%MSBuild% src\Heather.fsproj /tv:4.0 /p:TargetFrameworkVersion=v4.5;TargetFrameworkProfile="";Configuration=Release
::4.0
cls
if not exist "tools\FAKE\tools\Fake.exe" (
	"tools\nuget\nuget.exe" "install" "FAKE" "-Version" "2.1.28-alpha" "-OutputDirectory" "tools" "-ExcludeVersion"
	)
set ABS_PATH=%CD%
if not exist "Heather.dll" (
	%MSBuild% %ABS_PATH%\src\Heather.fsproj /p:Configuration=Release
	cp %ABS_PATH%\src\bin\Release\Heather.dll "Heather.dll"
	)
"tools\FAKE\tools\Fake.exe" build.fsx
pause
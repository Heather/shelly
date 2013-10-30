@echo off
cls
SET EnableNuGetPackageRestore=true
if %PROCESSOR_ARCHITECTURE%==x86 (
	set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
) else (
	set MSBUILD="%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
)

if not exist "tools\ctodo\tools\cctodo_100.exe" (
	"tools\nuget\nuget.exe" "install" "ctodo" "-OutputDirectory" "tools" "-ExcludeVersion"
)

%MSBuild% .\src\shelly.fsproj /nologo /p:Configuration=Release,TargetFrameworkVersion=v4.5.1

::Read todo
set todo=call todo.cmd
rm todo.db3

%todo% initdb
%todo% set git 0
%todo% set syncfile TODO
%todo% sync

echo +
echo + TODO:
%todo%

pause
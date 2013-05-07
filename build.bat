@echo off
SET EnableNuGetPackageRestore=true
if %PROCESSOR_ARCHITECTURE%==x86 (
	set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
) else (
	set MSBUILD=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe
)

if not exist "tools\ctodo\tools\cctodo_100.exe" (
	"tools\nuget\nuget.exe" "install" "ctodo" "-OutputDirectory" "tools" "-ExcludeVersion"
)

cls
set ABS_PATH=%CD%
if not exist "Heather.dll" (
	%MSBuild% %ABS_PATH%\src\Heather.fsproj /p:Configuration=Release
	cp %ABS_PATH%\src\bin\Release\Heather.dll "Heather.dll"
) else (
    if not exist "tools\Failess\tools\Failess.exe" (
        echo Getting Failess build tool with CSS EDSL library
        "tools\nuget\nuget.exe" "install" "Failess" "-OutputDirectory" "tools" "-ExcludeVersion"
    )
	"tools\Failess\tools\Failess.exe" build.fsx
)

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
#nowarn "0211"

#I @"tools\Failess\tools"

#I @"/usr/lib/mono/4.0/"
#I @"/usr/lib/mono/4.5/"

#I @"/usr/lib/mono/Failess/4.0/"
#I @"/usr/lib/mono/Failess/4.5/"
#r @"FakeLib.dll"

#I @"/usr/lib/mono/Heather/4.0/"
#I @"/usr/lib/mono/Heather/4.5/"
#r @"Heather.dll"

open System
open System.IO

open Heather
open Fake

Description "Cleans the last build"
Target "Clean" /> fun () -> 
    trace " --- Cleaning stuff --- "
    CleanDirs ["src/obj"; "src/bin"]

Target "Build" /> fun () -> 
    trace " --- Building the app --- "
    try match isLinux with
        | true ->
            shellxn "xbuild" "src/Heather.fsproj \"/p:Configuration=Release\""
        | false -> 
            let buildDir  = @".\src\bin\Release"
            let appReferences = !+ @"src\*.fsproj" |> Scan
            MSBuildRelease buildDir "Build" appReferences
                |> Log "AppBuild-Output: "
    with | _ as exc ->
        trace (" --- Failed to build: " + exc.Message)

"Clean"
  ==> "Build"

RunParameterTargetOrDefault "target" "Build"

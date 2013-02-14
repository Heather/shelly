#I @"/usr/lib/mono/4.0/"
#I @"/usr/lib/mono/4.5/"
#r @"FakeLib.dll"
#r @"Heather.dll"

open System
open System.IO

open Heather.Core
open Heather.Shell
open Heather.Syntax
open Fake

let CcolorMap = function
    | ImportantMessage _ -> ConsoleColor.Blue
    | ErrorMessage _     -> ConsoleColor.Red
    | LogMessage _       -> ConsoleColor.DarkGray
    | TraceMessage _     -> ConsoleColor.DarkBlue
    | FinishedMessage    -> ConsoleColor.Black
    | _                  -> ConsoleColor.DarkGray

let CConsoleTraceListener = ConsoleTraceListener(buildServer <> CCNet,CcolorMap)
listeners.[0] <- CConsoleTraceListener

Description "Cleans the last build"
Target "Clean" /> fun () -> 
    trace " --- Cleaning stuff --- "
    CleanDirs ["src/obj"; "src/bin"]

Target "Build" /> fun () -> 
    trace " --- Building the app --- "
    try
        match isLinux with
        | true ->
            shellxf "xbuild" "src/Heather.fsproj \"/p:Configuration=Release\""
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

#I @"/usr/lib/mono/4.0/"
#I @"/usr/lib/mono/4.5/"
#r @"FakeLib.dll"

open System
open System.IO

let isLinux =
    int Environment.OSVersion.Platform |> fun p ->
        (p = 4) || (p = 6) || (p = 128)

module cprintf =
    let cprintf c fmt = 
        Printf.kprintf 
            (fun s -> 
                let old = System.Console.ForegroundColor 
                try 
                  System.Console.ForegroundColor <- c
                  System.Console.Write s
                finally
                  System.Console.ForegroundColor <- old) 
            fmt
    let cprintfn c fmt = 
        cprintf c fmt
        printfn ""

open Fake

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

open cprintf

let shell cmd args =
    let proc = new System.Diagnostics.ProcessStartInfo(cmd)
    proc.RedirectStandardOutput <- true
    proc.UseShellExecute <- false
    proc.Arguments <- args
    let p = System.Diagnostics.Process.Start(proc)
    let tool_output = p.StandardOutput.ReadToEnd()
    p.WaitForExit()
    tool_output
let shellxf a b = cprintf ConsoleColor.DarkGreen <| "%s\n" <| shell a b

Description "Cleans the last build"
Target "Clean" (fun () -> 
    trace " --- Cleaning stuff --- "
    CleanDirs ["src/obj"; "src/bin"]
    )

Target "Build" (fun () -> 
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
    )

"Clean"
  ==> "Build"

RunParameterTargetOrDefault "target" "Build"

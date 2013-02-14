[<AutoOpen>]
module Heather.Core

open System
open System.IO

let isLinux =
    int Environment.OSVersion.Platform |> fun p ->
        (p = 4) || (p = 6) || (p = 128)
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
let deprecated deprecatedModule =
    cprintf ConsoleColor.DarkRed
    <| "WARNING: Module %s is deprecated or moved. Read git history on: https://github.com/gentoo-dotnet/Heather/commits/master\n"
    <| deprecatedModule
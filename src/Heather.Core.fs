[<AutoOpen>]
module Heather.Core

open System
open System.IO

let isLinux =
    int Environment.OSVersion.Platform |> fun p ->
        (p = 4) || (p = 6) || (p = 128)
let inline cprintf c fmt = 
    Printf.kprintf 
        (fun s -> 
            let old = System.Console.ForegroundColor 
            try 
                System.Console.ForegroundColor <- c
                System.Console.Write s
            finally
                System.Console.ForegroundColor <- old) 
        fmt
let inline cprintfn c fmt = 
    cprintf c fmt
    printfn ""
﻿namespace Heather

open System
open System.IO

open cprintf
open Core

module Shell =
    let shell cmd args =
        let proc = new System.Diagnostics.ProcessStartInfo(cmd)
        proc.RedirectStandardOutput <- true
        proc.UseShellExecute <- false
        proc.Arguments <- args
        let p = System.Diagnostics.Process.Start(proc)
        let tool_output = p.StandardOutput.ReadToEnd()
        p.WaitForExit()
        tool_output
    let shellx a b = cprintf ConsoleColor.DarkGray <| "%s" <| shell a b
    let shellxf a b = cprintf ConsoleColor.DarkGreen <| "%s\n" <| shell a b
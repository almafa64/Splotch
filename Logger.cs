﻿using System;
using System.Diagnostics;
using System.IO;

public static class Logger
{
    /// <summary>
    /// It gets the function that called the function thats happening rn.
    /// Basically if A() calls B() then B() calls this, then this will tell you
    /// what class A() is from
    /// </summary>
    /// <returns>The class that called your function</returns>
    private static string getCallingClass()
    {
        // Basically its meant for errors but it also logs every function that is called!
        StackTrace stackTrace = new StackTrace();
        if (stackTrace.FrameCount >= 3)
        {
            Type callingClass = stackTrace.GetFrame(2).GetMethod().DeclaringType;
            //Console.WriteLine($"Calling class: {callingClass?.Name}");
            return callingClass?.Name;
        }
        else
        {
            // This should never happen, but if it does here it is.
            return "n/a";
            //Console.WriteLine("Unable to determine calling class.");
        }
        return null;
    }

    /// <summary>
    /// Initializes the console.
    /// </summary>
    public static void InitLogger()
    {
        // Create a new process
        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = false;  // Set this to true if you want to hide the window (Might be how we disable the window)

        // Start the process
        process.Start();
        // It breaks if this isn't here.
        System.Threading.Thread.Sleep(1000);

        // Connects the console window to bopl
        AttachConsole((uint) process.Id);

        // Sets the output to the console window thing
        StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
        sw.AutoFlush = true;
        Console.SetOut(sw);


        Logger.Log("Log test");
        Logger.Warning("Warn test");
        Logger.Error("Error test");


        Logger.Log("Logging initialized.");

    }

    /// <summary>
    /// Logs into console and output_log.txt
    /// </summary>
    public static void Log(string message)
    {
        string formattedString = $"[INFO    : {getCallingClass()}] {message}";

        Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine(formattedString);
        UnityEngine.Debug.Log(formattedString);
    }

    /// <summary>
    /// Logs a warning into console and logs to output_log.txt
    /// </summary>
    public static void Warning(string message)
    {
        string formattedString = $"[WARNING : {getCallingClass()}] {message}";

        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine(formattedString);
        UnityEngine.Debug.LogWarning(formattedString);

        Console.ForegroundColor = ConsoleColor.Gray;
    }

    /// <summary>
    /// Logs an error into console and logs to output_log.txt
    /// </summary>
    public static void Error(string message)
    {
        string formattedString = $"[ERROR   : {getCallingClass()}] {message}";

        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine(formattedString);
        UnityEngine.Debug.LogError(formattedString);

        Console.ForegroundColor = ConsoleColor.Gray;
    }

    // AttachConsole lets me "hook" the logger into the thing
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool AttachConsole(uint dwProcessId);
}
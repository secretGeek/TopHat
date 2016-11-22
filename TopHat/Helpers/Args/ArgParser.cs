using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TopHat.Models;

namespace TopHat.Helpers
{
    public static class ArgParser
    {
        public static Settings ParseArgs(string[] args)
        {
            var settings = new Settings();

            if (args.Length == 0)
                return settings;

            AllocateConsole();

            var p = new OptionSet() {
                    { "s|snippets=",     "snippet file (separated by '#' comments)", s => settings.FileName = s},
                    { "r|replacements=", "comma separated set of {ReplaceThis}`{WithThis} token pairs;", s => settings.Replacements = Replacement.CreateList(s)},
                    { "?|h|help",        "show this message and exit", v => settings.ShowHelp = v != null } };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                //SIN: Coupling.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\nError parsing arguments");

                Console.WriteLine(e.Message);
                Console.ResetColor();

                if (e.Option != null)
                {
                    int i = 0;
                    p.WriteOptionPrototype(Console.Out, e.Option, ref i);
                    p.WriteOptionDetails(Console.Out, e.Option, i);
                    Console.WriteLine();
                }

                Console.WriteLine(String.Format("Try '{0} --help' for more information.", Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)));
            }

            if (settings.ShowHelp)
            {
                ShowHelp(p);
            }

            return settings;
        }

        static void ShowHelp(OptionSet p)
        {
            //SIN: coupling. 
            if (p.UnrecognizedOptions != null && p.UnrecognizedOptions.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\nError. Unrecognized commandline option" + (p.UnrecognizedOptions.Count > 1 ? "s" : "") + ".");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.ResetColor();
                foreach (var s in p.UnrecognizedOptions)
                {
                    Console.Write("Unrecognized: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(s);
                    Console.ResetColor();
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please see below for all valid options.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\r\nTop Hat "); Console.ResetColor();
                Console.WriteLine("Snippet assistant for presentations on Windows.\r\n");
            }

            Console.Write("Usage: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("TopHat.exe ");
            Console.ResetColor();
            Console.WriteLine("[options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        /// <summary>
        /// allocates a new console for the calling process.
        /// </summary>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. 
        /// To get extended error information, call Marshal.GetLastWin32Error.</returns>
        [DllImport("kernel32", SetLastError = true)]
        static extern bool AllocConsole();

        /// <summary>
        /// Attaches the calling process to the console of the specified process.
        /// </summary>
        /// <param name="dwProcessId">[in] Identifier of the process, usually will be ATTACH_PARENT_PROCESS</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. 
        /// To get extended error information, call Marshal.GetLastWin32Error.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        /// <summary>Identifies the console of the parent of the current process as the console to be attached.
        /// always pass this with AttachConsole in .NET for stability reasons and mainly because
        /// I have NOT tested interprocess attaching in .NET so dont blame me if it doesnt work! </summary>
        const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;

        /// <summary>
        /// calling process is already attached to a console
        /// </summary>
        const int ERROR_ACCESS_DENIED = 5;

        /// <summary>
        /// Allocate a console if application started from within windows GUI. 
        /// Detects the presence of an existing console associated with the application and
        /// attaches itself to it if available.
        /// </summary>
        private static void AllocateConsole()
        {
            //
            // the following should only be used in a non-console application type (C#)
            // (since a console is allocated/attached already when you define a console app.. :) )
            //

            var attached = AttachConsole(ATTACH_PARENT_PROCESS);
            var lastError = 0;
            if (!attached)
            {
                lastError = Marshal.GetLastWin32Error();
            }
            else
            {
                //HACK
                //Console.WriteLine("Attached to parent console.");
            }

            if (!attached && lastError == ERROR_ACCESS_DENIED)
            //if (!AttachConsole(ATTACH_PARENT_PROCESS) && Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
            {
                //HACK:
                //Log.WriteLine("Could not attach to parent process (access denied)");

                // A console was not allocated, so we need to make one.
                if (!AllocConsole())
                {
                    //Log.WriteLine("A console could not be allocated.");
                    MessageBox.Show("A console could not be allocated, sorry!");
                    throw new Exception("Console Allocation Failed");
                }
                else
                {
                    //Console.WriteLine("Is Attached, press a key...");
                    //Console.WriteLine("Is Attached, press a key...");
                    //Console.WriteLine("Is Attached, press a key...");
                    //Console.WriteLine("Is Attached, press a key...");
                    //Console.ReadKey(true);
                    // you now may use the Console.xxx functions from .NET framework
                    // and they will work as normal
                }

            }
        }
    }
}

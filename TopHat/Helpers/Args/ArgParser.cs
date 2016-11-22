using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
    }
}

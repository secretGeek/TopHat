using System;
using System.Windows.Forms;
using TopHat.Helpers;
using TopHat.Models;

namespace TopHat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Settings settings = ArgParser.ParseArgs(args);

            //SIN: coupling.
            if (settings.ShowHelp)
            {
                return;
            }

            Application.Run(new Form1(settings.FileName, settings.Replacements));
        }
    }
}

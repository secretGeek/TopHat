using System.Collections.Generic;

namespace TopHat.Models
{
    public class Settings
    {
        public string FileName { get; set; }
        public List<Replacement> Replacements { get; set; }
        public bool ShowHelp { get; set; }
    }
}

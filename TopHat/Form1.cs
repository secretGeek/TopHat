using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopHat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSnippets();
            GetSnippet(snippetIndex);
        }


        private void btnPrevious_Click(object sender, EventArgs e)
        {
            Previous();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        List<String> snippets = new List<String>();
        private void LoadSnippets()
        {
            string fileName = @"C:\users\leon\dropbox\secretgeek\all_someday_projects\ddd_linux_core\demo\scripts.md";
            var lines = File.ReadAllLines(fileName);
            var currentSnippet = string.Empty;
            
            foreach(var l in lines)
            {
                if (l.TrimStart().StartsWith("#"))
                {

                    if (currentSnippet != string.Empty)
                    {
                        snippets.Add(currentSnippet.TrimEnd("\n".ToCharArray()));
                    }
                    currentSnippet = string.Empty;
                    continue;
                    
                } else
                {
                    if (!string.IsNullOrWhiteSpace(l))
                    {
                        currentSnippet += l + "\n";
                    }
                }
            }

        }

        int snippetIndex = 0;
        private void Next()
        {
            snippetIndex++;
            if (snippetIndex >= snippets.Count)
                snippetIndex = 0;

            GetSnippet(snippetIndex);
            
            //TODO:
            //Clipboard.SetDataObject(txtResult.Text, true);

        }

        private void GetSnippet(int snippetIndex)
        {
            lblMessage.Text = snippets[snippetIndex];
            Clipboard.SetDataObject(snippets[snippetIndex], true);
        }

        private void Previous()
        {
            snippetIndex--;
            if (snippetIndex < 0)
                snippetIndex =  snippets.Count-1;

            GetSnippet(snippetIndex);
            //TODO:
        }


    }
}

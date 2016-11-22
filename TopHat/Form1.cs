using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TopHat.Models;

namespace TopHat
{
    public partial class Form1 : Form
    {
        string fileName = null;
        List<Replacement> replacements = null;

        public Form1(string fileName, List<Replacement> replacements)
        {
            InitializeComponent();
            this.fileName = fileName;
            this.replacements = replacements ?? Replacement.CreateList("45.55.151.240`45.55.151.239,ddd-test-4`ddd-test-3");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                string defaultFileName = @"C:\users\leon\dropbox\secretgeek\all_someday_projects\ddd_linux_core\demo\scripts.md";

                var f = new OpenFileDialog();

                if (File.Exists(defaultFileName))
                {
                    f.InitialDirectory = Path.GetDirectoryName(defaultFileName);
                    f.FileName = Path.GetFileName(defaultFileName);
                }

                f.Title = "Select snippet file";
                var result = f.ShowDialog();

                if (result == DialogResult.OK
                    && File.Exists(f.FileName))
                {
                    fileName = f.FileName;
                }
                else
                {
                    // Failed to pick a file to load snippets from?
                    // Time to die, application.
                    Application.Exit();
                    return;
                }
            }

            LoadSnippets(fileName);
            LoadReplacements(replacements);
            GetSnippet(snippetIndex);
        }

        private void LoadReplacements(List<Replacement> replacements)
        {
            contextMenuStrip1.Items.Clear();

            foreach (var replacement in replacements)
            {
                var added = contextMenuStrip1.Items.Add(replacement.ReplaceThis + " -> " + replacement.WithThis, null, editItemToolStripMenuItem_Click);
                added.Tag = replacement;
            }

            contextMenuStrip1.Items.Add("Add {ReplaceThis}`{WithThis} token...", null, addItemToolStripMenuItem_Click);
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
        private void LoadSnippets(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var currentSnippet = string.Empty;

            foreach (var l in lines)
            {
                if (l.TrimStart().StartsWith("#"))
                {
                    if (currentSnippet != string.Empty)
                    {
                        snippets.Add(currentSnippet.TrimEnd("\n".ToCharArray()));
                    }

                    currentSnippet = string.Empty;
                    continue;
                }
                else
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
        }

        private void Previous()
        {
            snippetIndex--;
            if (snippetIndex < 0)
                snippetIndex = snippets.Count - 1;

            GetSnippet(snippetIndex);
        }

        private void GetSnippet(int snippetIndex)
        {
            var rawSnippet = snippets[snippetIndex];
            var updatedSnippet = rawSnippet;
            foreach (var r in replacements)
            {
                updatedSnippet = updatedSnippet.Replace(r.ReplaceThis, r.WithThis);
            }
            lblMessage.Text = updatedSnippet;
            Clipboard.SetDataObject(updatedSnippet, true);
        }

        private void editItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem toolStripItem = sender as ToolStripItem;
            var replacement = toolStripItem.Tag as Replacement;
            var defaultText = replacement.ReplaceThis + "`" + replacement.WithThis;

            var item = Prompt.ShowDialog("Enter {ReplaceThis}`{WithThis} Token pair (separate with `)", "Edit {ReplaceThis}`{WithThis} Token pair", defaultText);

            if (!string.IsNullOrWhiteSpace(item))
            {
                if (item.IndexOf("`") <= 0)
                {
                    MessageBox.Show("Please enter a string in the form\r\n\"{ReplaceThis}`{WithThis}\"\r\n... note backtick separator.", "Missing backtick in Replace`With string");
                }
                else
                {
                    replacements.Remove(replacement);
                    var newReplacement = new Replacement(item);
                    replacements.Add(newReplacement);
                    LoadReplacements(replacements);
                }
            }
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = Prompt.ShowDialog("Enter {ReplaceThis}`{WithThis} Token pair (separate with `)", "Add {ReplaceThis}`{WithThis} Token pair");

            if (!string.IsNullOrWhiteSpace(item))
            {
                if (item.IndexOf("`") <= 0)
                {
                    MessageBox.Show("Please enter a string in the form\r\n\"{ReplaceThis}`{WithThis}\"\r\n... note backtick separator.", "Missing backtick in Replace`With string");
                }
                else
                {
                    var replacement = new Replacement(item);
                    replacements.Add(replacement);
                    LoadReplacements(replacements);
                }
            }
        }
    }
}

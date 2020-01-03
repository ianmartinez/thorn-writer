using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using Thorn.Inspectors;
using static Thorn.NotebookFile.KeyValue;

public enum NotebookInspectorValue
{
    PageCount
}

namespace Thorn.NotebookFile
{
    public class Notebook : IInspectable<NotebookInspectorValue>
    {
        public const double CurrentSpec = 3.1;
        public double SpecVersion { get; set; } = CurrentSpec;

        public event EventHandler<InspectorValueChangedEventArgs<NotebookInspectorValue>> ValueChanged;

        public string Title { get; set; } = "";
        public string Language { get; set; } = "";
        public string Author { get; set; } = "";
        public string Website { get; set; } = "";
        public string Info { get; set; } = "";

        public string Stylesheet { get; set; } = "";

        public List<string> Characters { get; set; } = new List<string>();
        public ObservableCollection<Page> Pages { get; set; } = new ObservableCollection<Page>();
        public Dictionary NotebookDictionary { get; set; } = new Dictionary();

        public bool Modified { get; set; } = false;
        public string FilePath { get; set; } = "";

        public Notebook()
        {
            Pages.CollectionChanged += Pages_CollectionChanged;
        }

        // Fire the ValueChanged event when the page count changes
        private void Pages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var args = new InspectorValueChangedEventArgs<NotebookInspectorValue>
            {
                OldValue = -1, // We don't care about the old value on this event
                NewValue = Pages.Count,
                TargetValue = NotebookInspectorValue.PageCount                
            };

            ValueChanged?.Invoke(this, args);
        }

        public void Save(string filePath)
        {
            var temp = new TempFolder();
            Directory.CreateDirectory(temp.PagesFolder);

            var dataFile = new List<Line>()
            {
                new Line(LineType.KeyValue, "title", Title),
                new Line(LineType.KeyValue, "language", Language),
                new Line(LineType.KeyValue, "author", Author),
                new Line(LineType.KeyValue, "website", Website),
                new Line(LineType.KeyValue, "specVersion", SpecVersion.ToString())
            };

            var pagesFile = new List<Line>();
            for (var i = 0; i < Pages.Count; i++)
            {
                var page = Pages[i];

                if (i > 0)
                    pagesFile.Add(new Line(LineType.Blank));

                pagesFile.Add(new Line(LineType.Comment, "Page " + i));
                pagesFile.Add(new Line(LineType.KeyValue, i + ".title", page.Title));
                File.WriteAllText(temp.GetPagePath(i), page.Content);
            }

            File.WriteAllText(temp.DataFile, Write(dataFile));
            File.WriteAllText(temp.StylesheetFile, Stylesheet);
            File.WriteAllText(temp.InfoFile, Info);
            File.WriteAllText(temp.CharactersFile, string.Join("\n", Characters));
            File.WriteAllText(temp.PagesFile, Write(pagesFile));
            File.WriteAllText(temp.DictionaryFile, NotebookDictionary.Save());

            // Delete the file if it already exists
            if (File.Exists(filePath))
                File.Delete(filePath);

            // Create zip file from the temp folder 
            ZipFile.CreateFromDirectory(temp.RootFolder, filePath, CompressionLevel.Optimal, false);

            // Delete temp folder now that it is no longer needed
            Directory.Delete(temp.RootFolder, true);
        }

        public void Open(string filePath)
        {
            var temp = new TempFolder();
            ZipFile.ExtractToDirectory(filePath, temp.RootFolder);

            // Read data file
            var dataFile = Read(File.ReadAllText(temp.DataFile));
            Title = Search(dataFile, "title");
            Language = Search(dataFile, "language");
            Author = Search(dataFile, "author");
            Website = Search(dataFile, "website");

            if(double.TryParse(Search(dataFile, "specVersion"), out double specVersion))
                SpecVersion = specVersion;

            // Clear out the old pages
            Pages.Clear();

            // Read the new pages
            var pagesFile = Read(File.ReadAllText(temp.PagesFile));
            var pageFiles = new List<string>(Directory.EnumerateFiles(temp.PagesFolder));
            for(var i=0; i<pageFiles.Count; i++)
            {
                var pageTitle = Search(pagesFile, i + ".title");
                var pageFilePath = temp.GetPagePath(i);
                if (File.Exists(pageFilePath))
                {
                    Pages.Add(new Page(this) {
                        Title = pageTitle,
                        Content = File.ReadAllText(pageFilePath)
                    });
                }
            }

            // Read stylesheet file
            Stylesheet = (File.Exists(temp.StylesheetFile)) ?
                            File.ReadAllText(temp.StylesheetFile) : "";

            // Read dictionary
            // TODO

            // Read custom characters
            Characters.Clear();
            var charactersFile = File.ReadAllText(temp.CharactersFile);
            var characters = charactersFile.Split(new string[] {
                Environment.NewLine, "\r\n", "\r", "\n"
            }, StringSplitOptions.RemoveEmptyEntries);
            Characters.AddRange(characters);

            // Delete temp folder now that it is no longer needed
            Directory.Delete(temp.RootFolder, true);
        }
    }
}

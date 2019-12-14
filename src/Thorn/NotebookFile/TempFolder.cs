using System;
using System.IO;

namespace Thorn.NotebookFile
{
    public class TempFolder
    {
        public static string AppDataFolder = Path.GetTempPath();

        public readonly string RootFolder;
        public readonly string PagesFolder;
        public readonly string DataFile;
        public readonly string PagesFile;
        public readonly string InfoFile;
        public readonly string CharactersFile;
        public readonly string DictionaryFile;
        public readonly string StylesheetFile;

        public TempFolder()
        {
            RootFolder = GetNewTempFolderRoot();

            if (Directory.Exists(RootFolder))
                Directory.Delete(RootFolder, true);

            Directory.CreateDirectory(RootFolder);

            PagesFolder = Path.Combine(RootFolder, "pages");
            DataFile = Path.Combine(RootFolder, "data.txt"); 
            PagesFile = Path.Combine(RootFolder, "pages.txt");
            InfoFile = Path.Combine(RootFolder, "info.txt");
            CharactersFile = Path.Combine(RootFolder, "characters.txt");
            DictionaryFile = Path.Combine(RootFolder, "dictionary.txt");
            StylesheetFile = Path.Combine(RootFolder, "style.css");
        }

        public static string GetNewTempFolderRoot()
        {
            return Path.Combine(AppDataFolder, string.Format("notebook-{0}", Guid.NewGuid().ToString()));
        }

        public string GetPagePath(int pageIndex)
        {
            return Path.Combine(PagesFolder, string.Format("{0}.html", pageIndex.ToString()));
        }
    }
}

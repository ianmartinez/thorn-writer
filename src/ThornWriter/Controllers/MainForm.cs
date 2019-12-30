using System;
using Eto.Forms;
using Thorn.NotebookFile;
using ThornWriter.Web;

namespace ThornWriter
{
    public partial class MainForm : Form
    {
        Notebook Document = new Notebook();
        TreeGridItemCollection PageSelectorItems = new TreeGridItemCollection();

        public void LoadPages()
        {
            PageSelectorItems.Clear();
            var pageIcon = Icons.Get("text");

            foreach (Page page in Document.Pages)
            {
                var notebookPagesTreeItem = new TreeGridItem()
                {
                    Expanded = false,
                    Values = new object[] { pageIcon, page.Title },
                };

                PageSelectorItems.Add(notebookPagesTreeItem);
            }

            PageSelector.DataStore = PageSelectorItems;

            if (PageSelectorItems.Count > 0 && this.Loaded)
                PageSelector.SelectedRow = 0;
        }

        public void DeletePage()
        {

        }

        /**
         * Event handlers
         */

        // File Menu
        FileFilter[] filters = {
            new FileFilter("Thorn Writer Notebook (*.thw)", ".thw")
        };

        public void OnNewNotebook(object sender, EventArgs e)
        {

        }
        
        public void OnOpenNotebook(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();

            foreach (var filter in filters)
                openDialog.Filters.Add(filter);

            openDialog.CurrentFilterIndex = 0;

            if (openDialog.ShowDialog(this) == DialogResult.Ok)
            {
                Document.Open(openDialog.FileName);
                LoadPages();
            }
        }

        public void OnSaveNotebook(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();

            foreach (var filter in filters)
                saveDialog.Filters.Add(filter);

            saveDialog.CurrentFilterIndex = 0;

            if (saveDialog.ShowDialog(this) == DialogResult.Ok)
            {
                Document.Save(saveDialog.FileName);
                Document.Modified = false;
            }
        }

        // Edit Menu
        public void OnUndo(object sender, EventArgs e)
        {

        }

        public void OnRedo(object sender, EventArgs e)
        {

        }

        public void OnCut(object sender, EventArgs e)
        {

        }

        public void OnCopy(object sender, EventArgs e)
        {

        }

        public void OnPaste(object sender, EventArgs e)
        {

        }

        // Language Menu
        public void OnCharacters(object sender, EventArgs e)
        {

        }

        // Application Menu
        public void OnQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }

        public void OnAbout(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.ProgramName = "Thorn Writer";
            aboutDialog.Version = "Version " + AppInfo.GetFormattedVersion();
            aboutDialog.Copyright = string.Format("© 2012-{0} Ian Martinez", DateTime.Now.Year.ToString());
            aboutDialog.ProgramDescription = "A editor for linguistics.";
            aboutDialog.License = AppInfo.GetLicense();
            aboutDialog.ShowDialog(this);
        }

        public void OnChangeSelection(object sender, EventArgs e)
        {
            var pageIndex = PageSelector.SelectedRow;

            if (pageIndex != -1)
                PageEditor.Content = Document.Pages[pageIndex].Content;
        }

        public void OnContentChanged(object sender, EventArgs e)
        {
            var pageIndex = PageSelector.SelectedRow;

            if (pageIndex != -1)
                Document.Pages[pageIndex].Content = PageEditor.Content;
        }

        public void OnChangePageTitle(object sender, GridViewCellEventArgs e)
        {
            var pageIndex = PageSelector.SelectedRow;

            if (pageIndex != -1)
            {
                var item = (TreeGridItem)e.Item;
                /*
                 * icon = item.Values[0]
                 * text = item.Values[1]
                 */
                var newTitle = item.Values[1].ToString();

                // Change page title
                Document.Pages[pageIndex].Title = newTitle;
            }
        }
    }
}

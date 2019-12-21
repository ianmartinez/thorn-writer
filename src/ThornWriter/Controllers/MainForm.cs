using System;
using Eto.Forms;
using Eto.Drawing;
using Thorn.NotebookFile;
using ThornWriter.Web;
using System.Collections.Generic;

namespace ThornWriter
{
    public partial class MainForm : Form
    {
        Notebook Document = new Notebook();
        TreeGridItemCollection PageSelectorItems = new TreeGridItemCollection();
        TextEditor DocumentEdit;

        public void LoadPages()
        {
            PageSelectorItems.Clear();
            var dummyIcon = new Bitmap(25, 25, PixelFormat.Format24bppRgb);

            foreach (Page page in Document.Pages)
            {
                var notebookPagesTreeItem = new TreeGridItem()
                {
                    Expanded = false,
                    Values = new object[] { dummyIcon, page.Title },
                };

                PageSelectorItems.Add(notebookPagesTreeItem);
            }

            PageSelector.DataStore = PageSelectorItems;
        }

        public void DeletePage()
        {

        }

        /**ß
         * Event handlers
         */
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

            if (openDialog.ShowDialog(this) == DialogResult.Ok)
            {
                Document.Open(openDialog.FileName);
                LoadPages();
            }
        }

        public void OnSaveNotebook(object sender, EventArgs e)
        {

        }

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
                DocumentEdit.Content = Document.Pages[pageIndex].Content;
        }
    }
}

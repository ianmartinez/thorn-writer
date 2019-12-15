using System;
using Eto.Forms;
using Eto.Drawing;
using Thorn.NotebookFile;
using System.Reflection;
using Thorn.Web;
using ThornWriter.Web;

namespace ThornWriter
{
    public partial class MainForm : Form
    {
        Notebook Document = new Notebook();
        TreeGridItemCollection PageSelectorItems = new TreeGridItemCollection();
        IWebViewManager PreviewManager;
        IWebViewManager EditManager;
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

        /**
         * Event handlers
         */

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
            {
                var pageBody = Document.Pages[pageIndex].Content;
                var pageContent = HtmlRenderer.Render(Resources.DocumentBase, "DocumentBody", pageBody);
                PreviewManager.Content = pageContent;
                var script = string.Format("document.body.style.color = 'rgb({0},{0},{0})'", pageIndex);
                var s = EditManager.RunScript(script);
                DocumentEdit.Theme = TextEditorTheme.Light;
            }
        }
    }
}

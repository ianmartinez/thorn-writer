using Eto.Forms;
using Eto.Drawing;
using Thorn.NotebookFile;
using ThornWriter.Web;
using Thorn.Web;
using System;

namespace ThornWriter
{
    public class MainForm : Form
    {
        IWebViewManager PreviewManager;
        IWebViewManager EditManager;
        TextEditor PageEditor;
        Notebook Document = new Notebook();

        #region "Windows"
        PreferencesForm preferencesDialog = new PreferencesForm();
        #endregion

        #region "Controls"
        WebView DocumentPreview = new WebView();
        WebView DocumentEditor = new WebView();
        TreeGridItemCollection PageSelectorItems = new TreeGridItemCollection();
        TreeGridView PageSelector = new TreeGridView();
        Panel MainPanel = new Panel();
        Splitter MainSplitter = new Splitter();
        Splitter DocumentSplitter = new Splitter();
        #endregion

        #region "UI"
        string appTitle = "Thorn Writer " + AppInfo.GetFormattedVersion();
        public MainForm()
        {
            Title = appTitle;
            ClientSize = new Size(700, 500);
            // Set managers
            PreviewManager = new EtoWebViewManager(DocumentPreview);
            EditManager = new EtoWebViewManager(DocumentEditor);
            PageEditor = new TextEditor(EditManager, PreviewManager);
            PageEditor.ContentChanged += OnContentChanged;

            var documentBase = Resources.DocumentBase;
            documentBase = HtmlRenderer.RenderStyle(documentBase, "DocumentStyle", Resources.DocumentStyle);
            PageEditor.PreviewBase = documentBase;

            // Page Selector
            PageSelector.Columns.Add(new GridColumn()
            {
                DataCell = new ImageTextCell(0, 1),
                AutoSize = true,
                Resizable = false,
                Editable = true,
            });
            PageSelector.ShowHeader = false;
            PageSelector.Columns[0].Width = 400;
            PageSelector.Border = BorderType.None;
            PageSelector.SelectionChanged += OnChangeSelection;
            PageSelector.CellEdited += OnChangePageTitle;

            // Document Splitter
            DocumentSplitter.Panel1 = DocumentPreview;
            DocumentSplitter.Panel2 = DocumentEditor;
            DocumentSplitter.Orientation = Orientation.Vertical;
            DocumentSplitter.Position = 2 * (ClientSize.Height / 3);
            DocumentSplitter.FixedPanel = SplitterFixedPanel.Panel1;

            // Main Splitter
            MainSplitter.Panel1 = PageSelector;
            PageSelector.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            MainSplitter.Panel2 = DocumentSplitter;
            MainSplitter.Orientation = Orientation.Horizontal;
            MainSplitter.Position = 1 * (ClientSize.Width / 3);
            MainSplitter.FixedPanel = SplitterFixedPanel.Panel1;

            MainPanel.Content = MainSplitter;
            Content = MainPanel;

            // Commands - Application
            var prefrencesCommand = new Command
            {
                MenuText = "&Preferences...",
                ToolBarText = "Preferences",
                Image = Icons.Get("config")
            };
            prefrencesCommand.Executed += OnPreferences;

            // Commands - File
            var newNotebookCommand = new Command
            {
                MenuText = "New Notebook",
                ToolBarText = "New",
                Image = Icons.Get("document-new")
            };
            newNotebookCommand.Executed += OnNewNotebook;

            var openNotebookCommand = new Command
            {
                MenuText = "Open...",
                ToolBarText = "Open",
                Image = Icons.Get("document-open")
            };
            openNotebookCommand.Executed += OnOpenNotebook;

            var saveNotebookCommand = new Command
            {
                MenuText = "Save...",
                ToolBarText = "Save",
                Image = Icons.Get("document-save")
            };
            saveNotebookCommand.Executed += OnSaveNotebook;

            // Commands - Application
            var quitCommand = new Command
            {
                MenuText = "Quit",
                Shortcut = Application.Instance.CommonModifier | Keys.Q
            };
            quitCommand.Executed += OnQuit;

            var aboutCommand = new Command
            {
                MenuText = "About..."
            };
            aboutCommand.Executed += OnAbout;

            // Commands - Edit
            var undoCommand = new Command
            {
                MenuText = "Undo",
                ToolBarText = "Undo",
                Image = Icons.Get("edit-undo")
            };
            undoCommand.Executed += OnUndo;

            var redoCommand = new Command
            {
                MenuText = "Redo",
                ToolBarText = "Redo",
                Image = Icons.Get("edit-redo")
            };
            redoCommand.Executed += OnRedo;

            var cutCommand = new Command
            {
                MenuText = "Cut",
                ToolBarText = "Cut",
                Image = Icons.Get("edit-cut")
            };
            cutCommand.Executed += OnCut;

            var copyCommand = new Command
            {
                MenuText = "Copy",
                ToolBarText = "Copy",
                Image = Icons.Get("edit-copy")
            };
            copyCommand.Executed += OnCopy;

            var pasteCommand = new Command
            {
                MenuText = "Paste",
                ToolBarText = "Paste",
                Image = Icons.Get("edit-paste")
            };
            pasteCommand.Executed += OnPaste;

            // Language
            var charactersCommand = new Command
            {
                MenuText = "Characters...",
                ToolBarText = "Characters",
                Image = Icons.Get("characters")
            };
            charactersCommand.Executed += OnCharacters;

            var dictionaryCommand = new Command
            {
                MenuText = "Dictionary...",
                ToolBarText = "Dictionary",
                Image = Icons.Get("dictionary")
            };
            dictionaryCommand.Executed += OnCharacters;

            // Menu
            Menu = new MenuBar
            {
                Items =
                {
                    new ButtonMenuItem {Text = "&File", Items = {
                        newNotebookCommand, openNotebookCommand,
                        saveNotebookCommand
                    }},
                    new ButtonMenuItem { Text = "&Edit", Items = {
                        undoCommand, redoCommand,
                        new SeparatorMenuItem(),
                        cutCommand, copyCommand, pasteCommand
                    }},
                    new ButtonMenuItem { Text = "&Language", Items = {
                        charactersCommand, dictionaryCommand
                    }}
                },
                ApplicationItems =
                {
                    prefrencesCommand
                },
                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };

            // Toolbar		
            ToolBar = new ToolBar
            {
                Items = {
                        newNotebookCommand, openNotebookCommand, saveNotebookCommand,
                        new SeparatorToolItem() { Type = SeparatorToolItemType.FlexibleSpace },
                        charactersCommand, dictionaryCommand
                    }
            };


            // Load document
            var testStr = "";
            Document.Title = "Test Document";
            for (int i = 0; i < 100; i++)
            {
                testStr += "<b>Hello World</b> #" + i + "\n";
                Document.Pages.Add(new Page()
                {
                    Title = "Page " + i,
                    Content = testStr
                });
            }

            LoadPages();
        }
        #endregion
        
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

            if (PageSelectorItems.Count > 0 && Loaded)
                PageSelector.SelectedRow = 0;

            UpdateAppTitle();
        }

        public void UpdateAppTitle()
        {
            var pageIndex = PageSelector.SelectedRow;
            var pageName = (pageIndex == -1) ? "" : " (" + Document.Pages[pageIndex].Title + ")";
            if (!string.IsNullOrEmpty(Document.Title))
                Title = string.Format("{0} - {1}{2}", appTitle, Document.Title, pageName);
            else
                Title = appTitle;
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
        public void OnPreferences(object sender, EventArgs e)
        {
            if (!preferencesDialog.Visible)
                preferencesDialog.Show();
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
                PageEditor.Content = Document.Pages[pageIndex].Content;

            UpdateAppTitle();
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
                UpdateAppTitle();
            }
        }
    }
}

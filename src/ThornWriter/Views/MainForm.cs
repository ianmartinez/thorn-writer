using Eto.Forms;
using Eto.Drawing;
using Thorn.NotebookFile;
using ThornWriter.Web;
using Thorn.Web;

namespace ThornWriter
{
    public partial class MainForm : Form
    {
        #region "Control Declarations"
        WebView DocumentPreview = new WebView();
        WebView DocumentEditor = new WebView();
        TreeGridView PageSelector = new TreeGridView();
        Panel MainPanel = new Panel();
        Splitter MainSplitter = new Splitter();
        Splitter DocumentSplitter = new Splitter();
        #endregion

        IWebViewManager PreviewManager;
        IWebViewManager EditManager;
        TextEditor PageEditor;
        public MainForm()
        {
            Title = "Thorn Writer " + AppInfo.GetFormattedVersion();
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
            PageSelector.Border = BorderType.None;
            PageSelector.SelectionChanged += OnChangeSelection;

            for (int i = 0; i < 100; i++)
            {
                Document.Pages.Add(new Page()
                {
                    Title = "Page " + i,
                    Content = "<b>Hello World</b> #" + i
                });
            }

            LoadPages();

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


            // Commands - File
            var newNotebookCommand = new Command {
                MenuText = "New Notebook",
                ToolBarText = "New",
                Image = Icons.Get("document-new")
            };
            newNotebookCommand.Executed += OnNewNotebook;

            var openNotebookCommand = new Command {
                MenuText = "Open...",
                ToolBarText = "Open",
                Image = Icons.Get("document-open")
            };
            openNotebookCommand.Executed += OnOpenNotebook;

            var saveNotebookCommand = new Command {
                MenuText = "Save...",
                ToolBarText = "Save",
                Image = Icons.Get("document-save")
            };
            saveNotebookCommand.Executed += OnSaveNotebook;

            // Commands - Application
            var quitCommand = new Command {
                MenuText = "Quit",
                Shortcut = Application.Instance.CommonModifier | Keys.Q
            };
            quitCommand.Executed += OnQuit;

            var aboutCommand = new Command {
                MenuText = "About..."
            };
            aboutCommand.Executed += OnAbout;
            
            // Commands - Edit
            var undoCommand = new Command {
                MenuText = "Undo",
                ToolBarText = "Undo",
                Image = Icons.Get("edit-undo")
            };
            undoCommand.Executed += OnUndo;

            var redoCommand = new Command {
                MenuText = "Redo",
                ToolBarText = "Redo",
                Image = Icons.Get("edit-redo")
            };
            redoCommand.Executed += OnRedo;

            var cutCommand = new Command {
                MenuText = "Cut",
                ToolBarText = "Cut",
                Image = Icons.Get("edit-cut")
            };
            cutCommand.Executed += OnCut;

            var copyCommand = new Command {
                MenuText = "Copy",
                ToolBarText = "Copy",
                Image = Icons.Get("edit-copy")
            };
            copyCommand.Executed += OnCopy;

            var pasteCommand = new Command {
                MenuText = "Paste",
                ToolBarText = "Paste",
                Image = Icons.Get("edit-paste")
            };
            pasteCommand.Executed += OnPaste;

            // Language
            var charactersCommand = new Command {
                MenuText = "Characters...",
                ToolBarText = "Characters",
                Image = Icons.Get("characters")            
            };
            charactersCommand.Executed += OnCharacters;

            var dictionaryCommand = new Command {
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
                    new ButtonMenuItem { Text = "&Preferences..." },
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
        }
    }
}

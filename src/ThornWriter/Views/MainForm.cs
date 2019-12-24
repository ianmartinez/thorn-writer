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
        Splitter MainSplitter = new Splitter();
        Splitter DocumentSplitter = new Splitter();
        #endregion

        IWebViewManager PreviewManager;
        IWebViewManager EditManager;
        public MainForm()
        {
            Title = "Thorn Writer " + AppInfo.GetFormattedVersion();
            ClientSize = new Size(700, 500);
            // Set managers
            PreviewManager = new EtoWebViewManager(DocumentPreview);
            EditManager = new EtoWebViewManager(DocumentEditor);
            DocumentEdit = new TextEditor(EditManager, PreviewManager);

            var documentBase = Resources.DocumentBase;
            documentBase = HtmlRenderer.RenderStyle(documentBase, "DocumentStyle", Resources.DocumentStyle);
            DocumentEdit.PreviewBase = documentBase;

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
            MainSplitter.Panel2 = DocumentSplitter;
            MainSplitter.Orientation = Orientation.Horizontal;
            MainSplitter.Position = 1 * (ClientSize.Width / 3);
            MainSplitter.FixedPanel = SplitterFixedPanel.Panel1;

            Content = MainSplitter;

            // Commands - File
            var newNotebookCommand = new Command {
                MenuText = "New Notebook",
                ToolBarText = "New",
                Image = Icon.FromResource("ThornWriter.Resources.Icons.document-new.png")
            };
            newNotebookCommand.Executed += OnNewNotebook;

            var openNotebookCommand = new Command {
                MenuText = "Open...",
                ToolBarText = "Open",
                Image = Icon.FromResource("ThornWriter.Resources.Icons.folder-open.png")
            };
            openNotebookCommand.Executed += OnOpenNotebook;

            var saveNotebookCommand = new Command {
                MenuText = "Save...",
                ToolBarText = "Save",
                Image = Icon.FromResource("ThornWriter.Resources.Icons.text-x-install.png")
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
                Image = Icon.FromResource("ThornWriter.Resources.Icons.edit-undo.png")
            };
            undoCommand.Executed += OnUndo;

            var redoCommand = new Command {
                MenuText = "Redo",
                ToolBarText = "Redo",
                Image = Icon.FromResource("ThornWriter.Resources.Icons.edit-redo.png")
            };
            redoCommand.Executed += OnRedo;

            var cutCommand = new Command {
                MenuText = "Cut",
                ToolBarText = "Cut"
            };
            cutCommand.Executed += OnCut;

            var copyCommand = new Command {
                MenuText = "Copy",
                ToolBarText = "Copy"
            };
            copyCommand.Executed += OnCopy;

            var pasteCommand = new Command {
                MenuText = "Paste",
                ToolBarText = "Paste"
            };
            pasteCommand.Executed += OnPaste;

            // Language
            var charactersCommand = new Command {
                MenuText = "Characters...",
                ToolBarText = "Characters",
                Image = Icon.FromResource("ThornWriter.Resources.Icons.preferences-desktop-locale.png")                
            };
            charactersCommand.Executed += OnCharacters;

            var dictionaryCommand = new Command {
                MenuText = "Dictionary...",
                ToolBarText = "Dictionary",
                Image = Icon.FromResource("ThornWriter.Resources.Icons.dictionary.png")
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
                        new SeparatorToolItem() { Type = SeparatorToolItemType.Space },
                        undoCommand, redoCommand,
                        new SeparatorToolItem() { Type = SeparatorToolItemType.FlexibleSpace },
                        charactersCommand, dictionaryCommand
                    }
            };
        }
    }
}

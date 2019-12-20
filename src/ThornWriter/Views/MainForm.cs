using Eto.Forms;
using Eto.Drawing;
using Thorn.NotebookFile;
using ThornWriter.Web;

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

        public MainForm()
		{
            Title = "Thorn Writer " + AppInfo.GetFormattedVersion();
			ClientSize = new Size(700, 500);
            // Set managers
            PreviewManager = new EtoWebViewManager(DocumentPreview);
            EditManager = new EtoWebViewManager(DocumentEditor);
            DocumentEdit = new TextEditor(EditManager);

            // Page Selector
            PageSelector.Columns.Add(new GridColumn() {
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

            // Commands
            var newNotebookCommand = new Command { MenuText = "New", ToolBarText = "New" };
			newNotebookCommand.Executed += OnNewNotebook;

            var openNotebookCommand = new Command { MenuText = "Open...", ToolBarText = "Open" };
            openNotebookCommand.Executed += OnOpenNotebook;

            var saveNotebookCommand = new Command { MenuText = "Save...", ToolBarText = "Save" };
            saveNotebookCommand.Executed += OnSaveNotebook;

            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
			quitCommand.Executed += OnQuit;

			var aboutCommand = new Command { MenuText = "About..." };
			aboutCommand.Executed += OnAbout;

			// create menu
			Menu = new MenuBar
			{
				Items =
				{
					new ButtonMenuItem {Text = "&File", Items = {
                        newNotebookCommand, openNotebookCommand,
                        saveNotebookCommand
                    }},
					new ButtonMenuItem { Text = "&Edit", Items = { /* commands/items */ } }
				},
				ApplicationItems =
				{
					new ButtonMenuItem { Text = "&Preferences..." },
				},
				QuitItem = quitCommand,
				AboutItem = aboutCommand
		    };

		    // create toolbar			
		    ToolBar = new ToolBar {Items = {
                        newNotebookCommand, openNotebookCommand,
                        saveNotebookCommand
                    }
            };
		}
	}
}

using Eto.Forms;
using Thorn.Web;

namespace ThornWriter.Web
{
    public enum TextEditTheme {
        Light,
        Dark,
        SolarizedLight,
        SolarizedDark
    }

    /*
     * A wrapper for a text edit instance hosted inside a web view
     * managed by ViewManager. Handles dealing with all interop between
     * the js side and the c# side.
     */ 
    public class TextEditor
    {
        private readonly IWebViewManager ViewManager;
        private readonly IWebViewManager PreviewViewManager;
        private UITimer PreviewTimer;

        public TextEditor(IWebViewManager viewManager, IWebViewManager previewViewManager = null)
        {
            ViewManager = viewManager;
            PreviewViewManager = previewViewManager;

            LoadTextEditDocument();

            if(previewViewManager != null)
                LoadPreview();
        }

        private TextEditTheme _theme;
        public TextEditTheme Theme
        {
            get
            {
                return _theme;
            }

            set
            {
                _theme = value;
                ViewManager.RunScript(string.Format("setEditorTheme('{0}');", _theme.ToString()));
            }
        }

        public string Content
        {
            get
            {
                return ViewManager.RunScript("return getEditorContent();");
            }

            set
            {
                var safeValue = MakeSafe(value);
                ViewManager.RunScript(string.Format("setEditorContent('{0}');", safeValue));
                RefreshPreview();
            }
        }

        /**
         * The base document to insert the editor code into before
         * inserting into the PreviewViewManager, if blank just insert
         * the text verbatim.
         */
        public string PreviewBase { get; set; }

        private string MakeSafe(string value)
        {
            var encodedValue = value.Replace("\n", "\\n");
            encodedValue = encodedValue.Replace("\r", "\\r");
            encodedValue = encodedValue.Replace("'", "\\'");
            return encodedValue;
        }

        private string lastPreview = "";
        public void RefreshPreview()
        {
            var newPreview = Content;

            if(PreviewViewManager != null && !lastPreview.Equals(newPreview))
            {
                /*
                 * Store the preview content that is being set so that the
                 * preview doesn't constantly refresh when it doesn't have
                 * to
                 */
                lastPreview = newPreview;

                if (!string.IsNullOrEmpty(PreviewBase))
                {
                    PreviewViewManager.Content = PreviewBase.Replace("{Content}", newPreview);
                }
                else
                {
                    PreviewViewManager.Content = newPreview;
                }
            }
        }

        private void LoadTextEditDocument()
        {
            string html = Resources.TextEdit;
            html = HtmlRenderer.RenderStyle(html, "CodeMirrorStyle", Resources.CodeMirrorStyle);
            html = HtmlRenderer.RenderScript(html, "CodeMirrorScript", Resources.CodeMirrorScript);
            
            html = HtmlRenderer.RenderScript(html, "CodeMirrorModeCss", Resources.CodeMirrorModeCss);
            html = HtmlRenderer.RenderScript(html, "CodeMirrorModeXml", Resources.CodeMirrorModeXml);
            html = HtmlRenderer.RenderScript(html, "CodeMirrorModeJs", Resources.CodeMirrorModeJs);
            html = HtmlRenderer.RenderScript(html, "CodeMirrorModeHtmlMixed", Resources.CodeMirrorModeHtmlMixed);

            html = HtmlRenderer.RenderStyle(html, "TextEditStyle", Resources.TextEditStyle);
            html = HtmlRenderer.RenderScript(html, "TextEditScript", Resources.TextEditScript);

            html = HtmlRenderer.RenderStyle(html, "CodeMirrorTheme", Resources.CodeMirrorThemeAyuMirage);

            ViewManager.Content = html;
        }

        private void LoadPreview()
        {
            // Refresh the preview every .5 seconds
            PreviewTimer = new UITimer { Interval = 0.5 };
            PreviewTimer.Elapsed += (sender, e) => RefreshPreview();
            PreviewTimer.Start();
        }
    }
}

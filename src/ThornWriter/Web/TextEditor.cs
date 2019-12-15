using System;
using Thorn.Web;

namespace ThornWriter.Web
{
    public enum TextEditorTheme {
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
        public TextEditor(IWebViewManager viewManager)
        {
            ViewManager = viewManager;
            LoadTextEditDocument();
        }

        private TextEditorTheme _theme;
        public TextEditorTheme Theme
        {
            get
            {
                return _theme;
            }

            set
            {
                _theme = value;
                ViewManager.RunScript(string.Format("setTheme('{0}');", _theme.ToString()));
            }
        }

        public string Text
        {
            get; set;
        }


        private void LoadTextEditDocument()
        {
            string html = Resources.TextEditor;
            html = HtmlRenderer.RenderStyle(html, "Style", "body{background: blue;}");
            ViewManager.Content = html;
        }
    }
}

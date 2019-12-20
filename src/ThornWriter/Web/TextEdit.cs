using System;
using System.Net;
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
        public TextEditor(IWebViewManager viewManager)
        {
            ViewManager = viewManager;
            LoadTextEditDocument();
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
            }
        }

        private string MakeSafe(string value)
        {
            var encodedValue = value.Replace("\n", "\\n");
            encodedValue = encodedValue.Replace("\r", "\\r");
            encodedValue = encodedValue.Replace("'", "\\'");
            return encodedValue;
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

            ViewManager.Content = html;
        }
    }
}

using System;
using Thorn.Web;

namespace ThornWriter.Web
{
    public class TextEditor
    {
        private IWebViewManager viewManager;
        
        public string Text
        {
            get; set;
        }

        public TextEditor(IWebViewManager viewManager)
        {
            this.viewManager = viewManager;
            LoadTextEditDocument();
        }


        private void LoadTextEditDocument()
        {
            string html = Resources.TextEditor;
            html = HtmlRenderer.RenderStyle(html, "Style", "body{background: blue;}");
            viewManager.Content = html;
        }
    }
}

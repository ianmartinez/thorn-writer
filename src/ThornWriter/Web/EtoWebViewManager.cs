﻿using System;
using System.Threading.Tasks;
using Eto.Forms;
using Thorn.Web;

namespace ThornWriter.Web
{
    public class EtoWebViewManager : IWebViewManager
    {
        private WebView webView;
        private bool hasLoaded;

        public EtoWebViewManager(WebView webView)
        {
            this.webView = webView;

            // Add an event handler to watch for the document to complete loading
            webView.DocumentLoaded += (sender, e) =>
            {
                hasLoaded = true;
            };
        }

        private string _html;
        public string Content {
            get => _html;
            set
            {
                hasLoaded = false;
                _html = value;
                webView.LoadHtml(_html);
            }
        }

        public string Url {
            get => webView.Url.ToString();
            set => webView.Url = new Uri(value);
        }

        public void GoBack()
        {
            webView.GoBack();
        }

        public void GoForward()
        {
            webView.GoForward();
        }

        public void Refresh()
        {
            webView.Reload();
        }

        /**
         * Execute a script on the page and return the string result, but only
         * *after* the document has loaded.
         */
        public string RunScript(string js)
        {
            // Wait until the document has loaded
            while (!hasLoaded)
                Task.Delay(100).Wait();

            // Now run the script
            return webView.ExecuteScript(js);

        }

    }
}

using System;
using System.Threading.Tasks;
using Eto.Forms;
using Thorn.Web;

namespace ThornWriter.Web
{
    public class EtoWebViewManager : IWebViewManager
    {
        private WebView webView;

        public EtoWebViewManager(WebView webView)
        {
            this.webView = webView;
        }

        private string _html;
        public string Content {
            get => _html;
            set
            {
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
         * I know its ugly and there's probably a better way,
         * but it works for now.
         */
        public async Task<string> RunScript(string js)
        {
            string result = await Task.Run(() => {
                var stringResult = "";
                var hasLoaded = false;

                EventHandler<WebViewLoadedEventArgs> onLoaded = null;
                onLoaded = (object sender, WebViewLoadedEventArgs e) =>
                {
                    hasLoaded = true;
                    stringResult = webView.ExecuteScript("return " + js);
                    webView.DocumentLoaded -= onLoaded;
                };

                webView.DocumentLoaded += onLoaded;

                while (!hasLoaded)
                    Task.Delay(100).Wait();

                return stringResult;
            });

            return result;
        }

    }
}

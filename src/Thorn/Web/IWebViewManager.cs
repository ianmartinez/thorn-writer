﻿using System;
using System.Threading.Tasks;

namespace Thorn.Web
{
    /**
     * Provides an interface to deal with different implementations
     * of web views across platforms
     */
    public interface IWebViewManager
    {
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; }
        public void Refresh();
        public void GoBack();
        public void GoForward();
        public string RunScript(string js);
    }
}

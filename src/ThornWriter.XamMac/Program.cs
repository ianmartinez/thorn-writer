using System;
using AppKit;
using Eto;
using Eto.Forms;
using Eto.Mac.Forms;
using Eto.Mac.Forms.Controls;

namespace ThornWriter.XamMac
{
	static class MainClass
	{
		static void Main(string[] args)
        {
            // Enable fullscreen
            Style.Add<ApplicationHandler>("application", handler => {
                handler.EnableFullScreen();
            });
            new Application(Eto.Platforms.XamMac2).Run(new MainForm());
        }
	}
}

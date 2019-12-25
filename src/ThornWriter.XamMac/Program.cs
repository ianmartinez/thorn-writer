using System;
using AppKit;
using Eto.Forms;
using Eto.Mac.Forms.Controls;

namespace ThornWriter.XamMac
{
	static class MainClass
	{
		static void Main(string[] args)
		{
            new Application(Eto.Platforms.XamMac2).Run(new MainForm());
		}
	}
}

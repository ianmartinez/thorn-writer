using Eto;
using Eto.Forms;
using Eto.Wpf.Forms.ToolBar;
using System;

namespace ThornWriter.Win
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Enable fullscreen
            Style.Add<ButtonToolItemHandler>("toolbarButton", handler => {
               var control =  handler.Control;

            });

            new Application(Platforms.Wpf).Run(new MainForm());
        }
    }
}

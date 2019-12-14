using System;
using Eto.Forms;
using Eto.Drawing;
using Thorn.NotebookFile;
using System.Reflection;

namespace ThornWriter
{
    public partial class MainForm : Form
    {
        public void OnQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }

        public void OnAbout(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog(this);
        }
    }
}

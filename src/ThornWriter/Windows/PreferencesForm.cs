using System;
using Eto.Drawing;
using Eto.Forms;

namespace ThornWriter
{
    public class PreferencesForm : Form
    {
        public PreferencesForm()
        {
            Title = "Preferences";
            ClientSize = new Size(600, 400);
            Resizable = false;
            Maximizable = false;
            Minimizable = false;
        }
    }
}

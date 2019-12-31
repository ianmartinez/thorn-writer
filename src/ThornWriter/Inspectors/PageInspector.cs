using Eto.Forms;
using Thorn.NotebookFile;

namespace ThornWriter.Inspectors
{
    public class PageInspector : Form, IInspector<Page>
    {
        private Page model;

        public PageInspector()
        {
            Title = "Page Properties";
        }

        public Page Model
        {
            get => model;

            set
            {
                model = value;
                RefreshAll();
            }
        }

        public void RefreshValue(string name)
        {

        }

        public void RefreshAll()
        {

        }
    }
}

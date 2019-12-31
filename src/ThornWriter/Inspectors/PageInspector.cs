using System;
using Eto.Drawing;
using Thorn.NotebookFile;

namespace ThornWriter.Inspectors
{
    public enum PageInspectorValue
    {
        Title,
        Position
    }

    public class PageInspector : InspectorForm<Page, PageInspectorValue>
    {
        public PageInspector()
        {
            Title = "Page Info";
            Size = new Size(300, 600);
        }

        public override void RefreshAll()
        {
            base.RefreshAll();
        }

        public override void RefreshValue(PageInspectorValue name)
        {
            switch (name)
            {
                case PageInspectorValue.Title:
                    Title =  Model.Title + " Info";
                    break;
                case PageInspectorValue.Position:
                    break;
            }
        }
    }
}

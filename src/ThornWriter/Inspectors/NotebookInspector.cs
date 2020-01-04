using System;
using Thorn.NotebookFile;

namespace ThornWriter.Inspectors
{
    public class NotebookInspector : InspectorForm<Notebook, NotebookInspectorValue>
    {
        public NotebookInspector()
        {
        }

        public override void RefreshValue(NotebookInspectorValue targetValue)
        {
            throw new NotImplementedException();
        }
    }
}

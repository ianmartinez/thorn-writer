using System;
using Eto.Drawing;
using Eto.Forms;
using Thorn.NotebookFile;

namespace ThornWriter.Inspectors
{
    public enum PageInspectorValue
    {
        Title,
        Index,
        Notes
    }

    public class PageInspector : InspectorForm<Page, PageInspectorValue>
    {
        TableLayout layout;
        TextBox titleTextBox;
        TextArea notesTextArea;
        NumericStepper indexStepper;

        public PageInspector()
        {
            Title = "Page Info";
            Size = new Size(300, -1);

            titleTextBox = new TextBox();
            titleTextBox.TextChanged += OnTitleChanged;

            indexStepper = new NumericStepper
            {
                MinValue = 0
            };
            indexStepper.ValueChanged += OnIndexChanged;

            notesTextArea = new TextArea();
            notesTextArea.TextChanged += OnNotesChanged;

            layout = new TableLayout()
            {
                Padding = new Padding(10), // padding around cells
                Spacing = new Size(5, 5), // spacing between each cell
                Rows =
                    {
                        new TableRow(new Label { Text = "Title:", VerticalAlignment = VerticalAlignment.Bottom }),
                        new TableRow(new TableCell(titleTextBox, true)),
                        new TableRow(new Label { Text = "Index:", VerticalAlignment = VerticalAlignment.Bottom}),
                        new TableRow(new TableCell(indexStepper, true)),
                        new TableRow(new Label { Text = "Notes:", VerticalAlignment = VerticalAlignment.Bottom}),
                        new TableRow(new TableCell(notesTextArea, true))
                    }
            };

            Content = new Panel()
            {
                Content = layout
            };
        }

        public override void UpdateValue<ValueType>(PageInspectorValue targetValue, IComparable oldValue, IComparable newValue)
        {
            base.UpdateValue<ValueType>(targetValue, oldValue, newValue);
        }

        public override void RefreshAll()
        {
            base.RefreshAll();
        }


        public override void RefreshValue(PageInspectorValue targetValue)
        {
            isRefreshing = true;

            switch (targetValue)
            {
                case PageInspectorValue.Title:
                    titleTextBox.Text = Model.Title;
                    UpdateTitle();
                    break;
                case PageInspectorValue.Index:
                    indexStepper.Value = Model.Index;
                    break;
            }

            isRefreshing = false;
        }

        private void UpdateTitle()
        {
            Title = titleTextBox.Text + " Info";
        }

        private void OnTitleChanged(object sender, EventArgs e)
        {
            UpdateTitle();
            UpdateValue<string>(PageInspectorValue.Title, Model.Title, titleTextBox.Text);
        }

        private void OnIndexChanged(object sender, EventArgs e)
        {
            UpdateValue<int>(PageInspectorValue.Index, Model.Index, indexStepper.Value);
        }

        private void OnNotesChanged(object sender, EventArgs e)
        {
            UpdateValue<string>(PageInspectorValue.Notes, Model.Notes, notesTextArea.Text);
        }
    }
}

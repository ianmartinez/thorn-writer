using System;
using Eto.Drawing;
using Eto.Forms;
using Thorn.Inspectors;
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
        private bool hasSubscribed;

        public PageInspector()
        {
            Title = "Page Info";
            Size = new Size(300, -1);

            ModelChanged += PageInspector_ModelChanged;

            titleTextBox = new TextBox();
            titleTextBox.TextChanged += OnTitleChanged;

            indexStepper = new NumericStepper
            {
                MinValue = 0,
                MaxValue = 1
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
                        new TableRow(new Label { Text = "Index:", VerticalAlignment = VerticalAlignment.Bottom }),
                        new TableRow(new TableCell(indexStepper, true)),
                        new TableRow(new Label { Text = "Notes:", VerticalAlignment = VerticalAlignment.Bottom }),
                        new TableRow(new TableCell(notesTextArea, true))
                    }
            };

            Content = new Panel()
            {
                Content = layout
            };
        }

        private void PageInspector_ModelChanged(object sender, EventArgs e)
        {
            // Remove event handler if it already exists
            if (hasSubscribed)
                Model.ParentNotebook.ValueChanged -= ParentNotebook_ValueChanged;

            Model.ParentNotebook.ValueChanged += ParentNotebook_ValueChanged;
            hasSubscribed = true;
        }

        private void ParentNotebook_ValueChanged(object sender, InspectorValueChangedEventArgs<NotebookInspectorValue> e)
        {
            if (!IsRefreshing)
            {
                switch (e.TargetValue)
                {
                    case NotebookInspectorValue.PageCount:
                        indexStepper.MaxValue = (int)e.NewValue - 1;
                        break;
                }
            }
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
            IsRefreshing = true;

            switch (targetValue)
            {
                case PageInspectorValue.Title:
                    titleTextBox.Text = Model?.Title ?? "";
                    UpdateTitle();
                    break;
                case PageInspectorValue.Index:
                    indexStepper.Value = Model?.Index ?? 0;
                    indexStepper.MaxValue = Model?.ParentNotebook.Pages.Count - 1 ?? 0;
                    break;
                case PageInspectorValue.Notes:
                    notesTextArea.Text = Model?.Notes ?? "";
                    break;
            }

            IsRefreshing = false;
        }

        private void UpdateTitle()
        {
            Title = titleTextBox.Text + " - Info";
        }

        private void OnTitleChanged(object sender, EventArgs e)
        {
            UpdateTitle();
            UpdateValue<string>(PageInspectorValue.Title, Model.Title, titleTextBox.Text);
        }

        private void OnIndexChanged(object sender, EventArgs e)
        {
            if(Model.Index != -1) // It can temporarily be -1 while it is being moved
                UpdateValue<int>(PageInspectorValue.Index, Model.Index, indexStepper.Value);
        }

        private void OnNotesChanged(object sender, EventArgs e)
        {
            UpdateValue<string>(PageInspectorValue.Notes, Model.Notes, notesTextArea.Text);
        }
    }
}

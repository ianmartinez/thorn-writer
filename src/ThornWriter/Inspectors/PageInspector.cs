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
        TableLayout pageLayout;
        TextBox titleTextBox;
        TextArea notesTextArea;
        NumericStepper indexStepper;
        Panel noPagePanel;
        Panel pagePanel;
        private bool hasSubscribed;
        

        public PageInspector()
        {
            UpdateTitle();
            Size = new Size(300, -1);
            MinimumSize = new Size(200, 100);

            noPagePanel = new Panel {
                Content = new Label {
                    Text = "No page selected",
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
                Padding = new Padding(10)
            };
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

            pageLayout = new TableLayout()
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

            pagePanel = new Panel { Content = pageLayout };
            Content = (Model == null) ? noPagePanel : pagePanel;
        }

        private void PageInspector_ModelChanged(object sender, EventArgs e)
        {
            UpdateTitle();

            if (Model != null)
            {
                Content = pagePanel;
                if (Height < 300)
                    Height = 300;

                // Remove event handler if it already exists
                if (hasSubscribed)
                    Model.ParentNotebook.ValueChanged -= ParentNotebook_ValueChanged;

                Model.ParentNotebook.ValueChanged += ParentNotebook_ValueChanged;
                hasSubscribed = true;
            }
            else
            {
                Content = noPagePanel;
            }
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
            if (Model != null)
            {
                IsRefreshing = true;

                switch (targetValue)
                {
                    case PageInspectorValue.Title:
                        titleTextBox.Text = Model.Title;
                        UpdateTitle();
                        break;
                    case PageInspectorValue.Index:
                        indexStepper.Value = Model.Index;
                        indexStepper.MaxValue = Model.ParentNotebook.Pages.Count - 1;
                        break;
                    case PageInspectorValue.Notes:
                        notesTextArea.Text = Model.Notes;
                        break;
                }

                IsRefreshing = false;
            }
        }

        private void UpdateTitle()
        {
            if(Model != null)
            {
                Title = titleTextBox.Text + " - Info";
            }
            else
            {
                Title = "Page Info";
            }
        }

        private void OnTitleChanged(object sender, EventArgs e)
        {
            if (Model != null)
            {
                UpdateTitle();
                UpdateValue<string>(PageInspectorValue.Title, Model.Title, titleTextBox.Text);
            }
        }

        private void OnIndexChanged(object sender, EventArgs e)
        {
            if (Model != null && Model.Index != -1) // It can temporarily be -1 while it is being moved
            { 
                UpdateValue<int>(PageInspectorValue.Index, Model.Index, indexStepper.Value);
            }
        }

        private void OnNotesChanged(object sender, EventArgs e)
        {
            if (Model != null)
            {
                UpdateValue<string>(PageInspectorValue.Notes, Model.Notes, notesTextArea.Text);
            }
        }
    }
}

using System;
using Eto.Drawing;
using Eto.Forms;
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
        TableLayout layout;
        TextBox titleTextBox;
        NumericStepper positionStepper;

        public PageInspector()
        {
            Title = "Page Info";
            Size = new Size(300, 600);

            titleTextBox = new TextBox();
            titleTextBox.TextChanged += OnTitleChanged;
            positionStepper = new NumericStepper();

            layout = new TableLayout()
            {
                Padding = new Padding(10), // padding around cells
                Spacing = new Size(5, 5), // spacing between each cell
                Rows =
                    {
                        new TableRow(
                            new Label { Text = "Title:", VerticalAlignment = VerticalAlignment.Center },
                            new TableCell(titleTextBox, true)
                        ),
                        new TableRow(
                            new Label { Text = "Position:", VerticalAlignment = VerticalAlignment.Center },
                            new TableCell(positionStepper, true)
                        ),
                        null
                    }
            };

            Content = new Panel()
            {
                Content = layout
            };
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
                    Title = Model.Title + " Info";
                    titleTextBox.Text = Model.Title;
                    break;
                case PageInspectorValue.Position:
                    break;
            }

            isRefreshing = false;
        }

        private void OnTitleChanged(object sender, EventArgs e)
        {
            UpdateValue<string>(PageInspectorValue.Title, Model.Title, titleTextBox.Text);
        }

        private void OnPositionChanged(object sender, EventArgs e)
        {

        }
    }
}

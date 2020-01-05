using System;
using Eto.Drawing;
using Eto.Forms;
using Thorn.Inspectors;

namespace ThornWriter.Inspectors
{
    /*
     * An inspector is a type of tool window/panel that contains
     * info for an item that can be viewed/modified
     */
    public abstract class InspectorForm<ModelType, ValueEnumType> : Form
    {
        private ModelType model;

        // Panel to show if there is no item selected to inspect
        public Panel NoModelPanel = new Panel
        {
            Content = new Label
            {
                Text = "Nothing selected",
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            },
            Padding = new Padding(10)
        };

        // Panel to show if there is an item to inspect
        public Panel ModelPanel = new Panel { };

        /*
         * If the value is being refreshed automatically, to avoid accidentally
         * triggering a ValueChanged event
         */
        public bool IsRefreshing { get; set; }

        // To be fired when the user changes something on the inspector and the value is different
        public event EventHandler<InspectorValueChangedEventArgs<ValueEnumType>> ValueChanged;

        // To be fired when the model is changed
        public event EventHandler<EventArgs> ModelChanged;

        // When the user has clicked the "delete" button on the inspector
        public event EventHandler<EventArgs> DeleteModel;

        // The object that is being inspected
        public ModelType Model
        {
            get => model;

            set
            {
                model = value;
                UpdatePanel();
                ModelChanged?.Invoke(this, new EventArgs());
                RefreshAll();
            }
        }

        // If there is currently a model
        public bool ModelExists => Model != null;

        // Choose which panel to show based on if there is a model or not
        private void UpdatePanel()
        {
            Content = ModelExists ? ModelPanel : NoModelPanel;
        }

        // Reload a specific value on the inspector
        public abstract void RefreshValue(ValueEnumType targetValue);

        // Reload the inspector completely
        public virtual void RefreshAll()
        {
            foreach (ValueEnumType value in Enum.GetValues(typeof(ValueEnumType)))
                RefreshValue(value);
        }

        // Fire InspectorValueChanged with EventArgs if the new value is different
        public virtual void UpdateValue<ValueType>(ValueEnumType targetValue, IComparable oldValue, IComparable newValue)
        {
            if (ValueChanged != null && oldValue != newValue && !IsRefreshing)
            {
                var args = new InspectorValueChangedEventArgs<ValueEnumType>()
                {
                    OldValue = oldValue,
                    NewValue = newValue,
                    TargetValue = targetValue
                };

                ValueChanged(this, args);
            }
        }

        // Trigger the delete event when the user has decided to delete the model
        public virtual void Delete()
        {
            DeleteModel?.Invoke(this, new EventArgs());
        }

        // Call instead of the standard .Show() method
        public void Show(Form parent)
        {
            if (!Visible)
            {
                UpdatePanel();
                Owner = null;
                RefreshAll();
                Show();
                Owner = parent;
            }
        }
    }
}

using System;
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
                ModelChanged?.Invoke(this, new EventArgs());
                RefreshAll();
            }
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
        public void ShowInspector(Form parent)
        {
            if (!Visible)
            {
                RefreshAll();
                Show();
                Owner = parent;
            }
        }
    }
}

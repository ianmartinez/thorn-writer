using System;
using Eto.Forms;

namespace ThornWriter.Inspectors
{
    public class InspectorValueChangedEventArgs<ValueEnumType> : EventArgs
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public ValueEnumType TargetValue { get; set; }
    }

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
        internal bool isRefreshing;

        // To be fired when the user changes something on the inspector and the value is different
        public event EventHandler<InspectorValueChangedEventArgs<ValueEnumType>> ValueChanged;

        // The object that is being inspected
        public ModelType Model
        {
            get => model;

            set
            {
                model = value;
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
            if (oldValue != newValue && !isRefreshing)
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

        // Call instead of the standard .Show() method
        public void ShowInspector(Form parent)
        {
            Owner = parent;

            if (!Visible)
                Show();
        }
    }
}

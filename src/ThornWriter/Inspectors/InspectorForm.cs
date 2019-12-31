using System;
using Eto.Forms;

namespace ThornWriter.Inspectors
{
    /*
     * An inspector is a type of tool window/panel that contains
     * info for an item that can be viewed/modified
     */
    public abstract class InspectorForm<ModelType, ValueEnumType> : Form
    {
        private ModelType model;

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
        public abstract void RefreshValue(ValueEnumType name);

        // Reload the inspector completely
        public virtual void RefreshAll()
        {
            foreach (ValueEnumType value in Enum.GetValues(typeof(ValueEnumType)))
                RefreshValue(value);
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

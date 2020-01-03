using System;

namespace Thorn.Inspectors
{
    public interface IInspectable<ValueEnumType>
    {
        // To be fired when something has changed on the object being inspected
        public event EventHandler<InspectorValueChangedEventArgs<ValueEnumType>> ValueChanged;
    }
}

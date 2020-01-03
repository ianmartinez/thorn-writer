using System;

namespace Thorn.Inspectors
{
    public class InspectorValueChangedEventArgs<ValueEnumType> : EventArgs
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public ValueEnumType TargetValue { get; set; }
    }
}

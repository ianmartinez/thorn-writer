namespace ThornWriter.Inspectors
{
    /*
     * An inspector is a type of tool window/panel that contains
     * info for an item that can be viewed/modified
     */
    public interface IInspector<T>
    {
        // The object that is being inspected
        public T Model { get; set; }

        // Reload the inspector completely
        public void RefreshAll();

        // Reload a specific value on the inspector
        public void RefreshValue(string name);
    }
}

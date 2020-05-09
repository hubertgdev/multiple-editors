namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Represents the options and default values of a CustomEditorExtension.
    /// </summary>
    [System.Serializable]
    public struct CustomEditorExtensionOptions
    {
        public static readonly CustomEditorExtensionOptions Default = new CustomEditorExtensionOptions();

        /// <summary>
        /// Creates a CustomEditorExtensionOptions instance.
        /// </summary>
        /// <param name="_DefaultOrder">The defaut position of the CustomEditorExtension in the open Editor where it's used. The higher the
        /// value, the higher the custom editor will appear in the inspector.</param>
        /// <param name="_DisplayName">The name that will be displayed in the Multiple Editors Manager window, in the editor. If null or
        /// empty, the window uses the class name of the CustomEditorExtension instead.</param>
        /// <param name="_Description">A short description about what the custom editor does.</param>
        /// <param name="_RequiresConstantRepaint">If true, the Editor instance that uses this CustomEditorExtension will set
        /// Editor.RequiresConstantRepaint() to true, so the inspector view will be drawn even if it's not focused.</param>
        public CustomEditorExtensionOptions(int _DefaultOrder = 0, string _DisplayName = null, string _Description = null, bool _RequiresConstantRepaint = false)
        {
            displayName = _DisplayName;
            description = _Description;
            defaultOrder = _DefaultOrder;
            requiresConstantRepaint = _RequiresConstantRepaint;
        }

        // The name that will be displayed in the Multiple Editors Manager window, in the editor. If null or empty, the window uses the
        // class name of the CustomEditorExtension instead.
        public string displayName;
        // A short description about what the custom editor does.
        public string description;
        // The defaut position of the CustomEditorExtension in the open Editor where it's used. The higher the value, the higher the custom
        // editor will appear in the inspector.
        public int defaultOrder;
        // If true, the Editor instance that uses this CustomEditorExtension will set Editor.RequiresConstantRepaint() to true, so the
        // inspector view will be drawn even if it's not focused.
        public bool requiresConstantRepaint;
    }

}
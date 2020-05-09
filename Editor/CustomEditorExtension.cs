using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Represents an editor extension for an actual Unity Editor instance.
    /// </summary>
    /// <typeparam name="TTarget">The type of the object that the editor decorates.</typeparam>
    public class CustomEditorExtension<TTarget> : ICustomEditorExtension
        where TTarget : Object
    {

        // Contains the object being inspected.
        public TTarget          Target              { get; private set; } = null;
        // Contains an array of all the objects being inspected.
        public TTarget[]        Targets             { get; private set; } = null;

        // Contains the reference to the open Editor that uses this extension.
        private Editor m_EditorReference = null;

        /// <summary>
        /// Initializes the targets and the reference to the open Editor instance that uses this extension.
        /// This method is called by MultipleEditorsManager, and shouldn't be called elsewhere.
        /// </summary>
        /// <param name="_Editor">The open Editor that uses this extension.</param>
        public void Init(Editor _Editor)
        {
            Target = _Editor.target as TTarget;
            Targets = new TTarget[_Editor.targets.Length];
            for(int i = 0; i < _Editor.targets.Length; i++)
            {
                Targets[i] = _Editor.targets[i] as TTarget;
            }

            m_EditorReference = _Editor;
        }

        /// <summary>
        /// Registers this custom editor with the manager to make it available for multiple editors system.
        /// </summary>
        /// <param name="_CreateEditorMethod">The method to use for creating an instance of this custom editor extension. For example if
        /// your custom editor class name is "TestEditorExtension", you can use:
        /// RegisterCustomEditor(() => { return new TestEditorExtension(); });</param>
        protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod)
        {
            MultipleEditorsManager.RegisterCustomEditor<TTarget>(_CreateEditorMethod);
        }

        /// <summary>
        /// Registers this custom editor with the manager to make it available for multiple editors system.
        /// </summary>
        /// <param name="_CreateEditorMethod">The method to use for creating an instance of this custom editor extension. For example if
        /// your custom editor class name is "TestEditorExtension", you can use:
        /// RegisterCustomEditor(() => { return new TestEditorExtension(); });</param>
        /// <param name="_Options">The options and default values to use for the CustomEditorExtension instance.</param>
        protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, CustomEditorExtensionOptions _Options)
        {
            MultipleEditorsManager.RegisterCustomEditor<TTarget>(_CreateEditorMethod, _Options);
        }

        /// <summary>
        /// Registers this custom editor with the manager to make it available for multiple editors system.
        /// </summary>
        /// <param name="_CreateEditorMethod">The method to use for creating an instance of this custom editor extension. For example if
        /// your custom editor class name is "TestEditorExtension", you can use:
        /// RegisterCustomEditor(() => { return new TestEditorExtension(); });</param>
        /// <param name="_Order">The position of the editor in the inspector. The more the order, the highest the editor is drawn.</param>
        /// <param name="_RequiresConstantRepaint">If true, the extended Editor will set Editor.RequiresConstantRepaint() to true.</param>
        protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, int _Order, bool _RequiresConstantRepaint = false)
        {
            MultipleEditorsManager.RegisterCustomEditor<TTarget>(_CreateEditorMethod, new CustomEditorExtensionOptions(_Order, null, null, _RequiresConstantRepaint));
        }

        /// <summary>
        /// Registers this custom editor with the manager to make it available for multiple editors system.
        /// </summary>
        /// <param name="_CreateEditorMethod">The method to use for creating an instance of this custom editor extension. For example if
        /// your custom editor class name is "TestEditorExtension", you can use:
        /// RegisterCustomEditor(() => { return new TestEditorExtension(); });</param>
        /// <param name="_Order">The position of the editor in the inspector. The more the order, the highest the editor is drawn.</param>
        /// <param name="_DisplayName">The name of the custom editor, displayed in the Multiple Editors Manager window.</param>
        /// <param name="_RequiresConstantRepaint">If true, the extended Editor will set Editor.RequiresConstantRepaint() to true.</param>
        protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, int _Order, string _DisplayName, bool _RequiresConstantRepaint = false)
        {
            MultipleEditorsManager.RegisterCustomEditor<TTarget>(_CreateEditorMethod, new CustomEditorExtensionOptions(_Order, _DisplayName, null, _RequiresConstantRepaint));
        }

        /// <summary>
        /// Registers this custom editor with the manager to make it available for multiple editors system.
        /// </summary>
        /// <param name="_CreateEditorMethod">The method to use for creating an instance of this custom editor extension. For example if
        /// your custom editor class name is "TestEditorExtension", you can use:
        /// RegisterCustomEditor(() => { return new TestEditorExtension(); });</param>
        /// <param name="_Order">The position of the editor in the inspector. The more the order, the highest the editor is drawn.</param>
        /// <param name="_DisplayName">The name of the custom editor, displayed in the Multiple Editors Manager window.</param>
        /// <param name="_Description">A short description about what your editor does, displayed in the Multiple Editors Manager window.</param>
        /// <param name="_RequiresConstantRepaint">If true, the extended Editor will set Editor.RequiresConstantRepaint() to true.</param>
        protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, int _Order, string _DisplayName, string _Description, bool _RequiresConstantRepaint = false)
        {
            MultipleEditorsManager.RegisterCustomEditor<TTarget>(_CreateEditorMethod, new CustomEditorExtensionOptions(_Order, _DisplayName, _Description, _RequiresConstantRepaint));
        }

        /// <summary>
        /// Registers this custom editor with the manager to make it available for multiple editors system.
        /// </summary>
        /// <param name="_CreateEditorMethod">The method to use for creating an instance of this custom editor extension. For example if
        /// your custom editor class name is "TestEditorExtension", you can use:
        /// RegisterCustomEditor(() => { return new TestEditorExtension(); });</param>
        /// <param name="_DisplayName">The name of the custom editor, displayed in the Multiple Editors Manager window.</param>
        /// <param name="_RequiresConstantRepaint">If true, the extended Editor will set Editor.RequiresConstantRepaint() to true.</param>
        protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, string _DisplayName, bool _RequiresConstantRepaint = false)
        {
            MultipleEditorsManager.RegisterCustomEditor<TTarget>(_CreateEditorMethod, new CustomEditorExtensionOptions(0, _DisplayName, null, _RequiresConstantRepaint));
        }

        /// <summary>
        /// Registers this custom editor with the manager to make it available for multiple editors system.
        /// </summary>
        /// <param name="_CreateEditorMethod">The method to use for creating an instance of this custom editor extension. For example if
        /// your custom editor class name is "TestEditorExtension", you can use:
        /// RegisterCustomEditor(() => { return new TestEditorExtension(); });</param>
        /// <param name="_DisplayName">The name of the custom editor, displayed in the Multiple Editors Manager window.</param>
        /// <param name="_Description">A short description about what your editor does, displayed in the Multiple Editors Manager window.</param>
        /// <param name="_RequiresConstantRepaint">If true, the extended Editor will set Editor.RequiresConstantRepaint() to true.</param>
        protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, string _DisplayName, string _Description, bool _RequiresConstantRepaint = false)
        {
            MultipleEditorsManager.RegisterCustomEditor<TTarget>(_CreateEditorMethod, new CustomEditorExtensionOptions(0, _DisplayName, _Description, _RequiresConstantRepaint));
        }

        /// <summary>
        /// Called when the extended Editor instance is enabled.
        /// </summary>
        public virtual void OnEnable() { }

        /// <summary>
        /// Called when the extended Editor instance is disabled.
        /// </summary>
        public virtual void OnDisable() { }

        /// <summary>
        /// Called before the default extended Editor's header is displayed.
        /// </summary>
        public virtual void OnBeforeHeaderGUI() { }

        /// <summary>
        /// Called after the default extended Editor's header is displayed.
        /// </summary>
        public virtual void OnHeaderGUI() { }

        /// <summary>
        /// Called before the default extended Editor's inspector is displayed.
        /// </summary>
        public virtual void OnBeforeInspectorGUI() { }

        /// <summary>
        /// Called after the default extended Editor's inspector is displayed.
        /// </summary>
        public virtual void OnInspectorGUI() { }

        /// <summary>
        /// Handles Scene view events.
        /// Note that this method is called only if the target type is a scene object (instances of MonoBehaviour for example).
        /// </summary>
        public virtual void OnSceneGUI() { }

        /// <summary>
        /// Alias of the extended Editor's Repaint() method.
        /// Repaint the inspector that shows this editor.
        /// </summary>
        protected void Repaint()
        {
            m_EditorReference.Repaint();
        }

        /// <summary>
        /// Gets the object being inspected.
        /// </summary>
        public TTarget target { get { return Target; } }

        /// <summary>
        /// Gets an array of all the objects being inspected.
        /// </summary>
        public TTarget[] targets { get { return Targets; } }

        /// <summary>
        /// Aliases of the extended Editor's serializeObject accessor.
        /// A SerializedObject representing the object or objects being inspected.
        /// </summary>
        public SerializedObject SerializedObject { get { return m_EditorReference.serializedObject; } }
        public SerializedObject serializedObject { get { return m_EditorReference.serializedObject; } }

    }

}
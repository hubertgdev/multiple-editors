using UnityEngine;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Base class for creating a custom editor that can display multiple different editors.
    /// </summary>
    /// <typeparam name="TTarget">The type of the inspected object.</typeparam>
    public abstract class MultipleEditorsHandler<TTarget> : TEditor<TTarget>
        where TTarget : Object
    {

        #region Properties

        // Contains the instances of the associated CustomEditorExtensions that uses the same target type as this editor.
        public ICustomEditorExtension[] CustomEditorExtensions { get; private set; } = { };
        
        // Defines the value to return in Editor.RequiresConstantRepaint() method.
        private bool m_RequiresConstantRepaint = false;

        #endregion


        #region Lifecycle & GUI

        /// <summary>
        /// This function is called when the object is loaded.
        /// Loads the custom editors extensions, and enable them.
        /// </summary>
        protected virtual void OnEnable()
        {
            LoadExtensions();
            EnableCustomEditors();
        }

        /// <summary>
        /// This function is called when the scriptable object goes out of scope.
        /// Disables the loaded custom editor extensions.
        /// </summary>
        protected virtual void OnDisable()
        {
            DisableCustomEditors();
        }

        /// <summary>
        /// Called when the header of the object being inspected is drawn.
        /// By default, calls OnBeforeHeaderGUI() on loaded custom editor extensions, draws the original header, then call OnHeaderGUI() on
        /// loaded custom editor extensions.
        /// </summary>
        protected override void OnHeaderGUI()
        {
            DrawCustomEditorsBeforeHeaderGUI();
            base.OnHeaderGUI();
            DrawCustomEditorsHeaderGUI();
        }

        /// <summary>
        /// Called when the inspector of the object being inspected is drawn.
        /// By default, calls OnBeforeInspectorGUI() on loaded custom editor extensions, draws the original inspector, then call
        /// OnInspectorGUI() on loaded custom editor extensions.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawCustomEditorsBeforeInspectorGUI();
            base.OnInspectorGUI();
            DrawCustomEditorsInspectorGUI();
        }

        /// <summary>
        /// Handles scene events. Note that this message is sent by Unity only when inspecting scene objects.
        /// </summary>
        protected void OnSceneGUI()
        {
            DrawCustomEditorsSceneGUI();
        }

        /// <summary>
        /// Defines if this Editor should be repainted constantly, (similar to an Update() for an Editor class).
        /// </summary>
        public override bool RequiresConstantRepaint()
        {
            return m_RequiresConstantRepaint;
        }

        #endregion


        #region Utility Methods

        /// <summary>
        /// Creates all the CustomEditorExtension that have the same target type as this Editor.
        /// </summary>
        protected void LoadExtensions()
        {
            CustomEditorExtensions = MultipleEditorsManager.CreateEditors<TTarget>(this, out bool requiresConstantRepaint);
            m_RequiresConstantRepaint = requiresConstantRepaint;
        }

        /// <summary>
        /// Calls OnEnable() on each loaded custom editor extensions.
        /// </summary>
        protected void EnableCustomEditors()
        {
            foreach (CustomEditorExtension<TTarget> customObjectEditor in CustomEditorExtensions)
            {
                customObjectEditor.OnEnable();
            }
        }

        /// <summary>
        /// Calls OnDisable() on each loaded custom editor extensions.
        /// </summary>
        protected void DisableCustomEditors()
        {
            foreach (CustomEditorExtension<TTarget> customObjectEditor in CustomEditorExtensions)
            {
                customObjectEditor.OnDisable();
            }
        }

        /// <summary>
        /// Calls OnBeforeHeaderGUI() on each loaded custom editor extensions.
        /// </summary>
        protected void DrawCustomEditorsBeforeHeaderGUI()
        {
            foreach (CustomEditorExtension<TTarget> customObjectEditor in CustomEditorExtensions)
            {
                customObjectEditor.OnBeforeHeaderGUI();
            }
        }

        /// <summary>
        /// Calls OnHeaderGUI() on each loaded custom editor extensions.
        /// </summary>
        protected void DrawCustomEditorsHeaderGUI()
        {
            foreach (CustomEditorExtension<TTarget> customObjectEditor in CustomEditorExtensions)
            {
                customObjectEditor.OnHeaderGUI();
            }
        }

        /// <summary>
        /// Calls OnBeforeInspectorGUI() on each loaded custom editor extensions.
        /// </summary>
        protected void DrawCustomEditorsBeforeInspectorGUI()
        {
            foreach (CustomEditorExtension<TTarget> customObjectEditor in CustomEditorExtensions)
            {
                customObjectEditor.OnBeforeInspectorGUI();
            }
        }

        /// <summary>
        /// Calls OnInspectorGUI() on each loaded custom editor extensions.
        /// </summary>
        protected void DrawCustomEditorsInspectorGUI()
        {
            foreach (CustomEditorExtension<TTarget> customObjectEditor in CustomEditorExtensions)
            {
                customObjectEditor.OnInspectorGUI();
            }
        }

        /// <summary>
        /// Calls OnSceneGUI() on each loaded custom editor extensions.
        /// </summary>
        protected void DrawCustomEditorsSceneGUI()
        {
            foreach (CustomEditorExtension<TTarget> customObjectEditor in CustomEditorExtensions)
            {
                customObjectEditor.OnSceneGUI();
            }
        }

        #endregion

    }

}
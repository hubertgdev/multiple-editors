using System;
using System.Reflection;

using UnityEditor;

using Object = UnityEngine.Object;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Inherit from this class to make a multiple editors manager for a native Object type (like Transform, GamObject, etc).
    /// </summary>
    /// <typeparam name="TTarget">The type of the inspected object.</typeparam>
    public abstract class NativeObjectMultipleEditorsHandler<TTarget> : MultipleEditorsHandler<TTarget>
        where TTarget : Object
    {

        // The instance of the native editor of the inspected object.
        private Editor m_NativeEditor = null;

        /// <summary>
        /// Called when this editor is open.
        /// </summary>
        protected override void OnEnable()
        {
            CreateNativeEditor();

            base.OnEnable();
        }

        /// <summary>
        /// Called when this editor is closed.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            if (NativeEditor != null)
            {
                DestroyNativeEditor(NativeEditor);
            }
        }

        /// <summary>
        /// Called when the header of the Editor is drawn.
        /// This implementation calls OnBeforeHeaderGUI() on all the editor extensions, draws the original header, and ends by calling
        /// OnHeaderGUI on all the editor extensions.
        /// </summary>
        protected override void OnHeaderGUI()
        {
            DrawCustomEditorsBeforeHeaderGUI();
            NativeEditor.DrawHeader();
            DrawCustomEditorsHeaderGUI();
        }

        /// <summary>
        /// Called when the inspector of the Editor is drawn.
        /// This implementation calls OnBeforeInspectorGUI() on all the editor extensions, draws the original inspector, and ends by
        /// calling OnInspectorGUI on all the editor extensions.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawCustomEditorsBeforeInspectorGUI();
            if(NativeEditor != null)
            {
                NativeEditor.OnInspectorGUI();
            }
            DrawCustomEditorsInspectorGUI();
        }

        /// <summary>
        /// Called when this editor is enabled.
        /// Creates an instance of the native editor of the inspected Object.
        /// </summary>
        /// <param name="_CreateEditorType">The type name of the native editor to get. If this value is set to null, use
        /// "UnityEditor.[TTarget]Inspector, UnityEditor".</param>
        protected virtual void CreateNativeEditor(string _CreateEditorType = null)
        {
            string createEditorType = !string.IsNullOrEmpty(_CreateEditorType) ? _CreateEditorType : $"UnityEditor.{typeof(TTarget).Name}Inspector, UnityEditor";
            NativeEditor = CreateEditor(target, Type.GetType(createEditorType));
        }

        /// <summary>
        /// Called when this editor is disabled.
        /// Destroys the created native editor properly in order to avoid memory leaks.
        /// By default, executes the OnDisable() method on the native editor, and destroys it using Object.DestroyImmediate().
        /// </summary>
        /// <param name="_NativeEditor">The created native Editor instance.</param>
        protected virtual void DestroyNativeEditor(Editor _NativeEditor)
        {
            MethodInfo disableMethod = NativeEditor.GetType()
                .GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (disableMethod != null)
            {
                disableMethod.Invoke(NativeEditor, null);
            }
            DestroyImmediate(NativeEditor);
        }

        /// <summary>
        /// Gets the native editor, normally created using CreateNativeEditor() method in OnEnable() message.
        /// Destroys the current cached native editor if there's one using DestroyNativeEditor() method, then assign the new native editor.
        /// The setter shouldn't be used out of the CreateNativeEditor() method.
        /// </summary>
        protected Editor NativeEditor
        {
            get { return m_NativeEditor; }
            set
            {
                if(m_NativeEditor != null)
                {
                    DestroyNativeEditor(m_NativeEditor);
                }
                m_NativeEditor = value;
            }
        }

    }

}
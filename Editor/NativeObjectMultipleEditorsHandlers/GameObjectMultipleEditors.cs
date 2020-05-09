using System.Reflection;

using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for GameObjects.
    /// </summary>
    [CustomEditor(typeof(GameObject))]
    [CanEditMultipleObjects]
    public class GameObjectMultipleEditors : NativeObjectMultipleEditorsHandler<GameObject>
    {

        /// <summary>
        /// Called when this editor is disabled.
        /// Destroys the created native editor properly in order to avoid memory leaks.
        /// Enables the GameObject (ensuring its preview cache has been initialized, and so avoiding errors), then destroys it properly
        /// using Editor.DestroyImmediate().
        /// </summary>
        /// <param name="_NativeEditor">The native editor instance to destroy.</param>
        protected override void DestroyNativeEditor(Editor _NativeEditor)
        {
            // Check if the preview cache is set or not
            object previewCache = _NativeEditor.GetType()
                .GetField("m_PreviewCache", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(_NativeEditor);

            // If the preview cache is not defined, call OnEnable() method to initialize the GameObject editor
            if (previewCache == null)
            {
                MethodInfo enableMethod = _NativeEditor.GetType()
                    .GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (enableMethod != null)
                {
                    enableMethod.Invoke(_NativeEditor, null);
                }
            }

            DestroyImmediate(_NativeEditor);
        }

    }

}
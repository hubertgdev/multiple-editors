using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for MonoScript component.
    /// </summary>
    [CustomEditor(typeof(MonoScript))]
    [CanEditMultipleObjects]
    public class MonoScriptMultipleEditors : NativeObjectMultipleEditorsHandler<MonoScript>
    {

        protected override void OnHeaderGUI()
        {
            GUILayout.Space(1f);
            DrawCustomEditorsBeforeHeaderGUI();
            DrawCustomEditorsHeaderGUI();
        }

    }

}
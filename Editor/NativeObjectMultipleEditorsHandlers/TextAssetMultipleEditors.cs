using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for TextAsset component.
    /// </summary>
    [CustomEditor(typeof(TextAsset))]
    [CanEditMultipleObjects]
    public class TextAssetMultipleEditors : MultipleEditorsHandler<TextAsset> { }

}
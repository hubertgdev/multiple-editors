using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils
{

    ///<summary>
    ///	Bundle of utility methods for Editor operations.
    ///</summary>
    public static class EditorHelpers
    {

        public const string DEFAULT_ASSET_EXTENSION = "asset";

        public static readonly float HORIZONTAL_MARGIN = EditorGUIUtility.standardVerticalSpacing;
        public static readonly float VERTICAL_MARGIN = EditorGUIUtility.standardVerticalSpacing;
        public static readonly float LINE_HEIGHT = EditorGUIUtility.singleLineHeight;

        #region GUI

        /// <summary>
        /// Draws an horizontal line.
        /// </summary>
        /// <param name="_Rect">The position and size of the line.</param>
        public static void HorizontalLine(Rect _Rect)
        {
            HorizontalLine(_Rect, Color.grey);
        }

        /// <summary>
        /// Draws an horizontal line.
        /// </summary>
        /// <param name="_Rect">The position and size of the line.</param>
        /// <param name="_Color">The color of the line.</param>
        public static void HorizontalLine(Rect _Rect, Color _Color)
        {
            EditorGUI.DrawRect(_Rect, _Color);
        }

        /// <summary>
        /// Draws an horizontal line using layout GUI.
        /// </summary>
        public static void HorizontalLineLayout()
        {
            HorizontalLineLayout(1f, Color.grey);
        }

        /// <summary>
        /// Draws an horizontal line using layout GUI.
        /// </summary>
        /// <param name="_Height">The height of the line.</param>
        public static void HorizontalLineLayout(float _Height)
        {
            HorizontalLineLayout(_Height, Color.grey);
        }

        /// <summary>
        /// Draws an horizontal line using layout GUI.
        /// </summary>
        /// <param name="_Height">The height of the line.</param>
        /// <param name="_Color">The color of the line.</param>
        public static void HorizontalLineLayout(float _Height, Color _Color)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, _Height);
            rect.height = _Height;
            EditorGUI.DrawRect(rect, _Color);
        }

        #endregion


        #region Editor Styles

        public static GUIStyle HelpBoxStyle
        {
            get
            {
                GUIStyle style = new GUIStyle("HelpBox");
                style.richText = true;
                style.fontSize = 11;
                return style;
            }
        }

        #endregion

    }

}
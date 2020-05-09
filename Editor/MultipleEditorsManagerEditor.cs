using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;

namespace MuffinDev.EditorUtils.MultipleEditors
{

	///<summary>
	/// Custom editor for MultipleEditorsManager asset.
    /// Allows you to manage all the registered custom editor extensions from the editor.
	///</summary>
    [CustomEditor(typeof(MultipleEditorsManager))]
	public class MultipleEditorsManagerEditor : TEditor<MultipleEditorsManager>
	{

        #region Enums & Subclasses

        /// <summary>
        /// Represents a list of settings that targets the same type.
        /// </summary>
        [System.Serializable]
        private class CustomEditorsList
        {

            // UI sizing

            private const float DESCRIPTION_HEIGHT = 28f;
            private const float INDENTATION = 16f;
            private const float LABEL_WIDTH = 180f;
            private const float ORDER_FIELD_WIDTH = 60f;

            // A list of settings that targets the same type
            [SerializeField]
            public List<CustomEditorExtensionSettings> m_Settings { get; private set; } = new List<CustomEditorExtensionSettings>();

            // Defines if this list displays the additial settings fields or not
            [SerializeField]
            private bool m_Extended = false;

            // Contains the reorderable list for the stored settings
            private ReorderableList m_ReorderableList = null;
            // The left offset to apply to foldout fields, set when using DrawList()
            private float m_LeftOffset = 0f;

            // Styles

            private GUIStyle m_DescriptionFieldStyle = null;
            private GUIStyle m_FoldoutStyle = null;

            /// <summary>
            /// Initializes the reorderable list.
            /// </summary>
            public void Init()
            {
                ResetReorderableList();
            }

            /// <summary>
            /// Draws the reorderable list.
            /// </summary>
            /// <param name="_LeftOffset">Defines the offset to apply to all foldout fields.</param>
            public void DrawList(float _LeftOffset)
            {
                m_LeftOffset = _LeftOffset;
                m_ReorderableList.DoLayoutList();
            }

            /// <summary>
            /// Called when the reorderable list header is drawn.
            /// </summary>
            private void OnDrawHeader(Rect _Rect)
            {
                if(m_Settings.Count > 0)
                {
                    Vector2 position = _Rect.position;
                    position.x += m_LeftOffset - (m_LeftOffset != 0f ? EditorHelpers.HORIZONTAL_MARGIN * 2 : 0f);
                    Vector2 size = new Vector2(_Rect.size.x - m_LeftOffset - ORDER_FIELD_WIDTH, _Rect.size.y);

                    bool extendedBefore = m_Extended;
                    m_Extended = EditorGUI.Foldout(new Rect(position, size), m_Extended, "Editor extensions for " + m_Settings[0].TargetType.Name, true, FoldoutStyle);

                    if(m_Extended != extendedBefore)
                    {
                        ResetReorderableList();
                    }

                    position.x += size.x + EditorHelpers.HORIZONTAL_MARGIN;
                    size.x = ORDER_FIELD_WIDTH;
                    EditorGUI.LabelField(new Rect(position, size), "Orders");
                }
            }
            
            /// <summary>
            /// Resets the reorderable list.
            /// </summary>
            private void ResetReorderableList()
            {
                m_ReorderableList = new ReorderableList(new List<CustomEditorExtensionSettings>(m_Settings), typeof(CustomEditorExtensionSettings), true, false, false, false);
                m_ReorderableList.drawHeaderCallback += OnDrawHeader;
                m_ReorderableList.drawElementCallback += OnDrawElement;
                m_ReorderableList.onReorderCallbackWithDetails += OnReorderElements;
                m_ReorderableList.elementHeight = (m_Extended)
                    ? EditorHelpers.LINE_HEIGHT * 2 + DESCRIPTION_HEIGHT + EditorHelpers.VERTICAL_MARGIN * 5
                    : EditorHelpers.LINE_HEIGHT + EditorHelpers.VERTICAL_MARGIN * 2;
            }

            /// <summary>
            /// Called when an element of the reorderable list is drawn.
            /// </summary>
            private void OnDrawElement(Rect _Rect, int _Index, bool _IsActive, bool _IsFocused)
            {
                EditorGUIUtility.labelWidth = LABEL_WIDTH;

                // Alias for settings fields being drawn
                CustomEditorExtensionSettings settings = m_Settings[_Index];

                // Draw the enable field, custom editor display name, and order field
                Vector2 position = _Rect.position;
                position.y += EditorHelpers.VERTICAL_MARGIN;
                Vector2 size = new Vector2(_Rect.width, EditorGUIUtility.singleLineHeight);
                DrawLabelAndOrderField(settings, position, size);

                // If the list is not extended, stop here
                if (!m_Extended)
                    return;

                // Draw the description of the custom editor
                position.y += size.y + EditorHelpers.VERTICAL_MARGIN;
                position.x += INDENTATION;
                size.x -= INDENTATION;
                GUI.Box(new Rect(position, new Vector2(size.x, DESCRIPTION_HEIGHT)), settings.Description, DescriptionFieldStyle);

                // Draws the "require constant repaint" option field
                position.y += DESCRIPTION_HEIGHT + EditorHelpers.VERTICAL_MARGIN;
                settings.RequiresConstantRepaint = EditorGUI.Toggle(new Rect(position, size), "Requires Constant Repaint", settings.RequiresConstantRepaint);

                // Draws an horizontal line if the current settings are not the last entry of the list
                if(_Index < m_Settings.Count - 1)
                {
                    position.x -= INDENTATION + m_LeftOffset;
                    size.x += INDENTATION + m_LeftOffset;
                    position.y += size.y + EditorHelpers.VERTICAL_MARGIN;
                    EditorHelpers.HorizontalLine(new Rect(position, new Vector2(size.x, 1f)));
                }
            }

            /// <summary>
            /// Draws the enable field, custom editor display name and order field
            /// </summary>
            private void DrawLabelAndOrderField(CustomEditorExtensionSettings _Settings, Vector2 _Position, Vector2 _Size)
            {
                // Draw the enable field
                Vector2 position = _Position;
                Vector2 size = new Vector2(_Size.x - ORDER_FIELD_WIDTH - EditorHelpers.HORIZONTAL_MARGIN * 2, _Size.y);
                _Settings.Enabled = EditorGUI.ToggleLeft(new Rect(position, size), _Settings.DisplayName, _Settings.Enabled, m_Extended ? EditorStyles.boldLabel : EditorStyles.label);

                // Draw the order field
                position.x += size.x + EditorHelpers.HORIZONTAL_MARGIN;
                size.x = ORDER_FIELD_WIDTH;
                int orderBefore = _Settings.Order;
                _Settings.Order = EditorGUI.IntField(new Rect(position, size), _Settings.Order);
            }

            /// <summary>
            /// Called when a list item has been reordered.
            /// </summary>
            private void OnReorderElements(ReorderableList _List, int _OldIndex, int _NewIndex)
            {
                // If the element has been moved up
                if (_NewIndex < _OldIndex)
                {
                    m_Settings[_OldIndex].Order = m_Settings[_NewIndex].Order + 1;
                    // Check all previous items, while previous items have the same order number than the moved one
                    int lastIndex = _OldIndex;
                    for (int i = _NewIndex - 1; i >= 0; i--)
                    {
                        if (m_Settings[lastIndex].Order == m_Settings[i].Order)
                        {
                            m_Settings[i].Order++;
                            lastIndex = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                // Else, if the element has been moved down
                else
                {
                    m_Settings[_OldIndex].Order = m_Settings[_NewIndex].Order - 1;
                    // Check all previous items, while previous items have the same order number than the moved one
                    int lastIndex = _OldIndex;
                    for (int i = _NewIndex + 1; i < m_Settings.Count; i++)
                    {
                        if (m_Settings[lastIndex].Order == m_Settings[i].Order)
                        {
                            m_Settings[i].Order--;
                            lastIndex = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                ResetReorderableList();
                m_Settings.Sort();
            }

            /// <summary>
            /// Gets the settings list.
            /// </summary>
            public List<CustomEditorExtensionSettings> Settings
            {
                get { return m_Settings; }
            }

            /// <summary>
            /// Gets the cached description field style, or create it if the cache is empty.
            /// </summary>
            private GUIStyle DescriptionFieldStyle
            {
                get
                {
                    if (m_DescriptionFieldStyle == null)
                    {
                        m_DescriptionFieldStyle = new GUIStyle(GUI.skin.label);
                        m_DescriptionFieldStyle.clipping = TextClipping.Clip;
                        m_DescriptionFieldStyle.wordWrap = true;
                        m_DescriptionFieldStyle.fontSize = 10;

                        Color textColor = m_DescriptionFieldStyle.normal.textColor;
                        textColor.a = .5f;
                        m_DescriptionFieldStyle.normal.textColor = textColor;
                    }
                    return m_DescriptionFieldStyle;
                }
            }

            /// <summary>
            /// Gets the cached foldout field style, or create it if the cache is empty.
            /// </summary>
            private GUIStyle FoldoutStyle
            {
                get
                {
                    if(m_FoldoutStyle == null)
                    {
                        m_FoldoutStyle = new GUIStyle("Foldout");
                        m_FoldoutStyle.fontStyle = FontStyle.Bold;
                    }
                    return m_FoldoutStyle;
                }
            }

        }

        /// <summary>
        /// Represents a list of settings that target the same type.
        /// </summary>
        [System.Serializable]
        private class CustomEditorListGroup
        {

            public string TargetTypeName { get; private set; } = null;
            public CustomEditorsList List { get; private set; } = null;

            public CustomEditorListGroup(string _TargetTypeName)
            {
                TargetTypeName = _TargetTypeName;
                List = new CustomEditorsList();
            }

        }

        #endregion


        #region Properties

        // UI sizing & margins

        private const int SETTINGS_BLOCK_MARGIN = 8;
        private const int SETTINGS_BLOCK_SMALL_MARGIN = 4;
        private const int BUTTON_HEIGHT = 26;
        private const float LEFT_OFFSET = 16f;

        // Stores the scroll position of the settings view.
        [SerializeField]
        private Vector2 m_ScrollPosition = Vector2.zero;

        // Contains the lists of all registered custom editor extensions settings, grouped by target type.
        [SerializeField]
        private List<CustomEditorListGroup> m_CustomEditors = new List<CustomEditorListGroup>();

        // Animates the Help block opening/closing.
        [SerializeField]
        private AnimBool m_ExpandHelpAnimBool = null;

        // Called when the help block animation is playing.
        [SerializeField]
        private UnityEvent m_RequiresRepaintEvent = new UnityEvent();

        // Defines if this editor should apply left offset on foldout fields.
        // It fixes the foldout fields position in the inspector when enabled, but when this editor is drawn in a window, the offset is not
        // necessary anymore.
        [SerializeField]
        private bool m_UseLeftOffset = true;

        // Styles

        private GUIStyle m_ContentBlockStyle = null;

        #endregion


        #region Lifecycle

        /// <summary>
        /// Called when this editor is open.
        /// </summary>
        private void OnEnable()
        {
            // Cleans the registered custom editor extensions of the removed settings.
            MultipleEditorsManager.Clean();

            // Load settings lists, grouped by target name
            foreach (CustomEditorExtensionSettings settings in Target.Settings)
            {
                CustomEditorListGroup group = m_CustomEditors.Find(currentGroup => { return currentGroup.TargetTypeName == settings.TargetTypeName; });
                if(group == null)
                {
                    group = new CustomEditorListGroup(settings.TargetTypeName);
                    m_CustomEditors.Add(group);
                }
                group.List.Settings.Add(settings);
            }

            // Initialize settings lists
            foreach(CustomEditorListGroup group in m_CustomEditors)
            {
                group.List.Init();
            }

            m_ExpandHelpAnimBool = new AnimBool(m_CustomEditors.Count == 0);
            m_ExpandHelpAnimBool.valueChanged.AddListener(() => { m_RequiresRepaintEvent.Invoke(); });
            m_RequiresRepaintEvent.AddListener(Repaint);
        }

        /// <summary>
        /// Called when the editor is closed, or when the core is reimported.
        /// </summary>
        private void OnDisable()
        {
            // Clear the settings list to avoid addings same settings if the code has been reimported but the editor is still open
            foreach (CustomEditorListGroup group in m_CustomEditors)
            {
                group.List.Settings.Clear();
            }
        }

        #endregion


        #region UI

        /// <summary>
        /// This editor doesn't need the default inspector margins.
        /// </summary>
        public override bool UseDefaultMargins()
        {
            return false;
        }

        /// <summary>
        /// Called when this editor is drawn in the inspector or a window.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawHelp();
            DrawManager();
        }

        /// <summary>
        /// Draw the custom editor settings as reorderable lists, grouped by target type.
        /// </summary>
        private void DrawManager()
        {
            // Display a help box if there's no registered custom editors
            if (m_CustomEditors.Count == 0)
            {
                EditorGUILayout.HelpBox("No custom editor", MessageType.Info);
                return;
            }

            // Draw the settings lists in a scroll view to avoid overflow clipping
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition, ScrollViewStyle, GUILayout.ExpandHeight(false));
            {
                foreach (CustomEditorListGroup group in m_CustomEditors)
                {
                    group.List.DrawList(LeftOffset);
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();

            // Draw the control buttons
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Reset To Default Settings", GUILayout.Height(BUTTON_HEIGHT)))
                {
                    if (EditorUtility.DisplayDialog("Reset Custom Editors", "Are you sure you want top reset all custom editor settings to their default values?", "Reset", "Cancel"))
                    {
                        MultipleEditorsManager.ResetDefaultOptions();
                        Recompiler.Recompile();
                    }
                }

                if (GUILayout.Button("Apply Changes (Reimport)", GUILayout.Height(BUTTON_HEIGHT)))
                {
                    Recompiler.Recompile();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the foldable help block.
        /// </summary>
        private void DrawHelp()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            controlRect.position = new Vector2(controlRect.x + LeftOffset, controlRect.y);
            m_ExpandHelpAnimBool.target = EditorGUI.Foldout(controlRect, m_ExpandHelpAnimBool.target, "Help", true);
            if (EditorGUILayout.BeginFadeGroup(m_ExpandHelpAnimBool.faded))
            {
                string help = "The <b>Multiple Editors Manager</b> allows you to create more than one custom editor for an Object, even for native Objects.";
                help += "\n\nYou can create a custom editor through code in two steps:";
                help += "\n\t- Create a custom editor class that inherits from <b><i>MultipleEditorsBase</i></b> (which is the main Editor actually displayed in the inspector), and that uses the <b><i>[CustomEditor]</i></b> attribute";
                help += "\n\t- Create each custom editor by creating a C# class that inherits from <b><i>CustomEditorExtension</i></b>. Inside this class, you can override for example <b><i>OnInspectorGUI()</i></b> to draw your own editor extension";
                help += "\n\nCreated <b><i>CustomEditorExtensions</i></b> must register in the <b>Multiple Editors Manager</b> to be available in \"real\" editors. This can be done by adding a <b><i>static constructor</i></b>, and use the <b><i>CustomEditorExtension.RegisterCustomEditor()</i></b> method in it.";
                help += "\n\nAfter creating your custom editors, you can use this window to setup the order of each extension per target type, see what extensions do, and enable/disable them as you want.";
                help += "\nNote that custom editors are registered in their <b><i>static constructor</i></b>. So changing their order or any other option requires to recompile. You can do that by clicking on <i>Apply Changes</i> button.";
                GUILayout.Box(help, EditorHelpers.HelpBoxStyle);
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.Space();
        }

        #endregion


        #region Accessors

        /// <summary>
        /// Called when the help block animation is playing.
        /// </summary>
        public UnityEvent OnRequiresRepaint
        {
            get { return m_RequiresRepaintEvent; }
        }

        /// <summary>
        /// Defines if this editor should apply left offset on foldout fields.
        /// </summary>
        public bool UseLeftOffset
        {
            get { return m_UseLeftOffset; }
            set { m_UseLeftOffset = value; }
        }

        /// <summary>
        /// Gets the left offset value if the leftv offset is used, otherwise returns 0.
        /// </summary>
        private float LeftOffset
        {
            get { return m_UseLeftOffset ? LEFT_OFFSET : 0f; }
        }

        /// <summary>
        /// Gets the settings lists scroll view style (no margin, horizontal padding).
        /// </summary>
        private GUIStyle ScrollViewStyle
        {
            get
            {
                if(m_ContentBlockStyle == null)
                {
                    m_ContentBlockStyle = new GUIStyle();
                    m_ContentBlockStyle.margin = new RectOffset();
                    m_ContentBlockStyle.padding = new RectOffset(SETTINGS_BLOCK_SMALL_MARGIN, SETTINGS_BLOCK_SMALL_MARGIN, 0, 0);
                }
                return m_ContentBlockStyle;
            }
        }

        #endregion

    }

}
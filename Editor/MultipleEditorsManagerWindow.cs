using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    ///<summary>
    /// This window allows you to manage all the registered custom editor extensions from the editor.
    ///</summary>
    public class MultipleEditorsManagerWindow : EditorWindow
	{

        #region Properties

        // Window settings

        private const string MENU_ITEM = "Muffin Dev/Multiple Editors Manager";
        private const string WINDOW_TITLE = "Multiple Editors Manager Window";

        // UI Layout

        private const int MIN_WINDOW_WIDTH = 400;
        private const int MIN_WINDOW_HEIGHT = 418;

        // Cache the original MultipleEditorsManagerEditor instance.
        [SerializeField]
        private MultipleEditorsManagerEditor m_ManagerEditor = null;

        #endregion


        #region Public Methods

        /// <summary>
        /// Gets and show the Multiple Editors Manager window in the editor.
        /// </summary>
        [MenuItem(MENU_ITEM, false)]
        public static void ShowWindow()
        {
            MultipleEditorsManagerWindow window = GetWindow<MultipleEditorsManagerWindow>(false, WINDOW_TITLE, true);
            window.minSize = new Vector2(MIN_WINDOW_WIDTH, MIN_WINDOW_HEIGHT);
            window.Show();
        }

        #endregion


        #region Lifecycle

        /// <summary>
        /// Called when the window is open.
        /// </summary>
        private void OnEnable()
        {
            // Creates and setup an instance of the original MultipleEditorsManagerEditor to display in the window.
            m_ManagerEditor = Editor.CreateEditor(Manager, typeof(MultipleEditorsManagerEditor)) as MultipleEditorsManagerEditor;
            m_ManagerEditor.UseLeftOffset = false;
            m_ManagerEditor.OnRequiresRepaint.AddListener(Repaint);
        }

        /// <summary>
        /// Called when the window is closed.
        /// </summary>
        private void OnDisable()
        {
            // Destroy the cached original MultipleEditorsManagerEditor instance.
            DestroyImmediate(m_ManagerEditor);
        }

        #endregion


        #region UI

        /// <summary>
        /// Draws this window GUI.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Space(4f);
            if(m_ManagerEditor != null)
            {
                m_ManagerEditor.OnInspectorGUI();
            }
        }

        #endregion


        #region Accessors

        /// <summary>
        /// Gets the active MultipleEditorsManager instance.
        /// </summary>
        public MultipleEditorsManager Manager
        {
            get { return MultipleEditorsManager.Instance; }
        }

        #endregion

    }

}
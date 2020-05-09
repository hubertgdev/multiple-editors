using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Handles all custom editor extensions for each possible target type.
    /// To create a custom editor extension, create a class that inherits from CustomEditorExtension, and register that class in its static
    /// constructor, using RegisterCustomExtension() method.
    /// </summary>
    public class MultipleEditorsManager : ScriptableObject
    {

        #region Properties

        /// <summary>
        /// Represents a method that crates an instance of a CustomEditorExtension.
        /// </summary>
        /// <returns>Returns the created instance.</returns>
        public delegate ICustomEditorExtension CreateEditorDelegate();

        // Stores the unique instance of this object.
        private static MultipleEditorsManager s_Instance = null;

        // Contains settings that can be managed using the MultipleEditorsManagerWindow.
        [SerializeField]
        private List<CustomEditorExtensionSettings> m_CustomEditorSettings = new List<CustomEditorExtensionSettings>();

        #endregion


        #region Public Methods

        /// <summary>
        /// Registers a CustomEditorExtension that customizes the inspector of the given target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the decorated Object.</typeparam>
        /// <param name="_CustomEditorCreator">The method to use to instantiate a CustomObectEditor.</param>
        /// <returns>Returns true if the CustomEditorExtension has successfully been registered, otherwise false.</returns>
        public static bool RegisterCustomEditor<TTarget>(CreateEditorDelegate _CustomEditorCreator)
            where TTarget : Object
        {
            return RegisterCustomEditor<TTarget>(_CustomEditorCreator, CustomEditorExtensionOptions.Default);
        }

        /// <summary>
        /// Registers a CustomEditorExtension that customizes the inspector of the given target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the decorated Object.</typeparam>
        /// <param name="_CustomEditorCreator">The method to use to instantiate a CustomObectEditor.</param>
        /// <param name="_Options">The options and default values to use for the CustomEditorExtension.</param>
        /// <returns>Returns true if the CustomEditorExtension has successfully been registered, otherwise false.</returns>
        public static bool RegisterCustomEditor<TTarget>(CreateEditorDelegate _CustomEditorCreator, CustomEditorExtensionOptions _Options)
            where TTarget : Object
        {
            Instance.AddOrUpdateCustomEditorSettings<TTarget>(_CustomEditorCreator, _Options);
            return true;
        }

        /// <summary>
        /// Creates instances of all CustomEditorExtension that decorate the given target type, and return thses instances.
        /// </summary>
        /// <typeparam name="TTarget">The type of the decorated Object.</typeparam>
        /// <param name="_EditorReference">The open Editor instance that uses the CustomEditorExtensions.</param>
        /// <param name="_RequiresConstantRepaint">This value is initialized to true if one of the created editor has its "requires
        /// constant repaint" option enabled.</param>
        /// <returns>Returns the created CustomEditorExtension instances.</returns>
        public static ICustomEditorExtension[] CreateEditors<TTarget>(Editor _EditorReference, out bool _RequiresConstantRepaint)
            where TTarget : Object
        {
            return CreateEditors(typeof(TTarget), _EditorReference, out _RequiresConstantRepaint);
        }

        /// <summary>
        /// Creates instances of all CustomEditorExtension that decorate the given target type, and return thses instances.
        /// </summary>
        /// <param name="_TargetType">The type of the decorated Object.</param>
        /// <param name="_EditorReference">The open Editor instance that uses the CustomEditorExtensions.</param>
        /// <param name="_RequiresConstantRepaint">This value is initialized to true if one of the created editor has its "requires
        /// constant repaint" option enabled.</param>
        /// <returns>Returns the created CustomEditorExtension instances.</returns>
        public static ICustomEditorExtension[] CreateEditors(Type _TargetType, Editor _EditorReference, out bool _RequiresConstantRepaint)
        {
            // Clean the eventually removed extensions from the list
            Clean();

            List<ICustomEditorExtension> customEditors = new List<ICustomEditorExtension>();
            _RequiresConstantRepaint = false;

            // For each settings
            foreach (CustomEditorExtensionSettings settings in Instance.m_CustomEditorSettings)
            {
                // If the settings match with the given target type
                if(settings.TargetType == _TargetType)
                {
                    // Create an instance of the extension
                    ICustomEditorExtension editorInstance = settings.CreateEditor();
                    // If the instance can be created (the editor is not disabled and a create method have been defined)
                    if(editorInstance != null)
                    {
                        // Initialize the instance, and add it to the output list
                        editorInstance.Init(_EditorReference);
                        customEditors.Add(editorInstance);

                        if (settings.RequiresConstantRepaint)
                            _RequiresConstantRepaint = true;
                    }
                }
            }

            // Return the created editor extension instances
            return customEditors.ToArray();
        }

        /// <summary>
        /// Resets all the settings of the list to their default values.
        /// </summary>
        public static void ResetDefaultOptions()
        {
            foreach(CustomEditorExtensionSettings settings in Instance.m_CustomEditorSettings)
            {
                settings.Reset();
            }
        }

        /// <summary>
        /// Cleans the settings list of eventually removed custom editors.
        /// </summary>
        public static void Clean()
        {
            for (int i = 0; i < Instance.m_CustomEditorSettings.Count; i++)
            {
                // If no create method have been set on the current custom editor extension settings, it's considered as removed
                if(Instance.m_CustomEditorSettings[i].CreateEditorMethod == null)
                {
                    Instance.m_CustomEditorSettings.RemoveAt(i);
                    i--;
                    continue;
                }
            }
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Gets the settings used by the given custom editor.
        /// </summary>
        /// <param name="_CustomEditorType">The type of the custom editor you want to get the settings.</param>
        /// <returns>Returns the found settings, or null if no settings has been created for the given custom editor type.</returns>
        private CustomEditorExtensionSettings GetCustomEditorSettings(Type _CustomEditorType)
        {
            return m_CustomEditorSettings.Find(setings => { return setings.CustomEditorType == _CustomEditorType; });
        }

        /// <summary>
        /// Adds a new settings entry in the list, or update an existing one.
        /// </summary>
        /// <typeparam name="TTarget">The target type of the custom editor.</typeparam>
        /// <param name="_CreateEditorMethod">The method to use to create an instance of the custom editor extension.</param>
        /// <param name="_DefaultOptions">The default options to use for the custom editor extension.</param>
        /// <returns>Returns the created settings.</returns>
        private CustomEditorExtensionSettings AddOrUpdateCustomEditorSettings<TTarget>(CreateEditorDelegate _CreateEditorMethod, CustomEditorExtensionOptions _DefaultOptions)
            where TTarget : Object
        {
            return AddOrUpdateCustomEditorSettings(_CreateEditorMethod, typeof(TTarget), _DefaultOptions);
        }

        /// <summary>
        /// Adds a new settings entry in the list, or update an existing one.
        /// </summary>
        /// <param name="_TargetType">The target type of the custom editor.</param>
        /// <param name="_CreateEditorMethod">The method to use to create an instance of the custom editor extension.</param>
        /// <param name="_DefaultOptions">The default options to use for the custom editor extension.</param>
        /// <returns>Returns the created settings.</returns>
        private CustomEditorExtensionSettings AddOrUpdateCustomEditorSettings(CreateEditorDelegate _CreateEditorMethod, Type _TargetType, CustomEditorExtensionOptions _DefaultOptions)
        {
            Type customEditorType = _CreateEditorMethod.Invoke().GetType();

            // Gets the existing settings for the custom inspector.
            CustomEditorExtensionSettings settings = m_CustomEditorSettings.Find(currentSettings => { return currentSettings.CustomEditorType == customEditorType; });
            // Update the existing settings if it exists
            if (settings != null)
            {
                settings.Update(_TargetType, _CreateEditorMethod, _DefaultOptions);
            }
            // If no matching settings has been found, create a new one using the given params, and add it to the list
            else
            {
                settings = new CustomEditorExtensionSettings(_TargetType, customEditorType, _CreateEditorMethod, _DefaultOptions);
                m_CustomEditorSettings.Add(settings);
            }

            m_CustomEditorSettings.Sort();
            return settings;
        }

        #endregion


        #region Accessors

        /// <summary>
        /// Gets the unique instance of MultipleEditorsManager.
        /// </summary>
        public static MultipleEditorsManager Instance
        {
            get
            {
                // Get an instance of MultipleEditorsManager from project assets if no one has been loaded before
                if (s_Instance == null)
                {
                    string[] managers = AssetDatabase.FindAssets($"t:{typeof(MultipleEditorsManager)}");
                    if (managers.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(managers[0]);
                        s_Instance = AssetDatabase.LoadAssetAtPath<MultipleEditorsManager>(path);
                    }
                }

                // Create the asset if no one exists in this project
                if(s_Instance == null)
                {
                    string scriptPath = ScriptableObjectExtension.GetScriptPath<MultipleEditorsManager>();
                    if (scriptPath != null)
                    {
                        s_Instance = CreateInstance<MultipleEditorsManager>();
                        string assetPath = $"{Path.GetDirectoryName(scriptPath)}/{typeof(MultipleEditorsManager).Name}.{EditorHelpers.DEFAULT_ASSET_EXTENSION}";
                        AssetDatabase.CreateAsset(s_Instance, assetPath);
#if MUFFINDEV_PROJECT
                        Debug.Log("No MultipleEditorsManager asset found in the project: a new one has been created at " + scriptPath);
                        EditorHelpers.FocusAsset(s_Instance, false, true);
#endif
                    }
                }
                return s_Instance;
            }
        }

        /// <summary>
        /// Gets the registered custom editor extensions settings.
        /// </summary>
        public CustomEditorExtensionSettings[] Settings
        {
            get { return m_CustomEditorSettings.ToArray(); }
        }

        #endregion

    }

}
using System;
using UnityEngine;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Represents the settings stored in the MultipleEditorsManager instance for one custom editor extension.
    /// </summary>
    [System.Serializable]
    public class CustomEditorExtensionSettings : IComparable<CustomEditorExtensionSettings>, ISerializationCallbackReceiver
    {

        // The target type full name string. Used mainly for serializing the "real" type.
        [SerializeField]
        private string m_TargetTypeName = string.Empty;

        // The target type full name string. Used mainly for serializing the "real" type.
        [SerializeField]
        private string m_CustomEditorTypeName = string.Empty;

        // The user defined order of the custom editor.
        [SerializeField]
        private int m_Order = 0;

        // Defines if the main Editor should set RequiresConstantRepaint to true.
        [SerializeField]
        private bool m_RequiresConstantRepaint = false;

        // Defines if the custom editor extension is enabled or not. If false, it won't be displayed in the inspector.
        [SerializeField]
        private bool m_Enabled = true;

        // Stores the default options of the custom editor, such as its dsplay name, default order, etc.
        [SerializeField]
        private CustomEditorExtensionOptions m_DefaultOptions = CustomEditorExtensionOptions.Default;

        // The target type of the custom editor.
        private Type m_TargetType = null;
        // The type of the custom editor.
        private Type m_CustomEditorType = null;

        // The method to use to create an instance of the custom editor.
        public MultipleEditorsManager.CreateEditorDelegate CreateEditorMethod { get; private set; } = null;

        /// <summary>
        /// Called before this object is serialize by Unity.
        /// Stores the target and custom editor types as string.
        /// </summary>
        public void OnBeforeSerialize()
        {
            m_TargetTypeName = TargetType.GetFullNameWithAssembly();
            m_CustomEditorTypeName = CustomEditorType.GetFullNameWithAssembly();
        }

        /// <summary>
        /// Called after this object has been deserialized by Unity.
        /// Retrieve the target and custom editor types from stored type names.
        /// </summary>
        public void OnAfterDeserialize()
        {
            TargetType = Type.GetType(m_TargetTypeName);
            CustomEditorType = Type.GetType(m_CustomEditorTypeName);
        }

        /// <summary>
        /// Creates an instance of CustomEditorExtensionSettings.
        /// </summary>
        /// <param name="_TargetType">The target type of the custom editor.</param>
        /// <param name="_CustomEditorType">The custom editor type.</param>
        /// <param name="_CreateEditorMethod">The method to use to create an instance of the custom editor.</param>
        /// <param name="_DefaultOptions">The default options to use for the custom editor.</param>
        public CustomEditorExtensionSettings(Type _TargetType, Type _CustomEditorType, MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, CustomEditorExtensionOptions _DefaultOptions)
        {
            CustomEditorType = _CustomEditorType;
            m_Order = _DefaultOptions.defaultOrder;
            m_RequiresConstantRepaint = _DefaultOptions.requiresConstantRepaint;

            Update(_TargetType, _CreateEditorMethod, _DefaultOptions);
        }

        /// <summary>
        /// Updates the settings of a custom editor.
        /// </summary>
        /// <param name="_TargetType">The new target type of the custom editor.</param>
        /// <param name="_CreateEditorMethod">The method to use to create an instance of the custom editor.</param>
        /// <param name="_DefaultOptions">The default options to use for the custom editor.</param>
        public void Update(Type _TargetType, MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, CustomEditorExtensionOptions _DefaultOptions)
        {
            TargetType = _TargetType;
            if (m_Order == m_DefaultOptions.defaultOrder)
            {
                m_Order = _DefaultOptions.defaultOrder;
            }
            m_DefaultOptions = _DefaultOptions;

            CreateEditorMethod = _CreateEditorMethod;
        }

        /// <summary>
        /// Resets all settings of the custom editor to their default values.
        /// </summary>
        public void Reset()
        {
            m_Order = m_DefaultOptions.defaultOrder;
            m_RequiresConstantRepaint = m_DefaultOptions.requiresConstantRepaint;
            m_Enabled = true;
        }

        /// <summary>
        /// Calls this CustomEditorExtension's creation method.
        /// </summary>
        /// <returns>Returns the created custom editor instance.</returns>
        public ICustomEditorExtension CreateEditor()
        {
            if (CreateEditorMethod == null)
            {
                Debug.LogError("The editor " + CustomEditorType.Name + " doesn't have a Create Editor method");
                return null;
            }

            if (!m_Enabled)
                return null;

            return CreateEditorMethod.Invoke();
        }

        /// <summary>
        /// Comapres these settings with another CustomEditorExtensionSettings instance.
        /// </summary>
        /// <param name="_Other">The other settings to compare with.</param>
        /// <returns>Place these settings before the other if they both have the same target type but these settings order is higher, or if
        /// the target type name is placed alphabetically before the other settings target type name.</returns>
        public int CompareTo(CustomEditorExtensionSettings _Other)
        {
            if (m_TargetTypeName == _Other.m_TargetTypeName)
            {
                return m_Order.CompareTo(_Other.m_Order, true);
            }
            return m_TargetTypeName.CompareTo(_Other.m_TargetTypeName);
        }

        /// <summary>
        /// Gets the target type of the custom editor.
        /// </summary>
        public Type TargetType
        {
            get { return m_TargetType; }
            set
            {
                m_TargetType = value;
                m_TargetTypeName = m_TargetType.GetFullNameWithAssembly();
            }
        }

        /// <summary>
        /// Gets the custom editor type.
        /// </summary>
        public Type CustomEditorType
        {
            get { return m_CustomEditorType; }
            set
            {
                m_CustomEditorType = value;
                m_CustomEditorTypeName = m_CustomEditorType.GetFullNameWithAssembly();
            }
        }

        /// <summary>
        /// Enables/Disables the custom editor.
        /// </summary>
        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        /// <summary>
        /// Gets the display name of the custom editor. If it has not been defined, returns the type name of the custom editor.
        /// </summary>
        public string DisplayName
        {
            get { return !string.IsNullOrEmpty(m_DefaultOptions.displayName) ? m_DefaultOptions.displayName : m_CustomEditorType.Name; }
        }

        /// <summary>
        /// Gets the description of the custom editor.
        /// </summary>
        public string Description
        {
            get { return m_DefaultOptions.description; }
        }

        /// <summary>
        /// Gets/sets the current order of the custom editor.
        /// </summary>
        public int Order
        {
            get { return m_Order; }
            set { m_Order = value; }
        }

        /// <summary>
        /// Checks if the main Editor should use Editor.RequiresConstantRepaint() when using the custom editor.
        /// </summary>
        public bool RequiresConstantRepaint
        {
            get { return m_RequiresConstantRepaint; }
            set { m_RequiresConstantRepaint = value; }
        }

        /// <summary>
        /// Gets the target type name of the custom editor.
        /// </summary>
        public string TargetTypeName
        {
            get { return m_TargetTypeName; }
        }

    }

}
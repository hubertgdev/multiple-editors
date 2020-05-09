using System;

namespace MuffinDev
{

    ///<summary>
    /// Extensions for System.Type.
    ///</summary>
    public static class TypeExtension
    {

        /// <summary>
        /// Gets the full type name string, including the assmebly name. The output string looks like:
        /// "TypeNamespace.TypeName, AssemblyName, Version=0.0.0.0, Culture=neutral, PublicKeyKoken=null"
        /// </summary>
        /// <param name="_Type">The type you wan to get the full name string.</param>
        /// <returns>Returns the computed full name string, or null if the given type is null.</returns>
        public static string GetFullNameWithAssembly(this Type _Type)
        {
            return _Type != null ? $"{_Type.FullName}, {_Type.Assembly}" : null;
        }

    }

}
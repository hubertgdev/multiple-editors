using System;

using UnityEngine;

using Object = UnityEngine.Object;

namespace MuffinDev
{

	///<summary>
	/// Extensions for ScriptableObjects.
	///</summary>
	public static class ScriptableObjectExtension
	{

        /// <summary>
        /// Gets the path to the source script used by the given ScriptableObject.
        /// WARNING: This method is meant to be used in the editor. Using it in another context will always return null.
        /// </summary>
        /// <param name="_Obj">The Scriptable Object you want to get the script path.</param>
        /// <returns>Returns script path, or null if the path to the file can't be found.</returns>
		public static string GetScriptPath(this ScriptableObject _Obj)
        {
            return GetScriptPath(_Obj.GetType());
        }

        /// <summary>
        /// Gets the path to the source script used by the given ScriptableObject.
        /// WARNING: This method is meant to be used in the editor. Using it in another context will always return null.
        /// </summary>
        /// <typeparam name="TScriptableObject">The type of the ScriptableObject you want to get the script path.</typeparam>
        /// <returns>Returns script path, or null if the path to the file can't be found.</returns>
        public static string GetScriptPath<TScriptableObject>()
            where TScriptableObject : ScriptableObject
        {
            return GetScriptPath(typeof(TScriptableObject));
        }

        /// <summary>
        /// Gets the path to the source script used by the given ScriptableObject.
        /// WARNING: This method is meant to be used in the editor. Using it in another context will always return null.
        /// </summary>
        /// <param name="_ScriptableObjectType">The type of the ScriptableObject you want to get the script path.</param>
        /// <returns>Returns script path, or null if the path to the file can't be found.</returns>
        public static string GetScriptPath(Type _ScriptableObjectType)
        {
#if UNITY_EDITOR
            ScriptableObject tmpInstance = ScriptableObject.CreateInstance(_ScriptableObjectType);
            UnityEditor.MonoScript sourceScriptAsset = UnityEditor.MonoScript.FromScriptableObject(tmpInstance);
            Object.DestroyImmediate(tmpInstance);
            return UnityEditor.AssetDatabase.GetAssetPath(sourceScriptAsset);
#else
            return null;
#endif
        }

    }

}
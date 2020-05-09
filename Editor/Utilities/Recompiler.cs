using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils
{

    ///<summary>
    /// This utility allows you to force Unity to recompile code.
    ///</summary>
    public class Recompiler : ScriptableObject
    {
        
        /// <summary>
        /// Recompiles code.
        /// </summary>
        public static void Recompile()
        {
            string scriptPath = ScriptableObjectExtension.GetScriptPath<Recompiler>();
            AssetDatabase.ImportAsset(scriptPath, ImportAssetOptions.ForceUpdate);
        }

    }

}
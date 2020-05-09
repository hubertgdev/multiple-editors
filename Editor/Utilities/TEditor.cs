using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils
{

	///<summary>
	///	Helper for making an Editor class with a "typed" target or targets.
	///	
	///	The "target" accessor from Editor class returns the selected target as an Object instance.
	///	The problem is that you have to cast that value in order to use its properties and methods.
	///	This class does the cast for you, and cache the cast object.
	///</summary>
	public class TEditor<TTarget> : Editor
		where TTarget : Object
	{

		private TTarget m_TypedTarget = null;
		private TTarget[] m_TypedTargets = null;

		/// <summary>
		/// Returns the cast target.
		/// </summary>
		protected TTarget TypedTarget
		{
            get { return (m_TypedTarget != null) ? m_TypedTarget : (target as TTarget); }
		}

        /// <summary>
		/// Returns the cast targets.
        /// </summary>
        protected TTarget[] TypedTargets
        {
            get
            {
                if(m_TypedTargets == null)
                {
                    m_TypedTargets = new TTarget[targets.Length];
                    for(int i = 0; i < targets.Length; i++)
                    {
                        m_TypedTargets[i] = targets[i] as TTarget;
                    }
                }
                return m_TypedTargets;
            }
        }

        /// <summary>
        /// Alias of TypedTarget accessor.
        /// </summary>
        protected TTarget Target
        {
            get { return TypedTarget; }
        }

        /// <summary>
        /// Alias of TypedTargets accessor.
        /// </summary>
        protected TTarget[] Targets
        {
            get { return m_TypedTargets; }
        }

	}

}
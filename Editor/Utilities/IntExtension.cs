using UnityEngine;

namespace MuffinDev
{

    ///<summary>
    /// Extensions for int objects.
    ///</summary>
    public static class IntExtension
	{

        /// <summary>
        /// Compares a number to another.
        /// </summary>
        /// <param name="_Number">The number to compare.</param>
        /// <param name="_Other">The number to compare with.</param>
        /// <param name="_Desc">If true, compares the number in a descending order.</param>
        /// <returns>Returns -1 if the number should be placed before the other, 1 if it should be placed after, or 0 if the numbers are
        /// even.</returns>
        public static int CompareTo(this int _Number, int _Other, bool _Desc)
        {
            if(_Desc)
            {
                if (_Number == _Other)
                    return 0;

                return (_Number < _Other) ? 1 : -1;
            }
            else
            {
                return _Number.CompareTo(_Other);
            }
        }

	}

}
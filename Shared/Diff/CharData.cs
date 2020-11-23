#region  Espacios de nombres 

using System;

#endregion

namespace Shared.Diff
{
    /// <summary>
    /// 
    /// </summary>
    /// 06/04/2012 by Juanda
    public class DiffList_CharData : IDiffList
    {
        private readonly char[] _charList;

        /// <summary>
        /// Inicializa una nueva instancia de la class <see cref="DiffList_CharData"/>.
        /// </summary>
        /// <param name="charData">The char data.</param>
        /// 06/04/2012 by Juanda
        public DiffList_CharData(string charData)
        {
            _charList = charData.ToCharArray();
        }

        #region IDiffList Members

        /// <summary>
        /// Counts.
        /// </summary>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public int Count()
        {
            return _charList.Length;
        }

        /// <summary>
        /// Gets the by index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public IComparable GetByIndex(int index)
        {
            return _charList[index];
        }

        #endregion
    }
}

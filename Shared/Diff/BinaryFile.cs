#region  Espacios de nombres 

using System;
using System.IO;

#endregion

namespace Shared.Diff
{
    /// <summary>
    /// 
    /// </summary>
    /// 06/04/2012 by Juanda
    public class DiffList_BinaryFile : IDiffList
    {
        private readonly byte[] _byteList;

        /// <summary>
        /// Inicializa una nueva instancia de la class <see cref="DiffList_BinaryFile"/>.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// 06/04/2012 by Juanda
        public DiffList_BinaryFile(string fileName)
        {
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                int len = (int) fs.Length;
                br = new BinaryReader(fs);
                _byteList = br.ReadBytes(len);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (br != null) br.Close();
                if (fs != null) fs.Close();
            }
        }

        #region IDiffList Members

        /// <summary>
        /// Counts.
        /// </summary>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public int Count()
        {
            return _byteList.Length;
        }

        /// <summary>
        /// Gets the by index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public IComparable GetByIndex(int index)
        {
            return _byteList[index];
        }

        #endregion
    }
}

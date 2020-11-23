#region  Espacios de nombres 

using System;
using System.Collections;
using System.IO;

#endregion

namespace Shared.Diff
{
    /// <summary>
    /// 
    /// </summary>
    /// 06/04/2012 by Juanda
    public class TextLine : IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        /// 06/04/2012 by Juanda
        public string Line;

        /// <summary>
        /// 
        /// </summary>
        /// 06/04/2012 by Juanda
        public int _hash;

        /// <summary>
        /// Inicializa una nueva instancia de la class <see cref="TextLine"/>.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// 06/04/2012 by Juanda
        public TextLine(string str)
        {
            Line = str.Replace("\t", "    ");
            _hash = str.GetHashCode();
        }

        #region IComparable Members

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="obj"/> is not the same type as this instance. </exception>
        /// 06/04/2012 by Juanda
        public int CompareTo(object obj)
        {
            return _hash.CompareTo(((TextLine) obj)._hash);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// 06/04/2012 by Juanda
    public class DiffList_TextFile : IDiffList
    {
        private const int MaxLineLength = 1024;
        private readonly ArrayList _lines;

        /// <summary>
        /// Inicializa una nueva instancia de la class <see cref="DiffList_TextFile"/>.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// 06/04/2012 by Juanda
        public DiffList_TextFile(string fileName)
        {
            _lines = new ArrayList();
            using (StreamReader sr = new StreamReader(fileName))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > MaxLineLength)
                    {
                        throw new InvalidOperationException(
                            string.Format("File contains a line greater than {0} characters.",
                                          MaxLineLength));
                    }
                    _lines.Add(new TextLine(line));
                }
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
            return _lines.Count;
        }

        /// <summary>
        /// Gets the by index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public IComparable GetByIndex(int index)
        {
            return (TextLine) _lines[index];
        }

        #endregion
    }
}

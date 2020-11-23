#region  Espacios de nombres 

using System;
using System.Diagnostics;

#endregion

namespace Shared.Diff
{
    /// <summary>
    /// 
    /// </summary>
    /// 06/04/2012 by Juanda
    public interface IDiffList
    {
        /// <summary>
        /// Counts.
        /// </summary>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        int Count();

        /// <summary>
        /// Gets the by index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        IComparable GetByIndex(int index);
    }

    internal enum DiffStatus
    {
        Matched = 1,
        NoMatch = -1,
        Unknown = -2
    }

    internal class DiffState
    {
        private const int BAD_INDEX = -1;
        private int _length;
        private int _startIndex;

        public DiffState()
        {
            SetToUnkown();
        }

        public int StartIndex
        {
            get { return _startIndex; }
        }

        public int EndIndex
        {
            get { return ((_startIndex + _length) - 1); }
        }

        public int Length
        {
            get
            {
                int len;
                if (_length > 0)
                    len = _length;
                else
                {
                    len = _length == 0 ? 1 : 0;
                }
                return len;
            }
        }

        public DiffStatus Status
        {
            get
            {
                DiffStatus stat;
                if (_length > 0)
                    stat = DiffStatus.Matched;
                else
                {
                    switch (_length)
                    {
                        case -1:
                            stat = DiffStatus.NoMatch;
                            break;
                        default:
                            Debug.Assert(_length == -2, "Invalid status: _length < -2");
                            stat = DiffStatus.Unknown;
                            break;
                    }
                }
                return stat;
            }
        }

        protected void SetToUnkown()
        {
            _startIndex = BAD_INDEX;
            _length = (int) DiffStatus.Unknown;
        }

        public void SetMatch(int start, int length)
        {
            Debug.Assert(length > 0, "Length must be greater than zero");
            Debug.Assert(start >= 0, "Start must be greater than or equal to zero");
            _startIndex = start;
            _length = length;
        }

        public void SetNoMatch()
        {
            _startIndex = BAD_INDEX;
            _length = (int) DiffStatus.NoMatch;
        }

        public bool HasValidLength(int newStart, int newEnd, int maxPossibleDestLength)
        {
            if (_length > 0) //have unlocked match
            {
                if ((maxPossibleDestLength < _length) ||
                    ((_startIndex < newStart) || (EndIndex > newEnd)))
                    SetToUnkown();
            }
            return (_length != (int) DiffStatus.Unknown);
        }
    }

    internal class DiffStateList
    {
#if USE_HASH_TABLE
		private Hashtable _table;
#else
        private readonly DiffState[] _array;
#endif

        public DiffStateList(int destCount)
        {
#if USE_HASH_TABLE
			_table = new Hashtable(Math.Max(9,destCount/10));
#else
            _array = new DiffState[destCount];
#endif
        }

        public DiffState GetByIndex(int index)
        {
#if USE_HASH_TABLE
			DiffState retval = (DiffState)_table[index];
			if (retval == null)
			{
				retval = new DiffState();
				_table.Add(index,retval);
			}
#else
            DiffState retval = _array[index];
            if (retval == null)
            {
                retval = new DiffState();
                _array[index] = retval;
            }
#endif
            return retval;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// 06/04/2012 by Juanda
    public enum DiffResultSpanStatus
    {
        /// <summary>
        /// 
        /// </summary>
        /// 06/04/2012 by Juanda
        NoChange,

        /// <summary>
        /// 
        /// </summary>
        /// 06/04/2012 by Juanda
        Replace,

        /// <summary>
        /// 
        /// </summary>
        /// 06/04/2012 by Juanda
        DeleteSource,

        /// <summary>
        /// 
        /// </summary>
        /// 06/04/2012 by Juanda
        AddDestination
    }

    /// <summary>
    /// 
    /// </summary>
    /// 06/04/2012 by Juanda
    public class DiffResultSpan : IComparable
    {
        private const int BAD_INDEX = -1;
        private readonly int _destIndex;
        private readonly int _sourceIndex;
        private readonly DiffResultSpanStatus _status;
        private int _length;

        /// <summary>
        /// Inicializa una nueva instancia de la class <see cref="DiffResultSpan"/>.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="destIndex">The dest index.</param>
        /// <param name="sourceIndex">The source index.</param>
        /// <param name="length">The length.</param>
        /// 06/04/2012 by Juanda
        protected DiffResultSpan(
            DiffResultSpanStatus status,
            int destIndex,
            int sourceIndex,
            int length)
        {
            _status = status;
            _destIndex = destIndex;
            _sourceIndex = sourceIndex;
            _length = length;
        }

        /// <summary>
        /// Gets the dest index.
        /// </summary>
        /// <value>
        /// The dest index.
        /// </value>
        /// 06/04/2012 by Juanda
        public int DestIndex
        {
            get { return _destIndex; }
        }

        /// <summary>
        /// Gets the source index.
        /// </summary>
        /// <value>
        /// The source index.
        /// </value>
        /// 06/04/2012 by Juanda
        public int SourceIndex
        {
            get { return _sourceIndex; }
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        /// 06/04/2012 by Juanda
        public int Length
        {
            get { return _length; }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        /// 06/04/2012 by Juanda
        public DiffResultSpanStatus Status
        {
            get { return _status; }
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
            return _destIndex.CompareTo(((DiffResultSpan) obj)._destIndex);
        }

        #endregion

        /// <summary>
        /// Creates the no change.
        /// </summary>
        /// <param name="destIndex">The dest index.</param>
        /// <param name="sourceIndex">The source index.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.NoChange, destIndex, sourceIndex, length);
        }

        /// <summary>
        /// Creates the replace.
        /// </summary>
        /// <param name="destIndex">The dest index.</param>
        /// <param name="sourceIndex">The source index.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.Replace, destIndex, sourceIndex, length);
        }

        /// <summary>
        /// Creates the delete source.
        /// </summary>
        /// <param name="sourceIndex">The source index.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public static DiffResultSpan CreateDeleteSource(int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.DeleteSource, BAD_INDEX, sourceIndex, length);
        }

        /// <summary>
        /// Creates the add destination.
        /// </summary>
        /// <param name="destIndex">The dest index.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public static DiffResultSpan CreateAddDestination(int destIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.AddDestination, destIndex, BAD_INDEX, length);
        }

        /// <summary>
        /// Adds the length.
        /// </summary>
        /// <param name="i">The i.</param>
        /// 06/04/2012 by Juanda
        public void AddLength(int i)
        {
            _length += i;
        }

        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <returns></returns>
        /// 06/04/2012 by Juanda
        public override string ToString()
        {
            return string.Format("{0} (Dest: {1},Source: {2}) {3}",
                                 _status,
                                 _destIndex,
                                 _sourceIndex,
                                 _length);
        }
    }
}

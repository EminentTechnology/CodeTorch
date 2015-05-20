using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Designer.Code
{
    /// <summary>
    /// Stores a set of items with the most recently added at item the front and the 
    /// least recently added item at the end
    /// </summary>
    [Serializable]
    public class RecentSet<T> : IEnumerable<T>
    {
        private List<T> _list;
        private int _size = -1;

        /// <summary>
        /// Creates a new RecentSet object.
        /// </summary>
        public RecentSet()
        {
            _list = new List<T>();
        }
        /// <summary>
        /// Creates a new RecentSet object with a fixed size. The return set may be smaller than
        /// the specified size but it will never be larger
        /// </summary>
        /// <param name="size">The maximum size of the set</param>
        public RecentSet(int size)
        {
            _list = new List<T>();
            _size = size;
        }

        /// <summary>
        /// Creates a new RecentSet object initializing it with the indicated items. Note: 
        /// the initialized RecentSet will be in the order of parameter items.  If items are {1, 2, 3, 4},
        /// iterating through RecentSet will result in a list of {1, 2, 3, 4} not {4, 3, 2, 1}        
        /// </summary>
        public RecentSet(IEnumerable<T> items)
        {
            _list = items.ToList();
        }

        /// <summary>
        /// Creates a new RecentSet object with a fixed size initializing it with the indicated items. Note: 
        /// the initialized RecentSet will be in the order of parameter items.  If items are {1, 2, 3, 4},
        /// iterating through RecentSet will result in a list of {1, 2, 3, 4} not {4, 3, 2, 1}        
        /// </summary>
        public RecentSet(int size, IEnumerable<T> items)
        {
            _list = items.ToList();
            _size = size;

            TrimList();
        }

        /// <summary>
        /// Adds an item to the RecentSet
        /// </summary>
        public void Add(T item)
        {
            // If the item is already in the set, remove it
            int i = _list.IndexOf(item);
            if (i > -1)
                _list.RemoveAt(i);

            // Add the item to the front of the list.
            _list.Insert(0, item);

            TrimList();
        }

        public int Count
        {
            get { return _list.Count; }
        }

        private void TrimList()
        {
            // If there is a set size, make sure the set only contains that many elements
            if (_size != -1)
                while (_list.Count > _size)
                    _list.RemoveAt(_list.Count - 1);
        }

        /// <summary>
        /// Returns the set in the form of a List
        /// </summary>
        public List<T> ToList()
        {
            return _list;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}

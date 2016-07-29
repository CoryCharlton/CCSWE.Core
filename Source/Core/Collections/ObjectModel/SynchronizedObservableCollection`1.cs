using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace CCSWE.Collections.ObjectModel
{
    //TODO: SynchronizedObservableCollection<T> - Add xmldoc
    //TODO: SynchronizedObservableCollection<T> - ObservableCollection<T>.Move() is not implemented...
    [Serializable]
    [ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]
    public class SynchronizedObservableCollection<T> : IDisposable, IList<T>, IList, IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Constructor
        public SynchronizedObservableCollection()
        {
            _context = SynchronizationContext.Current;
        }

        public SynchronizedObservableCollection(IEnumerable<T> collection): this()
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), "'collection' cannot be null");
            }

            foreach (var item in collection)
            {
                _items.Add(item);
            }
        }
        
        public SynchronizedObservableCollection(SynchronizationContext context)
        {
            _context = context;
        }

        public SynchronizedObservableCollection(IEnumerable<T> collection, SynchronizationContext context): this(context)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), "'collection' cannot be null");
            }

            foreach (var item in collection)
            {
                _items.Add(item);
            }
        }
        #endregion

        #region Private Fields
        private readonly SynchronizationContext _context;
        private bool _isDisposed;
        private readonly IList<T> _items = new List<T>();
        private readonly ReaderWriterLockSlim _itemsLocker = new ReaderWriterLockSlim();
        [NonSerialized] private Object _syncRoot;

        private readonly SimpleMonitor _monitor = new SimpleMonitor();
        #endregion

        #region Events
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private Properties
        bool IList.IsFixedSize
        {
            get
            {
                var list = _items as IList;
                if (list != null)
                {
                    return list.IsFixedSize;
                }

                return _items.IsReadOnly;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return _items.IsReadOnly; }
        }

        bool IList.IsReadOnly
        {
            get { return _items.IsReadOnly; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    _itemsLocker.EnterReadLock();

                    try
                    {
                        var c = _items as ICollection;
                        if (c != null)
                        {
                            _syncRoot = c.SyncRoot;
                        }
                        else
                        {
                            Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                        }
                    }
                    finally
                    {
                        _itemsLocker.ExitReadLock();
                    }
                }

                return _syncRoot;
            }
        }
        #endregion

        #region Public Properties
        public int Count
        {
            get
            {
                _itemsLocker.EnterReadLock();

                try
                {
                    return _items.Count;
                }
                finally
                {
                    _itemsLocker.ExitReadLock();
                }
            }
        }

        public T this[int index]
        {
            get
            {
                _itemsLocker.EnterReadLock();

                try
                {
                    CheckIndex(index);

                    return _items[index];
                }
                finally
                {
                    _itemsLocker.ExitReadLock();
                }
            }
            set
            {
                T oldValue;

                _itemsLocker.EnterWriteLock();

                try
                {
                    CheckIsReadOnly();
                    CheckIndex(index);
                    CheckReentrancy();

                    oldValue = this[index];

                    _items[index] = value;

                }
                finally
                {
                    _itemsLocker.ExitWriteLock();
                }

                OnPropertyChanged("Item[]");
                OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldValue, value, index);
            }
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                try
                {
                    this[index] = (T) value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException("'value' is the wrong type");
                }
            }
        }

        #endregion

        #region Private Methods
        private IDisposable BlockReentrancy()
        {
            _monitor.Enter();

            return _monitor;
        }

        // ReSharper disable once UnusedParameter.Local
        private void CheckIndex(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckIsReadOnly()
        {
            if (_items.IsReadOnly)
            {
                throw new NotSupportedException("Collection is readonly");
            }
        }

        private void CheckReentrancy()
        {
            if (_monitor.Busy && CollectionChanged != null && CollectionChanged.GetInvocationList().Length > 1)
            {
                throw new InvalidOperationException("SynchronizedObservableCollection reentrancy not allowed");
            }
        }

        private static bool IsCompatibleObject(object value)
        {
            // Non-null values are fine.  Only accept nulls if T is a class or Nullable<U>.
            // Note that default(T) is not equal to null for value types except when T is Nullable<U>. 
            return ((value is T) || (value == null && default(T) == null));
        }

        private void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var collectionChanged = CollectionChanged;
            if (collectionChanged == null)
            {
                return;
            }

            using (BlockReentrancy())
            {
                _context.Send(state => collectionChanged(this, e), null);
            }
        }

        private void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
            {
                return;
            }

            _context.Send(state => propertyChanged(this, e), null);
        }
        #endregion

        #region Protected Methods
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            _itemsLocker.Dispose();
            _isDisposed = true;
        }
        #endregion

        #region Public Methods
        public void Add(T item)
        {
            _itemsLocker.EnterWriteLock();

            var index = -1;

            try
            {
                CheckIsReadOnly();
                CheckReentrancy();

                index = _items.Count;

                _items.Insert(index, item);
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }

            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        int IList.Add(object value)
        {
            _itemsLocker.EnterWriteLock();

            var index = -1;
            T item;

            try
            {
                CheckIsReadOnly();
                CheckReentrancy();

                index = _items.Count; 
                item = (T)value;

                _items.Insert(index, item);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("'value' is the wrong type");
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }

            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);

            return index;
        }

        public void Clear()
        {
            _itemsLocker.EnterWriteLock();

            try
            {
                CheckIsReadOnly();
                CheckReentrancy();

                _items.Clear();
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }

            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionReset();
        }

        public void CopyTo(T[] array, int index)
        {
            _itemsLocker.EnterReadLock();

            try
            {
                _items.CopyTo(array, index);
            }
            finally
            {
                _itemsLocker.ExitReadLock();
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            _itemsLocker.EnterReadLock();

            try
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array", "'array' cannot be null");
                }

                if (array.Rank != 1)
                {
                    throw new ArgumentException("Multidimension arrays are not supported", "array");
                }

                if (array.GetLowerBound(0) != 0)
                {
                    throw new ArgumentException("Non-zero lower bound arrays are not supported", "array");
                }

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", "'index' is out of range");
                }

                if (array.Length - index < _items.Count)
                {
                    throw new ArgumentException("Array is too small");
                }

                var tArray = array as T[];
                if (tArray != null)
                {
                    _items.CopyTo(tArray, index);
                }
                else
                {
                    //
                    // Catch the obvious case assignment will fail.
                    // We can found all possible problems by doing the check though.
                    // For example, if the element type of the Array is derived from T,
                    // we can't figure out if we can successfully copy the element beforehand.
                    //
                    var targetType = array.GetType().GetElementType();
                    var sourceType = typeof (T);
                    if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                    {
                        throw new ArrayTypeMismatchException("Invalid array type");
                    }

                    //
                    // We can't cast array of value type to object[], so we don't support 
                    // widening of primitive types here.
                    //
                    var objects = array as object[];
                    if (objects == null)
                    {
                        throw new ArrayTypeMismatchException("Invalid array type");
                    }

                    var count = _items.Count;
                    try
                    {
                        for (var i = 0; i < count; i++)
                        {
                            objects[index++] = _items[i];
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArrayTypeMismatchException("Invalid array type");
                    }
                }
            }
            finally
            {
                _itemsLocker.ExitReadLock();
            }
        }

        public bool Contains(T item)
        {
            _itemsLocker.EnterReadLock();

            try
            {
                return _items.Contains(item);
            }
            finally
            {
                _itemsLocker.ExitReadLock();
            }
        }

        bool IList.Contains(object value)
        {
            if (IsCompatibleObject(value))
            {
                _itemsLocker.EnterReadLock();

                try
                {
                    return _items.Contains((T) value);
                }
                finally
                {
                    _itemsLocker.ExitReadLock();
                }
            }

            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            _itemsLocker.EnterReadLock();

            try
            {
                return _items.ToList().GetEnumerator();
            }
            finally
            {
                _itemsLocker.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            _itemsLocker.EnterReadLock();

            try
            {
                return ((IEnumerable) _items.ToList()).GetEnumerator();
            }
            finally
            {
                _itemsLocker.ExitReadLock();
            }
        }

        public int IndexOf(T item)
        {
            _itemsLocker.EnterReadLock();

            try
            {
                return _items.IndexOf(item);
            }
            finally
            {
                _itemsLocker.ExitReadLock();
            }
        }

        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value))
            {
                _itemsLocker.EnterReadLock();

                try
                {
                    return _items.IndexOf((T) value);
                }
                finally
                {
                    _itemsLocker.ExitReadLock();
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            _itemsLocker.EnterWriteLock();

            try
            {
                CheckIsReadOnly();
                CheckIndex(index);
                CheckReentrancy();

                _items.Insert(index, item);
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }

            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        void IList.Insert(int index, object value)
        {
            try
            {
                Insert(index, (T) value);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("'value' is the wrong type");
            }
        }

        public bool Remove(T item)
        {
            int index;
            T value;

            _itemsLocker.EnterWriteLock();

            try
            {
                CheckIsReadOnly();
                CheckReentrancy();

                index = _items.IndexOf(item);

                if (index < 0)
                {
                    return false;
                }

                value = _items[index];

                _items.RemoveAt(index);
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }

            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, value, index);

            return true;
        }

        void IList.Remove(object value)
        {
            if (IsCompatibleObject(value))
            {
                Remove((T) value);
            }
        }

        public void RemoveAt(int index)
        {
            T value;

            _itemsLocker.EnterWriteLock();

            try
            {
                CheckIsReadOnly();
                CheckIndex(index);
                CheckReentrancy();

                value = _items[index];

                _items.RemoveAt(index);
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }

            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, value, index);
        }
        #endregion

        #region SimpleMonitor Class
        private class SimpleMonitor : IDisposable
        {
            private int _busyCount;

            public bool Busy => _busyCount > 0;

            public void Enter()
            {
                ++_busyCount;
            }

            public void Dispose()
            {
                --_busyCount;
            }
        }
        #endregion
    }
}

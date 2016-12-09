using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace CCSWE.Collections.Generic
{
    /// <summary>
    /// A thread safe <see cref="Queue{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects contained in the <see cref="ThreadSafeQueue{T}"/></typeparam>
    public class ThreadSafeQueue<T> : IEnumerable<T>, ICollection
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeQueue{T}"/> class that is empty and has a default initial capacity.
        /// </summary>
        public ThreadSafeQueue()
        {
            _queue = new Queue<T>();
        }

        /// <summary>
        /// Initialized a new instance of the <see cref="ThreadSafeQueue{T}"/> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Generic.Queue`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="collection" /> is null.</exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ThreadSafeQueue(IEnumerable<T> collection)
        {
            Ensure.IsNotNull(nameof(collection), collection);

            _queue = new Queue<T>(collection);
        }

        /// <summary>Initializes a new instance of the <see cref="ThreadSafeQueue{T}" /> class that is empty and has the specified initial capacity.</summary>
        /// <param name="capacity">The initial number of elements that the <see cref="ThreadSafeQueue{T}" /> can contain.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="capacity" /> is less than zero.</exception>
        public ThreadSafeQueue(int capacity)
        {
            Ensure.IsInRange(nameof(capacity), capacity >= 0);

            _queue = new Queue<T>(capacity);
        }
        #endregion

        #region Private Fields
        private readonly ReaderWriterLockSlim _itemsLocker = new ReaderWriterLockSlim();
        private readonly Queue<T> _queue;
        [NonSerialized] private object _syncRoot;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the number of items contained in the <see cref="ThreadSafeQueue{T}"/>
        /// </summary>
        public int Count
        {
            get
            {
                _itemsLocker.EnterReadLock();

                try
                {
                    return _queue.Count;
                }
                finally
                {
                    _itemsLocker.ExitReadLock();
                }
            }
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                }

                return _syncRoot;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Removes all objects from the <see cref="ThreadSafeQueue{T}" />.
        /// </summary>
        public void Clear()
        {
            _itemsLocker.EnterWriteLock();

            try
            {
                _queue.Clear();
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }
        }

        /// <summary>Determines whether an element is in the <see cref="ThreadSafeQueue{T}" />.</summary>
        /// <returns>true if <paramref name="item" /> is found in the <see cref="ThreadSafeQueue{T}" />; otherwise, false.</returns>
        /// <param name="item">The object to locate in the <see cref="ThreadSafeQueue{T}" />. The value can be null for reference types.</param>
        public bool Contains(T item)
        {
            _itemsLocker.EnterReadLock();

            try
            {
                return _queue.Contains(item);
            }
            finally
            {
                _itemsLocker.ExitReadLock();
            }
        }

        /// <summary>Copies the <see cref="ThreadSafeQueue{T}" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="ThreadSafeQueue{T}" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex" /> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="ThreadSafeQueue{T}" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Ensure.IsNotNull(nameof(array), array);
            Ensure.IsInRange(nameof(arrayIndex), arrayIndex >= 0 && arrayIndex < array.Length);
            Ensure.IsValid<ArgumentException>(nameof(arrayIndex), array.Length - arrayIndex >= Count, "Invalid offset length.");

            _itemsLocker.EnterReadLock();

            try
            {
                _queue.CopyTo(array, arrayIndex);
            }
            finally
            {
                _itemsLocker.ExitReadLock();
            }

        }

        //TODO: ThreadSafeQueue<T> ICollection.CopyTo(Array array, int index) - Implement
        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException("Hope you didn't need this :)");
            //if (array == null)
            //    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            //if (array.Rank != 1)
            //    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            //if (array.GetLowerBound(0) != 0)
            //    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
            //int length1 = array.Length;
            //if (index < 0 || index > length1)
            //    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
            //if (length1 - index < this._size)
            //    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
            //int num = length1 - index < this._size ? length1 - index : this._size;
            //if (num == 0)
            //    return;
            //try
            //{
            //    int length2 = this._array.Length - this._head < num ? this._array.Length - this._head : num;
            //    Array.Copy((Array)this._array, this._head, array, index, length2);
            //    int length3 = num - length2;
            //    if (length3 <= 0)
            //        return;
            //    Array.Copy((Array)this._array, 0, array, index + this._array.Length - this._head, length3);
            //}
            //catch (ArrayTypeMismatchException ex)
            //{
            //    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
            //}
        }

        /// <summary>Removes and returns the object at the beginning of the <see cref="ThreadSafeQueue{T}" />.</summary>
        /// <returns>The object that is removed from the beginning of the <see cref="ThreadSafeQueue{T}" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="ThreadSafeQueue{T}" /> is empty.</exception>
        public T Dequeue()
        {
            if (Count <= 0)
            {
                // TODO: Can I put this into the Ensure class?
                throw new InvalidOperationException();
            }

            _itemsLocker.EnterWriteLock();

            try
            {
                return _queue.Dequeue();
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }
        }

        /// <summary>Adds an object to the end of the <see cref="ThreadSafeQueue{T}" />.</summary>
        /// <param name="item">The object to add to the <see cref="ThreadSafeQueue{T}" />. The value can be null for reference types.</param>
        public void Enqueue(T item)
        {

            _itemsLocker.EnterWriteLock();

            try
            {
                _queue.Enqueue(item);
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }
        }

        // TODO: Add XmlDoc
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void EnqueueRange(IEnumerable<T> items)
        {
            Ensure.IsNotNull(nameof(items), items);

            _itemsLocker.EnterWriteLock();

            try
            {
                foreach (var item in items)
                {
                    _queue.Enqueue(item);
                }
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }
        }

        // TODO: ThreadSafeQueue<T> - Return a copy?
        // TODO: Add XmlDoc
        public Queue<T>.Enumerator GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        // ReSharper disable RedundantCast
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (IEnumerator<T>) GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }
        // ReSharper restore RedundantCast

        /// <summary>Returns the object at the beginning of the <see cref="ThreadSafeQueue{T}" />.</summary>
        /// <returns>The object at the beginning of the <see cref="ThreadSafeQueue{T}" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="ThreadSafeQueue{T}" /> is empty.</exception>
        public T Peek()
        {
            if (Count <= 0)
            {
                // TODO: Can I put this into the Ensure class?
                throw new InvalidOperationException();
            }

            _itemsLocker.EnterReadLock();

            try
            {
                return _queue.Peek();
            }
            finally
            {
                _itemsLocker.ExitReadLock();
            }
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="ThreadSafeQueue{T}" />, if that number is less than 90 percent of current capacity.
        /// </summary>
        public void TrimExcess()
        {
            _itemsLocker.EnterWriteLock();

            try
            {
                _queue.TrimExcess();
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }
        }

        // TODO: Add XmlDoc
        public bool TryDequeue(out T item)
        {
            item = default(T);

            _itemsLocker.EnterUpgradeableReadLock();

            try
            {
                if (Count <= 0)
                {
                    return false;
                }

                _itemsLocker.EnterWriteLock();

                try
                {
                    item = _queue.Dequeue();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            finally
            {
                _itemsLocker.ExitUpgradeableReadLock();
            }
        }
        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace CCSWE.Collections.Generic
{
    /// <summary>Represents a first-in, first-out collection of objects that is thread safe.</summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    public class ThreadSafeQueue<T> : IEnumerable<T>, ICollection, IDisposable
    {
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

        private bool _isDisposed;
        private readonly ReaderWriterLockSlim _itemsLocker = new ReaderWriterLockSlim();
        private readonly Queue<T> _queue;
#if NETSTANDARD2_0 || NETFULL
        [NonSerialized] private object _syncRoot;
#else
        private object _syncRoot;
#endif

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

        /// <summary>Gets a value indicating whether access to the <see cref="ThreadSafeQueue{T}" /> is synchronized (thread safe).</summary>
        /// <returns>true if access to the <see cref="ThreadSafeQueue{T}" /> is synchronized (thread safe); otherwise, false.</returns>
        protected bool IsSynchronized => true;

        bool ICollection.IsSynchronized => IsSynchronized;

        /// <summary>Gets an object that can be used to synchronize access to the <see cref="ThreadSafeQueue{T}" />.</summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="ThreadSafeQueue{T}" />.</returns>
        protected object SyncRoot
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

        object ICollection.SyncRoot => SyncRoot;

        /// <summary>
        /// Releases all resources used by the <see cref="ThreadSafeQueue{T}"/>.
        /// </summary>
        /// <param name="disposing">Not used.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            _itemsLocker.Dispose();
            _isDisposed = true;
        }

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
            Ensure.IsValid(nameof(arrayIndex), array.Length - arrayIndex >= Count, "Invalid offset length.");

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

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            Ensure.IsNotNull(nameof(array), array);
            Ensure.IsValid(nameof(array), array.Rank == 1, "Multidimensional array are not supported");
            Ensure.IsValid(nameof(array), array.GetLowerBound(0) == 0, "Non-zero lower bound is not supported");
            Ensure.IsInRange(nameof(arrayIndex), arrayIndex >= 0 && arrayIndex < array.Length);
            Ensure.IsValid(nameof(arrayIndex), array.Length - arrayIndex >= Count, "Invalid offset length.");

            ((ICollection)_queue).CopyTo(array, arrayIndex);
        }

        /// <summary>Removes and returns the object at the beginning of the <see cref="ThreadSafeQueue{T}" />.</summary>
        /// <returns>The object that is removed from the beginning of the <see cref="ThreadSafeQueue{T}" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="ThreadSafeQueue{T}" /> is empty.</exception>
        public T Dequeue()
        {
            Ensure.IsValid<InvalidOperationException>(nameof(Count), Count > 0, $"'{nameof(Count)}' is less than or equal to zero.");

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

        /// <summary>
        /// Releases all resources used by the <see cref="ThreadSafeQueue{T}"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        /// <summary>Adds the elements of the specified collection to the end of the <see cref="ThreadSafeQueue{T}" />.</summary>
        /// <param name="collection">The collection whose elements should be added to the end of the <see cref="ThreadSafeQueue{T}" />. The collection itself cannot be null, but it can contain elements that are null, if type is a reference type.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="collection" /> is null.</exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void EnqueueRange(IEnumerable<T> collection)
        {
            Ensure.IsNotNull(nameof(collection), collection);

            _itemsLocker.EnterWriteLock();
 
           try
            {
                foreach (var item in collection)
                {
                    _queue.Enqueue(item);
                }
            }
            finally
            {
                _itemsLocker.ExitWriteLock();
            }
        }

        /// <summary>Returns an enumerator that iterates through the <see cref="ThreadSafeQueue{T}" />.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.Queue`1.Enumerator" /> for the <see cref="ThreadSafeQueue{T}" />.</returns>
        public Queue<T>.Enumerator GetEnumerator()
        {
            _itemsLocker.EnterReadLock();

            try
            {
                return new Queue<T>(_queue).GetEnumerator();
            }
            finally
            {
                _itemsLocker.ExitReadLock();   
            }
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
            Ensure.IsValid<InvalidOperationException>(nameof(Count), Count > 0, $"'{nameof(Count)}' is less than or equal to zero.");

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

        /// <summary>Tries to remove and return the object at the beginning of the queue.</summary>
        /// <returns><c>true</c> if an element was removed and returned from the beginning of the <see cref="ThreadSafeQueue{T}" /> successfully; otherwise, <c>false</c>.</returns>
        /// <param name="result">When this method returns, if the operation was successful, <paramref name="result" /> contains the object removed. If no object was available to be removed, the value is unspecified.</param>
        public bool TryDequeue(out T result)
        {
            result = default(T);

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
                    result = _queue.Dequeue();
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
    }
}

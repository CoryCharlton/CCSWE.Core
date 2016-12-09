using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace CCSWE.Collections.Generic
{
    //TODO: ThreadSafeQueue<T> - Add xmldoc
    //TODO: ThreadSafeQueue<T> - Change to ReaderWriterLockSlim?
    public class ThreadSafeQueue<T> : IEnumerable<T>, ICollection
    {
        #region Constructor
        public ThreadSafeQueue()
        {
            _queue = new Queue<T>();
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ThreadSafeQueue(IEnumerable<T> collection)
        {
            Ensure.IsNotNull(nameof(collection), collection);

            _queue = new Queue<T>(collection);
        }

        public ThreadSafeQueue(int capacity)
        {
            Ensure.IsInRange(nameof(capacity), capacity >= 0);

            _queue = new Queue<T>(capacity);
        }
        #endregion

        #region Private Fields
        private readonly object _lock = new object();
        private readonly Queue<T> _queue;
        [NonSerialized]
        private object _syncRoot;
        #endregion

        #region Public Properties
        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _queue.Count;
                }
            }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

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
        public void Clear()
        {
            lock (_lock)
            {
                _queue.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _queue.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Ensure.IsNotNull(nameof(array), array);
            Ensure.IsInRange(nameof(arrayIndex), arrayIndex >= 0 && arrayIndex < array.Length);
            Ensure.IsValid<ArgumentException>(nameof(arrayIndex), array.Length - arrayIndex >= Count, "Invalid offset length.");

            lock (_lock)
            {
                _queue.CopyTo(array, arrayIndex);                
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

        public T Dequeue()
        {
            lock (_lock)
            {
                return _queue.Dequeue();
            }
        }

        public void Enqueue(T item)
        {
            lock (_lock)
            {
                _queue.Enqueue(item);
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void EnqueueRange(IEnumerable<T> items)
        {
            Ensure.IsNotNull(nameof(items), items);

            lock (_lock)
            {
                foreach (var item in items)
                {
                    _queue.Enqueue(item);
                }
            }
        }

        //TODO: ThreadSafeQueue<T> - Return a copy?
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

        public T Peek()
        {
            lock (_lock)
            {
                return _queue.Peek();
            }
        }

        public void TrimExcess()
        {
            lock (_lock)
            {
                _queue.TrimExcess();
            }
        }

        public bool TryDequeue(out T item)
        {
            item = default(T);

            lock (_lock)
            {
                if (_queue.Count <= 0)
                {
                    return false;
                }

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
        }
        #endregion
    }
}

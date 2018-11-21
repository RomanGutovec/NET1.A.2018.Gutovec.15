using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueLib
{
    /// <summary>
    /// Represents a first-in, first-out collection of objects. 
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class Queue<T> : IEnumerable<T>, IEnumerable
    {
        private const int startCapasity = 4;
        private const int growFactor = 2;
        private T[] sourceArray;
        private int head;
        private int tail;
        private int size;
        private int version;

        /// <summary>
        /// Initializes a new instance of the Queue class that is empty,
        /// has the default initial capacity.
        /// </summary>
        public Queue()
        {
            sourceArray = new T[startCapasity];
        }

        /// <summary>
        /// Initializes a new instance of the queue class that is empty,
        /// has the specified initial capacity.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when capacity has incorrect value</exception>
        /// <param name="capacity">The initial number of elements.</param>
        public Queue(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException($"Capasity {nameof(capacity)} of queue must be more then 0");
            }

            sourceArray = new T[capacity];
        }

        /// <summary>
        /// Initializes a new instance of the Queue class that contains
        /// elements copied from the specified collection, has the same initial capacity
        /// as the number of elements copied.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when collection is null</exception>
        /// <param name="collection">Collection to copy elements from.</param>
        public Queue(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException($"Collection {nameof(collection)} haves null value");
            }

            sourceArray = new T[startCapasity];
            IEnumerator<T> enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Enqueue(enumerator.Current);
            }
        }

        /// <summary>
        /// Returns amount of elements in queue.
        /// </summary>
        public int Count
        {
            get { return size; }
        }

        /// <summary>
        /// Adds an element to the end of the Queue.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when element to add is null</exception>
        /// <param name="item">Element to add.</param>
        public void Enqueue(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException($"Element {nameof(item)} haves null value");
            }

            if (size >= sourceArray.Length)
            {
                ChangeCapasity(sourceArray.Length * growFactor);
            }

            sourceArray[tail++] = item;
            version++;
            size++;
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the Queue.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when queue is empty</exception>
        /// <returns>Object at the beginning of the Queue.</returns>
        public T Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue is empty");
            }

            if (sourceArray.Length > size * growFactor)
            {
                ChangeCapasity(sourceArray.Length / growFactor);
            }

            T resultValue = sourceArray[head];
            sourceArray[head] = default(T);
            size--;
            version++;
            head++;
            return resultValue;
        }

        /// <summary>
        /// Removes all Objects from the queue.
        /// </summary>
        public void Clear()
        {
            Array.Clear(sourceArray, 0, sourceArray.Length);
            head = 0;
            tail = 0;
            size = 0;
            version++;
        }

        /// <summary>
        /// Returns the object at the beginning of the Queue without removing
        /// it.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when queue is empty</exception>
        /// <returns></returns>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            return sourceArray[head];
        }

        /// <summary>
        /// Returns true if the queue contains at least one element equal to element in queue.
        /// </summary>
        /// <param name="item">Element to find</param>
        /// <returns>true if contains, false in opposite situation</returns>
        public bool Contains(T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < Count; i++)
            {
                if (item == null)
                {
                    if (sourceArray[i] == null)
                    {
                        return true;
                    }
                }
                else if (sourceArray[i] != null && comparer.Equals(sourceArray[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the Queue.
        /// </summary>
        /// <returns>Enumerator that iterates through the Queue.</returns>
        public Enumerator GetEnumerator()
        => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

        /// <summary>
        /// Copy elements of the queue to the array from the index.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index have incorrect value</exception>
        /// <param name="array">Array to copy elements</param>
        /// <param name="index">Index of array from which starts copying</param>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException($"Array {nameof(array)} haves null value");
            }

            if (index < 0 || array.Length - 1 - index > size)
            {
                throw new ArgumentOutOfRangeException($"Incorrect value of {nameof(index)}");
            }

            T[] resultArray = new T[Count];
            resultArray = sourceArray.Skip<T>(head).Take<T>(tail - head).ToArray<T>();
            resultArray.CopyTo(array, index);
        }

        private bool IsEmpty()
            => Count == 0;

        private void ChangeCapasity(int newCapasity)
        {
            T[] newSourceArray = new T[newCapasity];
            Array.Copy(sourceArray, head, newSourceArray, 0, tail - head);
            sourceArray = newSourceArray;
            head = 0;
            tail = size;
        }

        /// <summary>
        /// Implements an enumerator for a Queue.
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private readonly int version;
            private readonly Queue<T> collection;
            private int currentIndex;

            public Enumerator(Queue<T> collection)
            {
                this.collection = collection;
                currentIndex = -1;
                version = collection.version;
            }

            /// <summary>
            /// Returns current value
            /// <exceptions cref="InvalidOperationException">Thrown when index take incorrect value</exceptions>
            /// </summary>
            public T Current
            {
                get
                {
                    if ((currentIndex == -1) || (currentIndex == collection.Count))
                    {
                        throw new InvalidOperationException($"You should move or moving is imposible because of end of queue");
                    }

                    return collection.sourceArray[currentIndex];
                }
            }

            object IEnumerator.Current => Current;

            void IDisposable.Dispose()
            {
            }

            /// <summary>
            /// Check opportunity to move to another element
            /// </summary>
            /// <returns>true if moving is possible</returns>
            public bool MoveNext()
            {
                if (version != collection.version)
                {
                    throw new InvalidOperationException("Collection was modified");
                }

                return ++currentIndex < collection.Count;
            }

            /// <summary>
            /// Reset current state
            /// </summary>
            public void Reset()
            {
                currentIndex = -1;
            }
        }
    }
}

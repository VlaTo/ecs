using System;
using System.Collections.Generic;
using System.Threading;

namespace ClassLibrary1.Core
{
    internal sealed class PriorityQueue<T>
        where T : IComparable<T>
    {
        private static long count = long.MinValue;

        private IndexedItem[] items;
        private int size;

        public int Count => size;

        public PriorityQueue()
            : this(16)
        {
        }

        public PriorityQueue(int capacity)
        {
            items = new IndexedItem[capacity];
            size = 0;
        }

        private bool IsHigherPriority(int left, int right) => 0 > items[left].CompareTo(items[right]);

        private void Percolate(int index)
        {
            if (index >= size || index < 0)
            {
                return;
            }

            var parent = (index - 1) / 2;

            if (parent < 0 || parent == index)
            {
                return;
            }

            if (IsHigherPriority(index, parent))
            {
                var temp = items[index];
                items[index] = items[parent];
                items[parent] = temp;
                Percolate(parent);
            }
        }

        private void Heapify()
        {
            Heapify(0);
        }

        private void Heapify(int index)
        {
            if (index >= size || index < 0)
            {
                return;
            }

            var left = 2 * index + 1;
            var right = 2 * index + 2;
            var first = index;

            if (left < size && IsHigherPriority(left, first))
            {
                first = left;
            }

            if (right < size && IsHigherPriority(right, first))
            {
                first = right;
            }

            if (first != index)
            {
                var temp = items[index];

                items[index] = items[first];
                items[first] = temp;

                Heapify(first);
            }
        }

        public T Peek()
        {
            if (0 == size)
            {
                throw new InvalidOperationException("HEAP is Empty");
            }

            return items[0].Value;
        }

        private void RemoveAt(int index)
        {
            items[index] = items[--size];
            items[size] = default(IndexedItem);

            Heapify();

            if (size < items.Length / 4)
            {
                var temp = items;
                items = new IndexedItem[items.Length / 2];
                Array.Copy(temp, 0, items, 0, size);
            }
        }

        public T Dequeue()
        {
            var result = Peek();
            RemoveAt(0);
            return result;
        }

        public void Enqueue(T item)
        {
            if (size >= items.Length)
            {
                var temp = items;
                items = new IndexedItem[items.Length * 2];
                Array.Copy(temp, items, temp.Length);
            }

            var index = size++;

            items[index] = new IndexedItem { Value = item, Id = Interlocked.Increment(ref count) };

            Percolate(index);
        }

        public bool Remove(T item)
        {
            for (var i = 0; i < size; ++i)
            {
                if (EqualityComparer<T>.Default.Equals(items[i].Value, item))
                {
                    RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private struct IndexedItem : IComparable<IndexedItem>
        {
            public T Value;
            public long Id;

            public int CompareTo(IndexedItem other)
            {
                var c = Value.CompareTo(other.Value);

                if (c == 0)
                {
                    c = Id.CompareTo(other.Id);
                }

                return c;
            }
        }
    }
}
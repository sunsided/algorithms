using System.Collections.Generic;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// A min-heap.
    /// </summary>
    /// <typeparam name="T">The type of the heap elements.</typeparam>
    public sealed class MinHeap<T> : Heap<T>
        where T : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinHeap{T}"/> class.
        /// </summary>
        /// <param name="initialCapacity">The initial capacity of the heap.</param>
        /// <param name="comparer">An optional comparer for the items.</param>
        public MinHeap([ValueRange(0, int.MaxValue)] int initialCapacity = 0, IComparer<T>? comparer = default)
            : base(ItemWeight.NegativeInfinity, initialCapacity, comparer)
        {
        }

        /// <summary>
        /// Changes the value of the <paramref name="i"/>-th element.
        /// </summary>
        /// <param name="i">The index of the item to change.</param>
        /// <param name="value">The new value.</param>
        public override void ChangeValue(int i, in T value)
        {
            var oldValue = this[i];
            this[i] = value;
            if (IsSmaller(value, oldValue))
            {
                SiftUp(i);
            }
            else
            {
                SiftDown(i);
            }
        }

        /// <summary>
        /// Sifts the item with the <paramref name="i"/>-th index up.
        /// </summary>
        /// <param name="i">The index of the item to sift up.</param>
        protected override void SiftUp(int i)
        {
            // Nothing to do if the item is the root or already smaller than or equal to the parent.
            while (!IsRoot(i) && IsGreater(Parent(i), i))
            {
                Swap(Parent(i), i);
                i = Parent(i);
            }
        }

        /// <summary>
        /// Sifts the item with the <paramref name="i"/>-th index down.
        /// </summary>
        /// <param name="i">The index of the item to sift down.</param>
        protected override void SiftDown(int i)
        {
            while (true)
            {
                var minIndex = i;
                var l = LeftChild(i);
                var hasLeftChild = l < Count;
                if (hasLeftChild && IsSmaller(l, minIndex))
                {
                    minIndex = l;
                }

                var r = RightChild(i);
                var hasRightChild = r < Count;
                if (hasRightChild && IsSmaller(r, minIndex))
                {
                    minIndex = r;
                }

                if (i == minIndex) return;

                Swap(i, minIndex);
                i = minIndex;
            }
        }
    }
}
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// A max-heap.
    /// </summary>
    /// <typeparam name="T">The type of the heap elements.</typeparam>
    public sealed class MaxHeap<T> : Heap<T>
        where T : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaxHeap{T}"/> class.
        /// </summary>
        /// <param name="initialCapacity">The initial capacity of the heap.</param>
        /// <param name="comparer">An optional comparer for the items.</param>
        public MaxHeap([ValueRange(0, int.MaxValue)] int initialCapacity = 0, IComparer<T>? comparer = default)
            : base(ItemWeight.PositiveInfinity, initialCapacity, comparer)
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
            if (IsGreater(value, oldValue))
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
            while (!IsRoot(i) && IsSmaller(Parent(i), i))
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
                var maxIndex = i;
                var l = LeftChild(i);
                var hasLeftChild = l < Count;
                if (hasLeftChild && IsGreater(l, maxIndex))
                {
                    maxIndex = l;
                }

                var r = RightChild(i);
                var hasRightChild = r < Count;
                if (hasRightChild && IsGreater(r, maxIndex))
                {
                    maxIndex = r;
                }

                if (i == maxIndex) return;

                Swap(i, maxIndex);
                i = maxIndex;
            }
        }
    }
}
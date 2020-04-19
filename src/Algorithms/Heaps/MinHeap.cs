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
            while (ParentIsGreater(i))
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
            // Starting from the i'th item, we will select the smallest child
            // and sift the starting value down that path by swapping the items.
            // If we find no child to swap with, we stop.
            while (true)
            {
                var hasSmallerChild = TryFindSmallerChild(i, out var childIndex);
                if (!hasSmallerChild) return;

                Swap(i, childIndex);
                i = childIndex;
            }
        }

        /// <summary>
        /// Determines whether the parent of the <paramref name="i"/>-th item is greater
        /// than the <paramref name="i"/>-th item itself.
        /// </summary>
        /// <param name="i">The index of the item to compare with its parent.</param>
        /// <returns><see langword="true"/> if the parent is greater; <see langword="false"/> otherwise.</returns>
        private bool ParentIsGreater(int i) =>
            !IsRoot(i) && IsGreater(Parent(i), i);

        /// <summary>
        /// Finds the child that is greater than the item at the <paramref name="i"/>-th index.
        /// </summary>
        /// <param name="i">The root node.</param>
        /// <param name="childIndex">The index of the greater child; only meaningful if the method evaluates to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if a greater child exists; <see langword="false"/> otherwise.</returns>
        private bool TryFindSmallerChild(int i, out int childIndex)
        {
            childIndex = i;
            var hasLeftChild = TryGetLeftChild(i, out var leftChildIndex);
            var hasRightChild = TryGetRightChild(i, out var rightChildIndex);

            // Test if the left child is smaller than the current item.
            if (hasLeftChild && IsSmaller(leftChildIndex, childIndex))
            {
                childIndex = leftChildIndex;
            }

            // Test if the right child is smaller than the currently smallest item
            if (hasRightChild && IsSmaller(rightChildIndex, childIndex))
            {
                childIndex = rightChildIndex;
            }

            return childIndex != i;
        }
    }
}
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
            while (ParentIsSmaller(i))
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
            // Starting from the i'th item, we will select the greatest child
            // and sift the starting value down that path by swapping the items.
            // If we find no child to swap with, we stop.
            while (true)
            {
                var hasGreaterChild = TryFindGreaterChild(i, out var childIndex);
                if (!hasGreaterChild) return;

                Swap(i, childIndex);
                i = childIndex;
            }
        }

        /// <summary>
        /// Determines whether the parent of the <paramref name="i"/>-th item is smaller
        /// than the <paramref name="i"/>-th item itself.
        /// </summary>
        /// <param name="i">The index of the item to compare with its parent.</param>
        /// <returns><see langword="true"/> if the parent is smaller; <see langword="false"/> otherwise.</returns>
        private bool ParentIsSmaller(int i) =>
            !IsRoot(i) && IsSmaller(Parent(i), i);

        /// <summary>
        /// Finds the child that is greater than the item at the <paramref name="i"/>-th index.
        /// </summary>
        /// <param name="i">The root node.</param>
        /// <param name="childIndex">The index of the greater child; only meaningful if the method evaluates to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if a greater child exists; <see langword="false"/> otherwise.</returns>
        private bool TryFindGreaterChild(int i, out int childIndex)
        {
            childIndex = i;
            var hasLeftChild = TryGetLeftChild(i, out var leftChildIndex);
            var hasRightChild = TryGetRightChild(i, out var rightChildIndex);

            // Test if the left child is greater than the current item.
            if (hasLeftChild && IsGreater(leftChildIndex, childIndex))
            {
                childIndex = leftChildIndex;
            }

            // Test if the right child is greater than the currently greatest item
            if (hasRightChild && IsGreater(rightChildIndex, childIndex))
            {
                childIndex = rightChildIndex;
            }

            return childIndex != i;
        }
    }
}
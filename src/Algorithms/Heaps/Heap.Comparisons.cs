using System;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// A max-heap.
    /// </summary>
    /// <typeparam name="T">The type of the heap elements.</typeparam>
    public abstract partial class Heap<T>
    {
        /// <summary>
        /// Compares two items.
        /// </summary>
        /// <param name="lhs">The left item.</param>
        /// <param name="rhs">The right item.</param>
        /// <returns><c>-1</c> if <paramref name="lhs"/> is smaller than <paramref name="rhs"/>; <c>0</c> if both items are equal and <c>+1</c> if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.</returns>
        /// <seealso cref="IComparable.CompareTo"/>
        private int Compare(in HeapItem lhs, in HeapItem rhs)
        {
            // In this method, we're not adjusting the maximum token for min/max heap
            // differences, but only use adjusted values of actual item comparisons.
            // The side effect is this:
            //
            // During item removals, the maximum token always compares as greater than
            // any real value. Since we compare for ParentIsSmaller() in the SiftUp()
            // method (without adjusting for a min heap) the parent is _always_ smaller
            // than the max token. As a result, the max token always bubbles up
            // where needed.

            var value = lhs.IsMaximum.CompareTo(rhs.IsMaximum);
            if (value == 0) value = Compare(lhs.Value, rhs.Value);
            return value;
        }

        /// <summary>
        /// Compares two items.
        /// </summary>
        /// <param name="lhs">The left item.</param>
        /// <param name="rhs">The right item.</param>
        /// <returns><c>-1</c> if <paramref name="lhs"/> is smaller than <paramref name="rhs"/>; <c>0</c> if both items are equal and <c>+1</c> if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.</returns>
        /// <seealso cref="IComparable.CompareTo"/>
        private int Compare(in T lhs, in T rhs) =>
            _comparer.Compare(lhs, rhs) * (int)_heapType;

        /// <summary>
        /// Determines whether an item is strictly greater than another one.
        /// </summary>
        /// <param name="lhs">The left item's index.</param>
        /// <param name="rhs">The right item's index.</param>
        /// <returns><see langword="true"/> if the left item is greater than the right one; <see langword="false"/> otherwise.</returns>
        private bool IsGreater(in T lhs, in T rhs) =>
            Compare(lhs, rhs) > 0;

        /// <summary>
        /// Determines whether an item is strictly greater than another one.
        /// </summary>
        /// <param name="lhs">The left item's index.</param>
        /// <param name="rhs">The right item's index.</param>
        /// <returns><see langword="true"/> if the left item is greater than the right one; <see langword="false"/> otherwise.</returns>
        private bool IsGreater([ValueRange(0, int.MaxValue)] int lhs, [ValueRange(0, int.MaxValue)] int rhs) =>
            Compare(_heap[lhs], _heap[rhs]) > 0;

        /// <summary>
        /// Determines whether an item is strictly smaller than another one.
        /// </summary>
        /// <param name="lhs">The left item's index.</param>
        /// <param name="rhs">The right item's index.</param>
        /// <returns><see langword="true"/> if the left item is smaller than the right one; <see langword="false"/> otherwise.</returns>
        private bool IsSmaller([ValueRange(0, int.MaxValue)] int lhs, [ValueRange(0, int.MaxValue)] int rhs) =>
            Compare(_heap[lhs], _heap[rhs]) < 0;
    }
}
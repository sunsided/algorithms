using System;
using System.Diagnostics;
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
        /// Determines whether an item is strictly greater than another one.
        /// </summary>
        /// <param name="lhs">The left item's index.</param>
        /// <param name="rhs">The right item's index.</param>
        /// <returns><see langword="true"/> if the left item is greater than the right one; <see langword="false"/> otherwise.</returns>
        [DebuggerStepThrough]
        internal bool IsGreater(in T lhs, in T rhs) =>
            Compare(lhs, rhs) > 0;

        /// <summary>
        /// Determines whether an item is strictly greater than another one.
        /// </summary>
        /// <param name="lhs">The left item's index.</param>
        /// <param name="rhs">The right item's index.</param>
        /// <returns><see langword="true"/> if the left item is greater than the right one; <see langword="false"/> otherwise.</returns>
        [DebuggerStepThrough]
        protected bool IsGreater([ValueRange(0, int.MaxValue)] int lhs, [ValueRange(0, int.MaxValue)] int rhs) =>
            Compare(_heap[lhs], _heap[rhs]) > 0;

        /// <summary>
        /// Determines whether an item is strictly smaller than another one.
        /// </summary>
        /// <param name="lhs">The left item.</param>
        /// <param name="rhs">The right item.</param>
        /// <returns><see langword="true"/> if the left item is smaller than the right one; <see langword="false"/> otherwise.</returns>
        [DebuggerStepThrough]
        internal bool IsSmaller(in T lhs, in T rhs) =>
            Compare(lhs, rhs) < 0;

        /// <summary>
        /// Determines whether an item is strictly smaller than another one.
        /// </summary>
        /// <param name="lhs">The left item's index.</param>
        /// <param name="rhs">The right item's index.</param>
        /// <returns><see langword="true"/> if the left item is smaller than the right one; <see langword="false"/> otherwise.</returns>
        [DebuggerStepThrough]
        protected bool IsSmaller([ValueRange(0, int.MaxValue)] int lhs, [ValueRange(0, int.MaxValue)] int rhs) =>
            Compare(_heap[lhs], _heap[rhs]) < 0;

        /// <summary>
        /// Compares two items.
        /// </summary>
        /// <param name="lhs">The left item.</param>
        /// <param name="rhs">The right item.</param>
        /// <returns><c>-1</c> if <paramref name="lhs"/> is smaller than <paramref name="rhs"/>; <c>0</c> if both items are equal and <c>+1</c> if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.</returns>
        /// <seealso cref="IComparable.CompareTo"/>
        private int Compare(in T lhs, in T rhs) =>
            _comparer.Compare(lhs, rhs);

        /// <summary>
        /// Compares two items.
        /// </summary>
        /// <param name="lhs">The left item.</param>
        /// <param name="rhs">The right item.</param>
        /// <returns><c>-1</c> if <paramref name="lhs"/> is smaller than <paramref name="rhs"/>; <c>0</c> if both items are equal and <c>+1</c> if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.</returns>
        /// <seealso cref="IComparable.CompareTo"/>
        private int Compare(in HeapItem lhs, in HeapItem rhs)
        {
            var value = lhs.Polarity.CompareTo(rhs.Polarity);
            if (value == 0) value = _comparer.Compare(lhs.Value, rhs.Value);
            return value;
        }
    }
}
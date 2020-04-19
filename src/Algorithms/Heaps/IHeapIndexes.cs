using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// Item index calculations for heaps.
    /// </summary>
    public interface IHeapIndexes<T> : IHeap<T>
        where T : notnull
    {
        /// <summary>
        /// Determines whether the item determined by the <paramref name="i"/>-th index is the root element.
        /// </summary>
        /// <remarks>
        ///     This is only ever the case if <paramref name="i"/> is <c>0</c>.
        /// </remarks>
        /// <param name="i">The index of the item.</param>
        /// <returns><see langword="true"/> when <paramref name="i"/> is <c>0</c>; <see langword="false"/> otherwise.</returns>
        [Pure]
        bool IsRoot([ValueRange(0, int.MaxValue)] int i);

        /// <summary>
        /// Calculates the index of the parent of the <paramref name="i"/>-th item.
        /// </summary>
        /// <param name="i">The index of the item whose parent should be determined.</param>
        /// <returns>The index of the parent item.</returns>
        [Pure]
        int Parent([ValueRange(0, int.MaxValue)] int i);

        /// <summary>
        /// Calculates the index of the left child of the <paramref name="i"/>-th item.
        /// </summary>
        /// <param name="i">The index of the item whose left child should be determined.</param>
        /// <returns>The index of the left child item.</returns>
        [Pure]
        int LeftChild([ValueRange(0, int.MaxValue)] int i);

        /// <summary>
        /// Calculates the index of the right child of the <paramref name="i"/>-th item.
        /// </summary>
        /// <param name="i">The index of the item whose right child should be determined.</param>
        /// <returns>The index of the right child item.</returns>
        [Pure]
        int RightChild([ValueRange(0, int.MaxValue)] int i);
    }
}

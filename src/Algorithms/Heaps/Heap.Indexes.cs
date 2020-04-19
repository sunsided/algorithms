using System.Diagnostics;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// A max-heap.
    /// </summary>
    /// <typeparam name="T">The type of the heap elements.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public abstract partial class Heap<T>
    {
        /// <summary>
        /// Calculates the index of the parent of the <paramref name="i"/>-th item.
        /// </summary>
        /// <param name="i">The index of the item whose parent should be determined.</param>
        /// <returns>The index of the parent item or <c>-1</c> if the item is the root element (at <c>0</c>).</returns>
        [DebuggerStepThrough]
        [ValueRange(-1, int.MaxValue)]
        public int Parent([ValueRange(0, int.MaxValue)] int i) => (i - 1) / 2;

        /// <summary>
        /// Calculates the index of the left child of the <paramref name="i"/>-th item.
        /// </summary>
        /// <param name="i">The index of the item whose left child should be determined.</param>
        /// <returns>The index of the left child item.</returns>
        [DebuggerStepThrough]
        [ValueRange(1, int.MaxValue)]
        public int LeftChild([ValueRange(0, int.MaxValue)] int i) => 2 * i + 1;

        /// <summary>
        /// Calculates the index of the right child of the <paramref name="i"/>-th item.
        /// </summary>
        /// <param name="i">The index of the item whose right child should be determined.</param>
        /// <returns>The index of the right child item.</returns>
        [DebuggerStepThrough]
        [ValueRange(2, int.MaxValue)]
        public int RightChild([ValueRange(0, int.MaxValue)] int i) => 2 * i + 2;

        /// <summary>
        /// Determines whether the item determined by the <paramref name="i"/>-th index is the root element.
        /// </summary>
        /// <remarks>
        ///     This is only ever the case if <paramref name="i"/> is <c>0</c>.
        /// </remarks>
        /// <param name="i">The index of the item.</param>
        /// <returns><see langword="true"/> when <paramref name="i"/> is <c>0</c>; <see langword="false"/> otherwise.</returns>
        [DebuggerStepThrough]
        public bool IsRoot([ValueRange(0, int.MaxValue)] int i) => i == 0;
    }
}
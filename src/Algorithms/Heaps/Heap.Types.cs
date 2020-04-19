using System;
using System.Diagnostics;

namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// A max-heap.
    /// </summary>
    /// <typeparam name="T">The type of the heap elements.</typeparam>
    public abstract partial class Heap<T>
    {
        /// <summary>
        /// The type of a heap.
        /// </summary>
        protected enum HeapType
        {
            /// <summary>
            /// Min Heap.
            /// </summary>
            MinHeap = -1,

            /// <summary>
            /// Max Heap.
            /// </summary>
            MaxHeap = 1
        }

        /// <summary>
        /// Struct representing an item in the heap.
        /// </summary>
        private readonly struct HeapItem
        {
            public readonly bool IsMaximum;
            public readonly T Value;

            /// <summary>
            /// Initializes an instance of the <see cref="HeapItem"/> struct.
            /// </summary>
            /// <param name="value">The item value.</param>
            public HeapItem(T value)
            {
                IsMaximum = false;
                Value = value;
            }

            /// <summary>
            /// Initializes an instance of the <see cref="HeapItem"/> struct.
            /// </summary>
            /// <param name="isMaximum">Determines whether this is the maximum token.</param>
            public HeapItem(bool isMaximum)
            {
                IsMaximum = isMaximum;
                Value = default!;
            }

            /// <inheritdoc />
            public override string ToString() => IsMaximum ? "Special" : Value.ToString();

            /// <inheritdoc />
            public override int GetHashCode() => HashCode.Combine(IsMaximum, Value);
        }
    }
}
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
        /// The weight of an item in a heap.
        /// </summary>
        /// <remarks>
        ///    Used to sift up items that are to be removed.
        /// </remarks>
        protected enum ItemWeight
        {
            /// <summary>
            /// The item has the smallest possible value.
            /// </summary>
            NegativeInfinity = -1,

            /// <summary>
            /// The item has the largest possible value.
            /// </summary>
            PositiveInfinity = 1
        }

        /// <summary>
        /// Struct representing an item in the heap.
        /// </summary>
        private readonly struct HeapItem
        {
            public readonly int Polarity;
            public readonly T Value;

            /// <summary>
            /// Initializes an instance of the <see cref="HeapItem"/> struct.
            /// </summary>
            /// <param name="value">The item value.</param>
            public HeapItem(T value)
            {
                Polarity = 0;
                Value = value;
            }

            /// <summary>
            /// Initializes an instance of the <see cref="HeapItem"/> struct.
            /// </summary>
            /// <param name="weight">A weight of the item.</param>
            public HeapItem(ItemWeight weight)
            {
                Debug.Assert(weight == ItemWeight.NegativeInfinity || weight == ItemWeight.PositiveInfinity, "weight == ItemWeight.NegativeInfinity || weight == ItemWeight.PositiveInfinity");
                Polarity = (int) weight;
                Value = default!;
            }

            /// <inheritdoc />
            public override string ToString() => Polarity != 0 ? "Special" : Value.ToString();

            /// <inheritdoc />
            public override int GetHashCode() => HashCode.Combine(Polarity, Value);
        }
    }
}
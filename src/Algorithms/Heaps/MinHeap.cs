using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// A heap.
    /// </summary>
    /// <typeparam name="T">The type of the heap elements.</typeparam>
    public sealed class MinHeap<T> : IRawHeapAccess<T>
        where T : notnull
    {
        private readonly IComparer<T> _comparer;

        // TODO: min heap specific
        private readonly List<Item> _minHeap;

        /// <summary>
        /// Initializes a new instance of the <see cref="MinHeap{T}"/> class.
        /// </summary>
        /// <param name="initialCapacity"></param>
        /// <param name="comparer"></param>
        public MinHeap([ValueRange(0, int.MaxValue)] int initialCapacity = 0, IComparer<T>? comparer = default)
        {
            _comparer = comparer ?? Comparer<T>.Default;
            _minHeap = new List<Item>(initialCapacity);
        }

        /// <summary>
        /// Gets the number of items in the heap.
        /// </summary>
        [ValueRange(0, int.MaxValue)]
        public int Count => _minHeap.Count;

        /// <summary>
        /// Inserts a value into the heap.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        public void Insert(T value)
        {
            // At this point, we may want to add a check to ensure that we can only
            // insert to the heap if its size is less than the maximums size (i.e. the actual size of the array).
            // This is obviously not required when using a list rather than an array.
            _minHeap.Add(new Item(value));
            SiftUp(_minHeap.Count - 1);
        }

        /// <summary>
        /// Changes the value of the top item.
        /// </summary>
        /// <param name="value">The value to set for the top item.</param>
        public void ChangeValue(T value) => ChangeValue(0, value);

        /// <summary>
        /// Gets the first item from the heap without removing it.
        /// </summary>
        /// <returns>The item.</returns>
        public T Peek()
        {
            if (_minHeap.Count <= 0) throw new InvalidOperationException("The heap must contain at least one item.");
            return _minHeap[0].Value;
        }

        /// <summary>
        /// Removes and returns the first item from the heap.
        /// </summary>
        /// <returns>The first item.</returns>
        public T Extract()
        {
            if (_minHeap.Count <= 0) throw new InvalidOperationException("The heap must contain at least one item.");

            // In a min heap, the root item is the minimum.
            var result = _minHeap[0];

            // Replace it with the last item and sift that one down;
            // making sure the formerly "last" item is "removed" from the list.
            _minHeap[0] = _minHeap[^1];
            _minHeap.RemoveAt(_minHeap.Count - 1);
            SiftDown(0);

            return result.Value;
        }

        /// <summary>
        /// Gets raw access to the heap.
        /// </summary>
        /// <remarks>
        ///     This is intended solely for debugging.
        /// </remarks>
        internal IRawHeapAccess<T> RawAccess
        {
            [DebuggerStepThrough]
            get => this;
        }

        /// <inheritdoc cref="IRawHeapAccess{T}.this"/>
        T IRawHeapAccess<T>.this[int index]
        {
            [DebuggerStepThrough]
            get => _minHeap[index].Value;
        }

        /// <inheritdoc cref="IRawHeapAccess{T}.Count"/>
        int IRawHeapAccess<T>.Count
        {
            [DebuggerStepThrough]
            get => Count;
        }

        /// <inheritdoc cref="IRawHeapAccess{T}.Parent"/>
        [DebuggerStepThrough]
        int IRawHeapAccess<T>.Parent(int i) => Parent(i);

        /// <inheritdoc cref="IRawHeapAccess{T}.LeftChild"/>
        [DebuggerStepThrough]
        int IRawHeapAccess<T>.LeftChild(int i) => LeftChild(i);

        /// <inheritdoc cref="IRawHeapAccess{T}.RightChild"/>
        [DebuggerStepThrough]
        int IRawHeapAccess<T>.RightChild(int i) => RightChild(i);

        /// <inheritdoc cref="IRawHeapAccess{T}.IsRoot"/>
        [DebuggerStepThrough]
        bool IRawHeapAccess<T>.IsRoot(int i) => IsRoot(i);

        /// <inheritdoc cref="IRawHeapAccess{T}.Remove"/>
        [DebuggerStepThrough]
        void IRawHeapAccess<T>.Remove(int i) => Remove(i);

        /// <inheritdoc cref="IRawHeapAccess{T}.ChangeValue"/>
        [DebuggerStepThrough]
        void IRawHeapAccess<T>.ChangeValue(int i, T value) => ChangeValue(i, value);

        /// <summary>
        /// Calculates the index of the parent of the <paramref name="i"/>-th item.
        /// </summary>
        /// <param name="i">The index of the item whose parent should be determined.</param>
        /// <returns>The index of the parent item or <c>-1</c> if the item is the root element (at <c>0</c>).</returns>
        [DebuggerStepThrough]
        [ValueRange(-1, int.MaxValue)]
        private static int Parent([ValueRange(0, int.MaxValue)] int i) => (i - 1) / 2;

        /// <summary>
        /// Calculates the index of the left child of the <paramref name="i"/>-th item.
        /// </summary>
        /// <param name="i">The index of the item whose left child should be determined.</param>
        /// <returns>The index of the left child item.</returns>
        [DebuggerStepThrough]
        [ValueRange(1, int.MaxValue)]
        private static int LeftChild([ValueRange(0, int.MaxValue)] int i) => 2 * i + 1;

        /// <summary>
        /// Calculates the index of the right child of the <paramref name="i"/>-th item.
        /// </summary>
        /// <param name="i">The index of the item whose right child should be determined.</param>
        /// <returns>The index of the right child item.</returns>
        [DebuggerStepThrough]
        [ValueRange(2, int.MaxValue)]
        private static int RightChild([ValueRange(0, int.MaxValue)] int i) => 2 * i + 2;

        /// <summary>
        /// Determines whether the item determined by the <paramref name="i"/>-th index is the root element.
        /// </summary>
        /// <remarks>
        ///     This is only ever the case if <paramref name="i"/> is <c>0</c>.
        /// </remarks>
        /// <param name="i">The index of the item.</param>
        /// <returns><see langword="true"/> when <paramref name="i"/> is <c>0</c>; <see langword="false"/> otherwise.</returns>
        [DebuggerStepThrough]
        private static bool IsRoot([ValueRange(0, int.MaxValue)] int i) => i == 0;

        /// <summary>
        /// Determines whether an item is strictly greater than another one.
        /// </summary>
        /// <param name="lhs">The left item's index.</param>
        /// <param name="rhs">The right item's index.</param>
        /// <returns><see langword="true"/> if the left item is greater than the right one; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="Compare"/>
        [DebuggerStepThrough]
        private bool IsGreater(int lhs, int rhs) => Compare(_minHeap[lhs], _minHeap[rhs]) > 0;

        /// <summary>
        /// Determines whether an item is strictly smaller than another one.
        /// </summary>
        /// <param name="lhs">The left item's index.</param>
        /// <param name="rhs">The right item's index.</param>
        /// <returns><see langword="true"/> if the left item is smaller than the right one; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="Compare"/>
        [DebuggerStepThrough]
        private bool IsSmaller(int lhs, int rhs) => Compare(_minHeap[lhs], _minHeap[rhs]) < 0;

        /// <summary>
        /// Determines whether an item is strictly smaller than another one.
        /// </summary>
        /// <param name="lhs">The left item.</param>
        /// <param name="rhs">The right item.</param>
        /// <returns><see langword="true"/> if the left item is smaller than the right one; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="Compare"/>
        [DebuggerStepThrough]
        private bool IsSmaller(Item lhs, Item rhs) => Compare(lhs, rhs) < 0;

        /// <summary>
        /// Compares two items.
        /// </summary>
        /// <param name="lhs">The left item.</param>
        /// <param name="rhs">The right item.</param>
        /// <returns><c>-1</c> if <paramref name="lhs"/> is smaller than <paramref name="rhs"/>; <c>0</c> if both items are equal and <c>+1</c> if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.</returns>
        /// <seealso cref="IComparable.CompareTo"/>
        private int Compare(Item lhs, Item rhs)
        {
            if (!lhs.IsMinimumToken && !rhs.IsMinimumToken)
            {
                return _comparer.Compare(lhs.Value, rhs.Value);
            }

            // TODO: min heap specific
            if (lhs.IsMinimumToken && rhs.IsMinimumToken) return 0;
            if (lhs.IsMinimumToken) return -1;
            return 1;
        }

        /// <summary>
        /// Sifts the item with the <paramref name="i"/>-th index up.
        /// </summary>
        /// <param name="i">The index of the item to sift up.</param>
        private void SiftUp(int i)
        {
            // TODO: min heap specific
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
        private void SiftDown(int i)
        {
            while (true)
            {
                var minIndex = i;
                var l = LeftChild(i);
                var hasLeftChild = l < _minHeap.Count;
                if (hasLeftChild && IsSmaller(l, minIndex)) // TODO: min heap specific
                {
                    minIndex = l;
                }

                var r = RightChild(i);
                var hasRightChild = r < _minHeap.Count;
                if (hasRightChild && IsSmaller(r, minIndex)) // TODO: min heap specific
                {
                    minIndex = r;
                }

                if (i == minIndex) return;

                Swap(i, minIndex);
                i = minIndex;
            }
        }

        private void ChangeValue(int i, T value)
        {
            var oldValue = _minHeap[i];
            var newValue = new Item(value);
            _minHeap[i] = newValue;
            if (IsSmaller(newValue, oldValue)) // TODO: min heap specific
            {
                SiftUp(i);
            }
            else
            {
                SiftDown(i);
            }
        }

        // TOOD: Add test case
        private void Remove(int i)
        {
            // In a min heap, replace the item to be removed with "negative infinity",
            // sift the item up, then extract the maximum.
            // TODO: min heap specific
            _minHeap[i] = new Item(isMinimum: true);

            SiftUp(i);
            Extract();
        }

        /// <summary>
        /// Swaps the <paramref name="i"/>-th and <paramref name="j"/>-th item.
        /// </summary>
        /// <param name="i">The index of the first item.</param>
        /// <param name="j">The index of the second item.</param>
        private void Swap(int i, int j)
        {
            Debug.Assert(i >= 0 && i < _minHeap.Count, "i >= 0 && i < values.Count");
            Debug.Assert(j >= 0 && j < _minHeap.Count, "j >= 0 && j < values.Count");
            Debug.Assert(i != j, "i != j");

            (_minHeap[i], _minHeap[j]) = (_minHeap[j], _minHeap[i]);
        }

        /// <summary>
        /// Struct representing an item in the heap.
        /// </summary>
        private readonly struct Item
        {
            // TODO: min heap specific
            public readonly bool IsMinimumToken;
            public readonly T Value;

            /// <summary>
            /// Initializes an instance of the <see cref="Item"/> struct.
            /// </summary>
            /// <param name="value">The item value.</param>
            public Item(T value)
            {
                // TODO: min heap specific
                IsMinimumToken = false;
                Value = value;
            }

            /// <summary>
            /// Initializes an instance of the <see cref="Item"/> struct.
            /// </summary>
            /// <param name="isMinimum">A value indicating whether this item represents the minimum token.</param>
            public Item(bool isMinimum)
            {
                // TODO: min heap specific
                IsMinimumToken = isMinimum;
                Value = default!;
            }

            /// <inheritdoc />
            // TODO: min heap specific
            public override string ToString() => IsMinimumToken ? "Minimum" : Value.ToString();

            /// <inheritdoc />
            // TODO: min heap specific
            public override int GetHashCode() => HashCode.Combine(IsMinimumToken, Value);
        }
    }
}
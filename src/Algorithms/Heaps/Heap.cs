using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// A Heap that can both operate as a Min Heap and a Max Heap.
    /// </summary>
    /// <remarks>
    ///     The base implementation is that of a Max Heap; in Min Heap
    ///     mode, comparisons are inverted.
    /// </remarks>
    /// <typeparam name="T">The type of the heap elements.</typeparam>
    public partial class Heap<T> : IHeapIndexes<T>
        where T : notnull
    {
        private readonly HeapType _heapType;
        private readonly List<HeapItem> _heap;
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{T}"/> class.
        /// </summary>
        /// <param name="heapType">The type of heap to create.</param>
        /// <param name="initialCapacity">The initial capacity of the heap.</param>
        /// <param name="comparer">An optional comparer for the items.</param>
        protected Heap(
            HeapType heapType,
            [ValueRange(0, int.MaxValue)] int initialCapacity,
            IComparer<T>? comparer)
        {
            _heapType = heapType;
            _heap = new List<HeapItem>(initialCapacity);
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>
        /// Gets the number of items in the heap.
        /// </summary>
        public int Count => _heap.Count;

        /// <summary>
        /// Accessor for items in the underlying heap.
        /// </summary>
        /// <param name="index">The item of the element.</param>
        public T this[int index]
        {
            get => _heap[index].Value;
        }

        /// <summary>
        /// Inserts a value into the heap.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        public void Insert(in T value)
        {
            // At this point, we may want to add a check to ensure that we can only
            // insert to the heap if its size is less than the maximums size (i.e. the actual size of the array).
            // This is obviously not required when using a list rather than an array.
            _heap.Add(new HeapItem(value));
            SiftUp(_heap.Count - 1);
        }

        /// <summary>
        /// Gets the first item from the heap without removing it.
        /// </summary>
        /// <returns>The item.</returns>
        public T Peek()
        {
            if (_heap.Count <= 0) throw new InvalidOperationException("The heap must contain at least one item.");
            return _heap[0].Value;
        }

        /// <summary>
        /// Removes and returns the first item from the heap.
        /// </summary>
        /// <returns>The first item.</returns>
        public T Extract()
        {
            if (_heap.Count <= 0) throw new InvalidOperationException("The heap must contain at least one item.");

            // In a min heap, the root item is the minimum.
            var result = _heap[0];

            // Replace it with the last item and sift that one down;
            // making sure the formerly "last" item is "removed" from the list.
            _heap[0] = _heap[^1];
            _heap.RemoveAt(_heap.Count - 1);
            SiftDown(0);

            return result.Value;
        }

        /// <summary>
        /// Removes the <paramref name="i"/>-th element.
        /// </summary>
        /// <param name="i">The index of the item to change.</param>
        public void Remove(int i)
        {
            _heap[i] = new HeapItem(isMaximum: true);

            SiftUp(i);
            Extract();
        }

        /// <summary>
        /// Changes the value of the top item.
        /// </summary>
        /// <param name="value">The value to set for the top item.</param>
        public void ChangeValue(in T value) => ChangeValue(0, in value);

        /// <summary>
        /// Changes the value of the <paramref name="i"/>-th element.
        /// </summary>
        /// <param name="i">The index of the item to change.</param>
        /// <param name="value">The new value.</param>
        public void ChangeValue(int i, in T value)
        {
            var oldValue = _heap[i].Value;
            _heap[i] = new HeapItem(value);
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
        private void SiftUp(int i)
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
        private void SiftDown(int i)
        {
            while (HasGreaterChild(i, out var childIndex))
            {
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
        /// Finds the child that is greater than the item at the <paramref name="i"/>-th index
        /// or returns <see langword="false"/> if no such child exists.
        /// </summary>
        /// <param name="i">The root node.</param>
        /// <param name="childIndex">The index of the greater child; only meaningful if the method evaluates to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if a greater child exists; <see langword="false"/> otherwise.</returns>
        private bool HasGreaterChild(int i, out int childIndex)
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

        /// <summary>
        /// Swaps the <paramref name="i"/>-th and <paramref name="j"/>-th item.
        /// </summary>
        /// <param name="i">The index of the first item.</param>
        /// <param name="j">The index of the second item.</param>
        private void Swap(int i, int j)
        {
            Debug.Assert(i >= 0 && i < _heap.Count, "i >= 0 && i < values.Count");
            Debug.Assert(j >= 0 && j < _heap.Count, "j >= 0 && j < values.Count");
            Debug.Assert(i != j, "i != j");

            (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
        }
    }
}
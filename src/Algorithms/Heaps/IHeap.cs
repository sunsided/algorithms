using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// Interface to heap implementations.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    public interface IHeap<T>
        where T : notnull
    {
        /// <summary>
        /// Gets an item given its index.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        T this[[ValueRange(0, int.MaxValue)] int index] { get; }

        /// <summary>
        /// Gets the number of items in the heap.
        /// </summary>
        [ValueRange(0, int.MaxValue)]
        int Count { get; }

        /// <summary>
        /// Inserts a value into the heap.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        void Insert(in T value);

        /// <summary>
        /// Changes the value of the top item.
        /// </summary>
        /// <param name="value">The value to set for the top item.</param>
        void ChangeValue(in T value);

        /// <summary>
        /// Changes the value of the specified item.
        /// </summary>
        /// <param name="i">The index of the item to remove.</param>
        /// <param name="value">The new value.</param>
        void ChangeValue([ValueRange(0, int.MaxValue)] int i, in T value);

        /// <summary>
        /// Gets the first item from the heap without removing it.
        /// </summary>
        /// <returns>The item.</returns>
        T Peek();

        /// <summary>
        /// Removes and returns the first item from the heap.
        /// </summary>
        /// <returns>The first item.</returns>
        T Extract();

        /// <summary>
        /// Removes an item from the heap.
        /// </summary>
        /// <param name="i">The index of the item to remove.</param>
        void Remove([ValueRange(0, int.MaxValue)] int i);
    }
}

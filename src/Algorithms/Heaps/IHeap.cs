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
        /// Gets the number of items in the heap.
        /// </summary>
        [ValueRange(0, int.MaxValue)]
        int Count { get; }

        /// <summary>
        /// Inserts a value into the heap.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        void Insert(T value);

        /// <summary>
        /// Changes the value of the top item.
        /// </summary>
        /// <param name="value">The value to set for the top item.</param>
        void ChangeValue(T value);

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
    }
}

namespace Widemeadows.Algorithms.Tests.Data.Heap
{
    /// <summary>
    /// Type of heap modifications.
    /// </summary>
    public enum HeapOperationType
    {
        /// <summary>
        /// Inserts an item into the heap.
        /// </summary>
        Insert,

        /// <summary>
        /// Pulls the top item from the heap.
        /// </summary>
        PullTop,

        /// <summary>
        /// Changes the value of the top item.
        /// </summary>
        ChangeTop,

        /// <summary>
        /// Remove an item identified by a given index.
        /// </summary>
        /// <remarks>
        ///    Used in fuzzing tests.
        /// </remarks>
        RemoveAny,

        /// <summary>
        /// Change an item identified by a given index.
        /// </summary>
        /// <remarks>
        ///    Used in fuzzing tests.
        /// </remarks>
        ChangeAny
    }
}

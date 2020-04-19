namespace Widemeadows.Algorithms.Heaps
{
    /// <summary>
    /// Polarity of a heap.
    /// </summary>
    public enum HeapPolarity
    {
        /// <summary>
        /// Heap is a min-heap, i.e. smaller elements are sorted to the top.
        /// </summary>
        MinHeap = 1,

        /// <summary>
        /// Heap is a max-heap, i.e. larger elements are sorted to the top.
        /// </summary>
        MaxHeap = -1
    }
}

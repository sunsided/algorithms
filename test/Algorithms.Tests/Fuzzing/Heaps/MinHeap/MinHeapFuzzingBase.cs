using FluentAssertions;
using Widemeadows.Algorithms.Heaps;
using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Fuzzing.Heaps.MinHeap
{
    /// <summary>
    /// Base class for fuzzing <see cref="MinHeap{T}"/>.
    /// </summary>
    public abstract class MinHeapFuzzingBase : HeapFuzzingBase
    {
        protected MinHeapFuzzingBase()
            : base(new MinHeap<NumericalItem>())
        {}

        internal override void ValidateHeap(int step)
        {
            var heap = Heap;
            for (var i = 0; i < heap.Count; ++i)
            {
                var idxFirstChild = heap.LeftChild(i);
                var idxSecondChild = heap.RightChild(i);

                // If the first child index is out of bounds, the second one must be, too.
                // Since every following element would produce an index even greater,
                // we can terminate the entire check at this point.
                if (idxFirstChild >= heap.Count) break;
                heap[i].Should().BeLessOrEqualTo(heap[idxFirstChild], "because a min heap requires values[{0}] <= values[{1}]; current heap size: {2} after step {3}", i, idxFirstChild, heap.Count, step);

                // Even though a left child existed, a right child may not.
                if (idxSecondChild >= heap.Count) break;
                heap[i].Should().BeLessOrEqualTo(heap[idxFirstChild], "because a min heap requires values[{0}] <= values[{1}]; current heap size: {2} after step {3}", i, idxFirstChild, heap.Count, step);
            }
        }
    }
}

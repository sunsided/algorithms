using System;
using System.Collections.Generic;
using FluentAssertions;
using Widemeadows.Algorithms.Heaps;
using Widemeadows.Algorithms.Tests.Data.Heap;
using Widemeadows.Algorithms.Tests.Model;
using Xunit;

namespace Widemeadows.Algorithms.Tests
{
    /// <summary>
    /// Tests for <see cref="MinHeap{T}"/>.
    /// </summary>
    public class MinHeapTests
    {
        private readonly MinHeap<NumericalItem> _minHeap = new MinHeap<NumericalItem>();

        public static IEnumerable<object[]> RandomSeeds
        {
            get
            {
                const int numTestCases = 1000;
                var random = new Random();

                for (var i = 0; i < numTestCases; ++i)
                {
                    yield return new object[] {random.Next()};
                }
            }
        }

        [Theory]
        [ClassData(typeof(NumericalItemHeapDataGenerator))]
        public void HeapIsBuiltCorrectly(IList<NumericalItem> items)
        {
            foreach (var item in items)
            {
                _minHeap.Insert(item);
            }

            _minHeap.Count.Should().Be(items.Count, "because that is the number of items we inserted");
            ValidateMinHeap(_minHeap.RawAccess, items.Count);
        }

        [Theory]
        [ClassData(typeof(HeapOperationDataGenerator))]
        public void Heap_AfterModification_IsInCorrectState(HeapState<NumericalItem>[] states)
        {
            for (var index = 0; index < states.Length; index++)
            {
                var state = states[index];

                ApplySimpleOperation(_minHeap, state.Operation);

                if (state.ExpectItems)
                {
                    _minHeap.Count.Should().BeGreaterThan(0, "because we expect at least one item");

                    var top = _minHeap.Peek();
                    top.Should().Be(state.ExpectedMin, "because this is the new minimum value");
                    top.Value.Should().BeLessOrEqualTo(state.ExpectedMax.Value,
                        "because this is the expected maximum value");
                }
                else
                {
                    _minHeap.Count.Should().Be(0, "because we have removed the last item");
                }

                ValidateMinHeap(_minHeap.RawAccess, index);
            }
        }

        private static void ApplySimpleOperation(MinHeap<NumericalItem> heap, HeapOperation<NumericalItem> operation)
        {
            switch (operation.Type)
            {
                // These operations always leave at least one item in the heap.
                case HeapOperationType.Insert:
                    heap.Insert(operation.Value);
                    break;
                case HeapOperationType.ChangeTop:
                    heap.ChangeValue(operation.Value);
                    break;

                // This operation may remove the last item from the heap.
                case HeapOperationType.PullTop:
                    heap.Extract();
                    break;

                case HeapOperationType.ChangeAny:
                    throw new InvalidOperationException("ChangeAny is not supported by this method.");
                case HeapOperationType.RemoveAny:
                    throw new InvalidOperationException("RemoveAny is not supported by this method.");

                default:
                    throw new InvalidOperationException($"Unable to process operation of type {operation.Type}");
            }
        }

        private static void ValidateMinHeap(IRawHeapAccess<NumericalItem> heap, int step)
        {
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

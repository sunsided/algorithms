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
    /// Tests for <see cref="MaxHeap{T}"/>.
    /// </summary>
    public class MaxHeapTests
    {
        private readonly MaxHeap<NumericalItem> _maxHeap = new MaxHeap<NumericalItem>();

        [Theory]
        [ClassData(typeof(NumericalItemHeapDataGenerator))]
        public void HeapIsBuiltCorrectly(IList<NumericalItem> items)
        {
            foreach (var item in items)
            {
                _maxHeap.Insert(item);
            }

            _maxHeap.Count.Should().Be(items.Count, "because that is the number of items we inserted");
            ValidateMaxHeap(_maxHeap.RawAccess, items.Count);
        }

        [Theory]
        [ClassData(typeof(MaxHeapOperationDataGenerator))]
        public void Heap_AfterModification_IsInCorrectState(HeapState<NumericalItem>[] states)
        {
            for (var index = 0; index < states.Length; index++)
            {
                var state = states[index];

                ApplySimpleOperation(_maxHeap, state.Operation);

                if (state.ExpectItems)
                {
                    _maxHeap.Count.Should().BeGreaterThan(0, "because we expect at least one item");

                    var top = _maxHeap.Peek();
                    top.Should().Be(state.ExpectedMax, "because this is the new maximum value");
                    top.Value.Should().BeGreaterOrEqualTo(state.ExpectedMin.Value,
                        "because this is the expected minimum value");
                }
                else
                {
                    _maxHeap.Count.Should().Be(0, "because we have removed the last item");
                }

                ValidateMaxHeap(_maxHeap.RawAccess, index);
            }
        }

        private static void ApplySimpleOperation(MaxHeap<NumericalItem> heap, HeapOperation<NumericalItem> operation)
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
                case HeapOperationType.ChangeAny:
                    heap.RawAccess.ChangeValue(operation.Index, operation.Value);
                    break;

                // This operation may remove the last item from the heap.
                case HeapOperationType.Extract:
                    heap.Extract();
                    break;
                case HeapOperationType.RemoveAny:
                    heap.RawAccess.Remove(operation.Index);
                    break;

                default:
                    throw new InvalidOperationException($"Unable to process operation of type {operation.Type}");
            }
        }

        private static void ValidateMaxHeap(IRawHeapAccess<NumericalItem> heap, int step)
        {
            for (var i = 0; i < heap.Count; ++i)
            {
                var idxFirstChild = heap.LeftChild(i);
                var idxSecondChild = heap.RightChild(i);

                // If the first child index is out of bounds, the second one must be, too.
                // Since every following element would produce an index even greater,
                // we can terminate the entire check at this point.
                if (idxFirstChild >= heap.Count) break;
                heap[i].Should().BeGreaterOrEqualTo(heap[idxFirstChild], "because a min heap requires values[{0}] >= values[{1}]; current heap size: {2} after step {3}", i, idxFirstChild, heap.Count, step);

                // Even though a left child existed, a right child may not.
                if (idxSecondChild >= heap.Count) break;
                heap[i].Should().BeGreaterOrEqualTo(heap[idxFirstChild], "because a min heap requires values[{0}] >= values[{1}]; current heap size: {2} after step {3}", i, idxFirstChild, heap.Count, step);
            }
        }
    }
}

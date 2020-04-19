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
        private readonly MinHeap<NumericalItem> _minHeap;

        public MinHeapTests()
        {
            _minHeap = new MinHeap<NumericalItem>();
        }

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
            ValidateMinHeap(_minHeap.RawAccess);
        }

        [Theory]
        [ClassData(typeof(HeapOperationDataGenerator))]
        public void Heap_AfterModification_IsInCorrectState(HeapState<NumericalItem>[] states)
        {
            foreach (var state in states)
            {
                ApplyAction(_minHeap, state.Operation);

                if (state.ExpectItems)
                {
                    _minHeap.Count.Should().BeGreaterThan(0, "because we expect at least one item");

                    var top = _minHeap.Peek();
                    top.Should().Be(state.ExpectedMin, "because this is the new minimum value");
                    top.Value.Should().BeLessOrEqualTo(state.ExpectedMax.Value, "because this is the expected maximum value");
                }
                else
                {
                    _minHeap.Count.Should().Be(0, "because we have removed the last item");
                }

                ValidateMinHeap(_minHeap.RawAccess);
            }
        }

        [Theory]
        [MemberData(nameof(RandomSeeds), DisableDiscoveryEnumeration = true)]
        public void Fuzzing(int seed)
        {
            var rng = new Random(seed);
            var numItems = rng.Next(1, 10000);

            const HeapOperationType removeAnyOp = (HeapOperationType) (-1);
            const HeapOperationType changeAnyOp = (HeapOperationType) (-2);
            var nonEmptyHeapOperations = new[]
            {
                HeapOperationType.Insert,
                HeapOperationType.ChangeTop,
                HeapOperationType.PullTop,
                removeAnyOp,
                changeAnyOp,
            };

            var emptyHeapOperations = new[]
            {
                HeapOperationType.Insert
            };

            for (var i = 0; i < numItems; ++i)
            {
                var setOfOperations = _minHeap.Count > 0
                    ? nonEmptyHeapOperations
                    : emptyHeapOperations;

                var operation = setOfOperations[rng.Next(0, setOfOperations.Length)];
                switch (operation)
                {
                    case HeapOperationType.PullTop:
                    {
                        _minHeap.Extract();
                        break;
                    }

                    case HeapOperationType.Insert:
                    {
                        var value = rng.Next();
                        _minHeap.Insert(value);
                        break;
                    }

                    case HeapOperationType.ChangeTop:
                    {
                        var value = rng.Next();
                        _minHeap.ChangeValue(value);
                        break;
                    }

                    case removeAnyOp:
                    {
                        var index = rng.Next(0, _minHeap.Count);
                        _minHeap.RawAccess.Remove(index);
                        break;
                    }

                    case changeAnyOp:
                    {
                        var index = rng.Next(0, _minHeap.Count);
                        var value = rng.Next();
                        _minHeap.RawAccess.ChangeValue(index, value);
                        break;
                    }

                    default:
                        throw new InvalidOperationException($"Unknown operation type: {operation}");
                }
            }

            ValidateMinHeap(_minHeap.RawAccess);
        }

        private static void ApplyAction(MinHeap<NumericalItem> heap, HeapOperation<NumericalItem> operation)
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

                default:
                    throw new InvalidOperationException($"Unable to process operation of type {operation.Type}");
            }
        }

        private static void ValidateMinHeap(IRawHeapAccess<NumericalItem> heap)
        {
            for (var i = 0; i < heap.Count; ++i)
            {
                var idxFirstChild = heap.LeftChild(i);
                var idxSecondChild = heap.RightChild(i);

                // If the first child index is out of bounds, the second one must be, too.
                // Since every following element would produce an index even greater,
                // we can terminate the entire check at this point.
                if (idxFirstChild >= heap.Count) break;
                heap[i].Should().BeLessOrEqualTo(heap[idxFirstChild], "because a min heap requires values[{0}] <= values[{1}]", i, idxFirstChild);

                // Even though a left child existed, a right child may not.
                if (idxSecondChild >= heap.Count) break;
                heap[i].Should().BeLessOrEqualTo(heap[idxFirstChild], "because a min heap requires values[{0}] <= values[{1}]", i, idxFirstChild);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using FluentAssertions;
using Widemeadows.Algorithms.Heaps;
using Widemeadows.Algorithms.Tests.Data.Heap;
using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Fuzzing.MinHeap
{
    /// <summary>
    /// Base class for fuzzing <see cref="MinHeap{T}"/>.
    /// </summary>
    public abstract class MinHeapFuzzingBase
    {
        protected readonly MinHeap<NumericalItem> MinHeap = new MinHeap<NumericalItem>();

        protected void Fuzz(int seed, int maxOperations, int minValue = 0, int maxValue = int.MaxValue)
        {
            var rng = new Random(seed);
            var numOperations = rng.Next(1, maxOperations);
            var operations = FuzzOperations(rng, numOperations, minValue, maxValue);

            for (var i = 0; i < operations.Count; ++i)
            {
                var operation = operations[i];
                ApplyFuzzedOperation(operation);
                ValidateMinHeap(MinHeap.RawAccess, i);
            }
        }

        protected List<(HeapOperationType operationType, int index, NumericalItem value)> FuzzOperations(
            Random rng,
            int maxOperations,
            int minValue = 0,
            int maxValue = int.MaxValue)
        {
            var nonEmptyHeapOperations = new[]
            {
                HeapOperationType.Insert,
                HeapOperationType.ChangeTop,
                HeapOperationType.PullTop,
                HeapOperationType.RemoveAny,
                HeapOperationType.ChangeAny,
            };

            var emptyHeapOperations = new[]
            {
                HeapOperationType.Insert
            };

            // Generate operations.
            var operations = new List<(HeapOperationType operationType, int index, NumericalItem value)>(maxOperations);
            var simulatedHeapSize = 0;
            for (var i = 0; i < maxOperations; ++i)
            {
                var setOfOperations = simulatedHeapSize > 0
                    ? nonEmptyHeapOperations
                    : emptyHeapOperations;

                var operation = setOfOperations[rng.Next(0, setOfOperations.Length)];
                switch (operation)
                {
                    case HeapOperationType.PullTop:
                    {
                        --simulatedHeapSize;
                        operations.Add((operation, 0, int.MinValue));
                        break;
                    }

                    case HeapOperationType.Insert:
                    {
                        ++simulatedHeapSize;
                        var value = rng.Next(minValue, maxValue);
                        operations.Add((operation, -1, value));
                        break;
                    }

                    case HeapOperationType.ChangeTop:
                    {
                        var value = rng.Next(minValue, maxValue);
                        operations.Add((operation, 0, value));
                        break;
                    }

                    case HeapOperationType.RemoveAny:
                    {
                        var index = rng.Next(0, simulatedHeapSize);
                        --simulatedHeapSize;
                        operations.Add((operation, index, int.MinValue));
                        break;
                    }

                    case HeapOperationType.ChangeAny:
                    {
                        var index = rng.Next(0, simulatedHeapSize);
                        var value = rng.Next(minValue, maxValue);
                        operations.Add((operation, index, value));
                        break;
                    }

                    default:
                        throw new InvalidOperationException($"Unknown operation type: {operation}");
                }
            }

            return operations;
        }

        protected void ApplyFuzzedOperation((HeapOperationType operationType, int index, NumericalItem value) operation)
        {
            switch (operation.operationType)
            {
                case HeapOperationType.PullTop:
                {
                    MinHeap.Extract();
                    break;
                }

                case HeapOperationType.Insert:
                {
                    MinHeap.Insert(operation.value);
                    break;
                }

                case HeapOperationType.ChangeTop:
                {
                    MinHeap.ChangeValue(operation.value);
                    break;
                }

                case HeapOperationType.RemoveAny:
                {
                    MinHeap.RawAccess.Remove(operation.index);
                    break;
                }

                case HeapOperationType.ChangeAny:
                {
                    MinHeap.RawAccess.ChangeValue(operation.index, operation.value);
                    break;
                }

                default:
                    throw new InvalidOperationException($"Unknown operation type: {operation}");
            }
        }

        internal static void ValidateMinHeap(IRawHeapAccess<NumericalItem> heap, int step)
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

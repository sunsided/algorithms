using System;
using System.Collections.Generic;
using Widemeadows.Algorithms.Heaps;
using Widemeadows.Algorithms.Tests.Data.Heap;
using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Fuzzing.Heaps
{
    /// <summary>
    /// Base class for fuzzing <see cref="Heap{T}"/>.
    /// </summary>
    public abstract class HeapFuzzingBase
    {
        internal readonly IRawHeapAccess<NumericalItem> Heap;

        internal HeapFuzzingBase(IRawHeapAccess<NumericalItem> heap)
        {
            Heap = heap;
        }

        protected void Fuzz(int seed, int maxOperations, int minValue = 0, int maxValue = int.MaxValue)
        {
            var rng = new Random(seed);
            var numOperations = rng.Next(1, maxOperations);
            var operations = FuzzOperations(rng, numOperations, minValue, maxValue);

            for (var i = 0; i < operations.Count; ++i)
            {
                var operation = operations[i];
                ApplyFuzzedOperation(operation);
                ValidateHeap(i);
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
                HeapOperationType.Extract,
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
                    case HeapOperationType.Extract:
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
                case HeapOperationType.Extract:
                {
                    Heap.Extract();
                    break;
                }

                case HeapOperationType.Insert:
                {
                    Heap.Insert(operation.value);
                    break;
                }

                case HeapOperationType.ChangeTop:
                {
                    Heap.ChangeValue(operation.value);
                    break;
                }

                case HeapOperationType.RemoveAny:
                {
                    Heap.Remove(operation.index);
                    break;
                }

                case HeapOperationType.ChangeAny:
                {
                    Heap.ChangeValue(operation.index, operation.value);
                    break;
                }

                default:
                    throw new InvalidOperationException($"Unknown operation type: {operation}");
            }
        }

        internal abstract void ValidateHeap(int step);
    }
}

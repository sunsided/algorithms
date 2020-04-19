using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Data.Heap
{
    /// <summary>
    /// Struct describing the state of a heap after modifying it.
    /// </summary>
    public readonly struct HeapState<T>
        where T : notnull
    {
        /// <summary>
        /// The operation to apply.
        /// </summary>
        public readonly HeapOperation<T> Operation;

        /// <summary>
        /// A value encoding whether the heap is expected to have items after the operation.
        /// </summary>
        /// <returns><see langword="true"/> if the heap is expected to have items; <see langword="false"/> if the heap is expected to be empty.</returns>
        public readonly bool ExpectItems;

        /// <summary>
        /// The expected minimum value of the heap.
        /// </summary>
        /// <remarks>
        ///     Note that this value only is meaningful if <see cref="ExpectItems"/> evaluates to <see langword="true"/>;
        /// </remarks>
        public readonly NumericalItem ExpectedMin;

        /// <summary>
        /// The expected maximum value of the heap.
        /// </summary>
        /// <remarks>
        ///     Note that this value only is meaningful if <see cref="ExpectItems"/> evaluates to <see langword="true"/>;
        /// </remarks>
        public readonly NumericalItem ExpectedMax;

        public HeapState(HeapOperation<T> operation, bool expectItems, NumericalItem expectedMin, NumericalItem expectedMax)
        {
            Operation = operation;
            ExpectItems = expectItems;
            ExpectedMin = expectedMin;
            ExpectedMax = expectedMax;
        }

        public override string ToString() =>
            ExpectItems
                ? $"{Operation} ({ExpectedMin} ... {ExpectedMax})"
                : $"{Operation} (empty)";
    }
}

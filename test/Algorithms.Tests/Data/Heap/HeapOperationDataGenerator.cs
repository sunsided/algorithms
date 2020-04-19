using System.Collections.Generic;
using Widemeadows.Algorithms.Heaps;
using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Data.Heap
{
    /// <summary>
    /// Test case generator for heaps tests, e.g. <see cref="MinHeap{T}" />.
    /// </summary>
    public sealed class HeapOperationDataGenerator : TestCaseGeneratorBase
    {
        /// <inheritdoc />
        public override IEnumerator<object[]> GetEnumerator()
        {
            yield return BuildTrivialCase();
            yield return LeaveEmptyTree();
            yield return AddTwoRemoveOne();
            yield return IncreasingAlternatingAddRemove();
            yield return AlternatingAddRemove();
            yield return InsertAndChangeIncreasingly();
            yield return InsertAndChange();
        }

        /// <summary>
        /// Returns the default test case.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] BuildTrivialCase()
        {
            return new object[]
            {
                new[]
                {
                    BuildCase(HeapOperationType.Insert, 0, 0, 0)
                }
            };
        }

        /// <summary>
        /// Adds an item and immediately removes it.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] LeaveEmptyTree()
        {
            return new object[]
            {
                new[]
                {
                    BuildCase(HeapOperationType.Insert, 0, 0, 0),
                    BuildCase(HeapOperationType.PullTop, default, false, default, default)
                }
            };
        }

        /// <summary>
        /// Adds two items, then removes the top.
        /// </summary>
        /// <returns>The test case parameters</returns>
        private object[] AddTwoRemoveOne()
        {
            return new object[]
            {
                new[]
                {
                    BuildCase(HeapOperationType.Insert, 0, 0, 0),
                    BuildCase(HeapOperationType.Insert, 1, 0, 1),
                    BuildCase(HeapOperationType.PullTop, default, 1, 1)
                }
            };
        }

        /// <summary>
        /// Adds and removes items in an alternating fashion.
        /// </summary>
        /// <returns>The test case parameters</returns>
        private object[] IncreasingAlternatingAddRemove()
        {
            return new object[]
            {
                new[]
                {
                    BuildCase(HeapOperationType.Insert, 0, 0, 0),
                    BuildCase(HeapOperationType.Insert, 1, 0, 1),
                    BuildCase(HeapOperationType.PullTop, default, 1, 1),
                    BuildCase(HeapOperationType.Insert, 2, 1, 2),
                    BuildCase(HeapOperationType.PullTop, default, 2, 2),
                    BuildCase(HeapOperationType.Insert, 3, 2, 3),
                    BuildCase(HeapOperationType.PullTop, default, 3, 3),
                    BuildCase(HeapOperationType.Insert, 4, 3, 4),
                    BuildCase(HeapOperationType.PullTop, default, 4, 4),
                    BuildCase(HeapOperationType.Insert, 5, 4, 5),
                    BuildCase(HeapOperationType.PullTop, default, 5, 5)
                }
            };
        }

        /// <summary>
        /// Adds and removes items in an alternating fashion.
        /// </summary>
        /// <returns>The test case parameters</returns>
        private object[] AlternatingAddRemove()
        {
            return new object[]
            {
                new[] {
                    BuildCase(HeapOperationType.Insert, 0, 0, 0),
                    BuildCase(HeapOperationType.Insert, 5, 0, 5),
                    BuildCase(HeapOperationType.PullTop, default, 5, 5),
                    BuildCase(HeapOperationType.Insert, 1, 1, 5),
                    BuildCase(HeapOperationType.Insert, 2, 1, 5),
                    BuildCase(HeapOperationType.PullTop, default, 2, 5),
                    BuildCase(HeapOperationType.PullTop, default, 5, 5)
                }
            };
        }

        /// <summary>
        /// Inserts items and changes the top value to a bigger number.
        /// </summary>
        /// <returns>The test case parameters</returns>
        private object[] InsertAndChangeIncreasingly()
        {
            return new object[]
            {
                new[]
                {
                    BuildCase(HeapOperationType.Insert, 0, 0, 0),
                    BuildCase(HeapOperationType.Insert, 1, 0, 1),
                    BuildCase(HeapOperationType.ChangeTop, 2, 1, 2),
                    BuildCase(HeapOperationType.Insert, 3, 1, 3),
                    BuildCase(HeapOperationType.ChangeTop, 4, 2, 4),
                }
            };
        }

        /// <summary>
        /// Inserts items and changes the top value to a bigger number.
        /// </summary>
        /// <returns>The test case parameters</returns>
        private object[] InsertAndChange()
        {
            return new object[]
            {
                new[]
                {
                    BuildCase(HeapOperationType.Insert, 0, 0, 0),
                    BuildCase(HeapOperationType.Insert, 1, 0, 1),
                    BuildCase(HeapOperationType.ChangeTop, 2, 1, 2),
                    BuildCase(HeapOperationType.PullTop, default, 2, 2),
                    BuildCase(HeapOperationType.Insert, 3, 2, 3),
                    BuildCase(HeapOperationType.ChangeTop, 0, 0, 3),
                }
            };
        }

        private HeapState<NumericalItem> BuildCase(HeapOperationType type, NumericalItem value,
            NumericalItem expectedMin, NumericalItem expectedMax) =>
            BuildCase(type, value, true, expectedMin, expectedMax);

        private HeapState<NumericalItem> BuildCase(HeapOperationType type, NumericalItem value, bool expectItems, NumericalItem expectedMin, NumericalItem expectedMax) =>
            new HeapState<NumericalItem>
            (
                new HeapOperation<NumericalItem>(type, value),
                expectItems,
                expectedMin,
                expectedMax
            );
    }
}
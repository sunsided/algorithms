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
            yield return Regression1();
            yield return Regression2();
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
                    Insert(0, 0, 0)
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
                    Insert(0, 0, 0),
                    Extract(false, default, default)
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
                    Insert(0, 0, 0),
                    Insert(1, 0, 1),
                    Extract(true, 1, 1)
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
                    Insert(0, 0, 0),
                    Insert(1, 0, 1),
                    Extract(true, 1, 1),
                    Insert(2, 1, 2),
                    Extract(true, 2, 2),
                    Insert(3, 2, 3),
                    Extract(true, 3, 3),
                    Insert(4, 3, 4),
                    Extract(true, 4, 4),
                    Insert(5, 4, 5),
                    Extract(true, 5, 5)
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
                    Insert(0, 0, 0),
                    Insert(5, 0, 5),
                    Extract(true, 5, 5),
                    Insert(1, 1, 5),
                    Insert(2, 1, 5),
                    Extract(true, 2, 5),
                    Extract(true, 5, 5)
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
                    Insert(0, 0, 0),
                    Insert(1, 0, 1),
                    ChangeTop(2, 1, 2),
                    Insert(3, 1, 3),
                    ChangeTop(4, 2, 4),
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
                    Insert(0, 0, 0),
                    Insert(1, 0, 1),
                    ChangeTop(2, 1, 2),
                    Extract(true, 2, 2),
                    Insert(3, 2, 3),
                    ChangeTop(0, 0, 3),
                }
            };
        }

        /// <summary>
        /// Preventing first found regression.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] Regression1()
        {
            return new object[]
            {
                new[]
                {
                    Insert(8, 8, 8),
                    Insert(9, 8, 9),
                    Insert(5, 5, 9),
                    Insert(9, 5, 9),
                    Insert(4, 4, 9),
                    Insert(3, 3, 9),
                    RemoveAny(0, true, 4, 9),
                    Insert(4, 4, 9)
                }
            };
        }

        /// <summary>
        /// Preventing second found regression.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] Regression2()
        {
            return new object[]
            {
                new[]
                {
                    Insert(4, 4, 4),
                    Insert(5, 4, 5),
                    Insert(2, 2,5),
                    Insert(6, 2, 6),
                    Insert(1, 1, 6),
                    Extract(true, 2, 6),
                    Insert(7, 2, 7),
                    Insert(3, 2, 7)
                }
            };
        }

        private HeapState<NumericalItem> Insert(NumericalItem value, NumericalItem expectedMin, NumericalItem expectedMax) =>
            BuildCase(HeapOperationType.Insert, index: 0, value: value, true, expectedMin, expectedMax);

        private HeapState<NumericalItem> ChangeTop(NumericalItem value, NumericalItem expectedMin, NumericalItem expectedMax) =>
            BuildCase(HeapOperationType.ChangeTop, index: 0, value: value, true, expectedMin, expectedMax);

        private HeapState<NumericalItem> ChangeAny(int index, NumericalItem value, NumericalItem expectedMin, NumericalItem expectedMax) =>
            BuildCase(HeapOperationType.ChangeAny, index: index, value: value, true, expectedMin, expectedMax);

        private HeapState<NumericalItem> Extract(bool expectItems, NumericalItem expectedMin, NumericalItem expectedMax) =>
            BuildCase(HeapOperationType.Extract, index: 0, value: default, expectItems, expectedMin, expectedMax);

        private HeapState<NumericalItem> RemoveAny(int index, bool expectItems, NumericalItem expectedMin, NumericalItem expectedMax) =>
            BuildCase(HeapOperationType.RemoveAny, index: index, value: default, expectItems, expectedMin, expectedMax);

        private HeapState<NumericalItem> BuildCase(HeapOperationType type, int index, NumericalItem value, bool expectItems, NumericalItem expectedMin, NumericalItem expectedMax) =>
            new HeapState<NumericalItem>
            (
                new HeapOperation<NumericalItem>(type, index: index, value: value),
                expectItems,
                expectedMin,
                expectedMax
            );
    }
}
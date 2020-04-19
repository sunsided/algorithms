using System.Collections.Generic;
using Widemeadows.Algorithms.Heaps;
using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Data.Heap
{
    /// <summary>
    /// Test case generator for heaps tests, e.g. <see cref="MinHeap{T}" />.
    /// </summary>
    public sealed class NumericalItemHeapDataGenerator : TestCaseGeneratorBase
    {
        /// <inheritdoc />
        public override IEnumerator<object[]> GetEnumerator()
        {
            yield return BuildTrivialCase();
            yield return MonotonicallyDecreasingSequence();
            yield return MonotonicallyIncreasingSequence();
            yield return AlternatingSequence();
            yield return IdenticalSequence();
            yield return NonmonotonicallyDecreasingSequence();
            yield return NonmonotonicallyIncreasingSequence();
        }

        /// <summary>
        /// Returns the default test case.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] BuildTrivialCase()
        {
            return new object[] {new List<NumericalItem> {0}};
        }

        /// <summary>
        /// Returns a monotonically decreasing sequence.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] MonotonicallyDecreasingSequence()
        {
            return new object[]
            {
                new List<NumericalItem>
                {
                    5, 4, 3, 2, 1
                }
            };
        }

        /// <summary>
        /// Returns a monotonically increasing sequence.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] MonotonicallyIncreasingSequence()
        {
            return new object[]
            {
                new List<NumericalItem>
                {
                    5, 4, 3, 2, 1
                }
            };
        }

        /// <summary>
        /// Returns an alternating sequence.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] AlternatingSequence()
        {
            return new object[]
            {
                new List<NumericalItem>
                {
                    0, 1, -1, 2, -2, 3, -3, 4, -4, 5
                }
            };
        }

        /// <summary>
        /// Returns a sequence of identical items.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] IdenticalSequence()
        {
            return new object[]
            {
                new List<NumericalItem>
                {
                    0, 0, 0, 0, 0
                }
            };
        }

        /// <summary>
        /// Returns a sequence of non-monotonically decreasing items.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] NonmonotonicallyDecreasingSequence()
        {
            return new object[]
            {
                new List<NumericalItem>
                {
                    5, 5, 5, 4, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1, 1
                }
            };
        }

        /// <summary>
        /// Returns a sequence of non-monotonically increasing items.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] NonmonotonicallyIncreasingSequence()
        {
            return new object[]
            {
                new List<NumericalItem>
                {
                    1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5
                }
            };
        }
    }
}
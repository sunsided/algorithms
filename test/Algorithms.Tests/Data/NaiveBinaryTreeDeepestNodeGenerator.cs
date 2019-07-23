using System.Collections;
using System.Collections.Generic;
using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Data
{
    /// <summary>
    /// Test case generator for <see cref="Widemeadows.Algorithms.Trees.NaiveBinaryTree{T}" /> tests.
    /// </summary>
    public sealed class NaiveBinaryTreeDeepestNodeGenerator : IEnumerable<object[]>
    {
        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return BuildDefaultCase();
            yield return LeftSkewedTree();
            yield return RightSkewedTree();
            yield return ShortLeftLongRight();
            yield return LongLeftShortRight();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns the default test case.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] BuildDefaultCase()
        {
            //                 A
            //        B                  C
            //   D         E       F           G
            //     H             I   J
            //       K

            var a = new NumericalItem(0);

            // Left subtree of A
            var b = new NumericalItem(-10);

            // Left subtree of B
            var d = new NumericalItem(-15);
            var h = new NumericalItem(-13);
            var k = new NumericalItem(-12);

            // Right subtree of B
            var e = new NumericalItem(-5);

            // Right subtree of A
            var c = new NumericalItem(10);

            // Left subtree of C
            var f = new NumericalItem(5);
            var i = new NumericalItem(3);
            var j = new NumericalItem(7);

            // Right subtree of C
            var g = new NumericalItem(15);

            // Construct the tree
            return new object[] {new List<NumericalItem> {a, b, c, d, e, f, g, h, i, j, k}, k};
        }

        /// <summary>
        /// Returns a left-skewed tree.
        /// </summary>
        /// <returns>The default test case parameters</returns>
        private object[] LeftSkewedTree()
        {
            // Construct the tree
            return new object[]
            {
                new List<NumericalItem>
                {
                    0, -1, -2, -3, -4, -5, -6, -7, -8, -9, -10
                },
                -10
            };
        }

        /// <summary>
        /// Returns a tree with a short left, but a long right arm.
        /// </summary>
        /// <returns>The test case parameters</returns>
        private object[] ShortLeftLongRight()
        {
            // Construct the tree
            return new object[]
            {
                new List<NumericalItem>
                {
                    0, -1, -2, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
                },
                10
            };
        }

        /// <summary>
        /// Returns a tree with a long left, but a short right arm.
        /// </summary>
        /// <returns>The test case parameters</returns>
        private object[] LongLeftShortRight()
        {
            // Construct the tree
            return new object[]
            {
                new List<NumericalItem>
                {
                    0, 1, 2, -1, -2, -3, -4, -5, -6, -7, -8, -9, -10
                },
                -10
            };
        }

        /// <summary>
        /// Returns a right-skewed tree
        /// </summary>
        /// <returns>The test case parameters</returns>
        private object[] RightSkewedTree()
        {
            // Construct the tree
            return new object[]
            {
                new List<NumericalItem>
                {
                    0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
                },
                10
            };
        }
    }
}
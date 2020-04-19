using System.Collections.Generic;
using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Data
{
    /// <summary>
    /// Test case generator for <see cref="Widemeadows.Algorithms.Trees.NaiveBinaryTree{T}" /> tests.
    /// </summary>
    public sealed class NaiveBinaryTreeFullNodeCountGenerator : NaiveBinaryTreeTestCaseGeneratorBase
    {
        private readonly List<NumericalItem> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaiveBinaryTreeFullNodeCountGenerator"/> class.
        /// </summary>
        public NaiveBinaryTreeFullNodeCountGenerator()
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
            _items = new List<NumericalItem>
            {
                a, b, c, d, e, f, g, h, i, j, k
            };
        }

        /// <inheritdoc cref="TestCaseGeneratorBase.GetEnumerator"/>
        public override IEnumerator<object[]> GetEnumerator()
        {
            // Default tree.
            yield return new object[] { _items, 4 };

            // Empty tree.
            yield return new object[] { new NumericalItem[0], 0 };

            // Single-element tree.
            yield return new object[] {new NumericalItem[] {0}, 0};

            // Right-skewed tree
            yield return new object[] {new NumericalItem[] {0, 1, 2, 3, 4, 5, 6}, 0};

            // Left-skewed tree
            yield return new object[] {new NumericalItem[] {0, -1, -2, -3}, 0};

            // Tree with two arms
            yield return new object[] {new NumericalItem[] {0, -1, -2, -3, 1, 2}, 1};

            // Complete tree.
            yield return new object[] {new NumericalItem[] {0, -10, 10, -15, -5, 5, 15}, 3};
        }
    }
}
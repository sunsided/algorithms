using System.Collections.Generic;
using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Data
{
    /// <summary>
    /// Test case generator for <see cref="Widemeadows.Algorithms.Trees.NaiveBinaryTree{T}" /> tests.
    /// </summary>
    public sealed class NaiveBinaryTreeDiameterGenerator : NaiveBinaryTreeTestCaseGeneratorBase
    {
        private readonly List<NumericalItem> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaiveBinaryTreeDiameterGenerator"/> class.
        /// </summary>
        public NaiveBinaryTreeDiameterGenerator()
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
            // K, H, D, B, A, C, F, I
            yield return new object[] { _items, 8 };

            // -5, -10, 0, 100, 50, 25
            yield return new object[] { new NumericalItem[] { 0, -10, -5, 100, 50, 25, 75, 125, 150 }, 6 };

            // 25, 50, 100, 150, 175
            yield return new object[] { new NumericalItem[] { 0, 100, 50, 25, 75, 150, 175 }, 5 };

            // Tree without items.
            yield return new object[] { BuildMonotonicallySkewedTree(0, -1), 0 };

            // Tree with a single item (the root).
            yield return new object[] { BuildSingleElementList(0), 1 };

            // Left-skewed tree.
            yield return new object[] { BuildMonotonicallySkewedTree(2, -1), 2 };
            yield return new object[] { BuildMonotonicallySkewedTree(10, -1), 10 };

            // Right-skewed tree.
            yield return new object[] { BuildMonotonicallySkewedTree(2, 1), 2 };
            yield return new object[] { BuildMonotonicallySkewedTree(10, 1), 10 };

            // Skewed tree
            yield return new object[] { BuildSkewedTree(2, false), 2 };
            yield return new object[] { BuildSkewedTree(2, true), 2 };
            yield return new object[] { BuildSkewedTree(10, false), 10 };
            yield return new object[] { BuildSkewedTree(10, true), 10 };

            // Full tree
            yield return new object[] { BuildBalancedTreeItems(0), 1 };
            yield return new object[] { BuildBalancedTreeItems(1), 3 };
            yield return new object[] { BuildBalancedTreeItems(2), 5 };
            yield return new object[] { BuildBalancedTreeItems(3), 7 };
        }
    }
}
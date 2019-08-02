using System.Collections.Generic;

namespace Widemeadows.Algorithms.Tests.Data
{
    /// <summary>
    /// Test case generator for <see cref="Widemeadows.Algorithms.Trees.NaiveBinaryTree{T}" /> tests.
    /// </summary>
    public sealed class NaiveBinaryTreeGenerator : NaiveBinaryTreeTestCaseGeneratorBase
    {
        /// <inheritdoc cref="TestCaseGeneratorBase.GetEnumerator"/>
        public override IEnumerator<object[]> GetEnumerator()
        {
            // Tree without items.
            yield return new object[] { BuildMonotonicallySkewedTree(0, -1) };

            // Tree with a single item (the root).
            yield return new object[] { BuildSingleElementList(0) };
            yield return new object[] { BuildSingleElementList(int.MinValue + 1) };
            yield return new object[] { BuildSingleElementList(int.MaxValue - 1) };

            // Left-skewed tree.
            yield return new object[] { BuildMonotonicallySkewedTree(2, -1) };
            yield return new object[] { BuildMonotonicallySkewedTree(1000, -1) };

            // Right-skewed tree.
            yield return new object[] { BuildMonotonicallySkewedTree(2, 1) };
            yield return new object[] { BuildMonotonicallySkewedTree(1000, 1) };

            // Skewed tree
            yield return new object[] { BuildSkewedTree(2, false) };
            yield return new object[] { BuildSkewedTree(2, true) };
            yield return new object[] { BuildSkewedTree(1000, false) };
            yield return new object[] { BuildSkewedTree(1000, true) };

            // Full tree
            yield return new object[] { BuildBalancedTreeItems(0) };
            yield return new object[] { BuildBalancedTreeItems(1) };
            yield return new object[] { BuildBalancedTreeItems(2) };
            yield return new object[] { BuildBalancedTreeItems(3) };
        }
    }
}
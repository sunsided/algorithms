using System.Collections.Generic;

namespace Widemeadows.Algorithms.Tests.Data.BinaryTree
{
    /// <summary>
    /// Test case generator for <see cref="Widemeadows.Algorithms.Trees.NaiveBinaryTree{T}" /> tests.
    /// </summary>
    public sealed class NaiveBinaryTreeDivergingStructureGenerator : NaiveBinaryTreeTestCaseGeneratorBase
    {
        /// <inheritdoc cref="TestCaseGeneratorBase.GetEnumerator"/>
        public override IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { BuildSingleElementList(0), BuildMonotonicallySkewedTree(2, -1) };
            yield return new object[] { BuildMonotonicallySkewedTree(2, -1), BuildMonotonicallySkewedTree(100, -1) };
            yield return new object[] { BuildMonotonicallySkewedTree(2, -1), BuildMonotonicallySkewedTree(2, 1) };
            yield return new object[] { BuildMonotonicallySkewedTree(2, 1), BuildMonotonicallySkewedTree(2, -1) };
            yield return new object[] { BuildSkewedTree(2, false), BuildSkewedTree(2, true) };
            yield return new object[] { BuildSkewedTree(2, true), BuildSkewedTree(2, false) };
            yield return new object[] { BuildSkewedTree(1000, false), BuildSkewedTree(1000, true) };
            yield return new object[] { BuildSkewedTree(1000, false), BuildSkewedTree(10, false) };
            yield return new object[] { BuildBalancedTreeItems(0), BuildBalancedTreeItems(1) };
            yield return new object[] { BuildBalancedTreeItems(3), BuildBalancedTreeItems(2) };
        }
    }
}
using System.Collections.Generic;
using Widemeadows.Algorithms.Tests.Model;
using Widemeadows.Algorithms.Trees;

namespace Widemeadows.Algorithms.Tests.Data.BinaryTree
{
    /// <summary>
    /// Base class for <see cref="NaiveBinaryTree{T}"/> test case generators.
    /// </summary>
    public abstract class NaiveBinaryTreeTestCaseGeneratorBase : TestCaseGeneratorBase
    {
        /// <summary>
        /// Builds a list consisting of a single item.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <returns>The list of items.</returns>
        protected IList<NumericalItem> BuildSingleElementList(int value) => new List<NumericalItem> { new NumericalItem(value) };

        /// <summary>
        /// Builds items for a left- or right-skewed tree.
        /// </summary>
        /// <param name="count">The number of items.</param>
        /// <param name="sign">
        ///     The sign of the items; <c>-1</c> implies a left-skewed tree,
        ///     while <c>1</c> builds a right-skewed one.
        /// </param>
        /// <returns>The list of items.</returns>
        protected IList<NumericalItem> BuildMonotonicallySkewedTree(int count, int sign)
        {
            var items = new List<NumericalItem>(count);
            for (var i = 0; i < count; ++i)
            {
                items.Add(new NumericalItem(sign * i));
            }

            return items;
        }

        /// <summary>
        /// Builds items for a skewed tree.
        /// </summary>
        /// <param name="count">The number of items.</param>
        /// <param name="flip">Whether to flip the ordering of the elements.</param>
        /// <returns>The list of items.</returns>
        protected IList<NumericalItem> BuildSkewedTree(int count, bool flip)
        {
            var items = new List<NumericalItem>(count);
            for (var i = 0; i < count; ++i)
            {
                var item = i % 2 == (flip ? 0 : 1)
                    ? int.MaxValue - i - 1
                    : int.MinValue + i + 1;
                items.Add(new NumericalItem(item));
            }

            return items;
        }

        /// <summary>
        /// Builds items for a full binary tree.
        /// </summary>
        /// <param name="depth">The depth of the tree.</param>
        /// <returns>The list of items.</returns>
        protected IList<NumericalItem> BuildBalancedTreeItems(int depth) => Bisect(int.MinValue + 1, int.MaxValue - 1, depth);

        /// <summary>
        /// Builds a full binary tree by bisecting the available value range.
        /// </summary>
        /// <param name="min">The minimum value to consider.</param>
        /// <param name="max">The maximum value to consider.</param>
        /// <param name="remaining">The number of remaining levels to create.</param>
        /// <returns>The list of nodes obtained by bisecting the left and right sub-trees.</returns>
        private static IList<NumericalItem> Bisect(int min, int max, int remaining)
        {
            var pivot = (int)(((long)max + min) / 2);
            var list = new List<NumericalItem> { new NumericalItem(pivot) };
            if (remaining > 0)
            {
                list.AddRange(Bisect(min, pivot - 1, remaining - 1));
                list.AddRange(Bisect(pivot + 1, max, remaining - 1));
            }

            return list;
        }
    }
}
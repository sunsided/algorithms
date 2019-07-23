using System.Collections.Generic;
using JetBrains.Annotations;
using Widemeadows.Algorithms.Tests.Model;

namespace Widemeadows.Algorithms.Tests.Data
{
    /// <summary>
    /// Test case generator for <see cref="Widemeadows.Algorithms.Trees.NaiveBinaryTree{T}" /> tests.
    /// </summary>
    public sealed class NaiveBinaryTreeValueGenerator : TestCaseGeneratorBase
    {
        /// <inheritdoc cref="TestCaseGeneratorBase.GetEnumerator"/>
        public override IEnumerator<object[]> GetEnumerator()
        {
            // Tree without items.
            yield return new object[] { BuildMonotonicallySkewedTree(0, -1), 0, -1 };

            // Tree with a single item (the root).
            yield return new object[] { BuildSingleElementList(0), 1, 0 };
            yield return new object[] { BuildSingleElementList(int.MinValue), 1, 0 };
            yield return new object[] { BuildSingleElementList(int.MaxValue), 1, 0 };

            // Left-skewed tree.
            yield return new object[] { BuildMonotonicallySkewedTree(2, -1), 2, 1 };
            yield return new object[] { BuildMonotonicallySkewedTree(1000, -1), 1000, 999 };

            // Right-skewed tree.
            yield return new object[] { BuildMonotonicallySkewedTree(2, 1), 2, 1 };
            yield return new object[] { BuildMonotonicallySkewedTree(1000, 1), 1000, 999 };

            // Skewed tree
            yield return new object[] { BuildSkewedTree(2, false), 2, 1 };
            yield return new object[] { BuildSkewedTree(2, true), 2, 1 };
            yield return new object[] { BuildSkewedTree(1000, false), 1000, 999 };
            yield return new object[] { BuildSkewedTree(1000, true), 1000, 999 };

            // Full tree
            yield return new object[] { BuildBalancedTreeItems(0), 1, 0 };
            yield return new object[] { BuildBalancedTreeItems(1), 3, 1 };
            yield return new object[] { BuildBalancedTreeItems(2), 7, 2 };
            yield return new object[] { BuildBalancedTreeItems(3), 15, 3 };
        }

        /// <summary>
        /// Builds a list consisting of a single item.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <returns>The list of items.</returns>
        [NotNull]
        private IList<NumericalItem> BuildSingleElementList(int value) => new List<NumericalItem> { new NumericalItem(value) };

        /// <summary>
        /// Builds items for a left- or right-skewed tree.
        /// </summary>
        /// <param name="count">The number of items.</param>
        /// <param name="sign">
        ///     The sign of the items; <c>-1</c> implies a left-skewed tree,
        ///     while <c>1</c> builds a right-skewed one.
        /// </param>
        /// <returns>The list of items.</returns>
        [NotNull]
        private IList<NumericalItem> BuildMonotonicallySkewedTree(int count, int sign)
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
        [NotNull]
        private IList<NumericalItem> BuildSkewedTree(int count, bool flip)
        {
            var items = new List<NumericalItem>(count);
            for (var i = 0; i < count; ++i)
            {
                var item = i % 2 == (flip ? 0 : 1)
                    ? int.MaxValue - i
                    : int.MinValue + i;
                items.Add(new NumericalItem(item));
            }

            return items;
        }

        /// <summary>
        /// Builds items for a full binary tree.
        /// </summary>
        /// <param name="depth">The depth of the tree.</param>
        /// <returns>The list of items.</returns>
        [NotNull]
        private IList<NumericalItem> BuildBalancedTreeItems(int depth) => Bisect(int.MinValue, int.MaxValue, depth);

        /// <summary>
        /// Builds a full binary tree by bisecting the available value range.
        /// </summary>
        /// <param name="min">The minimum value to consider.</param>
        /// <param name="max">The maximum value to consider.</param>
        /// <param name="remaining">The number of remaining levels to create.</param>
        /// <returns>The list of nodes obtained by bisecting the left and right sub-trees.</returns>
        [NotNull]
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
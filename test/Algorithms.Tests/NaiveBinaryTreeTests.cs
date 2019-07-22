using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using Widemeadows.Algorithms.Tests.Data;
using Widemeadows.Algorithms.Tests.Model;
using Widemeadows.Algorithms.Trees;
using Xunit;

namespace Widemeadows.Algorithms.Tests
{
    public class NaiveBinaryTreeTests
    {
        [NotNull]
        private readonly Random _random = new Random();

        [NotNull]
        private readonly NaiveBinaryTree<NumericalItem> _tree;

        public NaiveBinaryTreeTests()
        {
            _tree = new NaiveBinaryTree<NumericalItem>();
        }

        /// <summary>
        /// Creates a new <see cref="NumericalItem"/> instance.
        /// </summary>
        /// <returns>A new <see cref="NumericalItem"/>.</returns>
        private NumericalItem RandomItem => new NumericalItem(_random.Next());

        [Fact]
        public void EmptyTreeIsSizeZero()
        {
            _tree.GetSize().Should().Be(0, "because the tree is empty");
        }

        [Fact]
        public void EmptyTreeHasNoHeight()
        {
            var hasHeight = _tree.TryGetHeight(out _);
            hasHeight.Should().BeFalse("because the tree is empty");
        }

        [Fact]
        public void SizeAfterInsertingOnceIsOne()
        {
            _tree.Insert(RandomItem);
            _tree.GetSize().Should().Be(1, "because the tree contains exactly one element");
        }

        [Fact]
        public void HeightAfterInsertingOnceIsOne()
        {
            _tree.Insert(RandomItem);
            var hasHeight = _tree.TryGetHeight(out var height);
            hasHeight.Should().BeTrue("because the tree is nonempty");
            height.Should().Be(0, "because the tree has exactly one element");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeValueGenerator))]
        public void SizeAndHeightIsCorrect([NotNull] IList<NumericalItem> items, int expectedSize, int expectedHeight)
        {
            var hasItems = items.Count > 0;
            foreach (var item in items)
            {
                _tree.Insert(item);
            }

            _tree.GetSize().Should().Be(expectedSize, "because we added {0} items", items.Count);
            _tree.TryGetHeight(out var height).Should().Be(hasItems, "because we added {0} items", items.Count);
            height.Should().Be(expectedHeight, "because the tree was constructed that way");
        }

        [Theory]
        [InlineData(10000)]
        public void SizeOfRandomlyBuiltTreeIsNumberOfItems(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                _tree.Insert(RandomItem);
            }

            _tree.GetSize().Should().Be(count, "because we added {0} items", count);
        }

        [Theory]
        [InlineData(10000)]
        public void HeightOfRandomlyBuiltTreeIsAtMostNumberOfItemsMinusOne(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                _tree.Insert(RandomItem);
            }

            _tree.TryGetHeight(out var height).Should().BeTrue("because the tree has items");
            height.Should().BeInRange(0, count - 1, "because the height must be less than the size");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeTraversalGenerator))]
        public void TreeTraversalVisitsNodesInCorrectOrder([NotNull] IList<NumericalItem> items, TraversalMode mode, [NotNull] IList<NumericalItem> expectedItems)
        {
            foreach (var item in items)
            {
                _tree.Insert(item);
            }

            var traversedItems = _tree.Traverse(mode).ToList();
            traversedItems.Should().ContainInOrder(expectedItems, "because the nodes are expected to be traversed in this order");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeTraversalGenerator))]
        public void RecursiveTreeTraversalVisitsNodesInCorrectOrder([NotNull] IList<NumericalItem> items, TraversalMode mode, [NotNull] IList<NumericalItem> expectedItems)
        {
            foreach (var item in items)
            {
                _tree.Insert(item);
            }

            var traversedItems = _tree.TraverseRecursively(mode).ToList();
            traversedItems.Should().ContainInOrder(expectedItems, "because the nodes are expected to be traversed in this order");
        }

        [Theory]
        [InlineData(TraversalMode.PreOrder)]
        [InlineData(TraversalMode.InOrder)]
        [InlineData(TraversalMode.PostOrder)]
        [InlineData(TraversalMode.LevelOrder)]
        public void EmptyTreeDoesntTraverseAnyNodes(TraversalMode mode)
        {
            var traversedItems = _tree.Traverse(mode).ToList();
            traversedItems.Should().BeEmpty("because no items were added to the list");
        }

        [Theory]
        [InlineData(TraversalMode.PreOrder)]
        [InlineData(TraversalMode.InOrder)]
        [InlineData(TraversalMode.PostOrder)]
        [InlineData(TraversalMode.LevelOrder)]
        public void EmptyTreeDoesntTraverseAnyNodesRecursively(TraversalMode mode)
        {
            var traversedItems = _tree.TraverseRecursively(mode).ToList();
            traversedItems.Should().BeEmpty("because no items were added to the list");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(4)]
        public void InvalidTraversalModeThrowsException(int mode)
        {
            Action action = () => _tree.Traverse((TraversalMode)mode);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>("because no items were added to the list");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(4)]
        public void InvalidRecursiveTraversalModeThrowsException(int mode)
        {
            Action action = () => _tree.TraverseRecursively((TraversalMode)mode);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>("because no items were added to the list");
        }

        [Fact]
        public void DepthFirstTraversalIsPostOrder()
        {
            TraversalMode.DepthFirst.Should().Be(TraversalMode.PostOrder, "because the traversal modes are identical");
        }

        [Fact]
        public void BreadthFirstTraversalIsLevelOrder()
        {
            TraversalMode.BreadthFirst.Should().Be(TraversalMode.LevelOrder, "because the traversal modes are identical");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void InsertedItemsCanBeFound(int count)
        {
            var items = new List<NumericalItem>(count);
            for (var i = 0; i < count; ++i)
            {
                var item = RandomItem;
                items.Add(item);
                _tree.Insert(item);
            }

            var random = new Random();
            foreach (var item in items.OrderBy(x => random.Next()))
            {
                _tree.Contains(item).Should().BeTrue("because the item was added before");
            }
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(1000, 10)]
        public void NotInsertedItemsCantBeFound(int count, int queryCount)
        {
            var set = new HashSet<NumericalItem>(count);
            for (var i = 0; i < count; ++i)
            {
                var item = RandomItem;
                set.Add(item);
                _tree.Insert(item);
            }

            for (var i = 0; i < queryCount; ++i)
            {
                NumericalItem query;
                do
                {
                    query = RandomItem;
                } while (set.Contains(query));

                _tree.Contains(query).Should().BeFalse("because the item was never inserted");
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void InOrderTraversalIsInAscendingOrder(int count)
        {
            var items = new List<NumericalItem>(count);
            for (var i = 0; i < count; ++i)
            {
                var item = RandomItem;
                items.Add(item);
                _tree.Insert(item);
            }

            items.Sort();

            _tree.Traverse(TraversalMode.InOrder)
                .Should()
                .ContainInOrder(items, "because we expect in-order traversal to be in ascending order");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void EnumeratingTreeIsSameAsInOrderTraversal(int count)
        {
            var items = new List<NumericalItem>(count);
            for (var i = 0; i < count; ++i)
            {
                var item = RandomItem;
                items.Add(item);
                _tree.Insert(item);
            }

            items.Sort();

            _tree.Should().ContainInOrder(items, "because we expect in-order traversal to be in ascending order");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void MinimumElementIsSmallestFromSource(int count)
        {
            var items = new List<NumericalItem>(count);
            for (var i = 0; i < count; ++i)
            {
                var item = RandomItem;
                items.Add(item);
                _tree.Insert(item);
            }

            items.Sort();

            _tree.GetSmallest().Should().Be(items.First(), "because this is the smallest inserted item");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void MaximumElementIsSmallestFromSource(int count)
        {
            var items = new List<NumericalItem>(count);
            for (var i = 0; i < count; ++i)
            {
                var item = RandomItem;
                items.Add(item);
                _tree.Insert(item);
            }

            items.Sort();

            _tree.GetLargest().Should().Be(items.Last(), "because this is the smallest inserted item");
        }
    }
}
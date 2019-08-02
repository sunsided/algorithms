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
            _tree.CalculateSize().Should().Be(0, "because the tree is empty");
            _tree.CalculateSizeRecursively().Should().Be(0, "because the tree is empty");
            _tree.Count.Should().Be(0, "because the tree is empty");
        }

        [Fact]
        public void AddingANullItemThrows()
        {
            var tree = new NaiveBinaryTree<ReferenceItem<NumericalItem>>();
            Action action = () => tree.Add(null);
            action.Should().Throw<ArgumentNullException>("because the argument was null");
        }

        [Fact]
        public void AddingANonNullSequenceDoesntThrow()
        {
            var tree = new NaiveBinaryTree<ReferenceItem<NumericalItem>>();
            Action action = () => tree.AddRange(Enumerable.Range(0, 10).Select(x => new ReferenceItem<NumericalItem>(RandomItem)));
            action.Should().NotThrow("because all elements were valid");
        }

        [Fact]
        public void AddingPartiallyNullElementSequenceThrows()
        {
            const int count = 10;
            var tree = new NaiveBinaryTree<ReferenceItem<NumericalItem>>();
            Action action = () => tree.AddRange(Enumerable.Range(0, count).Select((x, i) => i <= count / 2 ? new ReferenceItem<NumericalItem>(RandomItem) : null));
            action.Should().Throw<ArgumentNullException>("because some arguments were null");
        }

        [Fact]
        public void AddingNullSequenceThrows()
        {
            var tree = new NaiveBinaryTree<ReferenceItem<NumericalItem>>();
            Action action = () => tree.AddRange(null);
            action.Should().Throw<ArgumentNullException>("because some arguments were null");
        }

        [Fact]
        public void EmptyTreeHasNoHeight()
        {
            Action action = () => _tree.CalculateHeight();
            action.Should().ThrowExactly<InvalidOperationException>("because the tree is empty");
        }

        [Fact]
        public void EmptyTreeHeightPropertyThrows()
        {
            Func<int> func = () => _tree.Height;
            func.Should().ThrowExactly<InvalidOperationException>("because the tree is empty");
        }

        [Fact]
        public void SizeAfterInsertingOnceIsOne()
        {
            _tree.Add(RandomItem);
            _tree.CalculateSize().Should().Be(1, "because the tree contains exactly one element");
            _tree.CalculateSizeRecursively().Should().Be(1, "because the tree contains exactly one element");
            _tree.Count.Should().Be(1, "because the tree contains exactly one element");
        }

        [Fact]
        public void HeightAfterInsertingOnceIsOne()
        {
            _tree.Add(RandomItem);
            _tree.CalculateHeight().Should().Be(0, "because the tree has exactly one element");
            _tree.Height.Should().Be(0, "because the tree has exactly one element");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeValueGenerator))]
        public void SizeAndHeightIsCorrect([NotNull] IList<NumericalItem> items, int expectedSize, int expectedHeight)
        {
            var hasItems = items.Count > 0;
            _tree.AddRange(items);

            _tree.CalculateSize().Should().Be(expectedSize, "because we added {0} items", items.Count);
            _tree.CalculateSizeRecursively().Should().Be(expectedSize, "because we added {0} items", items.Count);
            _tree.Count.Should().Be(expectedSize, "because we added {0} items", items.Count);

            if (hasItems)
            {
                _tree.CalculateHeight().Should().Be(expectedHeight, "because the tree was constructed that way");
                _tree.Height.Should().Be(expectedHeight, "because the tree was constructed that way");
            }
            else
            {
                Action action = () => _tree.CalculateHeight();
                action.Should().ThrowExactly<InvalidOperationException>("because the tree has no elements");

                Func<int> func = () => _tree.Height;
                func.Should().ThrowExactly<InvalidOperationException>("because the tree has no elements");
            }
        }

        [Theory]
        [InlineData(10000)]
        public void SizeOfRandomlyBuiltTreeIsNumberOfItems(int count)
        {
            _tree.AddRange(Enumerable.Range(0, count).Select(x => RandomItem));
            _tree.CalculateSize().Should().Be(count, "because we added {0} items", count);
            _tree.CalculateSizeRecursively().Should().Be(count, "because we added {0} items", count);
            _tree.Count.Should().Be(count, "because we added {0} items", count);
        }

        [Theory]
        [InlineData(10000)]
        public void HeightOfRandomlyBuiltTreeIsAtMostNumberOfItemsMinusOne(int count)
        {
            _tree.AddRange(Enumerable.Range(0, count).Select(x => RandomItem));
            _tree.CalculateHeight().Should().BeInRange(0, count - 1, "because the height must be less than the size");
            _tree.Height.Should().BeInRange(0, count - 1, "because the height must be less than the size");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeTraversalGenerator))]
        public void TreeTraversalVisitsNodesInCorrectOrder([NotNull] IList<NumericalItem> items, TraversalMode mode, [NotNull] IList<NumericalItem> expectedItems)
        {
            _tree.AddRange(items);
            var traversedItems = _tree.Traverse(mode).ToList();
            traversedItems.Should().ContainInOrder(expectedItems, "because the nodes are expected to be traversed in this order");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeTraversalGenerator))]
        public void RecursiveTreeTraversalVisitsNodesInCorrectOrder([NotNull] IList<NumericalItem> items, TraversalMode mode, [NotNull] IList<NumericalItem> expectedItems)
        {
            _tree.AddRange(items);
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
                _tree.Add(item);
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
                _tree.Add(item);
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
                _tree.Add(item);
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
                _tree.Add(item);
            }

            items.Sort();

            _tree.Should().ContainInOrder(items, "because we expect in-order traversal to be in ascending order");
        }

        [Fact]
        public void MinimumElementCantBeObtainedIfTreeIsEmpty()
        {
            Action action = () => _tree.GetSmallest();
            action.Should().ThrowExactly<InvalidOperationException>("because the tree is empty");
        }

        [Fact]
        public void MaximumElementCantBeObtainedIfTreeIsEmpty()
        {
            Action action = () => _tree.GetLargest();
            action.Should().ThrowExactly<InvalidOperationException>("because the tree is empty");
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
                _tree.Add(item);
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
                _tree.Add(item);
            }

            items.Sort();

            _tree.GetLargest().Should().Be(items.Last(), "because this is the smallest inserted item");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1000)]
        public void DeletingTheTreeRemovesAllItems(int count)
        {
            _tree.AddRange(Enumerable.Range(0, count).Select(x => RandomItem));
            _tree.Clear();
            _tree.Should().BeEmpty("because we cleared the tree");
            _tree.Count.Should().Be(0, "because an empty tree has no elements");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeDeepestNodeGenerator))]
        public void TreeGetDeepestNodeFindsDeepestNode([NotNull] IList<NumericalItem> items, NumericalItem expectedItem)
        {
            _tree.AddRange(items);
            var deepestNode = _tree.GetDeepestNode();
            deepestNode.Should().Be(expectedItem, "because that is the deepest node");
        }

        [Fact]
        public void GetDeepestNodeOnEmptyTreeThrows()
        {
            Action action = () => _tree.GetDeepestNode();
            action.Should().ThrowExactly<InvalidOperationException>("because the tree is empty");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeLeafTraversalGenerator))]
        public void EnumerateLeavesEnumeratesLeavesInAscendingOrder([NotNull] IList<NumericalItem> items, [NotNull] IList<NumericalItem> expectedItems)
        {
            _tree.AddRange(items);
            _tree.TraverseLeaves().Should().ContainInOrder(expectedItems, "because these are the leaves of the tree");
        }

        [Fact]
        public void EnumerateLeavesOnEmptyTreeEnumeratesNothing()
        {
            _tree.TraverseLeaves().Should().BeEmpty("because the tree is empty");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeLeafCountGenerator))]
        public void NumberOfLeavesIsDeterminedCorrectly([NotNull] IList<NumericalItem> items, int expectedCount)
        {
            _tree.AddRange(items);
            _tree.CalculateNumberOfLeaves().Should().Be(expectedCount, "because this is the number of leaves in the tree");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeFullNodeCountGenerator))]
        public void NumberOfFullNodesIsDeterminedCorrectly([NotNull] IList<NumericalItem> items, int expectedCount)
        {
            _tree.AddRange(items);
            _tree.CalculateNumberOfFullNodes().Should().Be(expectedCount, "because this is the number of full nodes in the tree");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeHalfNodeCountGenerator))]
        public void NumberOfHalfNodesIsDeterminedCorrectly([NotNull] IList<NumericalItem> items, int expectedCount)
        {
            _tree.AddRange(items);
            _tree.CalculateNumberOfHalfNodes().Should().Be(expectedCount, "because this is the number of half nodes in the tree");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1000)]
        public void SumOfLeafHalfAndFullNodesIsNumberOfNodes(int count)
        {
            _tree.AddRange(Enumerable.Range(0, count).Select(x => RandomItem));

            var sumOfNodes = _tree.CalculateNumberOfLeaves() +
                             _tree.CalculateNumberOfHalfNodes() +
                             _tree.CalculateNumberOfFullNodes();

            sumOfNodes.Should().Be(_tree.Count, "because the sum of the node types should be equal to the count");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeGenerator))]
        public void TreeIsNeverStructurallyIdenticalToNullUnlessSourceTreeHasNoItems([NotNull] IList<NumericalItem> items)
        {
            _tree.AddRange(items);
            var structurallyEqual = _tree.IsStructurallyIdenticalTo(null);

            if (_tree.Count > 0)
            {
                structurallyEqual.Should().BeFalse("because the other tree is null");
            }
            else
            {
                structurallyEqual.Should().BeTrue("because the source tree has a nul lroot");
            }
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeGenerator))]
        public void TwoTreesAreStructurallyTheSameIfTheTreesAreIdentical([NotNull] IList<NumericalItem> items)
        {
            _tree.AddRange(items);
            var structurallyEqual = _tree.IsStructurallyIdenticalTo(_tree);
            structurallyEqual.Should().BeTrue("because we're comparing the tree to itself");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeGenerator))]
        public void TwoTreesAreStructurallyTheSameIfTheTreesHaveTheSameShape([NotNull] IList<NumericalItem> items)
        {
            _tree.AddRange(items);
            var otherTree = new NaiveBinaryTree<NumericalItem>();
            otherTree.AddRange(items.Select(x => new NumericalItem(x.Value + 1)));

            var structurallyEqual = _tree.IsStructurallyIdenticalTo(otherTree);
            structurallyEqual.Should().BeTrue("because we're comparing the same trees with shifted values");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeDivergingStructureGenerator))]
        public void TwoTreesAreStructurallyDifferentIfTheTreesDontShareTheSameShape([NotNull] IList<NumericalItem> items, [NotNull] IList<NumericalItem> otherItems)
        {
            _tree.AddRange(items);
            var otherTree = new NaiveBinaryTree<NumericalItem>();
            otherTree.AddRange(otherItems);

            var structurallyEqual = _tree.IsStructurallyIdenticalTo(otherTree);
            structurallyEqual.Should().BeFalse("because both trees are different");
        }

        [Theory]
        [ClassData(typeof(NaiveBinaryTreeDiameterGenerator))]
        public void DiameterOfTreeIsCalculatedCorrectly([NotNull] IList<NumericalItem> items, int expectedValue)
        {
            _tree.AddRange(items);

            var diameter = _tree.CalculateDiameter();
            diameter.Should().Be(expectedValue, "because that is the expected value");
        }
    }
}
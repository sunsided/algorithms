using System;
using System.Collections.Generic;
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
    }
}
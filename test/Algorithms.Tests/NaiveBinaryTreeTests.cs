using System;
using FluentAssertions;
using JetBrains.Annotations;
using Widemeadows.Algorithms.Tests.Data;
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
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public void SizeOfLeftSkewedTreeIsNumberOfItems(int count)
        {
            // Arrange left-skewed tree by generating monotonically decreasing item values.
            for (var i = 0; i < count; ++i)
            {
                _tree.Insert(new NumericalItem(-i));
            }

            _tree.GetSize().Should().Be(count, "because we added {0} items", count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public void SizeOfRightSkewedTreeIsNumberOfItems(int count)
        {
            // Arrange right-skewed tree by generating monotonically decreasing item values.
            for (var i = 0; i < count; ++i)
            {
                _tree.Insert(new NumericalItem(i));
            }

            _tree.GetSize().Should().Be(count, "because we added {0} items", count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(1000)]
        public void SizeOfRandomlyBuiltTreeIsNumberOfItems(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                _tree.Insert(RandomItem);
            }

            _tree.GetSize().Should().Be(count, "because we added {0} items", count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void HeightOfLeftSkewedTreeIsNumberOfItemsMinusOne(int count)
        {
            // Arrange left-skewed tree by generating monotonically decreasing item values.
            for (var i = 0; i < count; ++i)
            {
                _tree.Insert(new NumericalItem(-i));
            }

            _tree.GetSize().Should().Be(count, "because we added this many items");

            _tree.TryGetHeight(out var height).Should().BeTrue("because the tree has items");
            height.Should().Be(count - 1, "because the tree is skewed and the root is at depth 0");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void HeightOfRightSkewedTreeIsNumberOfItemsMinusOne(int count)
        {
            // Arrange right-skewed tree by generating monotonically decreasing item values.
            for (var i = 0; i < count; ++i)
            {
                _tree.Insert(new NumericalItem(i));
            }

            _tree.GetSize().Should().Be(count, "because we added this many items");

            _tree.TryGetHeight(out var height).Should().BeTrue("because the tree has items");
            height.Should().Be(count - 1, "because the tree is skewed and the root is at depth 0");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(1000)]
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(1000)]
        public void HeightOfSkewedTreeIsNumberOfItemsMinusOne(int count)
        {
            // Arrange generic skewed tree by generating monotonically decreasing item values.
            for (var i = 0; i < count; ++i)
            {
                var item = i % 2 == 0
                    ? int.MaxValue - i
                    : int.MinValue + i;
                _tree.Insert(new NumericalItem(item));
            }

            _tree.GetSize().Should().Be(count, "because we added this many items");

            _tree.TryGetHeight(out var height).Should().BeTrue("because the tree has items");
            height.Should().Be(count - 1, "because the tree is skewed and the root is at depth 0");
        }
    }
}
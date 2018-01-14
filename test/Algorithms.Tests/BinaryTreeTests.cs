using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Widemeadows.Algorithms.Tests
{
    /// <summary>
    /// Tests the <see cref="BinaryTree{TElement}"/> algorithm.
    /// </summary>
    public class BinaryTreeTests
    {
        /// <summary>
        /// A list of test items.
        /// </summary>
        [NotNull]
        private static readonly int[] Values = { 50, 30, 0, 10, 15, 30, 90, 100, 0 };
        
        /// <summary>
        /// Ensures that the number of items in the tree increases correctly when items are added.
        /// </summary>
        [Fact]
        public void AddingManyItemsIncreasesCount()
        {
            // arrange
            var tree = new BinaryTree<int>();
            
            // act
            tree.AddRange(Values);
            
            // assert
            tree.Count.Should().Be(Values.Length, "because this is the number of items we added");
        }
        
        /// <summary>
        /// Ensures that items can be added individually and the count increases accordingly.
        /// </summary>
        [Fact]
        public void AddingAnItemIncreasesCount()
        {
            // arrange
            var tree = new BinaryTree<int>();
            
            // act
            tree.Add(0);
            
            // assert
            tree.Count.Should().Be(1, "because we added one item");
        }
        
        /// <summary>
        /// Ensures that items can be removed correctly.
        /// </summary>
        [Theory]
        [InlineData(50)]
        [InlineData(0)]
        [InlineData(15)]
        [InlineData(100)]
        public void RemoveRemovesAnItemIfItExists(int item)
        {
            // arrange
            var tree = new BinaryTree<int>();
            tree.AddRange(Values);
            var count = tree.Count;
            
            // act
            var removed = tree.Remove(item);
            
            // assert
            removed.Should().BeTrue("because the item was removed.");
            tree.Count.Should().Be(count - 1, "because the item was removed");
        }
        
        /// <summary>
        /// Ensures that the tree is empty when all items are removed.
        /// </summary>
        [Fact]
        public void RemovingTheLastItemResultsInEmptyTree()
        {
            // arrange
            var tree = new BinaryTree<int>();
            var values = new[] {50, 25, 75, 12, 37, 63, 87, 0, 100};
            tree.AddRange(values);
            
            // act
            var removed = true;
            foreach (var value in values)
            {
                removed &= tree.Remove(value);
            }

            // assert
            removed.Should().BeTrue("because all items were removed.");
            tree.Count.Should().Be(0, "because all items were removed");
        }
        
        /// <summary>
        /// Ensures that the enumerated items are in ascending order.
        /// </summary>
        [Fact]
        public void EnumerateItemsAreInAscendingOrder()
        {
            // arrange
            var tree = new BinaryTree<int>();
            tree.AddRange(Values);
            
            // act
            var items = tree.Enumerate();
            
            // assert
            items.Should().BeInAscendingOrder("because this is what we expect of this method");
        }
        
        /// <summary>
        /// Ensures that no items are iterated when the tree is empty.
        /// </summary>
        [Fact]
        public void EnumerateItemsAreEmptyIfTreeIsEmpty()
        {
            // arrange
            var tree = new BinaryTree<int>();
            
            // act
            var items = tree.Enumerate();
            
            // assert
            items.Should().BeEmpty("because no items were added");
        }
        
        /// <summary>
        /// Ensures that the enumerated items are in ascending order.
        /// </summary>
        [Fact]
        public void EnumerateRecursiveItemsAreInAscendingOrder()
        {
            // arrange
            var tree = new BinaryTree<int>();
            tree.AddRange(Values);
            
            // act
            var items = tree.EnumerateRecursive();
            
            // assert
            items.Should().BeInAscendingOrder("because this is what we expect of this method");
        }
        
        /// <summary>
        /// Ensures that no items are iterated when the tree is empty.
        /// </summary>
        [Fact]
        public void EnumerateRecursiveItemsAreEmptyIfTreeIsEmpty()
        {
            // arrange
            var tree = new BinaryTree<int>();
            
            // act
            var items = tree.EnumerateRecursive();
            
            // assert
            items.Should().BeEmpty("because no items were added");
        }
        
        /// <summary>
        /// Ensures that the <see cref="BinaryTree{T}.Contains"/> method
        /// returns <see langword="true"/> for items we know to exist in the list.
        /// </summary>
        [Theory]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(0)]
        public void ContainsReturnsTrueForItemsInTheTree(int item)
        {
            // arrange
            var tree = new BinaryTree<int>();
            tree.AddRange(Values);
            
            // act
            var outcome = tree.Contains(item);
            
            // assert
            outcome.Should().BeTrue("because we added the item to the tree");
        }
        
        /// <summary>
        /// Ensures that the <see cref="BinaryTree{T}.Contains"/> method
        /// returns <see langword="true"/> for items we know to exist in the list.
        /// </summary>
        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void ContainsReturnsFalseForItemsInTheTree(int item)
        {
            // arrange
            var tree = new BinaryTree<int>();
            tree.AddRange(Values);
            
            // act
            var outcome = tree.Contains(item);
            
            // assert
            outcome.Should().BeFalse("because we did not add the item in question");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Widemeadows.Algorithms.Tests
{
    /// <summary>
    /// Tests the <see cref="Quickselect{TElement}"/> algorithm.
    /// </summary>
    public class QuickselectTests
    {
        /// <summary>
        /// The length of the list
        /// </summary>
        private const int Length = 100;

        /// <summary>
        /// The list to test against
        /// </summary>
        [NotNull]
        private readonly List<double> _list;

        /// <summary>
        /// Initializes an instance of the <see cref="QuickselectTests"/> class.
        /// </summary>
        public QuickselectTests()
        {
            var random = new Random();
            var list = new List<double>(Length);
            for (var i = 0; i < Length; ++i)
            {
                list.Add(random.NextDouble());
            }
            _list = list;
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the smallest element
        /// </summary>
        [Fact]
        public void RecursiveQuickSelectFindsSmallestElement()
        {
            // arrange
            var list = _list;
            var qs = new Quickselect<double>();

            // act
            var smallestIndex = qs.SelectRecursive(list, 0);

            // assert
            smallestIndex.Should().Be(0, "because quickselect returns a partially ordered list left of the returned index which is smaller - and that list must be empty for the smallest element");
            var smallestElement = list[smallestIndex];
            smallestElement.Should().Be(list.Min(), "because that is the smallest element");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the second-smallest element
        /// </summary>
        [Fact]
        public void RecursiveQuickSelectFindsSecondSmallestElement()
        {
            // arrange
            var list = _list;
            var qs = new Quickselect<double>();

            // act
            var secondSmallestIndex = qs.SelectRecursive(list, 1);

            // assert
            secondSmallestIndex.Should().Be(1, "because quickselect returns a partially ordered list left of the returned index which is smaller - and there must only be one element to the left");
            var secondSmallestElement = list[secondSmallestIndex];
            secondSmallestElement.Should().BeGreaterThan(list[0], "because the element left of the returned index should be smaller");
            secondSmallestElement.Should().BeLessOrEqualTo(list[2], "because the elements right of the returned index should be greater or equal");
            list.Skip(2).Should().OnlyContain(value => value >= secondSmallestElement, "because all elements right of the returned index should be greater or equal");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the n-th smallest element
        /// </summary>
        [Fact]
        public void RecursiveQuickSelectFindsNthSmallestElement()
        {
            // arrange
            var list = _list;
            var qs = new Quickselect<double>();
            const int n = Length / 2;

            // act
            var index = qs.SelectRecursive(list, n);

            // assert
            index.Should().BeInRange(0, Length - 1, "because we expect the result to be a list index");
            var element = list[index];
            list.Take(n).Should().OnlyContain(value => value < element, "because all elements left of the returned index should be smaller");
            list.Skip(n).Should().OnlyContain(value => value >= element, "because all elements right of the returned index should be greater or equal");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the smallest element
        /// </summary>
        [Fact]
        public void RecursiveQuickSelectFindsGreatestElement()
        {
            // arrange
            var list = _list;
            var qs = new Quickselect<double>();

            // act
            var smallestIndex = qs.SelectRecursive(list, Length - 1);
            smallestIndex.Should().Be(Length - 1, "because quickselect returns a partially ordered list right of the returned index which is greater or equal - and that list must be empty for the greatest element");

            // assert
            var smallestElement = list[smallestIndex];
            smallestElement.Should().Be(list.Max(), "because that is the greatest element");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the smallest element
        /// </summary>
        [Fact]
        public void QuickSelectFindsSmallestElement()
        {
            // arrange
            var list = _list;
            var qs = new Quickselect<double>();

            // act
            var smallestIndex = qs.Select(list, 0);

            // assert
            smallestIndex.Should().Be(0, "because quickselect returns a partially ordered list left of the returned index which is smaller - and that list must be empty for the smallest element");
            var smallestElement = list[smallestIndex];
            smallestElement.Should().Be(list.Min(), "because that is the smallest element");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the second-smallest element
        /// </summary>
        [Fact]
        public void QuickSelectFindsSecondSmallestElement()
        {
            // arrange
            var list = _list;
            var qs = new Quickselect<double>();

            // act
            var secondSmallestIndex = qs.Select(list, 1);

            // assert
            secondSmallestIndex.Should().Be(1, "because quickselect returns a partially ordered list left of the returned index which is smaller - and there must only be one element to the left");
            var secondSmallestElement = list[secondSmallestIndex];
            secondSmallestElement.Should().BeGreaterThan(list[0], "because the element left of the returned index should be smaller");
            secondSmallestElement.Should().BeLessOrEqualTo(list[2], "because the elements right of the returned index should be greater or equal");
            list.Skip(2).Should().OnlyContain(value => value >= secondSmallestElement, "because all elements right of the returned index should be greater or equal");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the n-th smallest element
        /// </summary>
        [Fact]
        public void QuickSelectFindsNthSmallestElement()
        {
            // arrange
            var list = _list;
            var qs = new Quickselect<double>();
            const int n = Length / 2;

            // act
            var index = qs.Select(list, n);

            // assert
            index.Should().BeInRange(0, Length - 1, "because we expect the result to be a list index");
            var element = list[index];
            list.Take(n).Should().OnlyContain(value => value < element, "because all elements left of the returned index should be smaller");
            list.Skip(n).Should().OnlyContain(value => value >= element, "because all elements right of the returned index should be greater or equal");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the smallest element
        /// </summary>
        [Fact]
        public void QuickSelectFindsGreatestElement()
        {
            // arrange
            var list = _list;
            var qs = new Quickselect<double>();

            // act
            var smallestIndex = qs.Select(list, Length - 1);

            // assert
            smallestIndex.Should().Be(Length - 1, "because quickselect returns a partially ordered list right of the returned index which is greater or equal - and that list must be empty for the greatest element");
            var smallestElement = list[smallestIndex];
            smallestElement.Should().Be(list.Max(), "because that is the greatest element");
        }

        /// <summary>
        /// Attempting to select on a <see langword="null"/> reference throws an exception.
        /// </summary>
        [Fact]
        public void SelectOnNullListThrowsException()
        {
            var qs = new Quickselect<double>();
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = () => qs.Select((List<double>) null, 0);
            action.Should().ThrowExactly<ArgumentNullException>();
        }
        
        /// <summary>
        /// Attempting to select on a <see langword="null"/> reference throws an exception.
        /// </summary>
        [Fact]
        public void SelectRecursiveOnNullListThrowsException()
        {
            var qs = new Quickselect<double>();
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = () => qs.SelectRecursive((List<double>) null, 0);
            action.Should().ThrowExactly<ArgumentNullException>();
        }
        
        /// <summary>
        /// Attempting to select on a <see langword="null"/> reference throws an exception.
        /// </summary>
        [Fact]
        public void SelectNegativeAmountThrowsException()
        {
            var qs = new Quickselect<double>();
            Action action = () => qs.Select(_list, -1);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
        
        /// <summary>
        /// Attempting to select on a <see langword="null"/> reference throws an exception.
        /// </summary>
        [Fact]
        public void SelectRecursiveNegativeAmountThrowsException()
        {
            var qs = new Quickselect<double>();
            Action action = () => qs.SelectRecursive(_list, -1);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
        
        /// <summary>
        /// Attempting to select on a <see langword="null"/> reference throws an exception.
        /// </summary>
        [Fact]
        public void SelectTooBigAmountThrowsException()
        {
            var qs = new Quickselect<double>();
            Action action = () => qs.Select(_list, _list.Count);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
        
        /// <summary>
        /// Attempting to select on a <see langword="null"/> reference throws an exception.
        /// </summary>
        [Fact]
        public void SelectRecursiveTooBigAmountThrowsException()
        {
            var qs = new Quickselect<double>();
            Action action = () => qs.SelectRecursive(_list, _list.Count);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }
}

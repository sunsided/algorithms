using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace widemeadows.algorithms.tests
{
    /// <summary>
    /// Tests the <see cref="Quickselect{T}"/> algorithm.
    /// </summary>
    [TestFixture]
    public class QuickselectTests
    {
        /// <summary>
        /// The length of the list
        /// </summary>
        private const int Length = 100;

        /// <summary>
        /// The list to test against
        /// </summary>
        private List<double> _list;

        /// <summary>
        /// Performs the test setup
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var list = new List<double>(Length);
            for (var i = 0; i < Length; ++i)
            {
                list.Add(random.NextDouble());
            }
            _list = list;
        }

        #region Recursive Quickselect

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the smallest element
        /// </summary>
        [Test]
        public void RecursiveQuickSelectFindsSmallestElement()
        {
            var list = _list;
            var qs = new Quickselect<double>();

            var smallestIndex = qs.SelectRecursive(list, 0);
            smallestIndex.Should().Be(0, "because quickselect returns a partially ordered list left of the returned index which is smaller - and that list must be empty for the smallest element");

            var smallestElement = list[smallestIndex];
            smallestElement.Should().Be(list.Min(), "because that is the smallest element");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the second-smallest element
        /// </summary>
        [Test]
        public void RecursiveQuickSelectFindsSecondSmallestElement()
        {
            var list = _list;
            var qs = new Quickselect<double>();

            var secondSmallestIndex = qs.SelectRecursive(list, 1);
            secondSmallestIndex.Should().Be(1, "because quickselect returns a partially ordered list left of the returned index which is smaller - and there must only be one element to the left");

            var secondSmallestElement = list[secondSmallestIndex];
            secondSmallestElement.Should().BeGreaterThan(list[0], "because the element left of the returned index should be smaller");
            secondSmallestElement.Should().BeLessOrEqualTo(list[2], "because the elements right of the returned index should be greater or equal");
            list.Skip(2).Should().OnlyContain(value => value >= secondSmallestElement, "because all elements right of the returned index should be greater or equal");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the n-th smallest element
        /// </summary>
        [Test]
        public void RecursiveQuickSelectFindsNthSmallestElement()
        {
            var list = _list;
            var qs = new Quickselect<double>();

            var n = Length/2;
            var index = qs.SelectRecursive(list, n);
            index.Should().BeInRange(0, Length - 1, "because we expect the result to be a list index");

            var element = list[index];
            list.Take(n).Should().OnlyContain(value => value < element, "because all elements left of the returned index should be smaller");
            list.Skip(n).Should().OnlyContain(value => value >= element, "because all elements right of the returned index should be greater or equal");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the smallest element
        /// </summary>
        [Test]
        public void RecursiveQuickSelectFindsGreatestElement()
        {
            var list = _list;
            var qs = new Quickselect<double>();

            var smallestIndex = qs.SelectRecursive(list, Length-1);
            smallestIndex.Should().Be(Length-1, "because quickselect returns a partially ordered list right of the returned index which is greater or equal - and that list must be empty for the greatest element");

            var smallestElement = list[smallestIndex];
            smallestElement.Should().Be(list.Max(), "because that is the greatest element");
        }

        #endregion Recursive Quickselect

        #region Nonrecursive Quickselect

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the smallest element
        /// </summary>
        [Test]
        public void QuickSelectFindsSmallestElement()
        {
            var list = _list;
            var qs = new Quickselect<double>();

            var smallestIndex = qs.Select(list, 0);
            smallestIndex.Should().Be(0, "because quickselect returns a partially ordered list left of the returned index which is smaller - and that list must be empty for the smallest element");

            var smallestElement = list[smallestIndex];
            smallestElement.Should().Be(list.Min(), "because that is the smallest element");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the second-smallest element
        /// </summary>
        [Test]
        public void QuickSelectFindsSecondSmallestElement()
        {
            var list = _list;
            var qs = new Quickselect<double>();

            var secondSmallestIndex = qs.Select(list, 1);
            secondSmallestIndex.Should().Be(1, "because quickselect returns a partially ordered list left of the returned index which is smaller - and there must only be one element to the left");

            var secondSmallestElement = list[secondSmallestIndex];
            secondSmallestElement.Should().BeGreaterThan(list[0], "because the element left of the returned index should be smaller");
            secondSmallestElement.Should().BeLessOrEqualTo(list[2], "because the elements right of the returned index should be greater or equal");
            list.Skip(2).Should().OnlyContain(value => value >= secondSmallestElement, "because all elements right of the returned index should be greater or equal");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the n-th smallest element
        /// </summary>
        [Test]
        public void QuickSelectFindsNthSmallestElement()
        {
            var list = _list;
            var qs = new Quickselect<double>();

            var n = Length / 2;
            var index = qs.Select(list, n);
            index.Should().BeInRange(0, Length - 1, "because we expect the result to be a list index");

            var element = list[index];
            list.Take(n).Should().OnlyContain(value => value < element, "because all elements left of the returned index should be smaller");
            list.Skip(n).Should().OnlyContain(value => value >= element, "because all elements right of the returned index should be greater or equal");
        }

        /// <summary>
        /// Tests that the quickselect algorithm indeed returns the smallest element
        /// </summary>
        [Test]
        public void QuickSelectFindsGreatestElement()
        {
            var list = _list;
            var qs = new Quickselect<double>();

            var smallestIndex = qs.Select(list, Length - 1);
            smallestIndex.Should().Be(Length - 1, "because quickselect returns a partially ordered list right of the returned index which is greater or equal - and that list must be empty for the greatest element");

            var smallestElement = list[smallestIndex];
            smallestElement.Should().Be(list.Max(), "because that is the greatest element");
        }

        #endregion Nonrecursive Quickselect
    }
}

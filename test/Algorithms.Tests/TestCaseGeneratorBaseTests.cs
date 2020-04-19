using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Widemeadows.Algorithms.Tests.Data;
using Xunit;

namespace Widemeadows.Algorithms.Tests
{
    public class TestCaseGeneratorBaseTests
    {
        /// <summary>
        /// Tests <see cref="TestCaseGeneratorBase"/>'s default implementation of <see cref="IEnumerable.GetEnumerator"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Not all value sources need to be tested here, as long as they use the same base class.
        ///     Since the generators are being used as part of tests, their functionality is covered and
        ///     implicitly tested in the consuming theories.
        /// </para>
        /// <para>
        ///     We're testing a couple of different classes here anyways to decrease the likelihood of this
        ///     theory being implemented wrongly.
        /// </para>
        /// </remarks>
        /// <param name="type">The type of generator to instantiate.</param>
        [Theory]
        [InlineData(typeof(NaiveBinaryTreeValueGenerator))]
        [InlineData(typeof(NaiveBinaryTreeLeafTraversalGenerator))]
        [InlineData(typeof(NaiveBinaryTreeDeepestNodeGenerator))]
        public void TestCaseGeneratorBaseEnumeratesCorrectly(Type type)
        {
            var instance = (TestCaseGeneratorBase?)Activator.CreateInstance(type);
            if (instance == null) throw new InvalidOperationException($"Instance of type {type} could not be created.");

            var explicitEnumerator = ((IEnumerable<object[]>) instance).GetEnumerator();
            var implicitEnumerator = ((IEnumerable)instance).GetEnumerator();

            do
            {
                var couldMoveExplicit = explicitEnumerator.MoveNext();
                var couldMoveImplicit = implicitEnumerator.MoveNext();
                couldMoveExplicit.Should()
                    .Be(couldMoveImplicit, "because both enumerators should return the same data");

                if (!couldMoveExplicit || !couldMoveImplicit) break;

                implicitEnumerator.Current.Should()
                    .BeOfType<object[]>("because the enumerator is returning object arrays");

                var current = implicitEnumerator.Current;
                current.Should().NotBeNull("because the enumerator is returning non-null object arrays");

                var implicitValues = (object[]) current!;

                explicitEnumerator.Current.Should().HaveCount(implicitValues.Length,
                    "because both enumerators should return the same amount of values");

                implicitEnumerator.Current.Should()
                    .BeEquivalentTo(implicitValues, "because both iterators should return the same values");
            } while (true);

            explicitEnumerator.Dispose();
        }
    }
}
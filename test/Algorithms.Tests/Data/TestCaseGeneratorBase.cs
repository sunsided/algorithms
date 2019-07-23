using System.Collections;
using System.Collections.Generic;

namespace Widemeadows.Algorithms.Tests.Data
{
    /// <summary>
    /// Base class for test case generators.
    /// </summary>
    public abstract class TestCaseGeneratorBase : IEnumerable<object[]>
    {
        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public abstract IEnumerator<object[]> GetEnumerator();

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Tests.Data
{
    /// <summary>
    /// Base class for test case generators.
    /// </summary>
    public abstract class TestCaseGeneratorBase : IEnumerable<object[]>
    {
        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        [NotNull]
        public abstract IEnumerator<object[]> GetEnumerator();

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        [NotNull]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
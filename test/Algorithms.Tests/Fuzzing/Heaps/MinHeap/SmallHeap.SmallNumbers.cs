using System;
using System.Collections.Generic;
using Widemeadows.Algorithms.Heaps;
using Xunit;

namespace Widemeadows.Algorithms.Tests.Fuzzing.Heaps.MinHeap
{
    /// <summary>
    /// Tests for <see cref="MinHeap{T}"/>.
    /// </summary>
    [Trait("Fuzzing","Fast")]
    public class SmallHeapSmallNumbers : MinHeapFuzzingBase
    {
        private const int MaxOperations = 10;

        public static IEnumerable<object[]> RandomSeeds
        {
            get
            {
                const int numTestCases = 100;
                var random = new Random();

                for (var i = 0; i < numTestCases; ++i)
                {
                    yield return new object[] {random.Next()};
                }
            }
        }

        [Theory]
        [MemberData(nameof(RandomSeeds), DisableDiscoveryEnumeration = true)]
        public void Fuzzing(int seed)
        {
            Fuzz(seed, MaxOperations, 0, 10);
        }
    }
}

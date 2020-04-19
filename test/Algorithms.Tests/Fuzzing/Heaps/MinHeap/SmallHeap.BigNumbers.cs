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
    public class SmallHeapBigNumbers : MinHeapFuzzingBase
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
        public void Fuzzing_SmallHeap_BigNumbers(int seed)
        {
            Fuzz(seed, MaxOperations);
        }

        [Fact]
        public void Fix_1001837906()
        {
            const int seed = 1001837906;
            const int minValue = 0;
            const int maxValue = int.MaxValue;

            var rng = new Random(seed);
            var numOperations = rng.Next(1, MaxOperations);
            var operations = FuzzOperations(rng, numOperations, minValue, maxValue);

            for (var i = 0; i < operations.Count; ++i)
            {
                var operation = operations[i];
                ApplyFuzzedOperation(operation);
                ValidateHeap(i);
            }
        }
    }
}

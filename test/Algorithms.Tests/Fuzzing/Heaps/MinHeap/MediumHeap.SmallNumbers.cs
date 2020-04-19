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
    public class MediumHeapSmallNumbers : MinHeapFuzzingBase
    {
        private const int MaxOperations = 100;

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

        [Fact]
        public void Fix_1107980859()
        {
            const int seed = 1107980859;
            const int minValue = 0;
            const int maxValue = 10;

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

        [Fact]
        public void Fix_186806847()
        {
            const int seed = 186806847;
            const int minValue = 0;
            const int maxValue = 10;

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

        [Fact]
        public void Fix_1276495089()
        {
            const int seed = 1276495089;
            const int minValue = 0;
            const int maxValue = 10;

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

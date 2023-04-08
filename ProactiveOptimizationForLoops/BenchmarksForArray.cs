using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ProactiveOptimizationForLoops
{
    [MemoryDiagnoser]
    public class BenchmarksForArray
    {
        private static User[] Items;

        [Params(100, 10_000, 10_000_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            Items = Enumerable.Range(0, Count)
                        .Select(n => new User($"Nion Lee {n}"))
                        .ToArray();
        }

        [Benchmark]
        public void NormalLoop()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                var user = Items[i];
                user.GetFullName();
            }
        }

        [Benchmark]
        public void ForeachLoop()
        {
            foreach (var item in Items)
            {
                item.GetFullName();
            }
        }

        [Benchmark]
        public void FastLoop()
        {
            ref var searchSpace = ref MemoryMarshal.GetArrayDataReference(Items);
            for (int i = 0; i < Items.Length; i++)
            {
                var user = Unsafe.Add(ref searchSpace, i);
                user.GetFullName();
            }
        }

        [Benchmark]
        public void FasterLoop()
        {
            ref var start = ref MemoryMarshal.GetArrayDataReference(Items);
            ref var end = ref Unsafe.Add(ref start, Items.Length);

            while (Unsafe.IsAddressLessThan(ref start, ref end))
            {
                start.GetFullName();
                start = ref Unsafe.Add(ref start, 1);
            }
        }

        [Benchmark]
        public void AsSpanLoop()
        {
            var span = Items.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                var user = span[i];
                user.GetFullName();
            }
        }
    }
}

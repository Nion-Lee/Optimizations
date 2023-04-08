using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;

namespace ProactiveOptimizationForLoops
{
    [MemoryDiagnoser]
    public class BenchmarksForList
    {
        private static List<User> Items;

        [Params(100, 10_000, 10_000_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            Items = Enumerable.Range(0, Count)
                        .Select(n => new User($"Nion Lee {n}"))
                        .ToList();
        }

        [Benchmark]
        public void NormalLoop()
        {
            for (int i = 0; i < Items.Count; i++)
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
        public void AsSpanLoop()
        {
            var span = CollectionsMarshal.AsSpan(Items);
            for (int i = 0; i < span.Length; i++)
            {
                var user = span[i];
                user.GetFullName();
            }
        }
    }
}

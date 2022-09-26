using BenchmarkDotNet.Attributes;
using MoreStructures.Lists.Sorting;
using MoreStructures.Tests.Lists.Sorting;
using static MoreStructures.Benchmarks.BenchmarkUtilities;

namespace MoreStructures.Benchmarks.Lists.Sorting;

public class ShellSortGapSequences
{
    private List<int> Numbers { get; }

    [Params(10, 100, 1000)]
    public int NumberOfItems { get; set; }

    [Params(10)]
    public int NumberOfChunks { get; set; }

    public ShellSortGapSequences()
    {
        Numbers = File
            .ReadAllText(TestDataRandomIntsFrom0To1000)
            .Split(',')
            .Select(x => int.Parse(x.Trim()))
            .ToList();
    }

    private void Run(IEnumerable<int> gapGenerator)
    {
        var sorter = new ShellSort(gapGenerator);
        foreach (var chunk in Numbers.Chunk(NumberOfItems).Take(NumberOfChunks))
            sorter.Sort(chunk);
    }

    [Benchmark(Baseline = true)]
    public void GapsSingleInsertionSort() => Run(new[] { 1 });

    [Benchmark]
    public void GapsPowerOf2() => Run(ShellSortTests_GapsPowerOf2.Sequence());

    [Benchmark]
    public void GapsPositiveInts() => Run(ShellSortTests_GapsPositiveInts.Sequence());

    [Benchmark]
    public void GapsSedgewick82() => Run(ShellSortTests_GapsSedgewick82.Sequence());

    [Benchmark]
    public void GapsCiura01() => Run(ShellSortTests_GapsCiura01.Sequence());
}


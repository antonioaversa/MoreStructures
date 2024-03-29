﻿using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class ShellSortTests_GapsPowerOf2 : InputSortingTests
{
    public static IEnumerable<int> Sequence() =>
        Enumerable.Range(0, int.MaxValue).Select(i => (int)Math.Pow(2, i));

    public ShellSortTests_GapsPowerOf2() : base(() => new ShellSort(Sequence()))
    {
    }
}

[TestClass]
public class ShellSortTests_GapsPositiveInts : InputSortingTests
{
    public static IEnumerable<int> Sequence() =>
        Enumerable.Range(1, int.MaxValue);

    public ShellSortTests_GapsPositiveInts() : base(() => new ShellSort(Sequence()))
    {
    }
}

[TestClass]
public class ShellSortTests_GapsSedgewick82 : InputSortingTests
{
    public static IEnumerable<int> Sequence()
    {
        yield return 1;
        for (var k = 0; true; k++)
            yield return (int)(Math.Pow(4, k) + 3 * Math.Pow(2, k - 1)) + 1;
    }

    public ShellSortTests_GapsSedgewick82() : base(() => new ShellSort(Sequence()))
    {
    }
}

[TestClass]
public class ShellSortTests_GapsCiura01 : InputSortingTests
{
    public static IEnumerable<int> Sequence() =>
        new[] { 1, 4, 10, 23, 57, 132, 301, 701 };

    public ShellSortTests_GapsCiura01() : base(() => new ShellSort(Sequence()))
    {
    }
}

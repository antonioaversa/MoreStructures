using System;
using System.Collections.Generic;

namespace MoreStructures.Tests;

internal static class TestUtilities
{
    public static TextWithTerminator ExampleText1 => new("aaa");
    public static TextWithTerminator ExampleText2 => new("ababaa");

    public static IEnumerable<int> MedianGenerator(int start, int end)
    {
        if (start > end)
            yield break;

        var middle = (int)Math.Ceiling((start + end) / 2.0);
        yield return middle;
        foreach (var item in MedianGenerator(middle + 1, end))
            yield return item;
        foreach (var item in MedianGenerator(start, middle - 1))
            yield return item;
    }
}

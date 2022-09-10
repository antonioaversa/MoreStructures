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

    public static IEnumerable<IList<int>> GeneratePermutations(IList<int> list)
    {
        if (list.Count <= 1)
        {
            yield return list;
            yield break;
        }

        var head = list[0];
        var tail = list.Skip(1);
        for (var i = 0; i < list.Count; i++)
            yield return tail.Take(i).Append(head).Concat(tail.Skip(i)).ToList();
    }
}

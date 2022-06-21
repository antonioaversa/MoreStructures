using MoreStructures.SuffixStructures.Matching;

namespace MoreStructures.Tests.SuffixTrees.Matching;

public abstract class SnssFinderTests
{
    protected ISnssFinder Finder { get; }

    protected SnssFinderTests(ISnssFinder finder)
    {
        Finder = finder;
    }

    [DataRow("A", "B", new string?[] { "A" })]
    [DataRow("A", "A", new string?[] { null })]
    [DataRow("AB", "BA", new string?[] { "AB" })]
    [DataRow("AB", "BBA", new string?[] { "AB" })]
    [DataRow("AB", "BBAA", new string?[] { "AB" })]
    [DataRow("ABAB", "BBAA", new string?[] { "AB" })]
    [DataRow("BXAA", "AXBB", new string?[] { "BX", "XA", "AA" })]
    [DataRow("BXZA", "AYBB", new string?[] { "X", "Z" })]
    [DataRow("BXAA", "BXAA", new string?[] { null })]
    [DataRow("AABBA", "AABBAAB", new string?[] { null })]
    [DataRow("AABBA", "ABBAAB", new string?[] { "AABB" })]
    [DataRow("AABBA", "BBAAB", new string?[] { "ABB" })]
    [DataRow("BBAAA", "BBAA", new string?[] { "AAA" })]
    [DataRow("AABBA", "BBAA", new string?[] { "AB" })]
    [DataTestMethod]
    public void Find_IsCorrect(string text1, string text2, string?[] expectedResults)
    {
        var result = Finder.Find(text1, text2);
        Assert.IsTrue(expectedResults.Contains(result));
    }

}

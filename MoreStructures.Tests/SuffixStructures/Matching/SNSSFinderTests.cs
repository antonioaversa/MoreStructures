using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixStructures.Matching;
using System;
using System.Linq;

namespace MoreStructures.Tests.SuffixTrees.Matching;

public abstract class SNSSFinderTests
{
    protected ISNSSFinder Finder { get; }

    protected SNSSFinderTests(ISNSSFinder finder)
    {
        Finder = finder;
    }

    [DataRow("AABBA", "AABBAAB", new string?[] { null })]
    [DataRow("AABBA", "ABBAAB", new string?[] { "AABB" })]
    [DataRow("AABBA", "BBAAB", new string?[] { "ABB" })]
    [DataRow("BBAAA", "BBAA", new string?[] { "AAA" })]
    [DataRow("AABBA", "BBAA", new string?[] { "AB" })]
    [DataRow("BXAA", "AXBB", new string?[] { "BX", "XA", "AA" })]
    [DataRow("BXZA", "AYBB", new string?[] { "X", "Z" })]
    [DataRow("BXAA", "BXAA", new string?[] { null })]
    [DataRow("AAAAAAAAAAAAAAAAAAAA", "BBBBBBBBBBBBBBB", new string?[] { "A" })]
    [DataRow("AAAAAAAAAAAAAAAAAAAA", "BBBBBBBBBBBBBBBAA", new string?[] { "AAA" })]
    [DataRow("AAAAAAAAAAAAAAAAAAAA", "BBBBBBBBBBBBBBBAAAA", new string?[] { "AAAAA" })]
    [DataTestMethod]
    public void Find_IsCorrect(string text1, string text2, string?[] expectedResults)
    {
        var result = Finder.Find(text1, text2);
        Assert.IsTrue(expectedResults.Contains(result));
    }

}

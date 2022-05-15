using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StringAlgorithms.SuffixTrees.Tests;

[TestClass]
public class SuffixTreeMatcherTests
{
    [TestMethod]
    public void Match_Preconditions()
    {
        var text = new TextWithTerminator("abcdaabcbcadaabca");
        var suffixTree = SuffixTreeBuilder.Build(text);

        Assert.ThrowsException<ArgumentException>(() => suffixTree.Match(text, new("")));
    }

    [TestMethod]
    public void Match_SuccessAndMatchedCharsCorrectness()
    {
        var text = new TextWithTerminator("abcdaabcbcadaabca");
        var suffixTree = SuffixTreeBuilder.Build(text);

        SuffixTreeMatch match;

        match = suffixTree.Match(text, new("ab"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: true, MatchedChars: 2 });
        match = suffixTree.Match(text, new("abc"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: true, MatchedChars: 3 });
        match = suffixTree.Match(text, new("abcdaabcbcadaabca"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: true, MatchedChars: 17 });

        match = suffixTree.Match(text, new("b"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: true, MatchedChars: 1 });
        match = suffixTree.Match(text, new("bc"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: true, MatchedChars: 2 });
        match = suffixTree.Match(text, new("bcdaabcbcadaabca"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: true, MatchedChars: 16 });

        match = suffixTree.Match(text, new("aab"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: true, MatchedChars: 3 });
    }

    [TestMethod]
    public void Match_UnsuccessAndMatchedCharsCorrectness()
    {
        var text = new TextWithTerminator("abcdaabcbcadaabca");
        var suffixTree = SuffixTreeBuilder.Build(text);

        SuffixTreeMatch match;

        match = suffixTree.Match(text, new("z"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: false, MatchedChars: 0 });
        match = suffixTree.Match(text, new("az"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: false, MatchedChars: 1 });
        match = suffixTree.Match(text, new("abz"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: false, MatchedChars: 2 });
        match = suffixTree.Match(text, new("abcdaabcbcadaabcaz"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: false, MatchedChars: 17 });
        match = suffixTree.Match(text, new("zabcdaabcbcadaabca"));
        Assert.IsTrue(match is SuffixTreeMatch { Success: false, MatchedChars: 0 });
    }

    [TestMethod]
    public void Match_SuccessBeginAndPathCorrectness()
    {
        var text = new TextWithTerminator("abcdaabcbcadaabca");
        var suffixTree = SuffixTreeBuilder.Build(text);

        foreach (var pattern in new string[] { "ab", "abc", "abcdaabcbcadaabca", "b", "bc", "bcdaabcbcadaabca", "aab" })
        {
            Assert.IsTrue(suffixTree.Match(text, new(pattern)) is
                SuffixTreeMatch { Success: true, Begin: var begin1, Path: var path1 } &&
                text[begin1..(begin1 + path1.TotalEdgesLength)] == path1.SuffixFor(text));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.SuffixTrees;
using System;

namespace StringAlgorithms.Tests.SuffixTrees;

[TestClass]
public class SuffixStructureMatcherTests
{
    private readonly SuffixTreeBuilder Builder = new();

    [TestMethod]
    public void Match_Preconditions()
    {
        var text = new TextWithTerminator("abcdaabcbcadaabca");
        var suffixTree = Builder.BuildTree(text);

        Assert.ThrowsException<ArgumentException>(() => suffixTree.Match(text, new("")));
    }

    [TestMethod]
    public void Match_SuccessAndMatchedCharsIsCorrect()
    {
        var text = new TextWithTerminator("abcdaabcbcadaabca");
        var suffixTree = Builder.BuildTree(text);

        SuffixStructureMatch<SuffixTreePath> match;

        match = suffixTree.Match(text, new("ab"));
        Assert.IsTrue(match is { Success: true, MatchedChars: 2 });
        match = suffixTree.Match(text, new("abc"));
        Assert.IsTrue(match is { Success: true, MatchedChars: 3 });
        match = suffixTree.Match(text, new("abcdaabcbcadaabca"));
        Assert.IsTrue(match is { Success: true, MatchedChars: 17 });

        match = suffixTree.Match(text, new("b"));
        Assert.IsTrue(match is { Success: true, MatchedChars: 1 });
        match = suffixTree.Match(text, new("bc"));
        Assert.IsTrue(match is { Success: true, MatchedChars: 2 });
        match = suffixTree.Match(text, new("bcdaabcbcadaabca"));
        Assert.IsTrue(match is { Success: true, MatchedChars: 16 });

        match = suffixTree.Match(text, new("aab"));
        Assert.IsTrue(match is { Success: true, MatchedChars: 3 });
    }

    [TestMethod]
    public void Match_UnsuccessAndMatchedCharsIsCorrect()
    {
        var text = new TextWithTerminator("abcdaabcbcadaabca");
        var suffixTree = Builder.BuildTree(text);

        SuffixStructureMatch<SuffixTreePath> match;

        match = suffixTree.Match(text, new("z"));
        Assert.IsTrue(match is { Success: false, MatchedChars: 0 });
        match = suffixTree.Match(text, new("az"));
        Assert.IsTrue(match is { Success: false, MatchedChars: 1 });
        match = suffixTree.Match(text, new("abz"));
        Assert.IsTrue(match is { Success: false, MatchedChars: 2 });
        match = suffixTree.Match(text, new("abcdaabcbcadaabcaz"));
        Assert.IsTrue(match is { Success: false, MatchedChars: 17 });
        match = suffixTree.Match(text, new("zabcdaabcbcadaabca"));
        Assert.IsTrue(match is { Success: false, MatchedChars: 0 });
    }

    [TestMethod]
    public void Match_SuccessBeginAndPathIsCorrect()
    {
        var text = new TextWithTerminator("abcdaabcbcadaabca");
        var suffixTree = Builder.BuildTree(text);

        foreach (var pattern in new string[] { "ab", "abc", "abcdaabcbcadaabca", "b", "bc", "bcdaabcbcadaabca", "aab" })
        {
            Assert.IsTrue(suffixTree.Match(text, new(pattern)) is
                { Success: true, Begin: var begin1, Path: var path1 } &&
                text[begin1..].StartsWith(path1.SuffixFor(text)));
        }
    }
}

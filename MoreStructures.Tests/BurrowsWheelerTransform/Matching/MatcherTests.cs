using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Matching;
using System;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching;

public abstract class MatcherTests
{
    private IBuilder Builder { get; } = new LastFirstPropertyBasedBuilder();
    private Func<RotatedTextWithTerminator, IMatcher> MatcherBuilderWithBWTOnly { get; }
    private Func<RotatedTextWithTerminator, RotatedTextWithTerminator, IMatcher> MatcherBuilderWithSortedBWT { get; }

    protected MatcherTests(
        Func<RotatedTextWithTerminator, IMatcher> matcherBuilderWithBWTOnly, 
        Func<RotatedTextWithTerminator, RotatedTextWithTerminator, IMatcher> matcherBuilderWithSortedBWT)
    {
        MatcherBuilderWithBWTOnly = matcherBuilderWithBWTOnly;
        MatcherBuilderWithSortedBWT = matcherBuilderWithSortedBWT;
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWithIncosistentBWTAndSortedBWT()
    {
        Assert.ThrowsException<ArgumentException>(
            () => MatcherBuilderWithSortedBWT(new("a#", '#'), new("$a", '$')));
        Assert.ThrowsException<ArgumentException>(
            () => MatcherBuilderWithSortedBWT(new("a#$", '#'), new("$#a", '$')));
    }

    [DataRow("mississippi", "issi", true, 4, 3, 4)]
    [DataRow("mississippi", "issip", true, 5, 3, 3)]
    [DataRow("mississippi", "xissi", false, 4, 3, 4)] // Matching goes backwards => matches 4 chars then fails
    [DataRow("mississippi", "issix", false, 0, -1, -1)] // Matching goes backwards => matches no char
    [DataRow("mississippi", "x", false, 0, -1, -1)] // Fail right away
    [DataRow("mississippi", "s", true, 1, 8, 11)]
    [DataRow("mississippi", "mi", true, 2, 5, 5)]
    [DataRow("mississippi", "mis", true, 3, 5, 5)]
    [DataRow("baba", "ba", true, 2, 3, 4)]
    [DataRow("baba", "bab", true, 3, 4, 4)]
    [DataRow("aaaaaaaaaaa", "a", true, 1, 1, 11)]
    [DataTestMethod]
    public void Match_IsCorrect(
        string textContent, string patternContent, bool expectedSuccess, int expectedMatchedChars, int expectedStart,
        int expectedEnd)
    {
        var text = new TextWithTerminator(textContent);
        var bwt = Builder.BuildTransform(text).Content;
        var matcher = MatcherBuilderWithBWTOnly(bwt);
        var (success, matchedChars, startIndex, endIndex) = matcher.Match(patternContent);
        Assert.AreEqual(expectedSuccess, success);
        Assert.AreEqual(expectedMatchedChars, matchedChars);
        Assert.AreEqual(expectedStart, startIndex);
        Assert.AreEqual(expectedEnd, endIndex);
    }


    [TestMethod]
    public void Match_RaisesExceptionWithEmptyPattern()
    {
        Assert.ThrowsException<ArgumentException>(
            () => MatcherBuilderWithSortedBWT(new("a$"), new("$a")).Match(""));
    }
}

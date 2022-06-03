using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;
using MoreStructures.BurrowsWheelerTransform.Matching;
using MoreStructures.Utilities;
using System;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching;

public abstract class MatcherTests
{
    private IBuilder Builder { get; } = new LastFirstPropertyBasedBuilder();
    private IMatcher Matcher { get; } = new NarrowingIntervalMatcher();

    protected MatcherTests(IMatcher matcher)
    {
        Matcher = matcher;
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
        var sbwt = ILastFirstFinder.QuickSort(bwt, CharOrTerminatorComparer.Build(text.Terminator));
        var (success, matchedChars, startIndex, endIndex) = Matcher.Match(bwt, sbwt, patternContent);
        Assert.AreEqual(expectedSuccess, success);
        Assert.AreEqual(expectedMatchedChars, matchedChars);
        Assert.AreEqual(expectedStart, startIndex);
        Assert.AreEqual(expectedEnd, endIndex);
    }

    [TestMethod]
    public void Match_RaisesExceptionWithIncosistentBWTAndSortedBWT()
    {
        Assert.ThrowsException<ArgumentException>(() => Matcher.Match(new("a#", '#'), new("$a", '$'), "a"));
    }

    [TestMethod]
    public void Match_RaisesExceptionWithEmptyPattern()
    {
        Assert.ThrowsException<ArgumentException>(() => Matcher.Match(new("a$"), new("$a"), ""));
    }
}

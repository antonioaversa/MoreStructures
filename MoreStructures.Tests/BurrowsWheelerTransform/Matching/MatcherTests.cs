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

    [DataRow("mississippi", "issi", true, 4)]
    [DataRow("mississippi", "issip", true, 5)]
    [DataRow("mississippi", "xissi", false, 4)] // Matching goes backwards
    [DataRow("mississippi", "issix", false, 0)]
    [DataRow("mississippi", "x", false, 0)]

    [DataTestMethod]
    public void Match_IsCorrect(
        string textContent, string patternContent, bool expectedSuccess, int expectedMatchedChars)
    {
        var text = new TextWithTerminator(textContent);
        var bwt = Builder.BuildTransform(text).Content;
        var sbwt = ILastFirstFinder.QuickSort(bwt, CharOrTerminatorComparer.Build(text.Terminator));
        var match = Matcher.Match(sbwt, bwt, patternContent);
        Assert.IsTrue(
            match is { Success: var success, MatchedChars: var matchedChars } && 
            success == expectedSuccess && 
            matchedChars == expectedMatchedChars);
    }

    [TestMethod]
    public void Match_RaisesExceptionWithIncosistentBWTAndSortedBWT()
    {
        Assert.ThrowsException<ArgumentException>(() => Matcher.Match(new("$a", '$'), new("a#", '#'), "a"));
    }

    [TestMethod]
    public void Match_RaisesExceptionWithEmptyPattern()
    {
        Assert.ThrowsException<ArgumentException>(() => Matcher.Match(new("$a"), new("a$"), ""));
    }
}

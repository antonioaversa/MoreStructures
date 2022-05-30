using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;
using System;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder.LastFirstFinders;

public abstract class LastFirstFinderTests
{
    protected Func<RotatedTextWithTerminator, ILastFirstFinder> FinderBuilder { get; }

    protected LastFirstFinderTests(Func<RotatedTextWithTerminator, ILastFirstFinder> finderBuilder)
    {
        FinderBuilder = finderBuilder;
    }

    [DataRow("a$bca", '$', 'a', 0, 0, 1)]
    [DataRow("a$bca", '$', 'a', 1, 4, 2)]
    [DataRow("a$bba", '$', '$', 0, 1, 0)]
    [DataRow("a$bba", '$', 'b', 0, 2, 3)]
    [DataRow("a$bba", '$', 'b', 1, 3, 4)]
    [DataTestMethod]
    public void FindIndexOfNthOccurrenceInBWTAndSortedBWT_IsCorrect(
        string bwtStr, char terminator, char charToFind, int occurrence, int expectedBWTResult, 
        int expectedSortedBWTResult)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.AreEqual(expectedBWTResult, finder.FindIndexOfNthOccurrenceInBWT(charToFind, occurrence));
        Assert.AreEqual(expectedSortedBWTResult, finder.FindIndexOfNthOccurrenceInSortedBWT(charToFind, occurrence));
    }

    [DataRow("a$bca", '$', 'x', 0)]
    [DataRow("a$bca", '$', 'a', 2)]
    [DataRow("a$bca", '$', 'b', -1)]
    [DataTestMethod]
    public void FindIndexOfNthOccurrenceInBWTAndSortedBWT_ThrowsExceptionOnInvalidInput(
        string bwtStr, char terminator, char charToFind, int occurrence)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.ThrowsException<ArgumentException>(
            () => finder.FindIndexOfNthOccurrenceInBWT(charToFind, occurrence));
        Assert.ThrowsException<ArgumentException>(
            () => finder.FindIndexOfNthOccurrenceInSortedBWT(charToFind, occurrence));
    }

    [DataRow("a$bcacc", '$', 0, 0, 0)]
    [DataRow("a$bcacc", '$', 1, 0, 0)]
    [DataRow("a$bcacc", '$', 2, 0, 1)]
    [DataRow("a$bcacc", '$', 3, 0, 0)]
    [DataRow("a$bcacc", '$', 4, 1, 0)]
    [DataRow("a$bcacc", '$', 5, 1, 1)]
    [DataRow("a$bcacc", '$', 6, 2, 2)]
    [DataTestMethod]
    public void FindOccurrenceOfCharInBWTAndSortedBWT_IsCorrect(
        string bwtStr, char terminator, int indexOfChar, int expectedBWTResult,
        int expectedSortedBWTResult)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.AreEqual(expectedBWTResult, finder.FindOccurrenceRankOfCharInBWT(indexOfChar));
        Assert.AreEqual(expectedSortedBWTResult, finder.FindOccurrenceRankOfCharInSortedBWT(indexOfChar));
    }

    [DataRow("a$bca", '$', -1)]
    [DataRow("a$bca", '$', 10)]
    [DataTestMethod]
    public void FindOccurrenceOfCharInSortedBWTAndSortedBWT_ThrowsExceptionOnInvalidInput(
        string bwtStr, char terminator, int indexOfChar)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.ThrowsException<ArgumentException>(
            () => finder.FindOccurrenceRankOfCharInBWT(indexOfChar));
        Assert.ThrowsException<ArgumentException>(
            () => finder.FindOccurrenceRankOfCharInSortedBWT(indexOfChar));
    }

    //$ccinnoossuu
    [DataRow("ssncuociun$o", '$', 0, 8)]
    [DataRow("ssncuociun$o", '$', 1, 9)]
    [DataRow("ssncuociun$o", '$', 2, 4)]
    [DataRow("ssncuociun$o", '$', 3, 1)]
    [DataRow("ssncuociun$o", '$', 4, 10)]
    [DataTestMethod]
    public void LastToFirst_IsCorrect(
            string bwtStr, char terminator, int last, int expectedFirst)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        var (first, _) = finder.LastToFirst(last);
        Assert.AreEqual(expectedFirst, first);
    }
}

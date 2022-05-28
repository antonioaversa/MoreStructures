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

    [DataRow("a$bca", '$', 'a', 0, 0)]
    [DataRow("a$bca", '$', 'a', 1, 4)]
    [DataRow("a$bba", '$', '$', 0, 1)]
    [DataRow("a$bba", '$', 'b', 0, 2)]
    [DataRow("a$bba", '$', 'b', 1, 3)]
    [DataTestMethod]
    public void FindIndexOfNthOccurrenceInBWT_IsCorrect(
        string bwtStr, char terminator, char charToFind, int occurrence, int expectedResult)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.AreEqual(expectedResult, finder.FindIndexOfNthOccurrenceInBWT(charToFind, occurrence));
    }

    [DataRow("a$bca", '$', 'x', 0)]
    [DataRow("a$bca", '$', 'a', 2)]
    [DataRow("a$bca", '$', 'b', -1)]
    [DataTestMethod]
    public void FindIndexOfNthOccurrenceInBWT_ThrowsExceptionOnInvalidInput(
        string bwtStr, char terminator, char charToFind, int occurrence)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.ThrowsException<ArgumentException>(
            () => finder.FindIndexOfNthOccurrenceInBWT(charToFind, occurrence));
    }

    [DataRow("a$bcacc", '$', 0, 0)]
    [DataRow("a$bcacc", '$', 1, 0)]
    [DataRow("a$bcacc", '$', 2, 1)]
    [DataRow("a$bcacc", '$', 3, 0)]
    [DataRow("a$bcacc", '$', 4, 0)]
    [DataRow("a$bcacc", '$', 5, 1)]
    [DataRow("a$bcacc", '$', 6, 2)]
    [DataTestMethod]
    public void FindOccurrenceOfCharInSortedBWT_IsCorrect(
        string bwtStr, char terminator, int indexOfChar, int expectedResult)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.AreEqual(expectedResult, finder.FindOccurrenceOfCharInSortedBWT(indexOfChar));
    }

    [DataRow("a$bca", '$', -1)]
    [DataRow("a$bca", '$', 10)]
    [DataTestMethod]
    public void FindOccurrenceOfCharInSortedBWT_ThrowsExceptionOnInvalidInput(
        string bwtStr, char terminator, int indexOfChar)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.ThrowsException<ArgumentException>(
            () => finder.FindOccurrenceOfCharInSortedBWT(indexOfChar));
    }
}

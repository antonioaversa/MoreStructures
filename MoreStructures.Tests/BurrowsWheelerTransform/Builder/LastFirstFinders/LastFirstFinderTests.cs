using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder.LastFirstFinders;

public abstract class LastFirstFinderTests
{
    protected Func<RotatedTextWithTerminator, ILastFirstFinder> FinderBuilder { get; }

    protected LastFirstFinderTests(Func<RotatedTextWithTerminator, ILastFirstFinder> finderBuilder)
    {
        FinderBuilder = finderBuilder;
    }

    [DataRow("a$bca", '$', 0, 0, 0, 1)] // First 'a', find occurrence with rank 0
    [DataRow("a$bca", '$', 0, 1, 4, 2)] // First 'a', find occurrence with rank 1
    [DataRow("a$bca", '$', 4, 0, 0, 1)] // Second 'a', find occurrence with rank 0
    [DataRow("a$bca", '$', 4, 1, 4, 2)] // Second 'a', find occurrence with rank 1
    [DataRow("a$bba", '$', 1, 0, 1, 0)] // The only '$', find occurrence with rank 0
    [DataRow("a$bba", '$', 2, 0, 2, 3)] // First 'b', find occurrence with rank 0
    [DataRow("a$bba", '$', 2, 1, 3, 4)] // First 'b', find occurrence with rank 1
    [DataRow("a$bba", '$', 3, 0, 2, 3)] // Second 'b', find occurrence with rank 0
    [DataRow("a$bba", '$', 3, 1, 3, 4)] // Second 'b', find occurrence with rank 1
    [DataTestMethod]
    public void FindIndexOfNthOccurrenceInBWTAndSortedBWT_IsCorrect(
        string bwtStr, char terminator, int indexOfCharInBWT, int occurrence, 
        int expectedBWTResult, int expectedSortedBWTResult)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.AreEqual(expectedBWTResult, 
            finder.FindIndexOfNthOccurrenceInBWT(indexOfCharInBWT, occurrence));
        Assert.AreEqual(expectedSortedBWTResult, 
            finder.FindIndexOfNthOccurrenceInSortedBWT(indexOfCharInBWT, occurrence));
    }

    [DataRow("a$bca", '$', -1, 0)]
    [DataRow("a$bca", '$', 0, -1)]
    [DataRow("a$bca", '$', 0, 2)]
    [DataRow("a$bca", '$', 1, -1)]
    [DataRow("a$bca", '$', 1, 1)]
    [DataRow("a$bca", '$', 2, -1)]
    [DataRow("a$bca", '$', 2, 1)]
    [DataTestMethod]
    public void FindIndexOfNthOccurrenceInBWTAndSortedBWT_ThrowsExceptionOnInvalidInput(
        string bwtStr, char terminator, int indexOfCharInBWT, int occurrence)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => finder.FindIndexOfNthOccurrenceInBWT(indexOfCharInBWT, occurrence));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => finder.FindIndexOfNthOccurrenceInSortedBWT(indexOfCharInBWT, occurrence));
    }

    [DataRow("a$bcacc", '$', 0, 0)]
    [DataRow("a$bcacc", '$', 1, 0)]
    [DataRow("a$bcacc", '$', 2, 0)]
    [DataRow("a$bcacc", '$', 3, 0)]
    [DataRow("a$bcacc", '$', 4, 1)]
    [DataRow("a$bcacc", '$', 5, 1)]
    [DataRow("a$bcacc", '$', 6, 2)]
    [DataTestMethod]
    public void FindOccurrenceOfCharInBWT_IsCorrect(
        string bwtStr, char terminator, int indexOfCharInBWT, int expectedOccurrenceRank)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.AreEqual(expectedOccurrenceRank, finder.FindOccurrenceRankOfCharInBWT(indexOfCharInBWT));
    }

    // Sorted BWT: $aabccc
    [DataRow("a$bcacc", '$', 1, 0)]
    [DataRow("a$bcacc", '$', 0, 0)]
    [DataRow("a$bcacc", '$', 3, 0)]
    [DataRow("a$bcacc", '$', 4, 0)]
    [DataRow("a$bcacc", '$', 2, 1)]
    [DataRow("a$bcacc", '$', 5, 1)]
    [DataRow("a$bcacc", '$', 6, 2)]
    [DataTestMethod]
    public void FindOccurrenceOfCharInSortedBWT_IsCorrect(
        string bwtStr, char terminator, int indexOfCharInSortedBWT, int expectedOccurrenceRank)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.AreEqual(expectedOccurrenceRank, finder.FindOccurrenceRankOfCharInSortedBWT(indexOfCharInSortedBWT));
    }

    [DataRow("a$bca", '$', -1)]
    [DataRow("a$bca", '$', 10)]
    [DataTestMethod]
    public void FindOccurrenceOfCharInSortedBWTAndSortedBWT_ThrowsExceptionOnInvalidInput(
        string bwtStr, char terminator, int indexOfChar)
    {
        var bwt = new RotatedTextWithTerminator(bwtStr, terminator);
        var finder = FinderBuilder(bwt);
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => finder.FindOccurrenceRankOfCharInBWT(indexOfChar));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => finder.FindOccurrenceRankOfCharInSortedBWT(indexOfChar));
    }

    // SortedBWT: $ccinnoossuu
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

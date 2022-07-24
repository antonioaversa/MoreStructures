using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

public abstract class DoubleLengthPcsSorterTests
{
    public sealed record SorterBuilderInput(
        string Input, int PcsLength, int[] Order, int[] EqClasses, bool InputWithTerminator);

    protected Func<SorterBuilderInput, IDoubleLengthPcsSorter> SorterBuilder { get; }

    protected DoubleLengthPcsSorterTests(Func<SorterBuilderInput, IDoubleLengthPcsSorter> sorterBuilder)
    {
        SorterBuilder = sorterBuilder;
    }

    // Without terminator
    [DataRow("ab", 1, new[] { 0, 1 }, new[] { 0, 1 }, false,
        new[] { 0, 1 })]
    [DataRow("aba", 1, new[] { 0, 2, 1 }, new[] { 0, 1, 0 }, false,
        new[] { 2, 0, 1 })]
    [DataRow("abba", 1, new[] { 0, 3, 1, 2 }, new[] { 0, 1, 1, 0 }, false,
        new[] { 3, 0, 2, 1 })]
    [DataRow("abba", 2, new[] { 3, 0, 2, 1 }, new[] { 1, 3, 2, 0 }, false,
        new[] { 3, 0, 2, 1 })]
    [DataRow("abba", 3, new[] { 3, 0, 2, 1 }, new[] { 1, 3, 2, 0 }, false,
        new[] { 3, 0, 2, 1 })]
    [DataRow("abba", 4, new[] { 3, 0, 2, 1 }, new[] { 1, 3, 2, 0 }, false,
        new[] { 3, 0, 2, 1 })]
    [DataRow("abcd", 4, new[] { 0, 1, 2, 3 }, new[] { 0, 1, 2, 3 }, false,
        new[] { 0, 1, 2, 3 })]
    [DataRow("cabccba", 1, new[] { 1, 6, 5, 2, 0, 4, 3 }, new[] { 2, 0, 1, 2, 2, 1, 0 }, false,
        new[] { 1, 6, 5, 2, 0, 4, 3 })]
    [DataRow("cabccba", 2, new[] { 1, 6, 5, 2, 0, 4, 3 }, new[] { 4, 0, 3, 6, 5, 2, 1 }, false,
        new[] { 1, 6, 5, 2, 0, 4, 3 })]
    [DataRow("cabccba", 3, new[] { 1, 6, 5, 2, 0, 4, 3 }, new[] { 4, 0, 3, 6, 5, 2, 1 }, false,
        new[] { 1, 6, 5, 2, 0, 4, 3 })]
    // With terminator
    [DataRow("ab", 1, new[] { 1, 0 }, new[] { 1, 0 }, true,
        new[] { 1, 0 })]
    [DataRow("aba", 1, new[] { 0, 2, 1 }, new[] { 0, 1, 0 }, true,
        new[] { 2, 0, 1 })]
    [DataRow("abba", 2, new[] { 3, 0, 2, 1 }, new[] { 1, 3, 2, 0 }, true,
        new[] { 3, 0, 2, 1 })]
    [DataRow("abcd", 4, new[] { 3, 0, 1, 2 }, new[] { 1, 2, 3, 0 }, true,
        new[] { 3, 0, 1, 2 })]
    [DataRow("cabccbb", 2, new[] { 5, 2, 6, 1, 4, 0, 3 }, new[] { 4, 2, 1, 5, 3, 0, 1 }, true,
        new[] { 5, 6, 2, 1, 4, 0, 3 })]
    [DataRow("cabccbb", 3, new[] { 5, 6, 2, 1, 4, 0, 3 }, new[] { 5, 3, 2, 6, 4, 0, 1 }, true,
        new[] { 5, 6, 2, 1, 4, 0, 3 })]
    [DataTestMethod]
    public void Sort_IsCorrect(
        string input, int pcsLength, int[] order, int[] eqClasses, bool inputWithTerminator, int[] expectedResult)
    {
        var sorterBuilderInput = new SorterBuilderInput(input, pcsLength, order, eqClasses, inputWithTerminator);
        var sorter = SorterBuilder(sorterBuilderInput);
        var result = sorter.Sort();
        Assert.IsTrue(expectedResult.SequenceEqual(result),
            $"Expected: [{string.Join(", ", expectedResult)}], Actual: [{string.Join(", ", result)}]");
    }
}

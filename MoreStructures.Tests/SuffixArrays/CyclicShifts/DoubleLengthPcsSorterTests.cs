using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

public abstract class DoubleLengthPcsSorterTests
{
    protected IDoubleLengthPcsSorter Sorter { get; }

    protected DoubleLengthPcsSorterTests(IDoubleLengthPcsSorter sorter)
    {
        Sorter = sorter;
    }

    [DataRow("ab", 1, new[] { 0, 1 }, new[] { 0, 1 }, 
        new[] { 0, 1 })]
    [DataRow("aba", 1, new[] { 0, 2, 1 }, new[] { 0, 1, 0 }, 
        new[] { 2, 0, 1 })]
    [DataRow("abba", 1, new[] { 0, 3, 1, 2 }, new[] { 0, 1, 1, 0 }, 
        new[] { 3, 0, 2, 1 })]
    [DataRow("abba", 2, new[] { 3, 0, 2, 1 }, new[] { 1, 3, 2, 0 },
        new[] { 3, 0, 2, 1 })]
    [DataRow("abba", 3, new[] { 3, 0, 2, 1 }, new[] { 1, 3, 2, 0 },
        new[] { 3, 0, 2, 1 })]
    [DataRow("abba", 4, new[] { 3, 0, 2, 1 }, new[] { 1, 3, 2, 0 },
        new[] { 3, 0, 2, 1 })]
    [DataRow("abcd", 4, new[] { 0, 1, 2, 3 }, new[] { 0, 1, 2, 3 },
        new[] { 0, 1, 2, 3 })]
    [DataRow("cabccba", 1, new[] { 1, 6, 5, 2, 0, 4, 3 }, new[] { 2, 0, 1, 2, 2, 1, 0 }, 
        new[] { 1, 6, 5, 2, 0, 4, 3 })]
    [DataRow("cabccba", 2, new[] { 1, 6, 5, 2, 0, 4, 3 }, new[] { 4, 0, 3, 6, 5, 2, 1 },
        new[] { 1, 6, 5, 2, 0, 4, 3 })]
    [DataRow("cabccba", 3, new[] { 1, 6, 5, 2, 0, 4, 3 }, new[] { 4, 0, 3, 6, 5, 2, 1 },
        new[] { 1, 6, 5, 2, 0, 4, 3 })]
    [DataTestMethod]
    public void Sort_IsCorrect(string input, int pcsLength, int[] order, int[] eqClasses, int[] expectedResult)
    {
        var result = Sorter.Sort(input, pcsLength, order, eqClasses);
        Assert.IsTrue(expectedResult.SequenceEqual(result),
            $"Expected [{string.Join(", ", expectedResult)}], Got: [{string.Join(", ", result)}]");
    }
}

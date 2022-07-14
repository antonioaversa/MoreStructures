using MoreStructures.Strings.Sorting;
using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class OrderBasedSingleCharPCSClassifierTests : SingleCharPCSClassifierTests
{
    private static ICharsSorter CharsSorter { get; } = new QuickSortCharsSorter();

    public OrderBasedSingleCharPCSClassifierTests() 
        : base(input => new OrderBasedSingleCharPCSClassifier(input, CharsSorter.Sort(input)))
    {
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenInputAndOrderHaveDifferentLength()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new OrderBasedSingleCharPCSClassifier("a", new List<int> { }));
        Assert.ThrowsException<ArgumentException>(
            () => new OrderBasedSingleCharPCSClassifier("ab", new List<int> { 1, 2, 3 }));
        Assert.ThrowsException<ArgumentException>(
            () => new OrderBasedSingleCharPCSClassifier("ab", new List<int> { 1 }));
    }
}

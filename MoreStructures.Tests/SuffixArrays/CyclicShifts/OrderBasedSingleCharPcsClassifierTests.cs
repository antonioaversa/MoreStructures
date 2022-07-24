using MoreStructures.Strings.Sorting;
using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class OrderBasedSingleCharPcsClassifierTests : SingleCharPcsClassifierTests
{
    private static ICharsSorter CharsSorter { get; } = new QuickSortCharsSorter();

    public OrderBasedSingleCharPcsClassifierTests() 
        : base(cbi => new OrderBasedSingleCharPcsClassifier(cbi.Input, CharsSorter.Sort(cbi.Input)))
    {
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenInputAndOrderHaveDifferentLength()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new OrderBasedSingleCharPcsClassifier("a", new List<int> { }));
        Assert.ThrowsException<ArgumentException>(
            () => new OrderBasedSingleCharPcsClassifier("ab", new List<int> { 1, 2, 3 }));
        Assert.ThrowsException<ArgumentException>(
            () => new OrderBasedSingleCharPcsClassifier("ab", new List<int> { 1 }));
    }
}

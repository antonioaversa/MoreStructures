using MoreStructures.Strings.Sorting;
using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class OrderBasedSingleCharPcsClassifierTests : SingleCharPcsClassifierTests
{
    public OrderBasedSingleCharPcsClassifierTests() 
        : base(cbi => new OrderBasedSingleCharPcsClassifier(
            cbi.Input,
            new QuickSortCharsSorter(cbi.InputWithTerminator ? cbi.Input[^1] : null).Sort(cbi.Input)))
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

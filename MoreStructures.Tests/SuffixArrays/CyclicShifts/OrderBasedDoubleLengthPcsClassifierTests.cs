using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class OrderBasedDoubleLengthPcsClassifierTests : DoubleLengthPcsClassifierTests
{
    public OrderBasedDoubleLengthPcsClassifierTests() 
        : base(cbi => new OrderBasedDoubleLengthPcsClassifier(cbi.Input, cbi.PcsLength, cbi.Order))
    {
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenPcsLengthIsNotCoherentWithInput()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new OrderBasedDoubleLengthPcsClassifier("a", -1, new[] { 0 }));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new OrderBasedDoubleLengthPcsClassifier("a", 0, new[] { 0 }));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new OrderBasedDoubleLengthPcsClassifier("a", 2, new[] { 0 }));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new OrderBasedDoubleLengthPcsClassifier("", 1, Array.Empty<int>()));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new OrderBasedDoubleLengthPcsClassifier("abc", 4, new[] { 0, 1, 2 }));
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenOrderIsNotCoherentWithInput()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new OrderBasedDoubleLengthPcsClassifier("a", 1, Array.Empty<int>()));
        Assert.ThrowsException<ArgumentException>(
            () => new OrderBasedDoubleLengthPcsClassifier("a", 1, new[] { 0, 1 }));
        Assert.ThrowsException<ArgumentException>(
            () => new OrderBasedDoubleLengthPcsClassifier("abc", 2, new[] { 0, 1, 2, 4 }));
    }
}
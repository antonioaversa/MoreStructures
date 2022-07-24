using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class CountingSortDoubleLengthPcsSorterTests : DoubleLengthPcsSorterTests
{
    public CountingSortDoubleLengthPcsSorterTests() : base(
        sbi => new CountingSortDoubleLengthPcsSorter(sbi.PcsLength, sbi.Order, sbi.EqClasses))
    {
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenPcsLengthIsInvalid()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new CountingSortDoubleLengthPcsSorter(0, new[] { 0 }, new[] { 0 }));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new CountingSortDoubleLengthPcsSorter(-1, new[] { 0 }, new[] { 0 }));
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenOrderAndEqClassesAreIncoherent()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new CountingSortDoubleLengthPcsSorter(2, new[] { 0, 1 }, new[] { 0 }));
        Assert.ThrowsException<ArgumentException>(
            () => new CountingSortDoubleLengthPcsSorter(2, new[] { 0, 1 }, new[] { 0, 1, 2 }));
    }
}
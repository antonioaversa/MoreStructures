using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class EqClassesBasedDoubleLengthPcsClassifierTests : DoubleLengthPcsClassifierTests
{
    public EqClassesBasedDoubleLengthPcsClassifierTests()
        : base(cbi => new EqClassesBasedDoubleLengthPcsClassifier(
            cbi.PcsLength, cbi.EqClassesPcsHalfLength, cbi.Order))
    {
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenPcsLengthIsNotCoherentWithEqClassesPcsHalfLength()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(-2, new[] { 0 }, new[] { 0 }));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(0, new[] { 0 }, new[] { 0 }));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(2, new[] { 0 }, new[] { 0 }));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(2, Array.Empty<int>(), Array.Empty<int>()));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(4, new[] { 0, 1, 2 }, new[] { 0, 1, 2 }));
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenOrderIsNotCoherentWithEqClassesPcsHalfLength()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(2, new[] { 0, 1 }, new[] { 0 }));
        Assert.ThrowsException<ArgumentException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(2, new[] { 0, 1 }, new[] { 0, 1, 2 }));
        Assert.ThrowsException<ArgumentException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(2, new[] { 0, 1 }, Array.Empty<int>()));
        Assert.ThrowsException<ArgumentException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(4, new[] { 0, 1, 2, 3 }, new[] { 0, 1, 2, 3, 4 }));
        Assert.ThrowsException<ArgumentException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(4, new[] { 0, 1, 2, 3 }, new[] { 0, 1, 2 }));
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenPcsLengthIsNotEven()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(1, new[] { 0, 1 }, new[] { 0, 1 }));
        Assert.ThrowsException<ArgumentException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(1, new[] { 0, 1 }, new[] { 0, 1 }));
        Assert.ThrowsException<ArgumentException>(
            () => new EqClassesBasedDoubleLengthPcsClassifier(3, new[] { 0, 1, 2, 4 }, new[] { 0, 1, 2, 4 }));
    }
}
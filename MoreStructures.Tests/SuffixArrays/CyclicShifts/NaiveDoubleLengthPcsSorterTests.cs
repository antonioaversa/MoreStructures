using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class NaiveDoubleLengthPcsSorterTests : DoubleLengthPcsSorterTests
{
    public NaiveDoubleLengthPcsSorterTests() : base(
        sbi => new NaiveDoubleLengthPcsSorter(sbi.Input, sbi.PcsLength, sbi.InputWithTerminator))
    {
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWhenPcsLengthIsInvalid()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new NaiveDoubleLengthPcsSorter("abc", 0, false));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new NaiveDoubleLengthPcsSorter("abc", -1, false));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new NaiveDoubleLengthPcsSorter("abc", 0, true));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new NaiveDoubleLengthPcsSorter("abc", -1, true));
    }
}

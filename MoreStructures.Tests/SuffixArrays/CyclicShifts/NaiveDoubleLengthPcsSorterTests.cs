using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class NaiveDoubleLengthPcsSorterTests : DoubleLengthPcsSorterTests
{
    public NaiveDoubleLengthPcsSorterTests() : base(new NaiveDoubleLengthPcsSorter())
    {
    }
}

using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class CountingSortDoubleLengthPcsSorterTests : DoubleLengthPcsSorterTests
{
    public CountingSortDoubleLengthPcsSorterTests() : base(new CountingSortDoubleLengthPcsSorter())
    {
    }
}
using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class MergeSortTests : InPlaceSortingTests
{
    public MergeSortTests() : base(() => new MergeSort())
    {
    }
}

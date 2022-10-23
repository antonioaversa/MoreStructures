using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class MergeSortTests : InputSortingTests
{
    public MergeSortTests() : base(() => new MergeSort())
    {
    }
}

using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class HeapSortTests : InPlaceSortingTests
{
    public HeapSortTests() : base(() => new HeapSort())
    {
    }
}

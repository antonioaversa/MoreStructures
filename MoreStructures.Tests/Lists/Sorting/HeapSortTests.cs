using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class HeapSortTests : InputSortingTests
{
    public HeapSortTests() : base(() => new HeapSort())
    {
    }
}

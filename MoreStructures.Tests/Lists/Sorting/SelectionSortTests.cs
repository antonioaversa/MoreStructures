using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class SelectionSortTests : InPlaceSortingTests
{
    public SelectionSortTests() : base(() => new SelectionSort())
    {
    }
}

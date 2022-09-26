using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class InsertionSortTests : InPlaceSortingTests
{
    public InsertionSortTests() : base(() => new InsertionSort())
    {
    }
}

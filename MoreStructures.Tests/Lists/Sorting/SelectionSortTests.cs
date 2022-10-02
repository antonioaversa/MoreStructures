using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class SelectionSortTests : InputSortingTests
{
    public SelectionSortTests() : base(() => new SelectionSort())
    {
    }
}

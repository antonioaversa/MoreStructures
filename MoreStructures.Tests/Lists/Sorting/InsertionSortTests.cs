using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class InsertionSortTests : InputSortingTests
{
    public InsertionSortTests() : base(() => new InsertionSort())
    {
    }
}

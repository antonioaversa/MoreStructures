using MoreStructures.Strings.Sorting;

namespace MoreStructures.Tests.Strings.Sorting;

[TestClass]
public class QuickSortCharsSorterTests : CharsSorterTests
{
    public QuickSortCharsSorterTests()
        : base(new QuickSortCharsSorter())
    {
    }
}

using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSort;

[TestClass]
public class MiddleIndexPivotSelectionStrategyTests : PivotSelectionStrategyTests
{
    public MiddleIndexPivotSelectionStrategyTests() : base(new MiddleIndexPivotSelectionStrategy())
    {
    }
}


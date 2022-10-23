using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSort;

[TestClass]
public class StartIndexPivotSelectionStrategyTests : PivotSelectionStrategyTests
{
    public StartIndexPivotSelectionStrategyTests() : base(new StartIndexPivotSelectionStrategy())
    {
    }
}


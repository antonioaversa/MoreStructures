using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSort;

[TestClass]
public class EndIndexPivotSelectionStrategyTests : PivotSelectionStrategyTests
{
    public EndIndexPivotSelectionStrategyTests() : base(new EndIndexPivotSelectionStrategy())
    {
    }
}


using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSort;

[TestClass]
public class LomutoThreeWayPartitionStrategyTests_PivotStartIndex : ThreeWayPartitionStrategyTests
{
    public LomutoThreeWayPartitionStrategyTests_PivotStartIndex()
        : base(new LomutoThreeWayPartitionStrategy(new StartIndexPivotSelectionStrategy()))
    {
    }
}

[TestClass]
public class LomutoThreeWayPartitionStrategyTests_PivotMiddleIndex : ThreeWayPartitionStrategyTests
{
    public LomutoThreeWayPartitionStrategyTests_PivotMiddleIndex()
        : base(new LomutoThreeWayPartitionStrategy(new MiddleIndexPivotSelectionStrategy()))
    {
    }
}

[TestClass]
public class LomutoThreeWayPartitionStrategyTests_PivotEndIndex : ThreeWayPartitionStrategyTests
{
    public LomutoThreeWayPartitionStrategyTests_PivotEndIndex()
        : base(new LomutoThreeWayPartitionStrategy(new EndIndexPivotSelectionStrategy()))
    {
    }
}

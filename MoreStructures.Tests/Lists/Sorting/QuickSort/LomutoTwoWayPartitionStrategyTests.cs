using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSort;

[TestClass]
public class LomutoTwoWayPartitionStrategyTests_PivotStartIndex : ThreeWayPartitionStrategyTests
{
    public LomutoTwoWayPartitionStrategyTests_PivotStartIndex() 
        : base(new LomutoTwoWayPartitionStrategy(new StartIndexPivotSelectionStrategy()))
    {
    }
}

[TestClass]
public class LomutoTwoWayPartitionStrategyTests_PivotMiddleIndex : ThreeWayPartitionStrategyTests
{
    public LomutoTwoWayPartitionStrategyTests_PivotMiddleIndex()
        : base(new LomutoTwoWayPartitionStrategy(new MiddleIndexPivotSelectionStrategy()))
    {
    }
}

[TestClass]
public class LomutoTwoWayPartitionStrategyTests_PivotEndIndex : ThreeWayPartitionStrategyTests
{
    public LomutoTwoWayPartitionStrategyTests_PivotEndIndex()
        : base(new LomutoTwoWayPartitionStrategy(new EndIndexPivotSelectionStrategy()))
    {
    }
}


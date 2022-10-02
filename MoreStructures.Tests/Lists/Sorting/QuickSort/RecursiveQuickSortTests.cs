using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSort;

[TestClass]
public class RecursiveQuickSortTests_WithLomutoTwo_PivotStartIndex : InputSortingTests
{
    public RecursiveQuickSortTests_WithLomutoTwo_PivotStartIndex()
        : base(() => new RecursiveQuickSort(
            new IdentityShuffleStrategy(),
            new LomutoTwoWayPartitionStrategy(
                new StartIndexPivotSelectionStrategy())))
    {
    }
}

[TestClass]
public class RecursiveQuickSortTests_WithLomutoTwo_PivotEndIndex : InputSortingTests
{
    public RecursiveQuickSortTests_WithLomutoTwo_PivotEndIndex()
        : base(() => new RecursiveQuickSort(
            new IdentityShuffleStrategy(),
            new LomutoTwoWayPartitionStrategy(
                new EndIndexPivotSelectionStrategy())))
    {
    }
}

[TestClass]
public class RecursiveQuickSortTests_WithLomutoTwo_PivotMiddleIndex : InputSortingTests
{
    public RecursiveQuickSortTests_WithLomutoTwo_PivotMiddleIndex()
        : base(() => new RecursiveQuickSort(
            new IdentityShuffleStrategy(),
            new LomutoTwoWayPartitionStrategy(
                new MiddleIndexPivotSelectionStrategy())))
    {
    }
}

[TestClass]
public class RecursiveQuickSortTests_WithLomutoThree_PivotStartIndex : InputSortingTests
{
    public RecursiveQuickSortTests_WithLomutoThree_PivotStartIndex()
        : base(() => new RecursiveQuickSort(
            new IdentityShuffleStrategy(),
            new LomutoThreeWayPartitionStrategy(
                new StartIndexPivotSelectionStrategy())))
    {
    }
}

[TestClass]
public class RecursiveQuickSortTests_WithLomutoThree_PivotEndIndex : InputSortingTests
{
    public RecursiveQuickSortTests_WithLomutoThree_PivotEndIndex()
        : base(() => new RecursiveQuickSort(
            new IdentityShuffleStrategy(),
            new LomutoThreeWayPartitionStrategy(
                new EndIndexPivotSelectionStrategy())))
    {
    }
}

[TestClass]
public class RecursiveQuickSortTests_WithLomutoThree_PivotMiddleIndex : InputSortingTests
{
    public RecursiveQuickSortTests_WithLomutoThree_PivotMiddleIndex()
        : base(() => new RecursiveQuickSort(
            new IdentityShuffleStrategy(),
            new LomutoThreeWayPartitionStrategy(
                new MiddleIndexPivotSelectionStrategy())))
    {
    }
}
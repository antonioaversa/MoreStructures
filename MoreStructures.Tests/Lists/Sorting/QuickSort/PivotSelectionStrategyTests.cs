using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSort;

public abstract class PivotSelectionStrategyTests
{
    protected IPivotSelectionStrategy Strategy { get; }

    protected PivotSelectionStrategyTests(IPivotSelectionStrategy strategy)
    {
        Strategy = strategy;
    }

    [TestMethod]
    public void Select_GivesAnIndexWithinTheBoundariesOfTheWindow()
    {
        var numbers0To9 = Enumerable.Range(0, 10).ToList();
        var pivotIndex1 = Strategy.Select(numbers0To9, Comparer<int>.Default, 0, 2);
        Assert.IsTrue(pivotIndex1 >= 0 && pivotIndex1 <= 2);

        var pivotIndex2 = Strategy.Select(numbers0To9, Comparer<int>.Default, 0, 0);
        Assert.IsTrue(pivotIndex2 == 0);

        var pivotIndex3 = Strategy.Select(numbers0To9, Comparer<int>.Default, 5, 7);
        Assert.IsTrue(pivotIndex3 >= 5 && pivotIndex3 <= 7);
    }
}

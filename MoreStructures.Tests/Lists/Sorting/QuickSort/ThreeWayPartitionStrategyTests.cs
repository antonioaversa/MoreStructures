using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSort;

[TestClass]
public abstract class ThreeWayPartitionStrategyTests
{
    protected IThreeWayPartitionStrategy Strategy { get; }

    protected ThreeWayPartitionStrategyTests(IThreeWayPartitionStrategy strategy)
    {
        Strategy = strategy;
    }

    [TestMethod]
    public void Partition_IsCorrect()
    {
        var numberOfNumbers = 7;
        var numbers = Enumerable.Range(0, numberOfNumbers).ToList();
        foreach (var permutation in TestUtilities.GeneratePermutations(numbers))
        {
            for (var start = 0; start < numberOfNumbers; start++)
            {
                for (var end = start; end < numberOfNumbers; end++)
                {
                    var copy = permutation.ToArray();
                    var (i1, i2) = Strategy.Partition(permutation, Comparer<int>.Default, start, end);

                    if (i1 <= i2)
                    {
                        // Middle segment contains at least a pivot
                        var pivot = permutation[i1];
                        Assert.IsTrue(permutation
                            .Where((n, i) => i >= start && i <= end && i < i1)
                            .All(n => n <= pivot));
                        Assert.IsTrue(permutation
                            .Where((n, i) => i >= start && i <= end && i >= i1 && i <= i2)
                            .All(n => n == pivot));
                        Assert.IsTrue(permutation
                            .Where((n, i) => i >= start && i <= end && i > i2)
                            .All(n => n >= pivot));
                    }
                    else
                    {
                        // Middle segment is empty: pivots are unsorted in left and/or right segments
                        Assert.IsTrue(permutation
                            .Where((n1, i) => i >= start && i <= end && i < i1)
                            .All(n1 => permutation
                                .Where((n, i) => i >= start && i <= end && i > i2)
                                .All(n2 => n1 <= n2)));
                    }
                }
            }
        }
    }
}

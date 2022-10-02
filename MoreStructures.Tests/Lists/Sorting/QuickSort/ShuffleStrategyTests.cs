using MoreStructures.Lists.Sorting.QuickSort;

namespace MoreStructures.Tests.Lists.Sorting.QuickSortp;

public abstract class ShuffleStrategyTests
{
    protected IShuffleStrategy Strategy { get; }

    protected ShuffleStrategyTests(IShuffleStrategy strategy)
    {
        Strategy = strategy;
    }

    [TestMethod]
    public void Shuffle_DoesNothingWhenWindowIsEmptyOrSingleton()
    {
        var list = Enumerable.Range(0, 10).Select(n => (n + 5) % 10).ToList();
        var copy = list.ToList();
        for (var start = 0; start < list.Count; start++)
        {
            Strategy.Shuffle(list, start, start);
            Assert.IsTrue(copy.SequenceEqual(list));
            Strategy.Shuffle(list, start, start - 1);
            Assert.IsTrue(copy.SequenceEqual(list));
            Strategy.Shuffle(list, start, start - 2);
            Assert.IsTrue(copy.SequenceEqual(list));
        }
    }

    [DataRow(new int[] { })]
    [DataRow(new int[] { 1 })]
    [DataRow(new[] { 1, 2, 3 })]
    [DataRow(new int[] { 3, 3, -2 })]
    [DataTestMethod]
    public void Shuffle_PreservesTheContentOfTheInput(int[] list)
    {
        var listSorted = list.OrderBy(n => n).ToList();
        for (var start = 0; start < list.Length; start++)
        {
            for (var end = start - 1; end < list.Length; end++)
            {
                Strategy.Shuffle(list, start, end);
                Assert.IsTrue(list.OrderBy(n => n).SequenceEqual(listSorted));
            }
        }
    }
}

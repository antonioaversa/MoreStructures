using MoreStructures.DisjointSets;

namespace MoreStructures.Tests.DisjointSets;

[TestClass]
public class WeightedQuickUnionDisjointSetTests : DisjointSetTests
{
    public WeightedQuickUnionDisjointSetTests() 
        : base(valuesCount => new WeightedQuickUnionDisjointSet(valuesCount))
    {
    }

    protected WeightedQuickUnionDisjointSetTests(Func<int, IDisjointSet> builder) 
        : base(builder)
    {
    }

    [TestMethod]
    public void GetRanks_IsZeroAfterCtor()
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(10);
        Assert.IsTrue(disjointSet.GetRanks().All(h => h == 0));
    }

    [TestMethod]
    public void GetRanks_ReturnsCopy()
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(10);
        var ranks1 = disjointSet.GetRanks();
        ranks1[0] = 1;
        var ranks2 = disjointSet.GetRanks();
        Assert.AreEqual(0, ranks2[0]);
    }

    [TestMethod]
    public void Union_UpdatesRanks()
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(10);
        disjointSet.Union(0, 1);
        var ranks = disjointSet.GetRanks();
        Assert.AreEqual(1, ranks.Count(h => h == 1));
        Assert.AreEqual(9, ranks.Count(h => h == 0));
    }

    [TestMethod]
    public void Union_MergesByRank()
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(10);
        disjointSet.Union(0, 1);
        disjointSet.Union(2, 3);
        disjointSet.Union(4, 5);
        disjointSet.Union(3, 4);
        disjointSet.Union(1, 2); 
        var ranks = disjointSet.GetRanks();
        Assert.AreEqual(1, ranks.Count(h => h == 2));
    }

    [DataRow(1)]
    [DataRow(5)]
    [DataRow(8)]
    [DataRow(10)]
    [DataTestMethod]
    public void Union_MinimizesRankGrow(int numberOfItems)
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(numberOfItems);
        for (var i = 0; i < numberOfItems - 1; i++)
            disjointSet.Union(i, i + 1);
        Assert.IsTrue(disjointSet.GetRanks().Max() <= Math.Ceiling(Math.Log(numberOfItems, 2)));
    }
}
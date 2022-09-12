using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.DisjointSets;

namespace MoreStructures.Tests.DisjointSets;

[TestClass]
public class WeightedQuickUnionDisjointSetTests : DisjointSetTests
{
    public WeightedQuickUnionDisjointSetTests() 
        : base(valuesCount => new WeightedQuickUnionDisjointSet(valuesCount))
    {
    }

    [TestMethod]
    public void GetHeights_IsZeroAfterCtor()
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(10);
        Assert.IsTrue(disjointSet.GetHeights().All(h => h == 0));
    }

    [TestMethod]
    public void GetHeights_ReturnsCopy()
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(10);
        var heights1 = disjointSet.GetHeights();
        heights1[0] = 1;
        var heights2 = disjointSet.GetHeights();
        Assert.AreEqual(0, heights2[0]);
    }

    [TestMethod]
    public void Union_UpdatesHeights()
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(10);
        disjointSet.Union(0, 1);
        var heights = disjointSet.GetHeights();
        Assert.AreEqual(1, heights.Count(h => h == 1));
        Assert.AreEqual(9, heights.Count(h => h == 0));
    }

    [TestMethod]
    public void Union_MergesByHeight()
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(10);
        disjointSet.Union(0, 1);
        disjointSet.Union(2, 3);
        disjointSet.Union(4, 5);
        disjointSet.Union(3, 4);
        disjointSet.Union(1, 2); 
        var heights = disjointSet.GetHeights();
        Assert.AreEqual(1, heights.Count(h => h == 2));
    }

    [DataRow(1)]
    [DataRow(5)]
    [DataRow(8)]
    [DataRow(10)]
    [DataTestMethod]
    public void Union_MinimizesHeightGrow(int numberOfItems)
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(numberOfItems);
        for (var i = 0; i < numberOfItems - 1; i++)
            disjointSet.Union(i, i + 1);
        Assert.IsTrue(disjointSet.GetHeights().Max() <= Math.Ceiling(Math.Log(numberOfItems, 2)));
    }
}
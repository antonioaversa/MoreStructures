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
    public void Heights_IsZeroAfterCtor()
    {
        var disjointSet = new WeightedQuickUnionDisjointSet(10);
        Assert.IsTrue(disjointSet.Heights.All(h => h == 0));
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
        Assert.IsTrue(disjointSet.Heights.Max() <= Math.Ceiling(Math.Log(numberOfItems, 2)));
    }
}
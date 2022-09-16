using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.DisjointSets;

namespace MoreStructures.Tests.DisjointSets;

[TestClass]
public class PathCompressionWeightedQuickUnionDisjointSetTests : WeightedQuickUnionDisjointSetTests
{
    public PathCompressionWeightedQuickUnionDisjointSetTests()
        : base(valuesCount => new PathCompressionWeightedQuickUnionDisjointSet(valuesCount))
    {
    }

    [TestMethod]
    public void Find_MakesTreeFlat()
    {
        var numberOfItems = 4;
        var disjointSet = new PathCompressionWeightedQuickUnionDisjointSet(numberOfItems);
        disjointSet.Union(0, 1);
        disjointSet.Union(2, 3);
        disjointSet.Union(0, 2);

        // 0 <- 1
        //   <- 2 <- 3
        var parentsBeforeFind = disjointSet.GetParents();
        
        disjointSet.Find(3);

        // 0 <- 1
        //   <- 2
        //   <- 3
        var parentsAfterFind = disjointSet.GetParents();
        Assert.AreNotEqual(parentsBeforeFind[3], parentsAfterFind[3]);
    }
}

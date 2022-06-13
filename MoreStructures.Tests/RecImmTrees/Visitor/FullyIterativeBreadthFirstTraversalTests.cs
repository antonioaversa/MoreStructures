using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees.Visitor;
using System.Linq;

namespace MoreStructures.Tests.RecImmTrees.Visitor;

[TestClass]
public class FullyIterativeBreadthFirstTraversalTests
    : BreadthFirstTraversalTests<FullyIterativeBreadthFirstTraversal<TreeMock.Edge, TreeMock.Node>>
{
    [TestMethod]
    public void Visit_DoesntOverflowWithDeepStructures()
    {
        var visitCounter = new VisitCounter();

        var numberOfIntermediateNodes = 10000;
        var root = TreeMock.BuildMostUnbalancedTree(numberOfIntermediateNodes);
        var visitStrategy = new FullyIterativeBreadthFirstTraversal<TreeMock.Edge, TreeMock.Node>();

        _ = visitStrategy.Visit(root).Select(visitCounter.Visitor).ToList();
        Assert.AreEqual(numberOfIntermediateNodes + 1, visitCounter.CountOfVisitedNodes); // Intermediates + 1 leaf
    }
}


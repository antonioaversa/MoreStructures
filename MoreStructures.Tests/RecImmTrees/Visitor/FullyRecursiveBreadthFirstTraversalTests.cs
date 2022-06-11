using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees.Visitor;

namespace MoreStructures.Tests.RecImmTrees.Visitor;

[TestClass]
public class FullyRecursiveBreadthFirstTraversalTests
    : BreadthFirstTraversalTests<FullyRecursiveBreadthFirstTraversal<TreeMock.Edge, TreeMock.Node>>
{
    [TestMethod]
    public void Visit_DoesntStackOverflowWithNotSoDeepStructures()
    {
        var visitCounter = new VisitCounter();

        var numberOfIntermediateNodes = 100;
        var root = TreeMock.BuildMostUnbalancedTree(numberOfIntermediateNodes);
        var visitStrategy = new FullyRecursiveDepthFirstTraversal<TreeMock.Edge, TreeMock.Node>();

        visitStrategy.Visit(root, visitCounter.Visitor);
        Assert.AreEqual(numberOfIntermediateNodes + 1, visitCounter.CountOfVisitedNodes); // Intermediates + 1 leaf
    }
}


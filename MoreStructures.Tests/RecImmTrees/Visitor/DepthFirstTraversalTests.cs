using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreStructures.Tests.RecImmTrees.Visitor;

[TestClass]
public class DepthFirstTraversalTests
{
    [TestMethod]
    public void Visit_TraversalOrderParentFirst()
    {
        var visits = new List<(int? edgeId, int nodeId)> { };
        Visitor<TreeMock.Node, TreeTraversalContext<TreeMock.Edge, TreeMock.Node>> visitsAppenderVisitor
            = (node, visitContext) => visits.Add((visitContext.IncomingEdge?.Id, node.Id));

        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new FullyRecursiveDepthFirstTraversal<TreeMock.Edge, TreeMock.Node>()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = edgesAndNodes => edgesAndNodes.OrderBy(edgeAndNode => edgeAndNode.Key.Id),
        };
        visitStrategy.Visit(root, visitsAppenderVisitor);
        Assert.IsTrue(visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (0, 1), (1, 2), (2, 3), (3, 4), (4, 5), (5, 6), (6, 7), (7, 8),
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderChildrenFirst()
    {
        var visits = new List<(int? edgeId, int nodeId)> { };
        Visitor<TreeMock.Node, TreeTraversalContext<TreeMock.Edge, TreeMock.Node>> visitsAppenderVisitor
            = (node, visitContext) => visits.Add((visitContext.IncomingEdge?.Id, node.Id));

        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new FullyRecursiveDepthFirstTraversal<TreeMock.Edge, TreeMock.Node>()
        {
            TraversalOrder = TreeTraversalOrder.ChildrenFirst,
            ChildrenSorter = edgesAndNodes => edgesAndNodes.OrderBy(edgeAndNode => edgeAndNode.Key.Id),
        };
        visitStrategy.Visit(root, visitsAppenderVisitor);
        Assert.IsTrue(visits.SequenceEqual(new (int?, int)[] 
        { 
            (0, 1), (3, 4), (4, 5), (2, 3), (5, 6), (6, 7), (1, 2), (7, 8), (null, 0),
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderNotSupported()
    {
        Visitor<TreeMock.Node, TreeTraversalContext<TreeMock.Edge, TreeMock.Node>> nopVisitor
            = (node, visitContext) => { };
        var visitStrategy = new FullyRecursiveDepthFirstTraversal<TreeMock.Edge, TreeMock.Node>()
        {
            TraversalOrder = (TreeTraversalOrder)(-1),
            ChildrenSorter = edgesAndNodes => edgesAndNodes.OrderBy(edgeAndNode => edgeAndNode.Key.Id),
        };

        Assert.ThrowsException<NotSupportedException>(() => visitStrategy.Visit(new TreeMock.Node(0), nopVisitor));
    }
}

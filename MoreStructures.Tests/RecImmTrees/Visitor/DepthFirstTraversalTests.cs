using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees.Visitor;
using System;
using System.Linq;

namespace MoreStructures.Tests.RecImmTrees.Visitor;

public abstract class DepthFirstTraversalTests<TDepthFirstTraversal>
     : TreeTraversalTests<TDepthFirstTraversal>
    where TDepthFirstTraversal : DepthFirstTraversal<TreeMock.Edge, TreeMock.Node>, new()
{
    [TestMethod]
    public void Visit_DocExamples_ParentFirst()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildDocExample();

        _ = new TDepthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        }.Visit(root).Select(visitAppender.Visitor).ToList();

        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (0, 1), (1, 2), (2, 3), (3, 4), (4, 5), (5, 6), (6, 7), (7, 8), (8, 9), (9, 10)
        }));

        visitAppender.Clear();

        _ = new TDepthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdDescBasedChildrenSorter,
        }.Visit(root).Select(visitAppender.Visitor).ToList();

        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (6, 7), (7, 8), (9, 10), (8, 9), (5, 6), (0, 1), (4, 5), (2, 3), (3, 4), (1, 2)
        }));

        visitAppender.Clear();

        _ = new TDepthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdMedianBasedChildrenSorter,
        }.Visit(root).Select(visitAppender.Visitor).ToList();

        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (5, 6), (6, 7), (7, 8), (9, 10), (8, 9), (0, 1), (2, 3), (3, 4), (4, 5), (1, 2)
        }));
    }

    [TestMethod]
    public void Visit_DocExamples_ChildrenFirst()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildDocExample();

        _ = new TDepthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ChildrenFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        }.Visit(root).Select(visitAppender.Visitor).ToList();

        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (1, 2), (3, 4), (2, 3), (4, 5), (0, 1), (5, 6), (8, 9), (9, 10), (7, 8), (6, 7), (null, 0)
        }));

        visitAppender.Visits.Clear();

        _ = new TDepthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ChildrenFirst,
            ChildrenSorter = TreeMock.EdgeIdDescBasedChildrenSorter,
        }.Visit(root).Select(visitAppender.Visitor).ToList();

        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
             (9, 10), (8, 9), (7, 8), (6, 7), (5, 6), (4, 5), (3, 4), (2, 3), (1, 2), (0, 1), (null, 0)
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderParentFirst()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new TDepthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        _ = visitStrategy.Visit(root).Select(visitAppender.Visitor).ToList();
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (0, 1), (1, 2), (2, 3), (3, 4), (4, 5), (5, 6), (6, 7), (7, 8),
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderChildrenFirst()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new TDepthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ChildrenFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        _ = visitStrategy.Visit(root).Select(visitAppender.Visitor).ToList();
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[] 
        { 
            (0, 1), (3, 4), (4, 5), (2, 3), (5, 6), (6, 7), (1, 2), (7, 8), (null, 0),
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderNotSupported_OnSingleton()
    {
        var visitStrategy = new TDepthFirstTraversal()
        {
            TraversalOrder = (TreeTraversalOrder)(-1),
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        Assert.ThrowsException<NotSupportedException>(
            () => visitStrategy.Visit(new TreeMock.Node(0)).ToList());
    }

    [TestMethod]
    public void Visit_TraversalOrderNotSupported_OnTreeWithMultipleNodes()
    {
        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new TDepthFirstTraversal()
        {
            TraversalOrder = (TreeTraversalOrder)(-1),
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        Assert.ThrowsException<NotSupportedException>(
            () => visitStrategy.Visit(root).ToList());
    }

    [TestMethod]
    public void Visit_ChildrenSorter_IsApplied()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new TDepthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdDescBasedChildrenSorter,
        };

        _ = visitStrategy.Visit(root).Select(visitAppender.Visitor).ToList();
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (7, 8), (1, 2), (6, 7), (5, 6), (2, 3), (4, 5), (3, 4), (0, 1),
        }));
    }

    [TestMethod]
    public void Visit_MultiLevelsBacktrackTree()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildMultiLevelsBacktrackTree();
        var visitStrategy = new TDepthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        _ = visitStrategy.Visit(root).Select(visitAppender.Visitor).ToList();
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (0, 1), (1, 2), (2, 3), (3, 4), (4, 5), (5, 6), (6, 7), (7, 8), (8, 9)
        }));
    }
}

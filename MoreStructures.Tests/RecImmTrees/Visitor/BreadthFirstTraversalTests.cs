using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees.Visitor;
using System;
using System.Linq;

namespace MoreStructures.Tests.RecImmTrees.Visitor;

public abstract class BreadthFirstTraversalTests<TBreadthFirstTraversal>
    where TBreadthFirstTraversal : BreadthFirstTraversal<TreeMock.Edge, TreeMock.Node>, new()
{
    [TestMethod]
    public void Visit_DocExamples_ParentFirst()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildDocExample();

        new TBreadthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        }.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[] 
        {
            (null, 0), (0, 1), (5, 6), (6, 7), (1, 2), (2, 3), (4, 5), (7, 8), (3, 4), (8, 9), (9, 10)
        }));

        visitAppender.Clear();

        new TBreadthFirstTraversal() 
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdDescBasedChildrenSorter,
        }.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (6, 7), (5, 6), (0, 1), (7, 8), (4, 5), (2, 3), (1, 2), (9, 10), (8, 9), (3, 4)
        }));

        visitAppender.Clear();

        new TBreadthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdMedianBasedChildrenSorter,
        }.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (5, 6), (6, 7), (0, 1), (7, 8), (2, 3), (4, 5), (1, 2), (9, 10), (8, 9), (3, 4)
        }));
    }

    [TestMethod]
    public void Visit_DocExamples_ChildrenFirst()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildDocExample();

        new TBreadthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ChildrenFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        }.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (3, 4), (8, 9), (9, 10), (1, 2), (2, 3), (4, 5), (7, 8), (0, 1), (5, 6), (6, 7), (null, 0)
        }));

        visitAppender.Clear();

        new TBreadthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ChildrenFirst,
            ChildrenSorter = TreeMock.EdgeIdDescBasedChildrenSorter,
        }.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (9, 10), (8, 9), (3, 4), (7, 8), (4, 5), (2, 3), (1, 2), (6, 7), (5, 6), (0, 1), (null, 0)
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderParentFirst()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new TBreadthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        visitStrategy.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (0, 1), (1, 2), (7, 8), (2, 3), (5, 6), (6, 7), (3, 4), (4, 5),
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderChildrenFirst()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new TBreadthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ChildrenFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        visitStrategy.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (3, 4), (4, 5), (2, 3), (5, 6), (6, 7), (0, 1), (1, 2), (7, 8), (null, 0),
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderNotSupported_OnSingleton()
    {
        static void nopVisitor(TreeMock.Node node, TreeTraversalContext<TreeMock.Edge, TreeMock.Node> visitContext)
        {
        }

        var visitStrategy = new TBreadthFirstTraversal()
        {
            TraversalOrder = (TreeTraversalOrder)(-1),
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        Assert.ThrowsException<NotSupportedException>(() => visitStrategy.Visit(new TreeMock.Node(0), nopVisitor));
    }

    [TestMethod]
    public void Visit_TraversalOrderNotSupported_OnTreeWithMultipleNodes()
    {
        static void nopVisitor(TreeMock.Node node, TreeTraversalContext<TreeMock.Edge, TreeMock.Node> visitContext)
        {
        }

        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new TBreadthFirstTraversal()
        {
            TraversalOrder = (TreeTraversalOrder)(-1),
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        Assert.ThrowsException<NotSupportedException>(() => visitStrategy.Visit(root, nopVisitor));
    }

    [TestMethod]
    public void Visit_ChildrenSorter_IsApplied()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildExampleTree();
        var visitStrategy = new TBreadthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdDescBasedChildrenSorter,
        };

        visitStrategy.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (7, 8), (1, 2), (0, 1), (6, 7), (5, 6), (2, 3), (4, 5), (3, 4),
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderParentFirst_MultiLevelsBacktrackTree()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildMultiLevelsBacktrackTree();
        var visitStrategy = new TBreadthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ParentFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        visitStrategy.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (null, 0), (0, 1), (1, 2), (4, 5), (8, 9), (2, 3), (5, 6), (3, 4), (6, 7), (7, 8)
        }));
    }

    [TestMethod]
    public void Visit_TraversalOrderChildrenFirst_MultiLevelsBacktrackTree()
    {
        var visitAppender = new VisitAppender();
        var root = TreeMock.BuildMultiLevelsBacktrackTree();
        var visitStrategy = new TBreadthFirstTraversal()
        {
            TraversalOrder = TreeTraversalOrder.ChildrenFirst,
            ChildrenSorter = TreeMock.EdgeIdBasedChildrenSorter,
        };

        visitStrategy.Visit(root, visitAppender.Visitor);
        Assert.IsTrue(visitAppender.Visits.SequenceEqual(new (int?, int)[]
        {
            (7, 8), (3, 4), (6, 7), (2, 3), (5, 6), (0, 1), (1, 2), (4, 5), (8, 9), (null, 0)
        }));
    }
}


using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.CountTrees;
using System.Collections.Generic;
using System.Linq;
using static MoreStructures.Tests.CountTrees.TreeMock;

namespace MoreStructures.Tests.CountTrees;

[TestClass]
public class CountTreeNodeTests
{
    [TestMethod]
    public void Equals_BasedOnWrappedNode()
    {
        var wrapping1 = new CountTreeNode<Edge, Node>(
            new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } });
        var wrapping2 = new CountTreeNode<Edge, Node>(
            new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } });
        Assert.AreEqual(wrapping1, wrapping2);

        var wrapping3 = new CountTreeNode<Edge, Node>(
            new Node(13) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } });
        Assert.AreNotEqual(wrapping1, wrapping3);

        var wrapping4 = new CountTreeNode<Edge, Node>(
            new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(21) } });
        Assert.AreNotEqual(wrapping1, wrapping4);
    }

    [TestMethod]
    public void Children_PreservedWrappedStructure()
    {
        var wrapped = new Node(1) 
        { 
            Children = new Dictionary<Edge, Node> 
            {
                [new(1)] = new(2),
                [new(2)] = new(3)
                {
                    Children = new Dictionary<Edge, Node>
                    {
                        [new(3)] = new(4),
                        [new(4)] = new(5),
                    }
                },
                [new(5)] = new(6),
            } 
        };

        var wrapping = new CountTreeNode<Edge, Node>(wrapped);
        Assert.AreEqual(3, wrapping.Children.Count);
        Assert.AreEqual(2, wrapping.Children.Single(n => n.Key.WrappedEdge.Id == 1).Value.WrappedNode.Id);
        Assert.AreEqual(0, wrapping.Children.Single(n => n.Key.WrappedEdge.Id == 1).Value.WrappedNode.Children.Count);
        Assert.AreEqual(3, wrapping.Children.Single(n => n.Key.WrappedEdge.Id == 2).Value.WrappedNode.Id);
        Assert.AreEqual(2, wrapping.Children.Single(n => n.Key.WrappedEdge.Id == 2).Value.WrappedNode.Children.Count);
        Assert.AreEqual(6, wrapping.Children.Single(n => n.Key.WrappedEdge.Id == 5).Value.WrappedNode.Id);
        Assert.AreEqual(0, wrapping.Children.Single(n => n.Key.WrappedEdge.Id == 5).Value.WrappedNode.Children.Count);
    }

    [TestMethod]
    public void WrappedNode_IsPreserved()
    {
        var wrapped = new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } };
        var wrapping = new CountTreeNode<Edge, Node>(wrapped);
        Assert.AreEqual(wrapped, wrapping.WrappedNode);
    }

    [TestMethod]
    public void DescendantsCount_OfLeafIsZero()
    {
        var leafWrapped = new Node(12) { Children = new Dictionary<Edge, Node> { } };
        var leafWrapping = new CountTreeNode<Edge, Node>(leafWrapped);
        Assert.AreEqual(0, leafWrapping.DescendantsCount);
    }

    [TestMethod]
    public void DescendantsCount_OfSingletonIsOne()
    {
        var leafWrapped = new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } };
        var leafWrapping = new CountTreeNode<Edge, Node>(leafWrapped);
        Assert.AreEqual(1, leafWrapping.DescendantsCount);
    }

    [TestMethod]
    public void DescendantsCount_OfThreeWithNNodesIsN()
    {
        Node leafWrapped = BuildExampleNode();
        var leafWrapping = new CountTreeNode<Edge, Node>(leafWrapped);
        Assert.AreEqual(4, leafWrapping.DescendantsCount);
    }

    [TestMethod]
    public void DescendantsCount_GivesConsistentResults()
    {
        Node leafWrapped = BuildExampleNode();
        var leafWrapping = new CountTreeNode<Edge, Node>(leafWrapped);
        Assert.AreEqual(4, leafWrapping.DescendantsCount);
        Assert.AreEqual(4, leafWrapping.DescendantsCount);
        Assert.AreEqual(4, leafWrapping.DescendantsCount);
    }

    [TestMethod]
    public void DescendantsCount_DoesntStackOverflowWithDeepStructures()
    {
        var numberOfIntermediateNodes = 10000;
        var root = BuildMostUnbalancedTree(numberOfIntermediateNodes);
        var countTreeRoot = new CountTreeNode<Edge, Node>(root);
        Assert.AreEqual(numberOfIntermediateNodes, countTreeRoot.DescendantsCount); // Root node excluded
    }

    private static Node BuildExampleNode()
    {
        return new Node(12)
        {
            Children = new Dictionary<Edge, Node>
            {
                [new(2)] = new(20)
                {
                    Children = new Dictionary<Edge, Node>
                    {
                        [new(3)] = new(21),
                        [new(4)] = new(22)
                        {
                            Children = new Dictionary<Edge, Node>
                            {
                                [new(5)] = new(23)
                            }
                        }
                    }
                }
            }
        };
    }
}

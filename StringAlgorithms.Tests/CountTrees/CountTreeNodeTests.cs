using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.CountTrees;
using System.Collections.Generic;
using static StringAlgorithms.Tests.CountTrees.TreeMock;

namespace StringAlgorithms.Tests.CountTrees;

[TestClass]
public class CountTreeNodeTests
{
    [TestMethod]
    public void Equals_BasedOnWrappedNode()
    {
        var wrapping1 = new CountTreeNode<Edge, Node, Builder>(
            new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } });
        var wrapping2 = new CountTreeNode<Edge, Node, Builder>(
            new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } });
        Assert.AreEqual(wrapping1, wrapping2);

        var wrapping3 = new CountTreeNode<Edge, Node, Builder>(
            new Node(13) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } });
        Assert.AreNotEqual(wrapping1, wrapping3);

        var wrapping4 = new CountTreeNode<Edge, Node, Builder>(
            new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(21) } });
        Assert.AreNotEqual(wrapping1, wrapping4);
    }

    [TestMethod]
    public void WrappedNode_IsPreserved()
    {
        var wrapped = new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } };
        var wrapping = new CountTreeNode<Edge, Node, Builder>(wrapped);
        Assert.AreEqual(wrapped, wrapping.WrappedNode);
    }

    [TestMethod]
    public void DescendantsCount_OfLeafIsZero()
    {
        var leafWrapped = new Node(12) { Children = new Dictionary<Edge, Node> { } };
        var leafWrapping = new CountTreeNode<Edge, Node, Builder>(leafWrapped);
        Assert.AreEqual(0, leafWrapping.DescendantsCount);
    }

    [TestMethod]
    public void DescendantsCount_OfSingletonIsOne()
    {
        var leafWrapped = new Node(12) { Children = new Dictionary<Edge, Node> { [new(2)] = new(20) } };
        var leafWrapping = new CountTreeNode<Edge, Node, Builder>(leafWrapped);
        Assert.AreEqual(1, leafWrapping.DescendantsCount);
    }

    [TestMethod]
    public void DescendantsCount_OfThreeWithNNodesIsN()
    {
        Node leafWrapped = BuildExampleNode();
        var leafWrapping = new CountTreeNode<Edge, Node, Builder>(leafWrapped);
        Assert.AreEqual(4, leafWrapping.DescendantsCount);
    }

    [TestMethod]
    public void DescendantsCount_GivesConsistentResults()
    {
        Node leafWrapped = BuildExampleNode();
        var leafWrapping = new CountTreeNode<Edge, Node, Builder>(leafWrapped);
        Assert.AreEqual(4, leafWrapping.DescendantsCount);
        Assert.AreEqual(4, leafWrapping.DescendantsCount);
        Assert.AreEqual(4, leafWrapping.DescendantsCount);
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

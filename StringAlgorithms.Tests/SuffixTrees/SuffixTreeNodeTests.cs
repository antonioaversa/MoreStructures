using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixTrees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.Tests.SuffixTrees;

[TestClass]
public class SuffixTreeNodeTests
{
    private record SuffixTreeNodeInvalidLeaf()
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { }, null);

    private record SuffixTreeNodeInvalidIntermediate()
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { [new(0, 1)] = new Leaf(0) }, 0);

    [TestMethod]
    public void Ctor_InvalidArguments()
    {
        Assert.ThrowsException<ArgumentException>(() => new SuffixTreeNode.Leaf(-1));
        Assert.ThrowsException<ArgumentException>(() => new SuffixTreeNodeInvalidLeaf());
        Assert.ThrowsException<ArgumentException>(() => new SuffixTreeNodeInvalidIntermediate());
    }

    [TestMethod]
    public void Indexer_RetrievesChild()
    {
        var root = BuildSuffixTreeExample();
        Assert.AreEqual(root.Children[new(0, 1)], root[new(0, 1)]);
        Assert.AreEqual(root.Children[new(3, 1)], root[new(3, 1)]);
    }

    [TestMethod]
    public void Children_Immutability_OnGet()
    {
        var root = BuildSuffixTreeExample();
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children.Clear());
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children[root.Children.First().Key] = new SuffixTreeNode.Leaf(0));
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children.Remove(root.Children.First().Key));
    }

    [TestMethod]
    public void Children_Immutability_FromCtorParam()
    {
        var rootChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode> 
        {
            [new(0, 2)] = new SuffixTreeNode.Leaf(0),
            [new(1, 1)] = new SuffixTreeNode.Leaf(1),
        };
        var root = new SuffixTreeNode.Intermediate(rootChildren);
        Assert.AreEqual(2, root.Children.Count);

        rootChildren.Add(new(0, 1), new SuffixTreeNode.Leaf(0));
        Assert.AreEqual(2, root.Children.Count);
    }

    /// <remarks>
    /// The example is built from the text <see cref="TestUtilities.ExampleText1"/>.
    /// </remarks>
    internal static SuffixTreeNode BuildSuffixTreeExample()
    {
        return new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(0, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(1, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(2, 2)] = new SuffixTreeNode.Leaf(0),
                    [new(3, 1)] = new SuffixTreeNode.Leaf(1),
                }),
                [new(3, 1)] = new SuffixTreeNode.Leaf(2),
            }),
            [new(3, 1)] = new SuffixTreeNode.Leaf(3),
        });
    }
}

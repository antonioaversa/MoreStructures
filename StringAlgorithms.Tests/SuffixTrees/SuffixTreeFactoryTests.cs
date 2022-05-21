using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees;
using StringAlgorithms.SuffixTrees;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.Tests.SuffixTrees;

[TestClass]
public class SuffixTreeFactoryTests
{
    [TestMethod]
    public void EmptyPath_IsCorrect()
    {
        Assert.IsFalse(new TreePath<SuffixTreeEdge, SuffixTreeNode, SuffixTreeBuilder>().PathNodes.Any());
    }

    [TestMethod]
    public void SingletonPath_IsCorrect()
    {
        var node = new SuffixTreeNode.Leaf(0);
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode, SuffixTreeBuilder>(new(0, 1), node);
        Assert.AreEqual(1, path.PathNodes.Count());
        Assert.AreEqual(new(0, 1), path.PathNodes.Single().Key);
        Assert.AreEqual(node, path.PathNodes.Single().Value);
    }

    [TestMethod]
    public void MultistepPath_IsCorrectWithParams()
    {
        var node2 = new SuffixTreeNode.Leaf(1);
        var node1 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(1, 1)] = node2
        });
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode, SuffixTreeBuilder>(
            (new(0, 1), node1), (new(1, 1), node2));
        AssertPath(node2, node1, path);
    }

    [TestMethod]
    public void MultistepPath_IsCorrectWithEnumerable()
    {
        var node2 = new SuffixTreeNode.Leaf(1);
        var node1 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(1, 1)] = node2
        });
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode, SuffixTreeBuilder>(
            new List<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> { new(new(0, 1), node1), new(new(1, 1), node2) });
        AssertPath(node2, node1, path);
    }

    private static void AssertPath(SuffixTreeNode.Leaf node2, SuffixTreeNode.Intermediate node1, 
        TreePath<SuffixTreeEdge, SuffixTreeNode, SuffixTreeBuilder> path)
    {
        Assert.AreEqual(2, path.PathNodes.Count());
        Assert.AreEqual(new(0, 1), path.PathNodes.ElementAt(0).Key);
        Assert.AreEqual(node1, path.PathNodes.ElementAt(0).Value);
        Assert.AreEqual(new(1, 1), path.PathNodes.ElementAt(1).Key);
        Assert.AreEqual(node2, path.PathNodes.ElementAt(1).Value);
    }
}

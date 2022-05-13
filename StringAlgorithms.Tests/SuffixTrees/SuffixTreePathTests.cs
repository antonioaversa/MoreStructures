using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.SuffixTrees.Tests;

[TestClass]
public class SuffixTreePathTests
{
    [TestMethod]
    public void Empty_Correctness()
    {
        Assert.IsFalse(SuffixTreePath.Empty().PathNodes.Any());
    }

    [TestMethod]
    public void Singleton_Correctness()
    {
        var node = new SuffixTreeNode.Leaf(0);
        var singletonPath = SuffixTreePath.Singleton(new(0, 1), node);
        Assert.AreEqual(1, singletonPath.PathNodes.Count());
        Assert.AreEqual(new(0, 1), singletonPath.PathNodes.Single().Key);
        Assert.AreEqual(node, singletonPath.PathNodes.Single().Value);
    }

    [TestMethod]
    public void TotalEdgesLength_EmptyPath()
    {
        Assert.AreEqual(0, new SuffixTreePath(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { }).TotalEdgesLength);
    }

    [TestMethod]
    public void TotalEdgesLength_SingletonPath()
    {
        Assert.AreEqual(3, SuffixTreePath.Singleton(new(0, 3), new SuffixTreeNode.Leaf(0)).TotalEdgesLength);
    }

    [TestMethod]
    public void TotalEdgesLength_MultipleNodesPath()
    {
        var root = SuffixTreeNodeTests.BuildSuffixTreeExample();
        var path = SuffixTreePath.From(
            (new SuffixTreeEdge(0, 1), root[new(0, 1)]),
            (new SuffixTreeEdge(1, 1), root[new(0, 1)][new(1, 1)]),
            (new SuffixTreeEdge(3, 1), root[new(0, 1)][new(1, 1)][new(3, 1)]));
        Assert.AreEqual(3, path.TotalEdgesLength);
    }

    [TestMethod]
    public void PathNodes_ImmutabilityOnGet()
    {
        var path = new SuffixTreePath(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { });
        if (path.PathNodes is IList<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> pathNodesAsList)
        {
            Assert.ThrowsException<NotSupportedException>(() => pathNodesAsList.Add(
                KeyValuePair.Create(new SuffixTreeEdge(0, 1), new SuffixTreeNode.Leaf(0) as SuffixTreeNode)));
        }
    }

    [TestMethod]
    public void PathNodes_ImmutabilityOnCtorParam()
    {
        var pathNodes = new Dictionary<SuffixTreeEdge, SuffixTreeNode> { };
        var path = new SuffixTreePath(pathNodes);
        Assert.AreEqual(0, path.PathNodes.Count());
        pathNodes[new(0, 1)] = new SuffixTreeNode.Leaf(0);
        Assert.AreEqual(0, path.PathNodes.Count());
    }
}

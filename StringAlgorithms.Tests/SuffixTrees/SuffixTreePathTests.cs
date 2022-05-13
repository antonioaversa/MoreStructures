using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixTrees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.Tests.SuffixTrees;

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
        var node = new SuffixTreeNode(0);
        var singletonPath = SuffixTreePath.Singleton(new(0, 1), node);
        Assert.AreEqual(1, singletonPath.PathNodes.Count());
        Assert.AreEqual(new(0, 1), singletonPath.PathNodes.Single().Key);
        Assert.AreEqual(node, singletonPath.PathNodes.Single().Value);
    }

    [TestMethod]
    public void PathNodes_ImmutabilityOnGet()
    {
        var path = new SuffixTreePath(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { });
        if (path.PathNodes is IList<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> pathNodesAsList)
        {
            Assert.ThrowsException<NotSupportedException>(() => 
                pathNodesAsList.Add(KeyValuePair.Create(new SuffixTreeEdge(0, 1), new SuffixTreeNode(0))));
        }
    }

    [TestMethod]
    public void PathNodes_ImmutabilityOnCtorParam()
    {
        var pathNodes = new Dictionary<SuffixTreeEdge, SuffixTreeNode> { };
        var path = new SuffixTreePath(pathNodes);
        Assert.AreEqual(0, path.PathNodes.Count());
        pathNodes[new(0, 1)] = new SuffixTreeNode(0);
        Assert.AreEqual(0, path.PathNodes.Count());
    }
}

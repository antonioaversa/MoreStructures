using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.SuffixTries.Tests;

[TestClass]
public class SuffixTriePathTests
{
    [TestMethod]
    public void Empty_Correctness()
    {
        Assert.IsFalse(SuffixTriePath.Empty().PathNodes.Any());
    }

    [TestMethod]
    public void Singleton_Correctness()
    {
        var node = new SuffixTrieNode.Leaf(0);
        var singletonPath = SuffixTriePath.Singleton(new(0), node);
        Assert.AreEqual(1, singletonPath.PathNodes.Count());
        Assert.AreEqual(new(0), singletonPath.PathNodes.Single().Key);
        Assert.AreEqual(node, singletonPath.PathNodes.Single().Value);
    }

    [TestMethod]
    public void PathNodes_ImmutabilityOnGet()
    {
        var path = new SuffixTriePath(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { });
        if (path.PathNodes is IList<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>> pathNodesAsList)
        {
            Assert.ThrowsException<NotSupportedException>(() => pathNodesAsList.Add(KeyValuePair.Create(
                new SuffixTrieEdge(0), new SuffixTrieNode.Leaf(0) as SuffixTrieNode)));
        }
    }

    [TestMethod]
    public void PathNodes_ImmutabilityOnCtorParam()
    {
        var pathNodes = new Dictionary<SuffixTrieEdge, SuffixTrieNode> { };
        var path = new SuffixTriePath(pathNodes);
        Assert.AreEqual(0, path.PathNodes.Count());
        pathNodes[new(0)] = new SuffixTrieNode.Leaf(0);
        Assert.AreEqual(0, path.PathNodes.Count());
    }
}

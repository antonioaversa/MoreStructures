using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixTries;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.Tests.SuffixTries;

[TestClass] 
public class SuffixTrieFactoryTests
{
    [TestMethod]
    public void EmptyPath_IsCorrect()
    {
        Assert.IsFalse(new SuffixTrieBuilder().EmptyPath().PathNodes.Any());
    }

    [TestMethod]
    public void SingletonPath_IsCorrect()
    {
        var node = new SuffixTrieNode.Leaf(0);
        var path = new SuffixTrieBuilder().SingletonPath(new(0), node);
        Assert.AreEqual(1, path.PathNodes.Count());
        Assert.AreEqual(new(0), path.PathNodes.Single().Key);
        Assert.AreEqual(node, path.PathNodes.Single().Value);
    }

    [TestMethod]
    public void MultistepPath_IsCorrectWithParams()
    {
        var node2 = new SuffixTrieNode.Leaf(1);
        var node1 = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(1)] = node2
        });
        var path = new SuffixTrieBuilder().MultistepsPath((new(0), node1), (new(1), node2));
        AssertPath(node2, node1, path);
    }

    [TestMethod]
    public void MultistepPath_IsCorrectWithEnumerable()
    {
        var node2 = new SuffixTrieNode.Leaf(1);
        var node1 = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(1)] = node2
        });
        var path = new SuffixTrieBuilder().MultistepsPath(
            new List<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>> { new(new(0), node1), new(new(1), node2) });
        AssertPath(node2, node1, path);
    }

    private static void AssertPath(SuffixTrieNode.Leaf node2, SuffixTrieNode.Intermediate node1, SuffixTriePath path)
    {
        Assert.AreEqual(2, path.PathNodes.Count());
        Assert.AreEqual(new(0), path.PathNodes.ElementAt(0).Key);
        Assert.AreEqual(node1, path.PathNodes.ElementAt(0).Value);
        Assert.AreEqual(new(1), path.PathNodes.ElementAt(1).Key);
        Assert.AreEqual(node2, path.PathNodes.ElementAt(1).Value);
    }
}

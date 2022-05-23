using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixTries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreStructures.Tests.SuffixTries;

[TestClass]
public class SuffixTrieNodeTests
{
    private record SuffixTrieNodeInvalidLeaf()
        : SuffixTrieNode(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { }, null);

    private record SuffixTrieNodeInvalidIntermediate()
        : SuffixTrieNode(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { [new(0)] = new Leaf(0) }, 0);

    [TestMethod]
    public void Ctor_InvalidArguments()
    {
        Assert.ThrowsException<ArgumentException>(() => new SuffixTrieNode.Leaf(-1));
        Assert.ThrowsException<ArgumentException>(() => new SuffixTrieNodeInvalidLeaf());
        Assert.ThrowsException<ArgumentException>(() => new SuffixTrieNodeInvalidIntermediate());
    }

    [TestMethod]
    public void Equality_IsAlwaysByValue()
    {
        var root1 = BuildSuffixTrieExample();
        var root2 = BuildSuffixTrieExample();
        Assert.AreEqual(root1, root2);
        Assert.IsTrue(root1 == root2);
        Assert.IsFalse(root1 != root2);
    }

    [TestMethod]
    public void Indexer_RetrievesChild()
    {
        var root = BuildSuffixTrieExample();
        Assert.AreEqual(root.Children[new(0)], root[new(0)]);
        Assert.AreEqual(root.Children[new(3)], root[new(3)]);
    }

    [TestMethod]
    public void Children_ImmutabilityOnGet()
    {
        var root = BuildSuffixTrieExample();
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children.Clear());
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children[root.Children.First().Key] = new SuffixTrieNode.Leaf(0));
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children.Remove(root.Children.First().Key));
    }

    [TestMethod]
    public void Children_Immutability_FromCtorParam()
    {
        var rootChildren = new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(1)] = new SuffixTrieNode.Leaf(1),
            }),
        };
        var root = new SuffixTrieNode.Intermediate(rootChildren);
        Assert.AreEqual(1, root.Children.Count);

        rootChildren.Add(new(1), new SuffixTrieNode.Leaf(1));
        Assert.AreEqual(1, root.Children.Count);
    }

    [TestMethod]
    public void ToString_IsTheSameOnEquivalentTrees()
    {
        var root1Str = BuildSuffixTrieExample().ToString();
        var root2Str = BuildSuffixTrieExample().ToString();
        Assert.AreEqual(root1Str, root2Str);
    }

    [TestMethod]
    public void ToString_OnLeafIncludesStart()
    {
        var root1Str = new SuffixTrieNode.Leaf(123).ToString();
        Assert.IsTrue(root1Str.Contains(123.ToString()));
    }

    /// <remarks>
    /// The example is built from the text <see cref="TestUtilities.ExampleText1"/>.
    /// </remarks>
    internal static SuffixTrieNode BuildSuffixTrieExample()
    {
        return new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(3)] = new SuffixTrieNode.Leaf(0)
                    }),
                    [new(3)] = new SuffixTrieNode.Leaf(1),
                }),
                [new(3)] = new SuffixTrieNode.Leaf(2),
            }),
            [new(3)] = new SuffixTrieNode.Leaf(3),
        });
    }
}

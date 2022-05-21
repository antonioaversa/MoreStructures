using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees;
using StringAlgorithms.SuffixTries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.Tests.SuffixTries;

[TestClass]
public class SuffixTrieTreePathTests
{
    [TestMethod]
    public void PathNodes_ImmutabilityOnGet()
    {
        var path = new TreePath<SuffixTrieEdge, SuffixTrieNode, SuffixTrieBuilder>(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { });
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
        var path = new TreePath<SuffixTrieEdge, SuffixTrieNode, SuffixTrieBuilder>(pathNodes);
        Assert.AreEqual(0, path.PathNodes.Count());
        pathNodes[new(0)] = new SuffixTrieNode.Leaf(0);
        Assert.AreEqual(0, path.PathNodes.Count());
    }

    /// <remarks>
    /// The example is the second "root to leaf"" path of the tree built from <see cref="ExampleText"/>: 
    /// a -> b -> a -> a -> $.
    /// </remarks>
    internal static TreePath<SuffixTrieEdge, SuffixTrieNode, SuffixTrieBuilder> BuildSuffixTriePathExample()
    {
        var leaf = new SuffixTrieNode.Leaf(2);
        var leafEdge = new SuffixTrieEdge(6);

        var intermediate1 = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [leafEdge] = leaf,
        });
        var intermediate1Edge = new SuffixTrieEdge(5);

        var intermediate2 = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            // We ignore the child at new(3) and its sub-tree, as it's not relevant for the path
            [intermediate1Edge] = intermediate1,
        });
        var intermediate2Edge = new SuffixTrieEdge(2);

        var intermediate3 = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [intermediate2Edge] = intermediate2,
        });
        var intermediate3Edge = new SuffixTrieEdge(1);
        
        var intermediate4 = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [intermediate3Edge] = intermediate3,
            // We ignore the child at new(5) and its sub-tree, as it's not relevant for the path
        });
        var intermediate4Edge = new SuffixTrieEdge(0);

        return new TreePath<SuffixTrieEdge, SuffixTrieNode, SuffixTrieBuilder>(new List<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>>
        {            
            new(intermediate4Edge, intermediate4),
            new(intermediate3Edge, intermediate3),
            new(intermediate2Edge, intermediate2),
            new(intermediate1Edge, intermediate1),
            new(leafEdge, leaf),
        });
    }
}

﻿using MoreStructures.RecImmTrees;
using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;
using static MoreStructures.Tests.CountTrees.TreeMock;

namespace MoreStructures.Tests.RecImmTrees;

[TestClass]
public class TreePathTests
{
    [TestMethod]
    public void Ctor_OfSuffixTree_EmptyPath_IsCorrect()
    {
        Assert.IsFalse(new TreePath<SuffixTreeEdge, SuffixTreeNode>().PathNodes.Any());
    }

    [TestMethod]
    public void Ctor_OfSuffixTree_SingletonPath_IsCorrect()
    {
        var node = new SuffixTreeNode.Leaf(0);
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>(new(0, 1), node);
        Assert.AreEqual(1, path.PathNodes.Count());
        Assert.AreEqual(new(0, 1), path.PathNodes.Single().Key);
        Assert.AreEqual(node, path.PathNodes.Single().Value);
    }

    [TestMethod]
    public void Ctor_OfSuffixTree_MultistepPath_IsCorrectWithParams()
    {
        var node2 = new SuffixTreeNode.Leaf(1);
        var node1 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(1, 1)] = node2
        });
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>(
            (new(0, 1), node1), (new(1, 1), node2));
        AssertPath_OfSuffixTree(node2, node1, path);
    }

    [TestMethod]
    public void Ctor_OfSuffixTree_MultistepPath_IsCorrectWithEnumerable()
    {
        var node2 = new SuffixTreeNode.Leaf(1);
        var node1 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(1, 1)] = node2
        });
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>(
            new List<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> { new(new(0, 1), node1), new(new(1, 1), node2) });
        AssertPath_OfSuffixTree(node2, node1, path);
    }

    [TestMethod]
    public void Ctor_OfSuffixTrie_EmptyPath_IsCorrect()
    {
        Assert.IsFalse(new TreePath<SuffixTrieEdge, SuffixTrieNode>().PathNodes.Any());
    }

    [TestMethod]
    public void Ctor_OfSuffixTrie_SingletonPath_IsCorrect()
    {
        var node = new SuffixTrieNode.Leaf(0);
        var path = new TreePath<SuffixTrieEdge, SuffixTrieNode>(new(0), node);
        Assert.AreEqual(1, path.PathNodes.Count());
        Assert.AreEqual(new(0), path.PathNodes.Single().Key);
        Assert.AreEqual(node, path.PathNodes.Single().Value);
    }

    [TestMethod]
    public void Ctor_OfSuffixTrie_MultistepPath_IsCorrectWithParams()
    {
        var node2 = new SuffixTrieNode.Leaf(1);
        var node1 = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(1)] = node2
        });
        var path = new TreePath<SuffixTrieEdge, SuffixTrieNode>((new(0), node1), (new(1), node2));
        AssertPath_OfSuffixTrie(node2, node1, path);
    }

    [TestMethod]
    public void Ctor_OfSuffixTrie_MultistepPath_IsCorrectWithEnumerable()
    {
        var node2 = new SuffixTrieNode.Leaf(1);
        var node1 = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(1)] = node2
        });
        var path = new TreePath<SuffixTrieEdge, SuffixTrieNode>(
            new List<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>> { new(new(0), node1), new(new(1), node2) });
        AssertPath_OfSuffixTrie(node2, node1, path);
    }

    [TestMethod]
    public void Equals_ByValue()
    {
        var path1 = new TreePath<Edge, Node>(
            new List<KeyValuePair<Edge, Node>> { new(new(6), new(7)), new(new(7), new(8)), new(new(8), new(9)) });

        var path2 = new TreePath<Edge, Node>(
            new List<KeyValuePair<Edge, Node>> { new(new(6), new(7)), new(new(7), new(8)), new(new(8), new(9)) });

        Assert.AreEqual(path1, path2);

        var path3 = new TreePath<Edge, Node>(
            new List<KeyValuePair<Edge, Node>> { new(new(6), new(7)), new(new(7), new(8)), new(new(8), new(90)) });

        Assert.AreNotEqual(path1, path3);

        var path4 = new TreePath<Edge, Node>(
            new List<KeyValuePair<Edge, Node>> { new(new(6), new(7)), new(new(7), new(8)), new(new(80), new(9)) });

        Assert.AreNotEqual(path1, path4);
    }

    [TestMethod]
    public void Equals_TakesIntoAccountPathNodesOrder()
    {
        var path1 = new TreePath<Edge, Node>(
            new List<KeyValuePair<Edge, Node>> { new(new(0), new(1)), new(new(4), new(5)) });

        var path2 = new TreePath<Edge, Node>(
            new List<KeyValuePair<Edge, Node>> { new(new(4), new(5)), new(new(0), new(1)) });

        Assert.AreNotEqual(path1, path2);
    }

    [TestMethod]
    public void GetHashCode_ByValue()
    {
        var path1 = new TreePath<Edge, Node>(
            new List<KeyValuePair<Edge, Node>> { new(new(6), new(7)), new(new(7), new(8)), new(new(8), new(9)) });

        var path2 = new TreePath<Edge, Node>(
            new List<KeyValuePair<Edge, Node>> { new(new(6), new(7)), new(new(7), new(8)), new(new(8), new(9)) });

        Assert.AreEqual(path1.GetHashCode(), path2.GetHashCode());
    }

    private static void AssertPath_OfSuffixTrie(SuffixTrieNode.Leaf node2, SuffixTrieNode.Intermediate node1,
        TreePath<SuffixTrieEdge, SuffixTrieNode> path)
    {
        Assert.AreEqual(2, path.PathNodes.Count());
        Assert.AreEqual(new(0), path.PathNodes.ElementAt(0).Key);
        Assert.AreEqual(node1, path.PathNodes.ElementAt(0).Value);
        Assert.AreEqual(new(1), path.PathNodes.ElementAt(1).Key);
        Assert.AreEqual(node2, path.PathNodes.ElementAt(1).Value);
    }

    private static void AssertPath_OfSuffixTree(SuffixTreeNode.Leaf node2, SuffixTreeNode.Intermediate node1,
        TreePath<SuffixTreeEdge, SuffixTreeNode> path)
    {
        Assert.AreEqual(2, path.PathNodes.Count());
        Assert.AreEqual(new(0, 1), path.PathNodes.ElementAt(0).Key);
        Assert.AreEqual(node1, path.PathNodes.ElementAt(0).Value);
        Assert.AreEqual(new(1, 1), path.PathNodes.ElementAt(1).Key);
        Assert.AreEqual(node2, path.PathNodes.ElementAt(1).Value);
    }

    [TestMethod]
    public void PathNodes_OfSuffixTree_ImmutabilityOnGet()
    {
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
        if (path.PathNodes is IList<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> pathNodesAsList)
        {
            Assert.ThrowsException<NotSupportedException>(() => pathNodesAsList.Add(
                KeyValuePair.Create(new SuffixTreeEdge(0, 1), new SuffixTreeNode.Leaf(0) as SuffixTreeNode)));
        }
    }

    [TestMethod]
    public void PathNodes_OfSuffixTree_ImmutabilityOnCtorParam()
    {
        var pathNodes = new Dictionary<SuffixTreeEdge, SuffixTreeNode> { };
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>(pathNodes);
        Assert.AreEqual(0, path.PathNodes.Count());
        pathNodes[new(0, 1)] = new SuffixTreeNode.Leaf(0);
        Assert.AreEqual(0, path.PathNodes.Count());
    }

    /// <remarks>
    /// The example is the second "root to leaf"" path of the tree built from <see cref="ExampleText"/>: 
    /// a -> ba -> a$.
    /// </remarks>
    internal static TreePath<SuffixTreeEdge, SuffixTreeNode> BuildSuffixTreePathExample()
    {
        var leaf = new SuffixTreeNode.Leaf(2);
        var leafEdge = new SuffixTreeEdge(5, 2);

        var intermediate1 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(3, 4)] = new SuffixTreeNode.Leaf(0),
            [leafEdge] = leaf,
        });
        var intermediate1Edge = new SuffixTreeEdge(1, 2);
        
        var intermediate2 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [intermediate1Edge] = intermediate1,
        });
        var intermediate2Edge = new SuffixTreeEdge(0, 1);

        return new(new List<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>>
        {
            new(intermediate2Edge, intermediate2),
            new(intermediate1Edge, intermediate1),
            new(leafEdge, leaf),            
        });
    }

    [TestMethod]
    public void PathNodes_OfSuffixTrie_ImmutabilityOnGet()
    {
        var path = new TreePath<SuffixTrieEdge, SuffixTrieNode>(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { });
        if (path.PathNodes is IList<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>> pathNodesAsList)
        {
            Assert.ThrowsException<NotSupportedException>(() => pathNodesAsList.Add(KeyValuePair.Create(
                new SuffixTrieEdge(0), new SuffixTrieNode.Leaf(0) as SuffixTrieNode)));
        }
    }

    [TestMethod]
    public void PathNodes_OfSuffixTrie_ImmutabilityOnCtorParam()
    {
        var pathNodes = new Dictionary<SuffixTrieEdge, SuffixTrieNode> { };
        var path = new TreePath<SuffixTrieEdge, SuffixTrieNode>(pathNodes);
        Assert.AreEqual(0, path.PathNodes.Count());
        pathNodes[new(0)] = new SuffixTrieNode.Leaf(0);
        Assert.AreEqual(0, path.PathNodes.Count());
    }

    /// <remarks>
    /// The example is the second "root to leaf"" path of the tree built from <see cref="ExampleText"/>: 
    /// a -> b -> a -> a -> $.
    /// </remarks>
    internal static TreePath<SuffixTrieEdge, SuffixTrieNode> BuildSuffixTriePathExample()
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

        return new TreePath<SuffixTrieEdge, SuffixTrieNode>(new List<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>>
        {
            new(intermediate4Edge, intermediate4),
            new(intermediate3Edge, intermediate3),
            new(intermediate2Edge, intermediate2),
            new(intermediate1Edge, intermediate1),
            new(leafEdge, leaf),
        });
    }

    [TestMethod]
    public void ToString_IsTheSameOnEquivalentPaths()
    {
        var path1Str = BuildSuffixTriePathExample().ToString();
        var path2Str = BuildSuffixTriePathExample().ToString();
        Assert.AreEqual(path1Str, path2Str);
    }

    [TestMethod]
    public void ToString_OfPathContainsToStringOfSubPath()
    {
        var path1 = BuildSuffixTriePathExample();
        var path2 = BuildSuffixTriePathExample().Concat(BuildSuffixTriePathExample());
        var path1Str = path1.ToString();
        var path2Str = path2.ToString();
        Assert.IsTrue(path2Str.Contains(path1Str));
        Assert.IsTrue(path2Str.Length > path1Str.Length);
    }

    [TestMethod]
    public void ToString_OfEmptyPathIsEmptyString()
    {
        var emptyPathStr = new TreePath<SuffixTreeEdge, SuffixTreeNode>().ToString();
        Assert.AreEqual(string.Empty, emptyPathStr);
    }

    [TestMethod]
    public void ToString_OfNonEmptyPathIsNonEmptyString()
    {
        var nonEmptyPathStr = BuildSuffixTriePathExample().ToString();
        Assert.IsFalse(string.IsNullOrEmpty(nonEmptyPathStr));
    }
}

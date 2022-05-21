using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees;
using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries;
using StringAlgorithms.Tests.SuffixTrees;
using StringAlgorithms.Tests.SuffixTries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.Tests.RecImmTrees;

[TestClass]
public class RecImmDictIndexedTreeNodeExtensionsTests
{
    [TestMethod]
    public void IsLeaf_IsCorrectForSuffixTree()
    {
        var root = SuffixTreeNodeTests.BuildSuffixTreeExample();
        Assert.IsFalse(root.IsLeaf());
        Assert.IsFalse(root[new(0, 1)][new(1, 1)].IsLeaf());
        Assert.IsTrue(root[new(0, 1)][new(1, 1)][new(3, 1)].IsLeaf());
    }

    [TestMethod]
    public void IsLeaf_IsCorrectForSuffixTrie()
    {
        var root = SuffixTrieNodeTests.BuildSuffixTrieExample();
        Assert.IsFalse(root.IsLeaf());
        Assert.IsFalse(root[new(0)][new(1)].IsLeaf());
        Assert.IsTrue(root[new(0)][new(1)][new(2)][new(3)].IsLeaf());
    }

    [TestMethod]
    public void GetAllNodeToLeafPaths_IsCorrect()
    {
        int CountOccurrencesByEdges(
            IEnumerable<TreePath<SuffixTrieEdge, SuffixTrieNode>> paths,
            params SuffixTrieEdge[] pathToFind) => (
                from path in paths
                let pathKeys = path.PathNodes.Select(kvp => kvp.Key)
                where pathKeys.SequenceEqual(pathToFind)
                select path)
                .Count();

        var root = SuffixTrieNodeTests.BuildSuffixTrieExample();
        var rootToLeafPaths = root.GetAllNodeToLeafPaths().ToList();

        Assert.AreEqual(4, rootToLeafPaths.Count);

        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0), new(1), new(2), new(3)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0), new(1), new(3)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0), new(3)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new SuffixTrieEdge(3)));
    }

    [TestMethod]
    public void GetAllNodeToLeafPaths_IsCorrect2()
    {
        int CountOccurrencesByEdges(
            IEnumerable<TreePath<SuffixTreeEdge, SuffixTreeNode>> paths,
            params SuffixTreeEdge[] pathToFind) => (
                from path in paths
                let pathKeys = path.PathNodes.Select(kvp => kvp.Key)
                where pathKeys.SequenceEqual(pathToFind)
                select path)
                .Count();

        var root = SuffixTreeNodeTests.BuildSuffixTreeExample();
        var rootToLeafPaths = root.GetAllNodeToLeafPaths().ToList();

        Assert.AreEqual(4, rootToLeafPaths.Count);

        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(1, 1), new(2, 2)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(1, 1), new(3, 1)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(3, 1)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new SuffixTreeEdge(3, 1)));
    }
}

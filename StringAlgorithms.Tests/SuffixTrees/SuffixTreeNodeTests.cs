﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.SuffixTrees.Tests;

[TestClass]
public class SuffixTreeNodeTests
{
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
            () => root.Children[root.Children.First().Key] = new SuffixTreeNode(0));
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children.Remove(root.Children.First().Key));
    }

    [TestMethod]
    public void Children_Immutability_FromCtorParam()
    {
        var rootChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode> { };
        var root = new SuffixTreeNode(rootChildren);
        Assert.IsTrue(root.IsLeaf);

        rootChildren.Add(new(0, 1), new SuffixTreeNode(0));
        Assert.IsTrue(root.IsLeaf);
    }

    [TestMethod]
    public void GetAllNodeToLeafPaths_Correctness()
    {
        var root = BuildSuffixTreeExample();
        var rootToLeafPaths = root.GetAllNodeToLeafPaths().ToList();

        Assert.AreEqual(4, rootToLeafPaths.Count);

        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(1, 1), new(2, 2)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(1, 1), new(3, 1)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(3, 1)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new SuffixTreeEdge(3, 1)));
    }

    [TestMethod]
    public void GetAllSuffixesFor_Correctness()
    {
        var text = new TextWithTerminator("abc");
        var root = BuildSuffixTreeExample();
        var suffixes = root.GetAllSuffixesFor(text);

        var t = text.Terminator;
        Assert.IsTrue(suffixes.OrderBy(s => s).SequenceEqual(
            new List<string> { $"abc{t}", $"ab{t}", $"a{t}", $"{t}" }.OrderBy(s => s)));
    }

    /// <remarks>
    /// Built from "aaa".
    /// </remarks>
    private static SuffixTreeNode BuildSuffixTreeExample()
    {
        return new SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(0, 1)] = new SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(1, 1)] = new SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(2, 2)] = new SuffixTreeNode(0),
                    [new(3, 1)] = new SuffixTreeNode(1),
                }),
                [new(3, 1)] = new SuffixTreeNode(2),
            }),
            [new(3, 1)] = new SuffixTreeNode(3),
        });
    }

    private static int CountOccurrencesByEdges(
        IEnumerable<SuffixTreePath> paths,
        params SuffixTreeEdge[] pathToFind) => (
            from path in paths
            let pathKeys = path.PathNodes.Select(kvp => kvp.Key)
            where pathKeys.SequenceEqual(pathToFind)
            select path)
            .Count();
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.Tests;

[TestClass]
public class SuffixTreeNodeTests
{
    [TestMethod]
    public void Children_Immutability_OnGet()
    {
        var root = BuildSuffixTreeExample();
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children.Clear());
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children[root.Children.First().Key] = new SuffixTreeNode());
        Assert.ThrowsException<NotSupportedException>(
            () => root.Children.Remove(root.Children.First().Key));
    }

    [TestMethod]
    public void Children_Immutability_FromCtorParam()
    {
        var rootChildren = new Dictionary<PrefixPath, SuffixTreeNode> { };
        var root = new SuffixTreeNode(rootChildren);
        Assert.IsTrue(root.IsLeaf);

        rootChildren.Add(new(0, 1), new SuffixTreeNode());
        Assert.IsTrue(root.IsLeaf);
    }

    [TestMethod]
    public void GetAllNodeToLeafPaths_Correctness()
    {
        var root = BuildSuffixTreeExample();
        var rootToLeafPaths = root.GetAllNodeToLeafPaths().ToList();

        Assert.AreEqual(4, rootToLeafPaths.Count);

        Assert.AreEqual(1, CountOccurrencesByPrefixPaths(rootToLeafPaths, new(0, 1), new(1, 1), new(2, 2)));
        Assert.AreEqual(1, CountOccurrencesByPrefixPaths(rootToLeafPaths, new(0, 1), new(1, 1), new(3, 1)));
        Assert.AreEqual(1, CountOccurrencesByPrefixPaths(rootToLeafPaths, new(0, 1), new(3, 1)));
        Assert.AreEqual(1, CountOccurrencesByPrefixPaths(rootToLeafPaths, new PrefixPath(3, 1)));
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

    private static SuffixTreeNode BuildSuffixTreeExample()
    {
        return new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode>
        {
            [new(0, 1)] = new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode>
            {
                [new(1, 1)] = new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode>
                {
                    [new(2, 2)] = new SuffixTreeNode(),
                    [new(3, 1)] = new SuffixTreeNode(),
                }),
                [new(3, 1)] = new SuffixTreeNode(),
            }),
            [new(3, 1)] = new SuffixTreeNode(),
        });
    }

    private static int CountOccurrencesByPrefixPaths(
        IEnumerable<SuffixTreePath> paths,
        params PrefixPath[] pathToFind) => (
            from path in paths
            let pathKeys = path.PathNodes.Select(kvp => kvp.Key)
            where pathKeys.SequenceEqual(pathToFind)
            select path)
            .Count();
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Paths;
using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;
using MoreStructures.Tests.SuffixTrees;
using MoreStructures.Tests.SuffixTries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreStructures.Tests.RecImmTrees.Paths;

using static TreeMock;

public abstract class NodeToLeafPathsBuilderTests
{
    protected INodeToLeafPathsBuilder Builder { get; }

    protected NodeToLeafPathsBuilderTests(INodeToLeafPathsBuilder builder)
    {
        Builder = builder;
    }

    [TestMethod]
    public void GetAllNodeToLeafPaths__DocExample()
    {
        Node[] nodes = new Node[11];
        var root = nodes[0] = new Node(0, new Dictionary<Edge, Node>
        {
            [new(0)] = nodes[1] = new Node(1, new Dictionary<Edge, Node>
            {
                [new(1)] = nodes[2] = new Node(2),
                [new(2)] = nodes[3] = new Node(3, new Dictionary<Edge, Node>
                {
                    [new(3)] = nodes[4] = new Node(4),
                }),
                [new(4)] = nodes[5] = new Node(5),
            }),
            [new(5)] = nodes[6] = new Node(6),
            [new(6)] = nodes[7] = new Node(7, new Dictionary<Edge, Node>
            {
                [new(7)] = nodes[8] = new Node(8, new Dictionary<Edge, Node>
                {
                    [new(8)] = nodes[9] = new Node(9),
                    [new(9)] = nodes[10] = new Node(10),
                }),
            }),
        });

        var rootToLeafPaths = Builder.GetAllNodeToLeafPaths<Edge, Node>(root).ToHashSet();
        var expectedRootToLeafPaths = new HashSet<TreePath<Edge, Node>>
        {
            new((new(0), nodes[1]), (new(1), nodes[2])),
            new((new(0), nodes[1]), (new(2), nodes[3]), new(new(3), nodes[4])),
            new((new(0), nodes[1]), (new(4), nodes[5])),
            new((new(5), nodes[6])),
            new((new(6), nodes[7]), (new(7), nodes[8]), (new(8), nodes[9])),
            new((new(6), nodes[7]), (new(7), nodes[8]), (new(9), nodes[10])),
        };
        Assert.IsTrue(rootToLeafPaths.SetEquals(expectedRootToLeafPaths));
    }

    [TestMethod]
    public void GetAllNodeToLeafPaths_IsCorrectWithTries()
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
        var rootToLeafPaths = Builder.GetAllNodeToLeafPaths<SuffixTrieEdge, SuffixTrieNode>(root).ToList();

        Assert.AreEqual(4, rootToLeafPaths.Count);

        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0), new(1), new(2), new(3)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0), new(1), new(3)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0), new(3)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new SuffixTrieEdge(3)));
    }

    [TestMethod]
    public void GetAllNodeToLeafPaths_IsCorrectWithTrees()
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
        var rootToLeafPaths = Builder.GetAllNodeToLeafPaths<SuffixTreeEdge, SuffixTreeNode>(root).ToList();

        Assert.AreEqual(4, rootToLeafPaths.Count);

        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(1, 1), new(2, 2)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(1, 1), new(3, 1)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new(0, 1), new(3, 1)));
        Assert.AreEqual(1, CountOccurrencesByEdges(rootToLeafPaths, new SuffixTreeEdge(3, 1)));
    }
}

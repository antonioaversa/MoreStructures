using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures.Conversions;
using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries;
using StringAlgorithms.Tests.SuffixTrees;
using StringAlgorithms.Tests.SuffixTries;
using System;
using System.Collections.Generic;

namespace StringAlgorithms.Tests.SuffixStructures.Conversions;

public abstract class ConverterTests
{
    protected IConverter Converter { get; init; }

    public ConverterTests(IConverter suffixStructuresConverter)
    {
        Converter = suffixStructuresConverter;
    }

    [TestMethod]
    public void TrieToTree_BuildsCorrectTree()
    {
        var trieRoot = SuffixTrieNodeTests.BuildSuffixTrieExample();
        var treeRoot = Converter.TrieToTree(trieRoot);
        Assert.AreEqual(SuffixTreeNodeTests.BuildSuffixTreeExample(), treeRoot);
    }

    private record SingletonSuffixTrieNode(SuffixTrieEdge SingleEdge, SuffixTrieNode SingleNode) : SuffixTrieNode(
        new Dictionary<SuffixTrieEdge, SuffixTrieNode> { [SingleEdge] = SingleNode }, null);
    
    [TestMethod]
    public void TrieToTree_OnlySupportsLeafAndIntermediateNodes_AsArgument()
    {
        var trieRoot = new SingletonSuffixTrieNode(new(0), new SuffixTrieNode.Leaf(7));
        Assert.ThrowsException<NotSupportedException>(() => Converter.TrieToTree(trieRoot));
    }

    [TestMethod]
    public void TrieToTree_OnlySupportsLeafAndIntermediateNodes_DeepInTheStructure()
    {
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>()
            {
                [new(3)] = new SingletonSuffixTrieNode(new(0), new SuffixTrieNode.Leaf(7)),
                [new(4)] = new SuffixTrieNode.Leaf(8),
            }),
        });
        Assert.ThrowsException<NotSupportedException>(() => Converter.TrieToTree(trieRoot));
    }

    [TestMethod]
    public void TrieToTree_BuildsLeafTreeFromLeafTrie()
    {
        var trieRoot = new SuffixTrieNode.Leaf(2);
        var treeRoot = Converter.TrieToTree(trieRoot);
        Assert.IsTrue(treeRoot is SuffixTreeNode.Leaf treeLeaf && treeLeaf.LeafStart == trieRoot.LeafStart);
    }

    [TestMethod]
    public void TrieToTree_Builds1LevelIntermediateFrom2LevelsCoalesceableIntermediate()
    {
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(4)] = new SuffixTrieNode.Leaf(7),
            }),
        });
        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(3, 2)] = new SuffixTreeNode.Leaf(7),
        });
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }

    [TestMethod]
    public void TrieToTree_Builds1LevelIntermediateFrom3LevelsCoalesceableIntermediate()
    {
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(4)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(5)] = new SuffixTrieNode.Leaf(7),
                }),
            }),
        });
        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(3, 3)] = new SuffixTreeNode.Leaf(7),
        });
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }
    
    [TestMethod]
    public void TrieToTree_RecurseOnChildren()
    {
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(4)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(5)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(6)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(7)] = new SuffixTrieNode.Leaf(7),
                        }),
                    }),
                }),
                [new(5)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(6)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(7)] = new SuffixTrieNode.Leaf(8),
                    }),
                }),
            }),
        });
        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(3, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(4, 4)] = new SuffixTreeNode.Leaf(7),
                [new(5, 3)] = new SuffixTreeNode.Leaf(8),
            }),
        });
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }
    
    [TestMethod]
    public void TrieToTree_Builds1LevelIntermediateFrom2LevelsNonCoalesceableIntermediate()
    {
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(4)] = new SuffixTrieNode.Leaf(7),
            }),
            [new(4)] = new SuffixTrieNode.Leaf(8),
        });
        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(3, 2)] = new SuffixTreeNode.Leaf(7),
            [new(4, 1)] = new SuffixTreeNode.Leaf(8),
        });
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }
    
    [TestMethod]
    public void TrieToTree_ReproducesALeafAsIs()
    {
        var trieRoot = new SuffixTrieNode.Leaf(2);
        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Leaf(2);
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }

    [TestMethod]
    public void TrieToTree_GoesDownOneLevel_EndingOnLeaf()
    { 
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(3)] = new SuffixTrieNode.Leaf(1),
        });

        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(3, 1)] = new SuffixTreeNode.Leaf(1),
        });
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }

    [TestMethod]
    public void TrieToTree_GoesDownOneLevel_EndingOnIntermediateWithMultipleChildren()
    {
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>()
            {
                [new(3)] = new SuffixTrieNode.Leaf(7),
                [new(4)] = new SuffixTrieNode.Leaf(8),
            }),
        });
        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(2, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>()
            {
                [new(3, 1)] = new SuffixTreeNode.Leaf(7),
                [new(4, 1)] = new SuffixTreeNode.Leaf(8),
            }),
        });
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }
    
    [TestMethod]
    public void TrieToTree_GoesDownTwoLevels_EndingOnLeaf()
    {
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(4)] = new SuffixTrieNode.Leaf(7),
            }),
        });
        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(3, 2)] = new SuffixTreeNode.Leaf(7),
        });
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }
    
    [TestMethod]
    public void TrieToTree_GoesDownTwoLevels_EndingOnIntermediateWithMultipleChildren()
    {
        var trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
        {
            [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(4)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(5)] = new SuffixTrieNode.Leaf(7),
                    [new(6)] = new SuffixTrieNode.Leaf(8),
                }),
            }),
        });
        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(3, 2)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(5, 1)] = new SuffixTreeNode.Leaf(7),
                [new(6, 1)] = new SuffixTreeNode.Leaf(8),
            }),
        });
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }

    [TestMethod]
    public void TrieToTree_GoesDownNLevels_EndingOnLeaf()
    {
        var trieRoot = new SuffixTrieNode.Leaf(7) as SuffixTrieNode;
        var depth = 10;
        for (var i = depth - 1; i >= 0; i--)
        {
            trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(i)] = trieRoot,
            });
        }
        var treeRoot = Converter.TrieToTree(trieRoot);
        var expectedTreeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(0, depth)] = new SuffixTreeNode.Leaf(7),
        });
        Assert.AreEqual(expectedTreeRoot, treeRoot);
    }
}

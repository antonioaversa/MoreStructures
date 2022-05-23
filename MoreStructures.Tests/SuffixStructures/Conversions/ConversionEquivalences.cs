using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;
using MoreStructures.Tests.SuffixTrees;
using MoreStructures.Tests.SuffixTries;
using System.Collections.Generic;

namespace MoreStructures.Tests.SuffixStructures.Conversions;

using static ConversionEquivalences.EquivalenceId;

public static class ConversionEquivalences
{
    public enum EquivalenceId
    {
        Leaf, 
        Example,
        TwoLevelsTreeSingleChainTwoLeafsToThreeLevelsTrie,
        TwoLevelsTreeSingleChainSingleLeafToThreeLevelsTrie, 
        TwoLevelsTreeWithSiblingToThreeLevelsTrie, 
        TwoLevelsTreeToFourLevelsTrie,
        TwoLevelsTreeToTwoLevelsTrie,
        ThreeLevelsTreeToFourLevelsTrie,
        ThreeLevelsTreeToSixLevelsTrie,
        OneLevelTreeToMostUnbalancedTenLevelsTrie,
        OneLevelTreeToMostUnbalancedAHundredLevelsTrie
    }

    public static readonly Dictionary<
        EquivalenceId, (SuffixTreeNode treeNode, SuffixTrieNode trieNode)> Equivalences = 
        new()
        {
            [Leaf] = (
                new SuffixTreeNode.Leaf(2),
                new SuffixTrieNode.Leaf(2)),

            [Example] = (
                SuffixTreeNodeTests.BuildSuffixTreeExample(),
                SuffixTrieNodeTests.BuildSuffixTrieExample()),
            
            [TwoLevelsTreeSingleChainSingleLeafToThreeLevelsTrie] = (
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(3, 2)] = new SuffixTreeNode.Leaf(7),
                }),
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(4)] = new SuffixTrieNode.Leaf(7),
                    }),
                })),

            [TwoLevelsTreeSingleChainTwoLeafsToThreeLevelsTrie] = (
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(2, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>()
                    {
                        [new(3, 1)] = new SuffixTreeNode.Leaf(7),
                        [new(4, 1)] = new SuffixTreeNode.Leaf(8),
                    }),
                }),
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>()
                    {
                        [new(3)] = new SuffixTrieNode.Leaf(7),
                        [new(4)] = new SuffixTrieNode.Leaf(8),
                    }),
                })),

            [TwoLevelsTreeWithSiblingToThreeLevelsTrie] = (
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(3, 2)] = new SuffixTreeNode.Leaf(7),
                    [new(4, 1)] = new SuffixTreeNode.Leaf(8),
                }), 
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(4)] = new SuffixTrieNode.Leaf(7),
                    }),
                    [new(4)] = new SuffixTrieNode.Leaf(8),
                })),

            [TwoLevelsTreeToFourLevelsTrie] = (
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(3, 3)] = new SuffixTreeNode.Leaf(7),
                }), 
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(4)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(5)] = new SuffixTrieNode.Leaf(7),
                        }),
                    }),
                })),

            [TwoLevelsTreeToTwoLevelsTrie] = (
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(3, 1)] = new SuffixTreeNode.Leaf(1),
                }),
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(3)] = new SuffixTrieNode.Leaf(1),
                })),

            [ThreeLevelsTreeToFourLevelsTrie] = (
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(3, 2)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(5, 1)] = new SuffixTreeNode.Leaf(7),
                        [new(6, 1)] = new SuffixTreeNode.Leaf(8),
                    }),
                }),
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(4)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(5)] = new SuffixTrieNode.Leaf(7),
                            [new(6)] = new SuffixTrieNode.Leaf(8),
                        }),
                    }),
                })),

            [ThreeLevelsTreeToSixLevelsTrie] = (
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(3, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(4, 4)] = new SuffixTreeNode.Leaf(7),
                        [new(5, 3)] = new SuffixTreeNode.Leaf(8),
                    }),
                }),
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
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
                })),

            [OneLevelTreeToMostUnbalancedTenLevelsTrie] = (
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 10)] = new SuffixTreeNode.Leaf(0),
                }),
                BuildMostUnbalancedTrie(10)),

            [OneLevelTreeToMostUnbalancedAHundredLevelsTrie] = (
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 100)] = new SuffixTreeNode.Leaf(0),
                }),
                BuildMostUnbalancedTrie(100))
            };

    private static SuffixTrieNode BuildMostUnbalancedTrie(int depth)
    {
        var trieRoot = new SuffixTrieNode.Leaf(0) as SuffixTrieNode;
        for (var i = depth - 1; i >= 0; i--)
        {
            trieRoot = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
            {
                [new(i)] = trieRoot,
            });
        }

        return trieRoot;
    }
}

using MoreStructures.SuffixTrees;
using System.Collections.Generic;

namespace MoreStructures.Tests.SuffixTrees.Builders;

using static BuilderEquivalences.EquivalenceId;

public static class BuilderEquivalences
{
    public enum EquivalenceId
    {
        EmptyString,
        SingleChar,
        TwoCharsString_DifferentPrefixes,
        TwoCharsString_SamePrefixes,
        ThreeCharsString_SamePrefixes,
        ThreeCharsString_PartiallySamePrefixes,
        ThreeCharsString_DifferentPrefixes,
        TwoChars_ExtendingPrefixes,
        ThreeChars_ExtendingPrefixes,
    }

    public static readonly Dictionary<EquivalenceId, (TextWithTerminator text, SuffixTreeNode treeNode)> Equivalences =
        new()
        {
            [EmptyString] = (
                new(string.Empty),
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 1)] = new SuffixTreeNode.Leaf(0),
                })),

            [SingleChar] = (
                new("a"),
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 2)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 1)] = new SuffixTreeNode.Leaf(1),
                })),

            [TwoCharsString_DifferentPrefixes] = (
                new("ab"),
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 3)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 2)] = new SuffixTreeNode.Leaf(1),
                    [new(2, 1)] = new SuffixTreeNode.Leaf(2),
                })),

            [TwoCharsString_SamePrefixes] = (
                new("aa"),
                BuildMostUnbalancedValidSuffixTree(2)),

            [ThreeCharsString_SamePrefixes] = (
                new("aaa"),
                BuildMostUnbalancedValidSuffixTree(3)),

            [ThreeCharsString_PartiallySamePrefixes] = (
                new("aba"),
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(1, 3)] = new SuffixTreeNode.Leaf(0),
                        [new(3, 1)] = new SuffixTreeNode.Leaf(2),
                    }),
                    [new(1, 3)] = new SuffixTreeNode.Leaf(1),
                    [new(3, 1)] = new SuffixTreeNode.Leaf(3),
                })),

            [ThreeCharsString_DifferentPrefixes] = (
                new("abc"),
                BuildFlattestValidSuffixTree(4)),

            [TwoChars_ExtendingPrefixes] = (
                new("aababcabcd"),
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(1, 10)] = new SuffixTreeNode.Leaf(0),
                        [new(2, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode> 
                        {
                            [new(3, 8)] = new SuffixTreeNode.Leaf(1),
                            [new(5, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                            {
                                [new(6, 5)] = new SuffixTreeNode.Leaf(3),
                                [new(9, 2)] = new SuffixTreeNode.Leaf(6),
                            }),
                        }),
                    }),
                    [new(2, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(3, 8)] = new SuffixTreeNode.Leaf(2),
                        [new(5, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                        {
                            [new(6, 5)] = new SuffixTreeNode.Leaf(4),
                            [new(9, 2)] = new SuffixTreeNode.Leaf(7),
                        }),
                    }),
                    [new(5, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(6, 5)] = new SuffixTreeNode.Leaf(5),
                        [new(9, 2)] = new SuffixTreeNode.Leaf(8),
                    }),
                    [new(9, 2)] = new SuffixTreeNode.Leaf(9),
                    [new(10, 1)] = new SuffixTreeNode.Leaf(10),
                })),

            [ThreeChars_ExtendingPrefixes] = (
                new("xyzxyaxyz"), new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 2)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(2, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                        {
                            [new(3, 7)] = new SuffixTreeNode.Leaf(0),
                            [new(9, 1)] = new SuffixTreeNode.Leaf(6),
                        }),
                        [new(5, 5)] = new SuffixTreeNode.Leaf(3),
                    }),
                    [new(1, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(2, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                        {
                            [new(3, 7)] = new SuffixTreeNode.Leaf(1),
                            [new(9, 1)] = new SuffixTreeNode.Leaf(7),
                        }),
                        [new(5, 5)] = new SuffixTreeNode.Leaf(4),
                    }),
                    [new(2, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(3, 7)] = new SuffixTreeNode.Leaf(2),
                        [new(9, 1)] = new SuffixTreeNode.Leaf(8),
                    }),
                    [new(5, 5)] = new SuffixTreeNode.Leaf(5),
                    [new(9, 1)] = new SuffixTreeNode.Leaf(9),
                })),
        };

    private static SuffixTreeNode BuildMostUnbalancedValidSuffixTree(int depth)
    {
        var treeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(depth - 1, 2)] = new SuffixTreeNode.Leaf(0),
            [new(depth, 1)] = new SuffixTreeNode.Leaf(1),

        });
        for (var i = depth - 2; i >= 0; i--)
        {
            treeRoot = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
            {
                [new(i, 1)] = treeRoot,
                [new(depth, 1)] = new SuffixTreeNode.Leaf(depth - i),
            });
        }

        return treeRoot;
    }

    private static SuffixTreeNode BuildFlattestValidSuffixTree(int fanout)
    {
        var children = new Dictionary<SuffixTreeEdge, SuffixTreeNode> { };
        for (var i = 0; i < fanout; i++)
            children[new(i, fanout - i)] = new SuffixTreeNode.Leaf(i);
        return new SuffixTreeNode.Intermediate(children);
    }
}

using MoreStructures.SuffixTrees;

namespace MoreStructures.Tests.SuffixTrees.Builders;

using static BuilderEquivalences.EquivalenceId;

public static class BuilderEquivalences
{
    public enum EquivalenceId
    {
        // Single TextWithTerminator input
        EmptyString,
        SingleChar,
        TwoCharsString_DifferentPrefixes,
        TwoCharsString_SamePrefixes,
        ThreeCharsString_SamePrefixes,
        ThreeCharsString_PartiallySamePrefixes,
        ThreeCharsString_DifferentPrefixes,
        TwoChars_ExtendingPrefixes,
        ThreeChars_ExtendingPrefixes,
        // Generalized
        EmptyStrings,
        OneNonEmptyOneEmpty,
        OneEmptyOneNonEmpty,
        TwoEmptyOneNonEmpty,
        TwoEmptyOneNonEmptyDifferentOrder,
        TwoNonSharingChars,
        TwoSharingChars,
        TwoSame,
        ThreeDifferent,
        TwoSameOneDifferent,
        ThreeSame,
    }

    public record EquivalenceValue(TextWithTerminator[] Texts, SuffixTreeNode TreeNode);

    public static readonly Dictionary<EquivalenceId, EquivalenceValue> Equivalences =
        new()
        {
            #region Single TextWithTerminator input

            [EmptyString] = new EquivalenceValue(
                new TextWithTerminator[] { new(string.Empty) },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 1)] = new SuffixTreeNode.Leaf(0),
                })),

            [SingleChar] = new(
                new TextWithTerminator[] { new("a") },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 2)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 1)] = new SuffixTreeNode.Leaf(1),
                })),

            [TwoCharsString_DifferentPrefixes] = new(
                new TextWithTerminator[] { new("ab") },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 3)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 2)] = new SuffixTreeNode.Leaf(1),
                    [new(2, 1)] = new SuffixTreeNode.Leaf(2),
                })),

            [TwoCharsString_SamePrefixes] = new(
                new TextWithTerminator[] { new("aa") },
                BuildMostUnbalancedValidSuffixTree(2)),

            [ThreeCharsString_SamePrefixes] = new(
                new TextWithTerminator[] { new("aaa") },
                BuildMostUnbalancedValidSuffixTree(3)),

            [ThreeCharsString_PartiallySamePrefixes] = new(
                new TextWithTerminator[] { new("aba") },
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

            [ThreeCharsString_DifferentPrefixes] = new(
                new TextWithTerminator[] { new("abc") },
                BuildFlattestValidSuffixTree(4)),

            [TwoChars_ExtendingPrefixes] = new(
                new TextWithTerminator[] { new("aababcabcd") },
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

            [ThreeChars_ExtendingPrefixes] = new(
                new TextWithTerminator[] { new("xyzxyaxyz") },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
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

            #endregion

            #region Generalized

            [EmptyStrings] = new(
                new TextWithTerminator[] { new(string.Empty, 'x'), new(string.Empty, 'y'), new(string.Empty, 'z') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 1)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 1)] = new SuffixTreeNode.Leaf(1),
                    [new(2, 1)] = new SuffixTreeNode.Leaf(2),
                })),

            [OneNonEmptyOneEmpty] = new(
                new TextWithTerminator[] { new("a", 'x'), new(string.Empty, 'y') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 2)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 1)] = new SuffixTreeNode.Leaf(1),
                    [new(2, 1)] = new SuffixTreeNode.Leaf(2),
                })),

            [OneEmptyOneNonEmpty] = new(
                new TextWithTerminator[] { new(string.Empty, 'x'), new("aba", 'y') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 1)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(2, 3)] = new SuffixTreeNode.Leaf(1),
                        [new(4, 1)] = new SuffixTreeNode.Leaf(3),
                    }),
                    [new(2, 3)] = new SuffixTreeNode.Leaf(2),
                    [new(4, 1)] = new SuffixTreeNode.Leaf(4),
                })),

            [TwoEmptyOneNonEmpty] = new(
                new TextWithTerminator[] { new(string.Empty, 'x'), new(string.Empty, 'y'), new("ab", 'z') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 1)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 1)] = new SuffixTreeNode.Leaf(1),
                    [new(2, 3)] = new SuffixTreeNode.Leaf(2),
                    [new(3, 2)] = new SuffixTreeNode.Leaf(3),
                    [new(4, 1)] = new SuffixTreeNode.Leaf(4),
                })),

            [TwoEmptyOneNonEmptyDifferentOrder] = new(
                new TextWithTerminator[] { new(string.Empty, 'x'), new("ab", 'y'), new(string.Empty, 'z') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 1)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 3)] = new SuffixTreeNode.Leaf(1),
                    [new(2, 2)] = new SuffixTreeNode.Leaf(2),
                    [new(3, 1)] = new SuffixTreeNode.Leaf(3),
                    [new(4, 1)] = new SuffixTreeNode.Leaf(4),
                })),

            [TwoNonSharingChars] = new(
                new TextWithTerminator[] { new("ab", 'x'), new("cd", 'y')},
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 3)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 2)] = new SuffixTreeNode.Leaf(1),
                    [new(2, 1)] = new SuffixTreeNode.Leaf(2),
                    [new(3, 3)] = new SuffixTreeNode.Leaf(3),
                    [new(4, 2)] = new SuffixTreeNode.Leaf(4),
                    [new(5, 1)] = new SuffixTreeNode.Leaf(5),
                })),

            [TwoSharingChars] = new(
                new TextWithTerminator[] { new("ab", 'x'), new("cb", 'y') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 3)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(2, 1)] = new SuffixTreeNode.Leaf(1),
                        [new(5, 1)] = new SuffixTreeNode.Leaf(4),
                    }),
                    [new(2, 1)] = new SuffixTreeNode.Leaf(2),
                    [new(3, 3)] = new SuffixTreeNode.Leaf(3),
                    [new(5, 1)] = new SuffixTreeNode.Leaf(5),
                })),

            [TwoSame] = new(
                new TextWithTerminator[] { new("abc", 'x'), new("abc", 'y') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 3)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(3, 1)] = new SuffixTreeNode.Leaf(0),
                        [new(7, 1)] = new SuffixTreeNode.Leaf(4),
                    }),
                    [new(1, 2)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(3, 1)] = new SuffixTreeNode.Leaf(1),
                        [new(7, 1)] = new SuffixTreeNode.Leaf(5),
                    }),
                    [new(2, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(3, 1)] = new SuffixTreeNode.Leaf(2),
                        [new(7, 1)] = new SuffixTreeNode.Leaf(6),
                    }),
                    [new(3, 1)] = new SuffixTreeNode.Leaf(3),
                    [new(7, 1)] = new SuffixTreeNode.Leaf(7),
                })),
            
            [ThreeDifferent] = new(
                new TextWithTerminator[] { new("a", 'x'), new("b", 'y'), new("cd", 'z') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 2)] = new SuffixTreeNode.Leaf(0),
                    [new(1, 1)] = new SuffixTreeNode.Leaf(1),
                    [new(2, 2)] = new SuffixTreeNode.Leaf(2),
                    [new(3, 1)] = new SuffixTreeNode.Leaf(3),
                    [new(4, 3)] = new SuffixTreeNode.Leaf(4),
                    [new(5, 2)] = new SuffixTreeNode.Leaf(5),
                    [new(6, 1)] = new SuffixTreeNode.Leaf(6),
                })),

            [TwoSameOneDifferent] = new(
                new TextWithTerminator[] { new("cd", 'x'), new("c", 'y'), new("cd", 'z') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(1, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                        {
                            [new(2, 1)] = new SuffixTreeNode.Leaf(0),
                            [new(7, 1)] = new SuffixTreeNode.Leaf(5),
                        }),
                        [new(4, 1)] = new SuffixTreeNode.Leaf(3),
                    }),
                    [new(1, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(2, 1)] = new SuffixTreeNode.Leaf(1),
                        [new(7, 1)] = new SuffixTreeNode.Leaf(6),
                    }),
                    [new(2, 1)] = new SuffixTreeNode.Leaf(2),
                    [new(4, 1)] = new SuffixTreeNode.Leaf(4),
                    [new(7, 1)] = new SuffixTreeNode.Leaf(7),
                })),

            [ThreeSame] = new(
                new TextWithTerminator[] { new("cd", 'x'), new("cd", 'y'), new("cd", 'z') },
                new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                {
                    [new(0, 2)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(2, 1)] = new SuffixTreeNode.Leaf(0),
                        [new(5, 1)] = new SuffixTreeNode.Leaf(3),
                        [new(8, 1)] = new SuffixTreeNode.Leaf(6),
                    }),
                    [new(1, 1)] = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
                    {
                        [new(2, 1)] = new SuffixTreeNode.Leaf(1),
                        [new(5, 1)] = new SuffixTreeNode.Leaf(4),
                        [new(8, 1)] = new SuffixTreeNode.Leaf(7),
                    }),
                    [new(2, 1)] = new SuffixTreeNode.Leaf(2),
                    [new(5, 1)] = new SuffixTreeNode.Leaf(5),
                    [new(8, 1)] = new SuffixTreeNode.Leaf(8),
                })),

            #endregion
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

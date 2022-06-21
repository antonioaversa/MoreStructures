using MoreStructures.SuffixTries;

namespace MoreStructures.Tests.SuffixTries.Builders;

using static BuilderEquivalences.EquivalenceId;

public static class BuilderEquivalences
{
    public enum EquivalenceId
    {
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

    public record EquivalenceValue(TextWithTerminator[] Texts, SuffixTrieNode TreeNode);

    public static readonly Dictionary<EquivalenceId, EquivalenceValue> Equivalences =
        new()
        {
            #region Generalized

            [EmptyStrings] = new(
                new TextWithTerminator[] { new(string.Empty, 'x'), new(string.Empty, 'y'), new(string.Empty, 'z') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Leaf(0),
                    [new(1)] = new SuffixTrieNode.Leaf(1),
                    [new(2)] = new SuffixTrieNode.Leaf(2),
                })),
            
            [OneNonEmptyOneEmpty] = new(
                new TextWithTerminator[] { new("a", 'x'), new(string.Empty, 'y') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(1)] = new SuffixTrieNode.Leaf(0),
                    }),
                    [new(1)] = new SuffixTrieNode.Leaf(1),
                    [new(2)] = new SuffixTrieNode.Leaf(2),
                })),

            [OneEmptyOneNonEmpty] = new(
                new TextWithTerminator[] { new(string.Empty, 'x'), new("aba", 'y') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Leaf(0),
                    [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                            {
                                [new(4)] = new SuffixTrieNode.Leaf(1),
                            }),
                        }),
                        [new(4)] = new SuffixTrieNode.Leaf(3),
                    }),
                    [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(4)] = new SuffixTrieNode.Leaf(2),
                        }),
                    }),
                    [new(4)] = new SuffixTrieNode.Leaf(4),
                })),

            [TwoEmptyOneNonEmpty] = new(
                new TextWithTerminator[] { new(string.Empty, 'x'), new(string.Empty, 'y'), new("ab", 'z') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Leaf(0),
                    [new(1)] = new SuffixTrieNode.Leaf(1),
                    [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(4)] = new SuffixTrieNode.Leaf(2),
                        }),
                    }),
                    [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(4)] = new SuffixTrieNode.Leaf(3),
                    }),
                    [new(4)] = new SuffixTrieNode.Leaf(4),
                })),

            [TwoEmptyOneNonEmptyDifferentOrder] = new(
                new TextWithTerminator[] { new(string.Empty, 'x'), new("ab", 'y'), new(string.Empty, 'z') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Leaf(0),
                    [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(3)] = new SuffixTrieNode.Leaf(1),
                        }),
                    }),
                    [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(3)] = new SuffixTrieNode.Leaf(2),
                    }),
                    [new(3)] = new SuffixTrieNode.Leaf(3),
                    [new(4)] = new SuffixTrieNode.Leaf(4),
                })),

            [TwoNonSharingChars] = new(
                new TextWithTerminator[] { new("ab", 'x'), new("cd", 'y') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(2)] = new SuffixTrieNode.Leaf(0),
                        }),
                    }),
                    [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(2)] = new SuffixTrieNode.Leaf(1),
                    }),
                    [new(2)] = new SuffixTrieNode.Leaf(2),
                    [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(4)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(5)] = new SuffixTrieNode.Leaf(3),
                        }),
                    }),
                    [new(4)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(5)] = new SuffixTrieNode.Leaf(4),
                    }),
                    [new(5)] = new SuffixTrieNode.Leaf(5),
                })),

            [TwoSharingChars] = new(
                new TextWithTerminator[] { new("ab", 'x'), new("cb", 'y') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(2)] = new SuffixTrieNode.Leaf(0),
                        }),
                    }),
                    [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(2)] = new SuffixTrieNode.Leaf(1),
                        [new(5)] = new SuffixTrieNode.Leaf(4),
                    }),
                    [new(2)] = new SuffixTrieNode.Leaf(2),
                    [new(3)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(4)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(5)] = new SuffixTrieNode.Leaf(3),
                        }),
                    }),
                    [new(5)] = new SuffixTrieNode.Leaf(5),
                })),

            [TwoSame] = new(
                new TextWithTerminator[] { new("abc", 'x'), new("abc", 'y') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                            {
                                [new(3)] = new SuffixTrieNode.Leaf(0),
                                [new(7)] = new SuffixTrieNode.Leaf(4),
                            }),
                        }),
                    }),
                    [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(3)] = new SuffixTrieNode.Leaf(1),
                            [new(7)] = new SuffixTrieNode.Leaf(5),
                        }),
                    }),
                    [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(3)] = new SuffixTrieNode.Leaf(2),
                        [new(7)] = new SuffixTrieNode.Leaf(6),
                    }),
                    [new(3)] = new SuffixTrieNode.Leaf(3),
                    [new(7)] = new SuffixTrieNode.Leaf(7),
                })),

            [ThreeDifferent] = new(
                new TextWithTerminator[] { new("a", 'x'), new("b", 'y'), new("cd", 'z') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(1)] = new SuffixTrieNode.Leaf(0),
                    }),
                    [new(1)] = new SuffixTrieNode.Leaf(1),
                    [new(2)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(3)] = new SuffixTrieNode.Leaf(2),
                    }),
                    [new(3)] = new SuffixTrieNode.Leaf(3),
                    [new(4)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(5)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(6)] = new SuffixTrieNode.Leaf(4),
                        }),
                    }),
                    [new(5)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(6)] = new SuffixTrieNode.Leaf(5),
                    }),
                    [new(6)] = new SuffixTrieNode.Leaf(6),
                })),

            [TwoSameOneDifferent] = new(
                new TextWithTerminator[] { new("cd", 'x'), new("c", 'y'), new("cd", 'z') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(2)] = new SuffixTrieNode.Leaf(0),
                            [new(7)] = new SuffixTrieNode.Leaf(5),
                        }),
                        [new(4)] = new SuffixTrieNode.Leaf(3),
                    }),
                    [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(2)] = new SuffixTrieNode.Leaf(1),
                        [new(7)] = new SuffixTrieNode.Leaf(6),
                    }),
                    [new(2)] = new SuffixTrieNode.Leaf(2),
                    [new(4)] = new SuffixTrieNode.Leaf(4),
                    [new(7)] = new SuffixTrieNode.Leaf(7),
                })),

            [ThreeSame] = new(
                new TextWithTerminator[] { new("cd", 'x'), new("cd", 'y'), new("cd", 'z') },
                new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                {
                    [new(0)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                        {
                            [new(2)] = new SuffixTrieNode.Leaf(0),
                            [new(5)] = new SuffixTrieNode.Leaf(3),
                            [new(8)] = new SuffixTrieNode.Leaf(6),
                        }),
                    }),
                    [new(1)] = new SuffixTrieNode.Intermediate(new Dictionary<SuffixTrieEdge, SuffixTrieNode>
                    {
                        [new(2)] = new SuffixTrieNode.Leaf(1),
                        [new(5)] = new SuffixTrieNode.Leaf(4),
                        [new(8)] = new SuffixTrieNode.Leaf(7),
                    }),
                    [new(2)] = new SuffixTrieNode.Leaf(2),
                    [new(5)] = new SuffixTrieNode.Leaf(5),
                    [new(8)] = new SuffixTrieNode.Leaf(8),
                })),

            #endregion
        };
}

﻿using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Conversions;
using MoreStructures.SuffixStructures;
using MoreStructures.Utilities;

namespace MoreStructures.SuffixTries;

/// <summary>
/// An immutable node of an immutable Suffix Trie, recursively pointing to its children nodes via 
/// <see cref="SuffixTrieEdge"/> instances, associated with selector characters.
/// </summary>
/// <param name="Children">The collection of children for the node, indexed by single char edges.</param>
/// <param name="Start">
///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}.Start" path="/summary"/>
/// </param>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Compare to suffix trees, suffix tries, although less performant and optimized on many operations, are simpler 
///     to build, navigate and understand.
///     </para>
///     <para id="immutability">
///     IMMUTABILITY
///     <br/>
///     Immutability is guaranteed by using <see cref="ValueReadOnlyCollection{T}"/>.
///     </para>
/// </remarks>
public abstract record SuffixTrieNode(IDictionary<SuffixTrieEdge, SuffixTrieNode> Children, int? Start)
    : ISuffixStructureNode<SuffixTrieEdge, SuffixTrieNode>
{
    /// <summary>
    /// Builds an intermediate node, i.e. a node with children and their corresponding incoming edges.
    /// </summary>
    public record Intermediate(IDictionary<SuffixTrieEdge, SuffixTrieNode> Children)
        : SuffixTrieNode(Children, null);

    /// <summary>
    /// Builds a leaf, i.e. a node with no children and the start index of the suffix in the text.
    /// </summary>
    public record Leaf(int LeafStart)
        : SuffixTrieNode(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { }, LeafStart);

    /// <inheritdoc/>
    public IDictionary<SuffixTrieEdge, SuffixTrieNode> Children { get; }
        = (Children.Any() == (Start == null)) && Start.GetValueOrDefault(0) >= 0
        ? Children.ToValueReadOnlyDictionary()
        : throw new ArgumentException($"Leafs needs to specificy a non-negative {nameof(Start)}.", nameof(Children));

    /// <inheritdoc/>
    public int? Start { get; } = Start;

    /// <summary>
    /// Indexes into the children of this node, by edge, which is a single char selector.
    /// </summary>
    public SuffixTrieNode this[SuffixTrieEdge edge] => Children[edge];

    private static readonly IStringifier<SuffixTrieEdge, SuffixTrieNode> Stringifier =
        new FullyIterativeStringifier<SuffixTrieEdge, SuffixTrieNode>(
            r => r.IsLeaf() ? $"R from {r.Start}" : "R",
            (e, n) => $"{e} -> {(n.IsLeaf() ? $"L from {n.Start}" : "I")}")
        {
            StopIndentingLevel = 10,
        };

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses a <see cref="IStringifier{TEdge, TNode}"/> to generate the string which show the node and its 
    ///     underlying structure.
    /// </summary>
    /// <remarks>
    /// Sealed to prevent compiler from superceding <see cref="ToString()"/> in derived record.
    /// </remarks>
    public sealed override string ToString() => Stringifier.Stringify(this);
}

using StringAlgorithms.SuffixStructures;
using StringAlgorithms.Utilities;

namespace StringAlgorithms.SuffixTries;

/// <summary>
/// An immutable node of an immutable Suffix Trie, recursively pointing to its children node.
/// </summary>
/// <param name="Children">The collection of children for the node, indexed by single char edges.</param>
/// <param name="Start">
///     <inheritdoc cref="ISuffixStructureNode{TEdge, TNode, TPath, TBuilder}.Start" path="/summary"/>
/// </param>
/// <remarks>
/// Immutability is guaranteed by using <see cref="ValueReadOnlyCollection{T}"/>.
/// </remarks>
public abstract record SuffixTrieNode(IDictionary<SuffixTrieEdge, SuffixTrieNode> Children, int? Start)
    : ISuffixStructureNode<SuffixTrieEdge, SuffixTrieNode, SuffixTriePath, SuffixTrieBuilder>
{
    /// <summary>
    /// Builds an intermediate node, i.e. a node with children and their corresponding incoming edges.
    /// </summary>
    public record Intermediate(IDictionary<SuffixTrieEdge, SuffixTrieNode> Children)
        : SuffixTrieNode(Children, null)
    { }

    /// <summary>
    /// Builds a leaf, i.e. a node with no children and the start index of the suffix in the text.
    /// </summary>
    public record Leaf(int LeafStart)
        : SuffixTrieNode(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { }, LeafStart)
    { }

    public IDictionary<SuffixTrieEdge, SuffixTrieNode> Children { get; }
        = (Children.Any() == (Start == null)) && Start.GetValueOrDefault(0) >= 0
        ? Children.ToValueReadOnlyDictionary()
        : throw new ArgumentException($"Leafs needs to specificy a non-negative {nameof(Start)}.", nameof(Children));

    public int? Start { get; } = Start;

    /// <summary>
    /// Indexes into the children of this node, by edge, which is a single char selector.
    /// </summary>
    public SuffixTrieNode this[SuffixTrieEdge edge] => Children[edge];
}

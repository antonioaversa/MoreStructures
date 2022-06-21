using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Conversions;
using MoreStructures.SuffixStructures;
using MoreStructures.Utilities;

namespace MoreStructures.SuffixTrees;

/// <summary>
/// An immutable node of an immutable Suffix Tree, recursively pointing to its children nodes via 
/// <see cref="SuffixTreeEdge"/> instances, associated with selector strings.
/// </summary>
/// <param name="Children">The collection of children for the node, indexed by string edges.</param>
/// <param name="Start">
/// <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}.Start" path="/summary"/>
/// </param>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Suffix Trees are more space-efficient than Suffix Tries due to the reduced number of 
///     <see cref="SuffixTreeEdge"/> and their <see cref="SuffixTreeNode"/>, compare to the corresponding
///     <see cref="SuffixTries.SuffixTrieEdge"/> and their <see cref="SuffixTries.SuffixTrieNode"/>, due to entire 
///     paths of single chains of chars in Suffix Tries being coalesced into a single selector string, which is stored
///     on the edge with path label compression, i.e. using two fixed sized numbers (<see cref="SuffixTreeEdge.Start"/> 
///     and <see cref="SuffixTreeEdge.Length"/>) instead of a variable-length string of characters.
///     <br/>
///     Furthermore, suffix trees, unlike suffix tries, can be constructed in linear time, for example via the 
///     <see cref="Builders.UkkonenSuffixTreeBuilder"/>.
///     </para>
///     <para id="immutability">
///     IMMUTABILITY
///     <br/>
///     Immutability is guaranteed by using <see cref="ValueReadOnlyCollection{T}"/>.
///     </para>
/// </remarks>
public abstract record SuffixTreeNode(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children, int? Start)
    : ISuffixStructureNode<SuffixTreeEdge, SuffixTreeNode>
{
    /// <summary>
    /// Builds an intermediate node, i.e. a node with children and their corresponding incoming edges.
    /// </summary>
    public record Intermediate(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children) 
        : SuffixTreeNode(Children, null);

    /// <summary>
    /// Builds a leaf, i.e. a node with no children and the start index of the suffix in the text.
    /// </summary>
    public record Leaf(int LeafStart) 
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { }, LeafStart);

    /// <inheritdoc/>
    public IDictionary<SuffixTreeEdge, SuffixTreeNode> Children { get; } 
        = (Children.Any() == (Start == null)) && Start.GetValueOrDefault(0) >= 0
        ? Children.ToValueReadOnlyDictionary()
        : throw new ArgumentException($"Leafs needs to specificy a non-negative {nameof(Start)}.", nameof(Children));

    /// <inheritdoc/>
    public int? Start { get; } = Start;

    /// <summary>
    /// Indexes into the children of this node, by edge.
    /// </summary>
    public SuffixTreeNode this[SuffixTreeEdge edge] => Children[edge];

    private static readonly IStringifier<SuffixTreeEdge, SuffixTreeNode> Stringifier =
        new FullyIterativeStringifier<SuffixTreeEdge, SuffixTreeNode>(
            r => r.IsLeaf() ? $"R from {r.Start}" : "R",
            (e, n) => $"{e} -> {(n.IsLeaf() ? $"L from {n.Start}" : "I")}")
            {
                StopIndentingLevel = 10,
            };

    /// <summary>
    /// <inheritdoc/>
    /// <br/>
    /// Uses a <see cref="IStringifier{TEdge, TNode}"/> to generate the string which show the node and its underlying
    /// structure.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    /// <remarks>
    /// Sealed to prevent compiler from superceding <see cref="ToString()"/> in derived record.
    /// </remarks>
    public override sealed string ToString() => Stringifier.Stringify(this);
}

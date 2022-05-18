using StringAlgorithms.SuffixStructures;
using StringAlgorithms.Utilities;

namespace StringAlgorithms.SuffixTrees;

/// <summary>
/// An immutable node of an immutable Suffix Tree, recursively pointing to its children node.
/// </summary>
/// <param name="Children">The collection of children for the node, indexed by string edges.</param>
/// <param name="Start">
/// <inheritdoc cref="ISuffixStructureNode{TEdge, TNode, TPath, TBuilder}.Start" path="/summary"/>
/// </param>
/// <remarks>
/// Immutability is guaranteed by using <see cref="ValueReadOnlyCollection{T}"/>.
/// </remarks>
public abstract record SuffixTreeNode(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children, int? Start)
    : ISuffixStructureNode<SuffixTreeEdge, SuffixTreeNode, SuffixTreePath, SuffixTreeBuilder>
{
    /// <summary>
    /// Builds an intermediate node, i.e. a node with children and their corresponding incoming edges.
    /// </summary>
    public record Intermediate(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children) 
        : SuffixTreeNode(Children, null)
    { }

    /// <summary>
    /// Builds a leaf, i.e. a node with no children and the start index of the suffix in the text.
    /// </summary>
    public record Leaf(int LeafStart) 
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { }, LeafStart) 
    { }

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
}

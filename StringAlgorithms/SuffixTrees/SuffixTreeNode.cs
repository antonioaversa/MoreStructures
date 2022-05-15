using System.Collections.ObjectModel;

namespace StringAlgorithms.SuffixTrees;

/// <summary>
/// An immutable node of an immutable Suffix Tree, recursively pointing to its children node.
/// </summary>
/// <param name="Children">The collection of children for the node, indexed by edges.</param>
/// <param name="Start">
/// The index of the character, the path of Suffix Tree leading to this leaf starts with. Non-null for leaves only.
/// </param>
/// <remarks>
/// Immutability is guaranteed by copying the provided children collection and exposing a readonly view.
/// </remarks>
public abstract record SuffixTreeNode(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children, int? Start)
{
    /// <summary>
    /// Builds a intermediate node, i.e. a node with children and their corresponding incoming edges.
    /// </summary>
    public record Intermediate(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children) 
        : SuffixTreeNode(Children, null) { }

    /// <summary>
    /// Builds a leaf, i.e. a node with no children and the start index of the suffix in the text.
    /// </summary>
    public record Leaf(int LeafStart) 
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { }, LeafStart) { }

    /// <summary>
    /// A readonly view of the children private collection of this Suffix Tree node. Empty for leaves.
    /// </summary>
    public IDictionary<SuffixTreeEdge, SuffixTreeNode> Children { get; } 
        = (Children.Count > 0 && Start == null) || (Children.Count == 0 && Start != null)
        ? new ReadOnlyDictionary<SuffixTreeEdge, SuffixTreeNode>(new Dictionary<SuffixTreeEdge, SuffixTreeNode>(Children))
        : throw new ArgumentException($"Leafs needs to specificy a non-negative {nameof(Start)}.", nameof(Children));

    /// <summary>
    /// <inheritdoc cref="SuffixTreeNode(IDictionary{SuffixTreeEdge, SuffixTreeNode}, int?)" 
    /// path="/param[@name='Start']"/>
    /// </summary>
    public int? Start { get; init; }
        = (Children.Count > 0 && Start == null) || (Children.Count == 0 && Start != null)
        ? Start
        : throw new ArgumentException($"Leafs needs to specificy a non-negative {nameof(Start)}.", nameof(Children));

    /// <summary>
    /// Indexes into the children of this node, by edge.
    /// </summary>
    public SuffixTreeNode this[SuffixTreeEdge edge] => Children[edge];

    /// <summary>
    /// Whether this node is a leaf of the Suffix Tree (no children), or not.
    /// </summary>
    public bool IsLeaf => Children.Count == 0;

    /// <summary>
    /// Returns all Suffix Tree paths from this node to a leaf.
    /// </summary>
    /// <returns>A sequence of pairs of Suffix Tree node and its incoming edge.</returns>
    public IEnumerable<SuffixTreePath> GetAllNodeToLeafPaths()
    {
        foreach (var edgeAndChild in Children)
        {
            var childToLeafPaths = edgeAndChild.Value.GetAllNodeToLeafPaths();
            var edgeAndChildSingleton = Enumerable.Repeat(edgeAndChild, 1);
            if (childToLeafPaths.Any())
            {
                foreach (var childToLeafPath in childToLeafPaths)
                    yield return new SuffixTreePath(edgeAndChildSingleton).Concat(childToLeafPath);
            }
            else
            {
                yield return new SuffixTreePath(edgeAndChildSingleton);
            }
        }
    }

    /// <summary>
    /// Returns all suffixes for the provided text from this node down the Suffix Tree, up to leaves.
    /// </summary>
    /// <param name="text">The text with terminator, whose suffixes have to be extracted.</param>
    /// <returns>A sequence of strings, each one being a suffix.</returns>
    public IEnumerable<string> GetAllSuffixesFor(TextWithTerminator text) =>
        GetAllNodeToLeafPaths().Select(rootToLeafPath => rootToLeafPath.SuffixFor(text));   
}

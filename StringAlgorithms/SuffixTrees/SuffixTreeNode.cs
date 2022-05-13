using System.Collections.ObjectModel;

namespace StringAlgorithms.SuffixTrees;

/// <summary>
/// An immutable node of an immutable Suffix Tree, recursively pointing to its children node.
/// </summary>
/// <param name="Children">The children of the node, indexed by edges. Empty collection for leaves.</param>
/// <remarks>
/// Immutability is guaranteed by copying the provided children collection and exposing a readonly view.
/// </remarks>
public record SuffixTreeNode(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children)
{
    /// <summary>
    /// A readonly view of the children private collection of this Suffix Tree node.
    /// </summary>
    public IDictionary<SuffixTreeEdge, SuffixTreeNode> Children { get; } 
        = new ReadOnlyDictionary<SuffixTreeEdge, SuffixTreeNode>(new Dictionary<SuffixTreeEdge, SuffixTreeNode>(Children));

    /// <summary>
    /// The index of the character, the path of Suffix Tree leading to this leaf starts with. Non-null for leaves only.
    /// </summary>
    public int? Start { get; init; } = default;

    /// <summary>
    /// Builds a Suffix Tree leaf, i.e. a node with no children, and with the index of the 1st character of the suffix.
    /// </summary>
    /// <param name="start"><inheritdoc cref="Start" path="/summary"/></param>
    public SuffixTreeNode(int start)
        : this(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { }) 
    {
        if (start < 0) throw new ArgumentOutOfRangeException(nameof(start), "Has to be non-negative.");

        Start = start;
    }

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
        GetAllNodeToLeafPaths().Select(rootToLeafPath => rootToLeafPath.Suffix(text));   
}

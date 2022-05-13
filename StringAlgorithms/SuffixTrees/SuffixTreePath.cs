using System.Text;

namespace StringAlgorithms.SuffixTrees;

/// <summary>
/// An immutable sequence of Suffix Tree nodes, where each node is child of its predecessor and parent of its successor.
/// </summary>
/// <param name="PathNodes">The sequence of Suffix nodes respecting the parent-child relationship.</param>
/// <remarks>
/// Immutability is guaranteed by cloning the provided path nodes collection and exposing a readonly view, combined 
/// with the immutability of underlying data structures (Suffix Tree edges and nodes).
/// </remarks>
public record SuffixTreePath(IEnumerable<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> PathNodes)
{
    /// <summary>
    /// An empty path, i.e. an empty sequence of Suffix Tree nodes.
    /// </summary>
    public static SuffixTreePath Empty() => 
        new(Enumerable.Empty<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>>());

    /// <summary>
    /// A Suffix Tree path composed of a single node with its incoming edge.
    /// </summary>
    /// <param name="edge">The Suffix Tree edge leading to the Suffix Tree node.</param>
    /// <param name="node">The Suffix Tree node defining the singleton path.</param>
    public static SuffixTreePath Singleton(SuffixTreeEdge edge, SuffixTreeNode node) =>
        new(Enumerable.Repeat(KeyValuePair.Create(edge, node), 1));

    /// <summary>
    /// A readonly view of the private collection of path nodes.
    /// </summary>
    public IEnumerable<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> PathNodes { get; } = PathNodes.ToList().AsReadOnly();

    /// <summary>
    /// Append the provided path of Suffix Tree nodes to this path.
    /// </summary>
    /// <param name="path">The Suffix Tree path, whose nodes have to be appended.</param>
    /// <returns>A new Suffix Tree path, whose nodes are the concatenation of the nodes of the two paths.</returns>
    public SuffixTreePath Concat(SuffixTreePath path)
    {
        return new SuffixTreePath(PathNodes.Concat(path.PathNodes));
    }

    /// <summary>
    /// Calculate the suffix corresponding to this Suffix Tree path on the provided terminator-including text.
    /// </summary>
    /// <param name="text">The text, including the terminator character.</param>
    /// <returns>A string containing the suffix.</returns>
    public string Suffix(TextWithTerminator text) => PathNodes
        .Aggregate(new StringBuilder(),
            (acc, node) => acc.Append(text.AsString.AsSpan(node.Key.Start, node.Key.Length)))
        .ToString();
}
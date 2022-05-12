using System.Text;

namespace StringAlgorithms;

/// <summary>
/// An immutable sequence of Suffix Tree nodes, where each node is child of its predecessor and parent of its successor.
/// </summary>
/// <param name="PathNodes">The sequence of Suffix nodes respecting the parent-child relationship.</param>
/// <remarks>
/// Immutability is guaranteed by cloning the provided path nodes collection and exposing a readonly view, combined 
/// with the immutability of underlying data structures (prefix paths and Suffix Tree nodes).
/// </remarks>
public record SuffixTreePath(IEnumerable<KeyValuePair<PrefixPath, SuffixTreeNode>> PathNodes)
{
    /// <summary>
    /// A readonly view of the private collection of path nodes.
    /// </summary>
    public IEnumerable<KeyValuePair<PrefixPath, SuffixTreeNode>> PathNodes { get; } = PathNodes.ToList().AsReadOnly();

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
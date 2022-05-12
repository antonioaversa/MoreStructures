namespace StringAlgorithms;

/// <summary>
/// A sequence of Suffix Tree nodes, where each node is child of its predecessor and parent of its successor.
/// </summary>
/// <param name="PathNodes">The sequence of Suffix nodes respecting the parent-child relationship.</param>
public record SuffixTreePath(IEnumerable<KeyValuePair<PrefixPath, SuffixTreeNode>> PathNodes)
{
    /// <summary>
    /// Append the provided path of Suffix Tree nodes to this path.
    /// </summary>
    /// <param name="path">The Suffix Tree path, whose nodes have to be appended.</param>
    /// <returns>A new Suffix Tree path, whose nodes are the concatenation of the nodes of the two paths.</returns>
    public SuffixTreePath Concat(SuffixTreePath path)
    {
        return new SuffixTreePath(PathNodes.Concat(path.PathNodes));
    }
}
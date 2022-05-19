using StringAlgorithms.SuffixStructures;
using StringAlgorithms.Utilities;

namespace StringAlgorithms.SuffixTrees;

/// <summary>
/// An immutable sequence of Suffix Tree nodes, where each node is child of its predecessor and parent of its successor.
/// </summary>
/// <param name="PathNodes">The sequence of nodes respecting the parent-child relationship.</param>
/// <remarks>
/// Immutability is guaranteed by using <see cref="ValueReadOnlyDictionary{TKey, TValue}"/>.
/// </remarks>
public record SuffixTreePath(IEnumerable<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> PathNodes)
    : ISuffixStructurePath<SuffixTreeEdge, SuffixTreeNode, SuffixTreePath, SuffixTreeBuilder>
{
    /// <inheritdoc/>
    public IEnumerable<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> PathNodes { get; } = 
        PathNodes.ToValueReadOnlyCollection();
}
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.Utilities;

namespace StringAlgorithms.SuffixTries;

/// <summary>
/// An immutable sequence of Suffix Trie nodes, where each node is child of its predecessor and parent of its successor.
/// </summary>
/// <param name="PathNodes">The sequence of nodes respecting the parent-child relationship.</param>
/// <remarks>
/// Immutability is guaranteed by using <see cref="ValueReadOnlyDictionary{TKey, TValue}"/>.
/// </remarks>
public record SuffixTriePath(IEnumerable<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>> PathNodes)
    : ISuffixStructurePath<SuffixTrieEdge, SuffixTrieNode, SuffixTriePath, SuffixTrieBuilder>
{
    /// <inheritdoc/>
    public IEnumerable<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>> PathNodes { get; } =
        PathNodes.ToValueReadOnlyCollection();
}
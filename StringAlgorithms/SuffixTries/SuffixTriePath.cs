using System.Text;

namespace StringAlgorithms.SuffixTries;

/// <summary>
/// An immutable sequence of Suffix Trie nodes, where each node is child of its predecessor and parent of its successor.
/// </summary>
/// <param name="PathNodes">The sequence of Suffix nodes respecting the parent-child relationship.</param>
/// <remarks>
/// Immutability is guaranteed by cloning the provided path nodes collection and exposing a readonly view, combined 
/// with the immutability of underlying data structures (Suffix Trie edges and nodes).
/// </remarks>
public record SuffixTriePath(IEnumerable<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>> PathNodes)
{
    /// <summary>
    /// An empty path, i.e. an empty sequence of Suffix Trie nodes.
    /// </summary>
    public static SuffixTriePath Empty() => 
        new(Enumerable.Empty<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>>());

    /// <summary>
    /// A Suffix Trie path composed of a single node with its incoming edge.
    /// </summary>
    /// <param name="edge">The Suffix Trie edge leading to the Suffix Trie node.</param>
    /// <param name="node">The Suffix Trie node defining the singleton path.</param>
    public static SuffixTriePath Singleton(SuffixTrieEdge edge, SuffixTrieNode node) =>
        new(Enumerable.Repeat(KeyValuePair.Create(edge, node), 1));

    /// <summary>
    /// A readonly view of the private collection of path nodes.
    /// </summary>
    public IEnumerable<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>> PathNodes { get; } = PathNodes.ToList().AsReadOnly();

    /// <summary>
    /// Append the provided path of Suffix Trie nodes to this path.
    /// </summary>
    /// <param name="path">The Suffix Trie path, whose nodes have to be appended.</param>
    /// <returns>A new Suffix Trie path, whose nodes are the concatenation of the nodes of the two paths.</returns>
    public SuffixTriePath Concat(SuffixTriePath path)
    {
        return new SuffixTriePath(PathNodes.Concat(path.PathNodes));
    }

    /// <summary>
    /// Calculate the suffix corresponding to this Suffix Trie path on the provided terminator-including text.
    /// </summary>
    /// <param name="text">The text, including the terminator character.</param>
    /// <returns>A string containing the suffix.</returns>
    public string SuffixFor(TextWithTerminator text) => PathNodes
        .Aggregate(new StringBuilder(), (acc, node) => acc.Append(text[node.Key]))
        .ToString();
}
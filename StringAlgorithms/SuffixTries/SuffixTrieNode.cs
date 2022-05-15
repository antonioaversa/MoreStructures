using System.Collections.ObjectModel;

namespace StringAlgorithms.SuffixTries;

/// <summary>
/// An immutable node of an immutable Suffix Trie, recursively pointing to its children node.
/// </summary>
/// <param name="Children">The collection of children for the node, indexed by a single chars.</param>
/// <param name="Start">
/// The index of the character, the path of Suffix Trie leading to this leaf starts with. Non-null for leaves only.
/// </param>
/// <remarks>
/// Immutability is guaranteed by copying the provided children collection and exposing a readonly view.
/// </remarks>
public abstract record SuffixTrieNode(IDictionary<SuffixTrieEdge, SuffixTrieNode> Children, int? Start)
{
    /// <summary>
    /// Builds an intermediate node, i.e. a node with children and their corresponding incoming edges.
    /// </summary>
    public record Intermediate(IDictionary<SuffixTrieEdge, SuffixTrieNode> Children)
        : SuffixTrieNode(Children, null)
    { }

    /// <summary>
    /// Builds a leaf, i.e. a node with no children and the start index of the suffix in the text.
    /// </summary>
    public record Leaf(int LeafStart)
        : SuffixTrieNode(new Dictionary<SuffixTrieEdge, SuffixTrieNode> { }, LeafStart)
    { }

    /// <summary>
    /// A readonly view of the children private collection of this Suffix Trie node. Empty for leaves.
    /// </summary>
    public IDictionary<SuffixTrieEdge, SuffixTrieNode> Children { get; }
        = (Children.Count > 0 && Start == null) || (Children.Count == 0 && Start != null)
        ? new ReadOnlyDictionary<SuffixTrieEdge, SuffixTrieNode>(
            new Dictionary<SuffixTrieEdge, SuffixTrieNode>(Children))
        : throw new ArgumentException($"Leafs needs to specificy a non-negative {nameof(Start)}.", nameof(Children));

    /// <summary>
    /// <inheritdoc cref="SuffixTrieNode(IDictionary{SuffixTrieEdge, SuffixTrieNode}, int?)" 
    /// path="/param[@name='Start']"/>
    /// </summary>
    public int? Start { get; init; }
        = (Children.Count > 0 && Start == null) || (Children.Count == 0 && Start != null)
        ? Start
        : throw new ArgumentException($"Leafs needs to specificy a non-negative {nameof(Start)}.", nameof(Children));

    /// <summary>
    /// Indexes into the children of this node, by edge, which is a single char selector.
    /// </summary>
    public SuffixTrieNode this[SuffixTrieEdge edge] => Children[edge];

    /// <summary>
    /// Whether this node is a leaf of the Suffix Trie (no children), or not.
    /// </summary>
    public bool IsLeaf => Children.Count == 0;

    /// <summary>
    /// Returns all Suffix Trie paths from this node to a leaf.
    /// </summary>
    /// <returns>A sequence of pairs of Suffix Trie edge and node.</returns>
    public IEnumerable<SuffixTriePath> GetAllNodeToLeafPaths()
    {
        foreach (var edgeAndChild in Children)
        {
            var childToLeafPaths = edgeAndChild.Value.GetAllNodeToLeafPaths();
            var edgeAndChildSingleton = Enumerable.Repeat(edgeAndChild, 1);
            if (childToLeafPaths.Any())
            {
                foreach (var childToLeafPath in childToLeafPaths)
                    yield return new SuffixTriePath(edgeAndChildSingleton).Concat(childToLeafPath);
            }
            else
            {
                yield return new SuffixTriePath(edgeAndChildSingleton);
            }
        }
    }

    /// <summary>
    /// Returns all suffixes for the provided text from this node down the Suffix Trie, up to leaves.
    /// </summary>
    /// <param name="text">The text with terminator, whose suffixes have to be extracted.</param>
    /// <returns>A sequence of strings, each one being a suffix.</returns>
    public IEnumerable<string> GetAllSuffixesFor(TextWithTerminator text) =>
        GetAllNodeToLeafPaths().Select(rootToLeafPath => rootToLeafPath.SuffixFor(text));   
}

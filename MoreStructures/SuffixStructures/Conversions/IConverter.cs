using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;

namespace MoreStructures.SuffixStructures.Conversions;

/// <summary>
/// A converter between different <see cref="ISuffixStructureNode{TEdge, TNode}"/> structures, 
/// such as <see cref="SuffixTrieNode"/> and <see cref="SuffixTreeNode"/>.
/// </summary>
public interface IConverter
{
    /// <summary>
    /// Converts the provided <see cref="SuffixTrieNode"/> instance into an equivalent instance of 
    /// <see cref="SuffixTreeNode"/>, building its entire structure.
    /// </summary>
    /// <param name="trieNode">The node identifying the trie structure to be converted.</param>
    /// <returns>A tree, equivalent to the provided trie.</returns>
    SuffixTreeNode TrieToTree(SuffixTrieNode trieNode);

    /// <summary>
    /// Converts the provided <see cref="SuffixTreeNode"/> instance into an equivalent instance of 
    /// <see cref="SuffixTrieNode"/>, building its entire structure.
    /// </summary>
    /// <param name="treeNode">The node identifying the trie structure to be converted.</param>
    /// <returns>A trie, equivalent to the provided tree.</returns>
    SuffixTrieNode TreeToTrie(SuffixTreeNode treeNode);
}

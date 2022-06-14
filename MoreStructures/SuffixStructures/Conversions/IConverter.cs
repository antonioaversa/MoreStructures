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
    /// <remarks>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     Time Complexity = O(n) and Space Complexity = O(n) where n = number of nodes in the input structure.
    ///     Each node of the input trie is visited at most twice and coalescing reduces the number of nodes.
    ///     </para>
    /// </remarks>
    SuffixTreeNode TrieToTree(SuffixTrieNode trieNode);

    /// <summary>
    /// Converts the provided <see cref="SuffixTreeNode"/> instance into an equivalent instance of 
    /// <see cref="SuffixTrieNode"/>, building its entire structure.
    /// </summary>
    /// <param name="treeNode">The node identifying the trie structure to be converted.</param>
    /// <returns>A trie, equivalent to the provided tree.</returns>
    /// <remarks>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     Time Complexity = O(n^2) and Space Complexity = O(n^2) where n = number of nodes in the input structure.
    ///     <br/>
    ///     Each node of the input tree is visited at most twice.
    ///     <br/>
    ///     However, expansion increase the number of nodes, in the worst case to the number of characters in all 
    ///     suffixes of the text which has generated the tree.
    ///     </para>
    /// </remarks>
    SuffixTrieNode TreeToTrie(SuffixTreeNode treeNode);
}

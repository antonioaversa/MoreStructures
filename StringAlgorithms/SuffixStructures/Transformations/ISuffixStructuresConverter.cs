using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries;

namespace StringAlgorithms.SuffixStructures.Transformations
{
    /// <summary>
    /// A converter between different <see cref="ISuffixStructureNode{TEdge, TNode, TPath, TBuilder}"/> structures, 
    /// such as <see cref="SuffixTrieNode"/> and <see cref="SuffixTreeNode"/>.
    /// </summary>
    public interface ISuffixStructuresConverter
    {
        /// <summary>
        /// Converts the provided <see cref="SuffixTrieNode"/> instance into an equivalent instance of 
        /// <see cref="SuffixTreeNode"/>, building its entire structure.
        /// </summary>
        /// <param name="trieNode">The node identifying the trie structure to be converted.</param>
        /// <returns>A Suffix Tree, equivalent to the provided trie.</returns>
        SuffixTreeNode TrieToTree(SuffixTrieNode trieNode);
    }
}

using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries;

namespace StringAlgorithms.SuffixStructures
{
    /// <summary>
    /// Conversion utilities between suffix data structures, such as Suffix Tree and Tries.
    /// </summary>
    public static class SuffixStructuresConverter
    {
        /// <summary>
        /// Converts the provided <see cref="SuffixTrieNode"/> into a <see cref="SuffixTreeNode"/>, building its
        /// entire structure.
        /// </summary>
        /// <param name="trieNode">The node identifying the trie structure to be converted.</param>
        /// <returns>A Suffix Tree.</returns>
        /// <exception cref="NotImplementedException"></exception>
        [Obsolete("Not implemented yet")] 
        public static SuffixTreeNode TrieToTree(SuffixTrieNode trieNode)
        {
            throw new NotImplementedException();
        }
    }
}
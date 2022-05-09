
namespace StringAlgorithms
{
    /// <summary>
    /// The index key of the collection of children of a Suffix Tree node, which identifies a substring in text, used 
    /// as a selector to navigate the Suffix Tree.
    /// </summary>
    /// <param name="Start">The index of the first character of the prefix path in the text.</param>
    /// <param name="Length">The length of the prefix path.</param>
    public record PrefixPath(int Start, int Length)
    {
    }

    /// <summary>
    /// A node of a Suffix Tree, recursively pointing to its children node.
    /// </summary>
    /// <param name="Children">The children of the node, indexed by prefix paths. Empty collection for leaves.</param>
    public record SuffixTreeNode(IDictionary<PrefixPath, SuffixTreeNode> Children)
    {
        /// <summary>
        /// Indexes into the children of this node, by prefix path.
        /// </summary>
        public SuffixTreeNode this[PrefixPath prefixPath] => Children[prefixPath];

        /// <summary>
        /// Whether this node is a leaf of the Suffix Tree (no children), or not.
        /// </summary>
        public bool IsLeaf => Children.Count == 0;
    }

    /// <summary>
    /// Exposes utility methods to build Suffix Trees, such as <see cref="Build(string, char)"/>.
    /// </summary>
    public static class SuffixTreeBuilder
    {
        /// <summary>
        /// The special character used as a default terminator for the text to build the Suffix Tree of, when no custom 
        /// terminator is specified. Should not be present in the text.
        /// </summary>
        public const char DefaultTerminator = '$';

        /// <summary>
        /// Build a Suffix Tree of the provided text, which is a n-ary search tree in which edges coming out of a node
        /// are substrings of text which identify prefixes shared by all paths to leaves, starting from the node.
        /// </summary>
        /// <param name="text">The text to build the Suffix Tree of.</param>
        /// <param name="terminator">A special character used as string terminator, not present in text.</param>
        /// <returns>The root node of the Suffix Tree.</returns>
        /// <remarks>
        /// Substrings of text are identified by their start position in text and their length.
        /// </remarks>
        public static SuffixTreeNode Build(string text, char terminator = DefaultTerminator)
        {
            if (text.Contains(terminator))
                throw new ArgumentException($"{nameof(terminator)} shouldn't be included in {nameof(text)}");

            text += terminator;
            var root = new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode> { });

            for (var currentSuffixBeginIndex = 0; currentSuffixBeginIndex < text.Length; currentSuffixBeginIndex++)
                IncludeSuffixIntoTree(text, currentSuffixBeginIndex, root);

            return root;
        }

        private static void IncludeSuffixIntoTree(string text, int suffixBeginIndex, SuffixTreeNode root)
        {
            var currentNode = root;
            var prefixPathWithTheSameFirstChar = currentNode.Children.Keys.SingleOrDefault(
                prefixPath => text[prefixPath.Start] == text[suffixBeginIndex]);

            if (prefixPathWithTheSameFirstChar == null)
            {
                var prefixPath = new PrefixPath(suffixBeginIndex, text.Length - suffixBeginIndex);
                currentNode.Children[prefixPath] =
                    new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode> { });
            }
            else
            {
                // Compare text[currentSuffixBeginIndex, ...] and text[prefixPathWithTheSameFirstChar.Start, ...] for
                // longest prefix in common

                //if (currentSuffixBeginIndex < )

                //currentNode = currentNode.Children[prefixPathWithTheSameFirstChar]
            }
        }
    }
}
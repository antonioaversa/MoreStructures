using static StringAlgorithms.StringUtilities;

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
        /// Builds a Suffix Tree leaf, i.e. a node with no children.
        /// </summary>
        public SuffixTreeNode()
            : this(new Dictionary<PrefixPath, SuffixTreeNode> { }) 
        { 
        }

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
            var root = new SuffixTreeNode();

            for (var currentSuffixBeginIndex = 0; currentSuffixBeginIndex < text.Length; currentSuffixBeginIndex++)
                IncludeSuffixIntoTree(text, currentSuffixBeginIndex, root);

            return root;
        }

        private static void IncludeSuffixIntoTree(string text, int suffixBeginIndex, SuffixTreeNode root)
        {
            var currentNode = root;
            var prefixPathSame1stChar = currentNode.Children.Keys.SingleOrDefault(
                prefixPath => text[prefixPath.Start] == text[suffixBeginIndex]);

            if (prefixPathSame1stChar == null)
            {
                var prefixPath = new PrefixPath(suffixBeginIndex, text.Length - suffixBeginIndex);
                currentNode.Children[prefixPath] = new SuffixTreeNode(); 
            }
            else
            {
                // Compare text[suffixBeginIndex, ...] and text[prefixPathWithTheSameFirstChar.Start, ...] for longest
                // prefix in common. If the prefix in common is shorter than the prefix path with the same first char,
                // create an intermediate node, push down the child pointed by the prefix path in the current node and
                // add a new node for the reminder of text[suffixBeginIndex, ...] as second child of the intermediate.
                // Otherwise, eat prefixLength chars from the prefix path, move to the child pointed by the prefix path
                // entirely matching the beginning of the current suffix and repeat the same operation.

                var prefixLength = LongestPrefixInCommon(
                    text[suffixBeginIndex..], 
                    text.Substring(prefixPathSame1stChar.Start, prefixPathSame1stChar.Length));

                if (prefixLength < prefixPathSame1stChar.Length)
                {
                    var oldChild = currentNode[prefixPathSame1stChar];
                    var newLeaf = new SuffixTreeNode();
                    var newIntermediate = new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode>() 
                    {
                        [new (
                            prefixPathSame1stChar.Start + prefixLength, 
                            prefixPathSame1stChar.Length - prefixLength)] = oldChild,
                        [new (
                            suffixBeginIndex + prefixLength, 
                            text.Length - suffixBeginIndex - prefixLength)] = newLeaf,
                    });
                    currentNode.Children.Remove(prefixPathSame1stChar);
                    currentNode.Children[new (prefixPathSame1stChar.Start, prefixLength)] = newIntermediate;
                }
                else
                {
                    IncludeSuffixIntoTree(text, suffixBeginIndex + prefixLength, currentNode[prefixPathSame1stChar]);
                }
            }
        }
    }
}
namespace StringAlgorithms
{
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

        /// <summary>
        /// Returns all Suffix Tree paths from this node to a leaf.
        /// </summary>
        /// <returns>A sequence of pairs of prefix path and Suffix Tree nodes.</returns>
        public IEnumerable<IEnumerable<KeyValuePair<PrefixPath, SuffixTreeNode>>> GetAllNodeToLeafPaths()
        {
            foreach (var prefixPathAndChild in Children)
            {
                var childToLeafPaths = prefixPathAndChild.Value.GetAllNodeToLeafPaths();
                if (childToLeafPaths.Any())
                {
                    foreach (var childToLeafPath in childToLeafPaths)
                        yield return Enumerable.Repeat(prefixPathAndChild, 1).Concat(childToLeafPath);
                }
                else
                {
                    yield return Enumerable.Repeat(prefixPathAndChild, 1);
                }
            }
        }
    }
}
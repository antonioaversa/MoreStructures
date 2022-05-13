using static StringAlgorithms.StringUtilities;

namespace StringAlgorithms.SuffixTrees;

/// <summary>
/// Exposes utility methods to build Suffix Trees, such as <see cref="Build(TextWithTerminator)"/>.
/// </summary>
public static class SuffixTreeBuilder
{
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
    public static SuffixTreeNode Build(TextWithTerminator text)
    {
        var root = new SuffixTreeNode(0);

        for (var suffixIndex = 0; suffixIndex < text.ToString().Length; suffixIndex++)
            root = IncludeSuffixIntoTree(text, suffixIndex, suffixIndex, root);

        return root;
    }

    public static SuffixTreeNode Build(string text) => Build(new TextWithTerminator(text));

    private static SuffixTreeNode IncludeSuffixIntoTree(
        TextWithTerminator text, int suffixCurrentIndex, int suffixIndex, SuffixTreeNode node)
    {
        var nodeChildren = new Dictionary<PrefixPath, SuffixTreeNode>(node.Children);
        var prefixPathSame1stChar = nodeChildren.Keys.SingleOrDefault(
            prefixPath => text.AsString[prefixPath.Start] == text.AsString[suffixCurrentIndex]);

        if (prefixPathSame1stChar == null)
        {
            var prefixPath = new PrefixPath(suffixCurrentIndex, text.AsString.Length - suffixCurrentIndex);
            nodeChildren[prefixPath] = new SuffixTreeNode(suffixIndex);
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
                text.AsString[suffixCurrentIndex..], prefixPathSame1stChar.Of(text));

            var oldChild = nodeChildren[prefixPathSame1stChar];
            if (prefixLength < prefixPathSame1stChar.Length)
            {
                var newLeaf = new SuffixTreeNode(suffixIndex);
                var newIntermediate = new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode>()
                {
                    [new(
                        prefixPathSame1stChar.Start + prefixLength,
                        prefixPathSame1stChar.Length - prefixLength)] = oldChild,
                    [new(
                        suffixCurrentIndex + prefixLength,
                        text.AsString.Length - suffixCurrentIndex - prefixLength)] = newLeaf,
                });
                nodeChildren.Remove(prefixPathSame1stChar);
                nodeChildren[new(prefixPathSame1stChar.Start, prefixLength)] = newIntermediate;
            }
            else
            {
                var newChild = IncludeSuffixIntoTree(text, suffixCurrentIndex + prefixLength, suffixIndex, oldChild);
                nodeChildren.Remove(prefixPathSame1stChar);
                nodeChildren[prefixPathSame1stChar] = newChild;
            }

            return new SuffixTreeNode(nodeChildren);
        }

        return new SuffixTreeNode(nodeChildren);
    }
}
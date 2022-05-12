using static StringAlgorithms.StringUtilities;

namespace StringAlgorithms;

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
        var root = new SuffixTreeNode();

        for (var suffixBeginIndex = 0; suffixBeginIndex < text.ToString().Length; suffixBeginIndex++)
            root = IncludeSuffixIntoTree(text, suffixBeginIndex, root);

        return root;
    }

    public static SuffixTreeNode Build(string text) => Build(new TextWithTerminator(text));

    private static SuffixTreeNode IncludeSuffixIntoTree(
        TextWithTerminator text, int suffixBeginIndex, SuffixTreeNode node)
    {
        var nodeChildren = new Dictionary<PrefixPath, SuffixTreeNode>(node.Children);
        var prefixPathSame1stChar = nodeChildren.Keys.SingleOrDefault(
            prefixPath => text.AsString[prefixPath.Start] == text.AsString[suffixBeginIndex]);

        if (prefixPathSame1stChar == null)
        {
            var prefixPath = new PrefixPath(suffixBeginIndex, text.AsString.Length - suffixBeginIndex);
            nodeChildren[prefixPath] = new SuffixTreeNode();
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
                text.AsString[suffixBeginIndex..], prefixPathSame1stChar.Of(text));

            var oldChild = nodeChildren[prefixPathSame1stChar];
            if (prefixLength < prefixPathSame1stChar.Length)
            {
                var newLeaf = new SuffixTreeNode();
                var newIntermediate = new SuffixTreeNode(new Dictionary<PrefixPath, SuffixTreeNode>()
                {
                    [new(
                        prefixPathSame1stChar.Start + prefixLength,
                        prefixPathSame1stChar.Length - prefixLength)] = oldChild,
                    [new(
                        suffixBeginIndex + prefixLength,
                        text.AsString.Length - suffixBeginIndex - prefixLength)] = newLeaf,
                });
                nodeChildren.Remove(prefixPathSame1stChar);
                nodeChildren[new(prefixPathSame1stChar.Start, prefixLength)] = newIntermediate;
            }
            else
            {
                var newChild = IncludeSuffixIntoTree(text, suffixBeginIndex + prefixLength, oldChild);
                nodeChildren.Remove(prefixPathSame1stChar);
                nodeChildren[prefixPathSame1stChar] = newChild;
            }

            return new SuffixTreeNode(nodeChildren);
        }

        return new SuffixTreeNode(nodeChildren);
    }
}
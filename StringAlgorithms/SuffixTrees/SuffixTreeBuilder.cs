using StringAlgorithms.SuffixStructures;
using static StringAlgorithms.StringUtilities;

namespace StringAlgorithms.SuffixTrees;

public class SuffixTreeBuilder
    : ISuffixStructureBuilder<SuffixTreeEdge, SuffixTreeNode, SuffixTreePath, SuffixTreeBuilder>
{
    public SuffixTreePath EmptyPath() =>
        new (Enumerable.Empty<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>>());

    public SuffixTreePath SingletonPath(SuffixTreeEdge edge, SuffixTreeNode node) =>
        new(Enumerable.Repeat(KeyValuePair.Create(edge, node), 1));

    public SuffixTreePath MultistepsPath(params (SuffixTreeEdge edge, SuffixTreeNode node)[] pathNodes) =>
        new(pathNodes.Select(pathNode => KeyValuePair.Create(pathNode.edge, pathNode.node)));

    public SuffixTreePath MultistepsPath(IEnumerable<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> pathNodes) =>
        new(pathNodes);

    /// <summary>
    /// Build a Suffix Tree of the provided text, which is a n-ary search tree in which edges coming out of a node
    /// are substrings of text which identify edges shared by all paths to leaves, starting from the node.
    /// </summary>
    /// <param name="text">The text to build the Suffix Tree of, with its terminator (required for traversal).</param>
    /// <returns>The root node of the Suffix Tree.</returns>
    /// <remarks>
    /// Substrings of text are identified by their start position in text and their length.
    /// </remarks>
    public static SuffixTreeNode Build(TextWithTerminator text)
    {
        SuffixTreeNode root = new SuffixTreeNode.Leaf(0);

        for (var suffixIndex = 0; suffixIndex < text.Length; suffixIndex++)
            root = IncludeSuffixIntoTree(text, suffixIndex, suffixIndex, root);

        return root;
    }

    /// <summary><inheritdoc cref="Build(TextWithTerminator)"/></summary>
    /// <param name="text">The text to build the Suffix Tree of, without any terminator (automatically added).</param>
    /// <returns><inheritdoc cref="Build(TextWithTerminator)"/></returns>
    public static SuffixTreeNode Build(string text) => Build(new TextWithTerminator(text));

    private static SuffixTreeNode.Intermediate IncludeSuffixIntoTree(
        TextWithTerminator text, int suffixCurrentIndex, int suffixIndex, SuffixTreeNode node)
    {
        var nodeChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode>(node.Children);
        var edgeSame1stChar = nodeChildren.Keys.SingleOrDefault(
            edge => text[edge.Start] == text[suffixCurrentIndex]);

        if (edgeSame1stChar == null)
        {
            var edge = new SuffixTreeEdge(suffixCurrentIndex, text.Length - suffixCurrentIndex);
            nodeChildren[edge] = new SuffixTreeNode.Leaf(suffixIndex);
        }
        else
        {
            // Compare text[suffixCurrentIndex, ...] and text[edgeSame1stChar.Start, ...] for longest edge in common.
            // If the prefix in common is shorter than the edge with the same first char, create an intermediate node,
            // push down the child pointed by the edge in the current node and add a new node for the reminder of
            // text[suffixCurrentIndex, ...] as second child of the intermediate.
            // Otherwise, eat prefixLength chars from the edge, move to the child pointed by the edge entirely matching
            // the beginning of the current suffix and repeat the same operation.

            var prefixLength = LongestPrefixInCommon(
                text[suffixCurrentIndex..], edgeSame1stChar.Of(text));

            var oldChild = nodeChildren[edgeSame1stChar];
            if (prefixLength < edgeSame1stChar.Length)
            {
                var newLeaf = new SuffixTreeNode.Leaf(suffixIndex);
                var newIntermediate = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>()
                {
                    [new(
                        edgeSame1stChar.Start + prefixLength,
                        edgeSame1stChar.Length - prefixLength)] = oldChild,
                    [new(
                        suffixCurrentIndex + prefixLength,
                        text.Length - suffixCurrentIndex - prefixLength)] = newLeaf,
                });
                nodeChildren.Remove(edgeSame1stChar);
                nodeChildren[new(edgeSame1stChar.Start, prefixLength)] = newIntermediate;
            }
            else
            {
                var newChild = IncludeSuffixIntoTree(text, suffixCurrentIndex + prefixLength, suffixIndex, oldChild);
                nodeChildren.Remove(edgeSame1stChar);
                nodeChildren[edgeSame1stChar] = newChild;
            }
        }

        return new SuffixTreeNode.Intermediate(nodeChildren);
    }
}

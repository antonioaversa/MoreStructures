﻿using static StringAlgorithms.StringUtilities;

namespace StringAlgorithms.SuffixTrees;

/// <summary>
/// Exposes utility methods to build Suffix Trees, such as <see cref="Build(TextWithTerminator)"/>.
/// </summary>
public static class SuffixTreeBuilder
{
    /// <summary>
    /// Build a Suffix Tree of the provided text, which is a n-ary search tree in which edges coming out of a node
    /// are substrings of text which identify edges shared by all paths to leaves, starting from the node.
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
        var nodeChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode>(node.Children);
        var edgeSame1stChar = nodeChildren.Keys.SingleOrDefault(
            edge => text.AsString[edge.Start] == text.AsString[suffixCurrentIndex]);

        if (edgeSame1stChar == null)
        {
            var edge = new SuffixTreeEdge(suffixCurrentIndex, text.AsString.Length - suffixCurrentIndex);
            nodeChildren[edge] = new SuffixTreeNode(suffixIndex);
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
                text.AsString[suffixCurrentIndex..], edgeSame1stChar.Of(text));

            var oldChild = nodeChildren[edgeSame1stChar];
            if (prefixLength < edgeSame1stChar.Length)
            {
                var newLeaf = new SuffixTreeNode(suffixIndex);
                var newIntermediate = new SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode>()
                {
                    [new(
                        edgeSame1stChar.Start + prefixLength,
                        edgeSame1stChar.Length - prefixLength)] = oldChild,
                    [new(
                        suffixCurrentIndex + prefixLength,
                        text.AsString.Length - suffixCurrentIndex - prefixLength)] = newLeaf,
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

            return new SuffixTreeNode(nodeChildren);
        }

        return new SuffixTreeNode(nodeChildren);
    }
}
using MoreStructures.SuffixStructures.Builders;
using static MoreStructures.Utilities.StringUtilities;

namespace MoreStructures.SuffixTrees.Builders;

/// <summary>
/// Builds objects, such as edges and nodes, for <see cref="SuffixTreeNode"/> structures.
/// </summary>
/// <remarks>
///     <para>
///     Implemented as an iteration of recursive visit of the tree being built, with as many iterations as the number 
///     of suffix of the input (where the longest suffix is the text itself) and one level of recursion per char of 
///     each suffix. 
///     </para>
///     <para>
///     Limited by call stack depth and usable with input text of a "reasonable" length (i.e. string having a length 
///     &lt; ~1K chars).
///     </para>
///     <para id="complexity">
///     Time Complexity = O(t^2 * as) and Space Complexity = O(t) where t = length of the text to match and
///     as = size of the alphabet of the text. If the alphabet is of constant size, Time Complexity is quadratic.
///         <para>
///         Compared to tries, trees are more compact due to edge coalescing and edge label compression (i.e. edge 
///         strings stored as pair (start, length), rather than as a substring of length chars). Each recursion add a 
///         leaf and at most one intermediate node, so Space Complexity ~ 2 * t = O(t). 
///         </para>
///     </para>
/// </remarks>
public class NaivePartiallyRecursiveSuffixTreeBuilder
    : IBuilder<SuffixTreeEdge, SuffixTreeNode>
{
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="NaivePartiallyRecursiveSuffixTreeBuilder" path="/remarks"/>
    /// </remarks>
    public SuffixTreeNode BuildTree(TextWithTerminator text)
    {
        SuffixTreeNode root = new SuffixTreeNode.Leaf(0);

        for (var suffixIndex = 0; suffixIndex < text.Length; suffixIndex++)
            root = IncludeSuffixIntoTree(text, suffixIndex, suffixIndex, root);

        return root;
    }

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

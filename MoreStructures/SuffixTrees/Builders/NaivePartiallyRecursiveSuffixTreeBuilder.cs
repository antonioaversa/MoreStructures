using MoreStructures.SuffixStructures.Builders;
using static MoreStructures.Utilities.StringUtilities;

namespace MoreStructures.SuffixTrees.Builders;

/// <summary>
/// Builds objects, such as edges and nodes, for <see cref="SuffixTreeNode"/> structures.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Implemented as an iteration of recursive visit of the tree being built, with as many iterations as the number 
///       of suffixes of the input (where the longest suffix is the text itself) and one level of recursion per char of 
///       each suffix. 
///       <br/>
///     - Limited by call stack depth and usable with input text of a "reasonable" length (i.e. string having a length 
///       &lt; ~1K chars).
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - For each suffix S of the input T, start from the root node N of the tree, initialized as a simple leaf.
///       <br/>
///     - Compare the first char of S with the first char of each of the edges coming from N.
///       <br/>
///     - If there is no edge e such that <c>label(e)[0] == S[0]</c>, it means that there is no descendant of N sharing
///       a path with S. So, create a new leaf under N, attached by an edge with a label equal to S.
///       <br/>
///     - Otherwise, it means that there is a path to be shared between the new leaf and at least one of the 
///       descendants of N.
///       <br/>
///     - In this case, compare the current suffix and the label of such edge e, for the LCP.
///       <br/>
///     - If the prefix in common is shorter than the length of the label of the edge e, create an intermediate node, 
///       push down the child pointed by the edge in the current node and add a new node for the reminder of the suffix
///       (up to next terminator) as second child of the intermediate.
///       <br/>
///     - Otherwise, eat prefixLength chars from the edge, move to the child pointed by the edge entirely matching the 
///       beginning of the current suffix and repeat the same operation.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - The generation of the full text, done by 
///       <see cref="TextWithTerminatorExtensions.GenerateFullText(MoreStructures.TextWithTerminator[])"/>, is a O(n)
///       operation, where n is the sum of the length of each <see cref="TextWithTerminator"/> in the input 
///       (terminators included).
///       <br/>
///     - The initial tree, just a root-leaf, is created in constant time.
///       <br/>
///     - The top level loop is executed n times, once per char of the full text.
///       <br/>
///     - Duplicating the node children dictionary, as well as finding the edge with the same first char, are
///       O(avgEdges), where avgEdges is the average number of edges coming out of a node of the tree.
///       <br/>
///     - Over all the n nodes of the tree, taking into account the worst case where avgEdges is O(n), Time Complexity
///       would become O(n^2). However, the total number of edges in a tree is O(n), so the overall cost of these 
///       operations for the entire tree is O(n), and not O(n^2).
///       <br/>
///     - Finding the first index in the suffix which corresponds to a terminator, finding the LCP between the suffix
///       and the label of the current edge and finding the number of chars up to the terminator are all O(n) 
///       operations, since the length of a generic suffix of the text is O(n) and they all require iterating over all
///       the chars.
///       <br/>
///     - In conclusion, Time Complexity = O(n^2 * as) and Space Complexity = O(n) where n = length of the text to 
///       match and as = size of the alphabet of the text. If the alphabet is of constant size, Time Complexity is 
///       quadratic.
///       <br/>
///     - Compared to tries, trees are more compact due to edge coalescing and edge label compression (i.e. edge 
///       strings stored as pair (start, length), rather than as a substring of length chars). Each recursion add a 
///       leaf and at most one intermediate node, so Space Complexity ~ 2 * n = O(n).
///     </para>
/// </remarks>
public class NaivePartiallyRecursiveSuffixTreeBuilder
    : IBuilder<SuffixTreeEdge, SuffixTreeNode>
{
    /// <inheritdoc path="//*[not self::summary or self::remarks]"/>
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="NaivePartiallyRecursiveSuffixTreeBuilder" path="/remarks"/>
    /// </remarks>
    public SuffixTreeNode BuildTree(params TextWithTerminator[] texts)
    {
        var (fullText, terminators) = texts.GenerateFullText();

        SuffixTreeNode root = new SuffixTreeNode.Leaf(0);

        for (var suffixIndex = 0; suffixIndex < fullText.Length; suffixIndex++)
            root = IncludeSuffixIntoTree(fullText, terminators, suffixIndex, suffixIndex, root);

        return root;
    }

    private static SuffixTreeNode.Intermediate IncludeSuffixIntoTree(
        TextWithTerminator text, ISet<char> terminators, int suffixCurrentIndex, int suffixIndex, SuffixTreeNode node)
    {
        var nodeChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode>(node.Children);
        var currentChar = text[suffixCurrentIndex];
        var edgeSame1stChar = nodeChildren.Keys.SingleOrDefault(edge => text[edge.Start] == currentChar);

        if (edgeSame1stChar == null)
        {
            // Find first index which corresponds to a terminator
            var index1stTerminator = Enumerable
                .Range(suffixCurrentIndex, text.Length - suffixCurrentIndex)
                .First(i => terminators.Contains(text[i]));

            var edge = new SuffixTreeEdge(suffixCurrentIndex, index1stTerminator - suffixCurrentIndex + 1);
            nodeChildren[edge] = new SuffixTreeNode.Leaf(suffixIndex);
        }
        else
        {
            var prefixLength = LongestCommonPrefix(text[suffixCurrentIndex..], edgeSame1stChar.Of(text));

            var oldChild = nodeChildren[edgeSame1stChar];
            if (prefixLength < edgeSame1stChar.Length)
            {
                var charsToNextSeparator = Enumerable
                    .Range(0, text.Length - suffixCurrentIndex - prefixLength)
                    .First(i => terminators.Contains(text[suffixCurrentIndex + prefixLength + i]));
                
                var newLeaf = new SuffixTreeNode.Leaf(suffixIndex);
                var newIntermediate = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>()
                {
                    [new(
                        edgeSame1stChar.Start + prefixLength,
                        edgeSame1stChar.Length - prefixLength)] = oldChild,
                    [new(
                        suffixCurrentIndex + prefixLength,
                        charsToNextSeparator + 1)] = newLeaf,
                });
                nodeChildren.Remove(edgeSame1stChar);
                nodeChildren[new(edgeSame1stChar.Start, prefixLength)] = newIntermediate;
            }
            else
            {
                var newChild = IncludeSuffixIntoTree(
                    text, terminators, suffixCurrentIndex + prefixLength, suffixIndex, oldChild);
                nodeChildren.Remove(edgeSame1stChar);
                nodeChildren[edgeSame1stChar] = newChild;
            }
        }

        return new SuffixTreeNode.Intermediate(nodeChildren);
    }
}

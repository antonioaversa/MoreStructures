using MoreStructures.MutableTrees;
using MoreStructures.SuffixStructures.Builders;
using static MoreStructures.Utilities.StringUtilities;
using static MoreStructures.MutableTrees.MutableTree;

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
///     - Finding the edge with the same first char is O(avgEdges), where avgEdges is the average number of edges 
///       coming out of a node of the tree. This operation is done for all cases (leaf creation alone or leaf + 
///       intermediate node).
///       <br/>
///     - Over all the n nodes of the tree, taking into account the worst case where avgEdges is O(n), Time Complexity
///       would become O(n^2). However, the total number of edges in a tree is O(n), so the overall cost of these 
///       operations for the entire tree is O(n), and not O(n^2).
///       <br/>
///     - Finding the LCP between the suffix and the label of the current edge are O(n) operations, since the length 
///       of a generic suffix of the text is O(n) and they require iterating over all the chars.
///       <br/>
///     - Such operations are only done when an intermediate node is required. However, in the worst case there are as
///       many intermediate as leaves in the tree (i.e. each iteration adds an intermediate node and a leaf). 
///       Therefore, the number of intermediate nodes is O(n), and the two O(n) operations above are to be repeated 
///       O(n) times.
///       <br/>
///     - In conclusion, Time Complexity = O(n^2) and Space Complexity = O(n) where n = length of the text to match.
///       <br/>
///     - Compared to tries, trees are more compact due to edge coalescing and edge label compression (i.e. edge 
///       strings stored as pair (start, length), rather than as a substring of length chars). Each recursion add a 
///       leaf and at most one intermediate node, so Space Complexity ~ 2 * n = O(n).
///     </para>
/// </remarks>
public class NaivePartiallyRecursiveSuffixTreeBuilder
    : IBuilder<SuffixTreeEdge, SuffixTreeNode>
{
    private static readonly MutableTrees.Conversions.IConversion MutableTreeConversion =
        new MutableTrees.Conversions.FullyIterativeConversion();

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

        var root = Node.BuildRoot();

        for (var suffixIndex = 0; suffixIndex < fullText.Length; suffixIndex++)
            IncludeSuffixIntoTree(fullText, terminators, suffixIndex, suffixIndex, root);

        return MutableTreeConversion.ConvertToSuffixTree(root, fullText, terminators);
    }

    private static void IncludeSuffixIntoTree(
        TextWithTerminator text, ISet<char> terminators, int suffixCurrentIndex, int suffixIndex, Node node)
    {
        var currentChar = text[suffixCurrentIndex];
        var edgeSame1stChar = node.Children.Keys.SingleOrDefault(edge => text[edge.Start] == currentChar);

        if (edgeSame1stChar == null)
        {
            var edgeToNewLeaf = Edge.Build(suffixCurrentIndex, text.Length - suffixCurrentIndex);
            var newLeaf = Node.Build(node, edgeToNewLeaf, suffixIndex);

            node.Children[edgeToNewLeaf] = newLeaf;
        }
        else
        {
            var prefixLength = LongestCommonPrefix(
                text[suffixCurrentIndex..], 
                text[edgeSame1stChar.Start..(edgeSame1stChar.Start + edgeSame1stChar.Length)]);

            var oldChild = node.Children[edgeSame1stChar];
            if (prefixLength < edgeSame1stChar.Length)
            {
                var edgeToNewIntermediate = Edge.Build(edgeSame1stChar.Start, prefixLength);
                var newIntermediate = Node.Build(node, edgeToNewIntermediate, null);
                
                var edgeToOldChild = Edge.Build(
                    edgeSame1stChar.Start + prefixLength, edgeSame1stChar.Length - prefixLength);
                var edgeToNewLeaf = Edge.Build(
                    suffixCurrentIndex + prefixLength, text.Length - suffixCurrentIndex - prefixLength);
                var newLeaf = Node.Build(
                    newIntermediate, edgeToNewLeaf, suffixIndex);

                newIntermediate.Children[edgeToOldChild] = oldChild;
                newIntermediate.Children[edgeToNewLeaf] = newLeaf;

                node.Children.Remove(edgeSame1stChar);
                node.Children[edgeToNewIntermediate] = newIntermediate;
            }
            else
            {
                IncludeSuffixIntoTree(text, terminators, suffixCurrentIndex + prefixLength, suffixIndex, oldChild);
            }
        }
    }
}

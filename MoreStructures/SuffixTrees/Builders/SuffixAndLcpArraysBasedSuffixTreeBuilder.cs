using MoreLinq;
using MoreStructures.MutableTrees;
using MoreStructures.SuffixArrays;
using MoreStructures.SuffixArrays.LongestCommonPrefix;
using MoreStructures.SuffixStructures.Builders;

namespace MoreStructures.SuffixTrees.Builders;

/// <summary>
/// A <see cref="IBuilder{TEdge, TNode}"/> implementation of <see cref="SuffixTreeNode"/> structures, using the
/// <see cref="SuffixArray"/> and the <see cref="LcpArray"/> of the full text, to build the tree efficiently.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Like <see cref="NaivePartiallyRecursiveSuffixTreeBuilder"/> and <see cref="UkkonenSuffixTreeBuilder"/>,
///       it builds the tree one suffix of the text at a time, adding at least a leaf and potentially an intermediate 
///       node at every iteration. 
///       <br/>
///     - Unlike them, the suffixes are not added to the tree in the order in which they appear in the text, i.e. they
///       are not processed from the longest to the shortest, rather from the smallest to the biggest in lexicographic
///       ascending order.
///       <br/>
///     - It also requires the construction of the <see cref="SuffixArray"/> and the <see cref="LcpArray"/> of the 
///       input, whereas the naive and the Ukkonen implementations don't require any auxiliary structure.
///       <br/>
///     - Compared to the naive implementation, it has better runtime, even including the cost of building auxiliary
///       structures: all-round performance is less than the quadratic performance of the naive approach, but worse
///       than the linear performance of the Ukkonen algorithm.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - First, build full text, from the provided <see cref="TextWithTerminator"/> instances, and the corresponding 
///       <see cref="SuffixArray"/> SA and <see cref="LcpArray"/> LCPA, which are both fully enumerated in order to be 
///       able to direct access them by index.
///       <br/>
///     - Then, build the initial mutable tree: just the root node with a single leaf linked by an edge labelled with 
///       the 1st suffix in the <see cref="SuffixArray"/>: <c>SA[0]</c>.
///       <br/>
///     - This represents the initial state of the mutable tree, which will be modified iteration by iteration going 
///       through the Suffix Array from the 2nd to the last suffix (in lexicographic ascending order).
///       <br/>
///     - The item of the LCP Array in position i - 1, gives the LCP of such suffix with the previous suffix: 
///       <c>LCPA[i - 1] = LCP(SA[i - 1], SA[i])</c>. Such LCP value is compared with the LCP value calculated at the
///       previous iteration.
///       <br/>
///     - Different insertion strategies are applied, depending on the result of the comparison. At each iteration the 
///       reference to the last inserted leaf and to the previous LCP value are kept.
///       <br/>
///     - <b>Case 1</b>: if the current LCP is smaller than the previous, the root-to-leaf path of the leaf about to be
///       added shares a shorter path with the previous leaf. So move up in the tree from the last inserted leaf, up 
///       until the first node which makes current LCP value non smaller than the previous LCP value. Then check 
///       whether it's fallbacking to Case 2 or 3.
///       <br/>
///     - <b>Case 2</b>: if the current LCP is the same as the previous, the root-to-leaf path of the leaf about to be
///       added shares exactly the same path with the previous leaf. Therefore the new leaf can be attached directly
///       to the parent of the previous leaf, which becomes a sibling of the new leaf. No new intermediate node.
///       <br/>
///     - <b>Case 3</b>: if the current LCP is bigger than the previous, a part of the previous edge is in common 
///       between the previous leaf and the leaf about to be created in this iteration. Therefore, build new 
///       intermediate, detach old edge going to previous leaf, attach new intermediate edge.
///       <br/>
///     - Finally, after all iterations have been executed and all suffixes of the text have been included in the 
///       mutable tree, build the final <see cref="SuffixTreeNode"/> structure from the mutable tree, using a
///       <see cref="MutableTrees.Conversions.FullyIterativeConversion"/>.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - The complexity of building the <see cref="SuffixArray"/> and the <see cref="LcpArray"/>, via the provided 
///       <see cref="SuffixAndLcpArraysBuilder"/> is not included in this cost analysis. This has to be taken into 
///       account when comparing this implementation of <see cref="IBuilder{TEdge, TNode}"/> of 
///       <see cref="SuffixTreeNode"/> structures with other implementations, such as 
///       <see cref="NaivePartiallyRecursiveSuffixTreeBuilder"/> or <see cref="UkkonenSuffixTreeBuilder"/>.
///       <br/>
///     - Building the initial mutable tree requires building two nodes and an edge, which can both be done in constant
///       time.
///       <br/>
///     - Then, n - 1 iterations are performed, where n is the length of the input, executing optionally Case 1 and 
///       then either Case 2 or 3.
///       <br/>
///     - Cases 2 and 3 perform the insertion of a leaf and potentially of an intermediate node, both O(1) operations.
///       Case 1 may seem an O(n) operation. However, due to LCP Array property, moving up in the tree will happen at
///       most once per iteration.
///       <br/>
///     - Final step is building the final tree from the mutable tree, which requires the traversal of the O(n) nodes 
///       of the tree.
///       <br/>
///     - Tree traversal is done iteratively via a stack, where a node is visited at most twice, and each frame stack
///       is processed in constant time. The stack then contains at most 2 * n frames, each of constant size.
///       <br/>
///     - That makes final tree construction a O(n) operation, both in time and space.
///       <br/>
///     - In conclusion, both Time and Space Complexity are O(n).
///     </para>
/// </remarks>
public class SuffixAndLcpArraysBasedSuffixTreeBuilder : IBuilder<SuffixTreeEdge, SuffixTreeNode>
{
    private static readonly MutableTrees.Conversions.IConversion MutableTreeConversion = 
        new MutableTrees.Conversions.FullyIterativeConversion();

    /// <summary>
    /// The builder to be used to construct the <see cref="SuffixArray"/> and the <see cref="LcpArray"/> of the full 
    /// text, both required by this algorithm to build the <see cref="SuffixTreeNode"/> structure.
    /// </summary>
    public Func<TextWithTerminator, (SuffixArray, LcpArray)> SuffixAndLcpArraysBuilder { get; }

    /// <summary>
    ///     <inheritdoc cref="SuffixAndLcpArraysBasedSuffixTreeBuilder"/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="SuffixAndLcpArraysBasedSuffixTreeBuilder"/>
    /// </remarks>
    /// <param name="suffixAndLcpArraysBuilder">
    ///     <inheritdoc cref="SuffixAndLcpArraysBuilder" path="/summary"/>
    /// </param>
    public SuffixAndLcpArraysBasedSuffixTreeBuilder(
        Func<TextWithTerminator, (SuffixArray, LcpArray)> suffixAndLcpArraysBuilder)
    {
        SuffixAndLcpArraysBuilder = suffixAndLcpArraysBuilder;
    }

    /// <inheritdoc path="//*[not self::summary or self::remarks]"/>
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="SuffixAndLcpArraysBasedSuffixTreeBuilder" path="/remarks"/>
    /// </remarks>
    public SuffixTreeNode BuildTree(params TextWithTerminator[] texts)
    {
        var (fullText, terminators) = texts.GenerateFullText();
        var n = fullText.Length;
        var (suffixArray, lcpArray) = SuffixAndLcpArraysBuilder(fullText);
        var (suffixArrayValues, lcpArrayValues) = (suffixArray.Indexes.ToList(), lcpArray.Lengths.ToList());

        var firstSuffix = suffixArrayValues[0];
        MutableTree.Node root = MutableTree.Node.BuildRoot();
        MutableTree.Node previousLeaf = MutableTree.Node.Build(
            root, MutableTree.Edge.Build(firstSuffix, n - firstSuffix), firstSuffix);
        root.Children[previousLeaf.IncomingEdge] = previousLeaf;

        var previousLcp = 0;
        for (var i = 1; i < suffixArrayValues.Count; i++)
        {
            var currentSuffix = suffixArrayValues[i];
            var currentSuffixLength = n - currentSuffix;
            var currentLcp = lcpArrayValues[i - 1];

            MutableTree.Node currentLeaf;

            // Case 1: move up in the tree
            while (currentLcp < previousLcp)
            {
                previousLcp -= previousLeaf.ParentNode.IncomingEdge.Length;
                previousLeaf = previousLeaf.ParentNode;
            }

            if (currentLcp == previousLcp)
            {
                // Case 2: new leaf sibling of previous leaf, no intermediate node

                // Build current edge and leaf, for the reminder of the currentLcp.
                var currentEdge = MutableTree.Edge.Build(currentSuffix + currentLcp, currentSuffixLength - currentLcp);
                currentLeaf = MutableTree.Node.Build(previousLeaf.ParentNode, currentEdge, currentSuffix);

                // Attach to the parent of the previous leaf. Nothing happens to the previous leaf itself.
                previousLeaf.ParentNode.Children[currentEdge] = currentLeaf;
            }
            else
            {
                // Case 3: intermediate node required

                var matchingChars = currentLcp - previousLcp;

                // However, the edge to the new leaf can never includes the entire previous edge, because each suffix
                // in this context is terminated by a unique terminator, which makes each suffix never a prefix of
                // another suffix (which was exactly the point of introducing terminators in the first place.
                // Therefore, remaining chars has to be bigger than 0.

                // Build new intermediate, detach old edge going to previous leaf, attach new intermediate edge
                var intermediateEdge = MutableTree.Edge.Build(previousLeaf.IncomingEdge.Start, matchingChars);
                var intermediateNode = MutableTree.Node.Build(previousLeaf.ParentNode, intermediateEdge, null);
                previousLeaf.ParentNode.Children.Remove(previousLeaf.IncomingEdge);
                previousLeaf.ParentNode.Children[intermediateEdge] = intermediateNode;

                // Build current edge and leaf
                var currentEdge = MutableTree.Edge.Build(
                    currentSuffix + currentLcp, 
                    currentSuffixLength - currentLcp);
                currentLeaf = MutableTree.Node.Build(intermediateNode, currentEdge, currentSuffix);

                // Build new edge for the reminder of the previous leaf
                var remainingEdge = MutableTree.Edge.Build(
                    previousLeaf.IncomingEdge.Start + matchingChars, 
                    previousLeaf.IncomingEdge.Length - matchingChars);
                    
                // Attach previous leaf and current leaf to the new intermediate
                intermediateNode.Children[remainingEdge] = previousLeaf;
                intermediateNode.Children[currentEdge] = currentLeaf;
            }

            previousLcp = currentLcp;
            previousLeaf = currentLeaf;
        }

        return MutableTreeConversion.ConvertToSuffixTree(root, fullText, terminators);
    }
}

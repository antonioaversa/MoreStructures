using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Visitor;
using MoreStructures.SuffixStructures;

namespace MoreStructures.SuffixArrays.Builders;

/// <summary>
/// An algorithm for building Suffix Arrays from an already built <see cref="ISuffixStructureNode{TEdge, TNode}"/> 
/// structure for the provided <see cref="TextWithTerminator"/>.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <remarks>
///     <inheritdoc cref="ISuffixArrayBuilder" path="/remarks/para[@id='definition']"/>
///     <para id="algo">
///     The Suffix Tree for the text "mississippi$" is:
///     <code>
///     R
///     - $ (11)
///     - i
///       - $ (10)
///       - ssi
///         - p..$ (7)
///         - s..$ (4)
///     - m..$ 
///     - ...
///     </code>
///     A DFS visit of the leaves of the tree, navigating edges in lexicographic order, gives { 11, 10, 7, 4, ... },
///     which is the Suffix Array of "mississippi$".
///     </para>
///     <para id="complexity">
///     - The Suffix Tree of the text is provided as input, and the complexity of building or keeping it in memoery 
///       is not considered here.
///       <br/>
///     - The DFS of the Tree is linear in m, where m is the number of nodes in the Suffix Tree. m is O(n) for Suffix 
///       Trees and O(n^2) for Suffix Tries, where n is the length of the text.
///       <br/>
///     - While DFS itself is linear in m, edges have to be sorted in lexicographic order, which is done via the LINQ 
///       <see cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/> method.
///       <br/>
///     - There are O(m) edges in the tree, so sorting can be O(m * log(m)) if the alphabet of the text is O(n), using
///       QuickSort or similar, and O(m) if the alphabet is constant w.r.t. to n, using Counting Sort or similar.
///       <br/>
///     - For each visited node, constant work is done: checking whether the node is a leaf and potentially returning 
///       it.
///       <br/>
///     - Therefore Time Complexity is O(n^2 * log(n)) for a Suffix Trie with a non-constant alphabet, O(n^2) for a 
///       Suffix Trie with a constant alphabet, O(n * log(n)) for a Suffix Tree with a non-constant alphabet and
///       O(n) for a Suffix Tree with a constant alphabet.
///       <br/>
///     - Space Complexity is dominated by the space required to sort edges in lexicographic order, before visiting 
///       them in DFS: O(n^2) for a Suffix Trie and O(n) for a Suffix Tree.
///     </para>
/// </remarks>
public class SuffixStructureBasedSuffixArrayBuilder<TEdge, TNode> : ISuffixArrayBuilder
    where TEdge : ISuffixStructureEdge<TEdge, TNode>
    where TNode : ISuffixStructureNode<TEdge, TNode>
{
    private readonly IVisitStrategy<TNode, TreeTraversalVisit<TEdge, TNode>> DepthFirstTraversal;

    /// <summary>
    /// The <see cref="TextWithTerminator"/>, to build the Suffix Array of.
    /// </summary>
    public TextWithTerminator Text { get; }

    /// <summary>
    /// The root node of the <see cref="ISuffixStructureNode{TEdge, TNode}"/> structure, to build the Suffix Array of.
    /// </summary>
    public TNode Node { get; }

    /// <summary>
    /// <inheritdoc cref="SuffixStructureBasedSuffixArrayBuilder{TEdge, TNode}" path="/summary"/>
    /// </summary>
    /// <param name="text"><inheritdoc cref="Text" path="/summary"/></param>
    /// <param name="node"><inheritdoc cref="Node" path="/summary"/></param>
    public SuffixStructureBasedSuffixArrayBuilder(TextWithTerminator text, TNode node)
    {
        Text = text;
        Node = node;
        DepthFirstTraversal = new FullyRecursiveDepthFirstTraversal<TEdge, TNode>()
        {
            ChildrenSorter = visit => visit.Node.Children.OrderBy(kvp => Text[kvp.Key]),
            TraversalOrder = TreeTraversalOrder.ChildrenFirst
        };
    }

    /// <inheritdoc path="//*[self::summary or self::remarks]"/>
    /// <summary>
    /// Builds the Suffix Array for <see name="Node"/>.
    /// </summary>
    public IEnumerable<int> Build() =>
        from visit in DepthFirstTraversal.Visit(Node)
        let node = visit.Node
        where node.IsLeaf()
        select node.Start!.Value;
}

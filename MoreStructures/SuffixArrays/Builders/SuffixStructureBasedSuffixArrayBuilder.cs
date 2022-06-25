using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Visitor;
using MoreStructures.SuffixStructures;

namespace MoreStructures.SuffixArrays.Builders;

/// <summary>
/// An algorithm for building Suffix Arrays from an already built <see cref="ISuffixStructureNode{TEdge, TNode}"/> 
/// structure for the provided <see cref="TextWithTerminator"/>.
/// </summary>
public class SuffixStructureBasedSuffixArrayBuilder<TEdge, TNode> : ISuffixArrayBuilder<TEdge, TNode>
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

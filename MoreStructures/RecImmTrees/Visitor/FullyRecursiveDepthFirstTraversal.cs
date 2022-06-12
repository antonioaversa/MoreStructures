namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// A fully-recursive, depth-first <see cref="IVisitStrategy{TNode, TVisitContext}"/> implementation, i.e. a 
/// traversing strategy which goes in depth as far as possible along each path of the tree, only backtracking when a 
/// leaf is reached.
/// </summary>
/// <remarks>
///     <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="/remarks"/>
///     <para>
///     Implemented fully recursively, so limited by stack depth and usable with tree of a "reasonable" height.
///     </para>
/// </remarks>
public class FullyRecursiveDepthFirstTraversal<TEdge, TNode> 
    : DepthFirstTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <inheritdoc 
    ///     cref="TreeTraversal{TEdge, TNode}.Visit(TNode, Visitor{TNode, TreeTraversalContext{TEdge, TNode}})" 
    ///     path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// <b>Eagerly and recursively</b> visits the structure of the provided<paramref name= "node" />, calling the 
    /// provided <paramref name="visitor"/> on each <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the 
    /// structure, in depth-first order.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="FullyRecursiveDepthFirstTraversal{TEdge, TNode}" path="/remarks"/>
    ///     <para id = "algo" >
    ///     - The algorithm visits all nodes in structure in natural recursion/depth-first order, calling the visitor.
    ///     </para>
    ///     <para id="complexity1">
    ///     - Excluding visitor, constant time work is done for each of the n nodes of the tree (such as construction 
    ///       of the input record for the visitor). 
    ///       <br/>
    ///     - Iteration-cost is constant w.r.t. n. <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> cost 
    ///       depends on the actual algorithm used.
    ///       <br/>
    ///     - So Time Complexity is dominated by <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> and visitor.
    ///     </para>
    ///     <para id="complexity2">
    ///     In conclusion:
    ///     <br/>.
    ///     - Time Complexity is O(n * (Ts + Tv)), where Ts is the amortized time cost of
    ///       <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> per node and Tv is the time cost of the 
    ///       visitor per node.
    ///       <br/>
    ///     - Space Complexity is O(n * Sv), where Sv is the space cost of visitor per node.
    ///     </para>
    /// </remarks>
    public override void Visit(TNode node, Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor) => 
        Visit(node, visitor, default, default);

    private void Visit(
        TNode node,
        Visitor<TNode, TreeTraversalContext<TEdge, TNode>> visitor, 
        TNode? parentNode,
        TEdge? parentEdge)
    {
        switch (TraversalOrder)
        {
            case TreeTraversalOrder.ParentFirst:
                visitor(node, new(parentNode, parentEdge));
                foreach (var child in ChildrenSorter(node.Children))
                    Visit(child.Value, visitor, node, child.Key);

                break;

            case TreeTraversalOrder.ChildrenFirst:
                foreach (var child in ChildrenSorter(node.Children))
                    Visit(child.Value, visitor, node, child.Key);
                visitor(node, new(parentNode, parentEdge));

                break;

            default:
                throw new NotSupportedException($"{nameof(TreeTraversalOrder)} {TraversalOrder} is not supported");
        }
    }
}

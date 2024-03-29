﻿namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// A lazy, fully-recursive, depth-first <see cref="IVisitStrategy{TNode, TVisitContext}"/> implementation, i.e. a 
/// traversing strategy which goes in depth as far as possible along each path of the tree, only backtracking when a 
/// leaf is reached.
/// </summary>
/// <remarks>
///     <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="/remarks"/>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Implemented fully recursively, so limited by stack depth and usable with tree of a "reasonable" height.
///     </para>
/// </remarks>
public class FullyRecursiveDepthFirstTraversal<TEdge, TNode> 
    : DepthFirstTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <inheritdoc 
    ///     cref="TreeTraversal{TEdge, TNode}.Visit(TNode)" 
    ///     path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// <b>Lazily and recursively</b> visits the structure of the provided<paramref name= "node" />, returning the
    /// sequence of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure, in depth-first order.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="FullyRecursiveDepthFirstTraversal{TEdge, TNode}" path="/remarks"/>
    ///     <para id = "algo" >
    ///     ALGORITHM
    ///     <br/>
    ///     - The algorithm visits all nodes in structure in natural recursion/depth-first order, yielding to the 
    ///       output sequence, so that the client code implementing the visitor can lazily process the nodes.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Excluding visitor, constant time work is done for each of the n nodes of the tree (such as construction 
    ///       of the input record for the visitor). 
    ///       <br/>
    ///     - Iteration-cost is constant w.r.t. n. <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> cost 
    ///       depends on the actual algorithm used.
    ///       <br/>
    ///     - So Time Complexity is dominated by <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> and visitor.
    ///     <br/>
    ///     In conclusion:
    ///     <br/>
    ///     - Time Complexity is O(n * Ts), where Ts is the amortized time cost of
    ///       <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> per node. Taking into account the visit of
    ///       each emitted node, Time Complexity is O(n * (Ts + Tv)), where Tv is the time cost of the visitor per 
    ///       node. 
    ///       <br/>
    ///     - Space Complexity is O(n). Taking into account the visit of each emitted node, Space Complexity is 
    ///       O(n * Sv), where Sv is the space cost of visitor per node.
    ///     </para>
    /// </remarks>
    public override IEnumerable<TreeTraversalVisit<TEdge, TNode>> Visit(TNode node) => 
        Visit(new(node, default, default, 0));

    private IEnumerable<TreeTraversalVisit<TEdge, TNode>> Visit(TreeTraversalVisit<TEdge, TNode> visit)
    {
        var (node, _, _, level) = visit;

        switch (TraversalOrder)
        {
            case TreeTraversalOrder.ParentFirst:
                yield return visit;
                foreach (var child in ChildrenSorter(visit))
                    foreach (var childVisit in Visit(new(child.Value, node, child.Key, level + 1)))
                        yield return childVisit;

                break;

            case TreeTraversalOrder.ChildrenFirst:
                foreach (var child in ChildrenSorter(visit))
                    foreach (var childVisit in Visit(new(child.Value, node, child.Key, level + 1)))
                        yield return childVisit;
                yield return visit;

                break;

            default:
                throw new NotSupportedException($"{nameof(TreeTraversalOrder)} {TraversalOrder} is not supported");
        }
    }
}

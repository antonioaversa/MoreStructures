namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// A lazy, fully-iterative, depth-first <see cref="IVisitStrategy{TNode, TVisitContext}"/> implementation, i.e. a 
/// traversing strategy which goes in depth as far as possible along each path of the tree, only backtracking when a 
/// leaf is reached.
/// </summary>
/// <remarks>
///     <inheritdoc cref="DepthFirstTraversal{TEdge, TNode}" path="/remarks"/>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Implemented fully iteratively, so not limited by call stack depth but rather by the maximum size of the stack 
///     stored in the heap. Convenient with deep trees (i.e. trees having a height &gt; ~1K nodes).
///     </para>
/// </remarks>
public class FullyIterativeDepthFirstTraversal<TEdge, TNode> 
    : DepthFirstTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    private record struct StackFrame(
        TNode? ParentNode, TEdge? IncomingEdge, TNode Node, bool ChildrenStacked, int Level);

    /// <inheritdoc 
    ///     cref="TreeTraversal{TEdge, TNode}.Visit(TNode)" 
    ///     path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// <b>Lazily and iteratively</b> visits the structure of the provided <paramref name= "node" />, returning the
    /// sequence of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> of the structure, in depth-first order.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="FullyIterativeDepthFirstTraversal{TEdge, TNode}" path="/remarks"/>
    ///     <para id = "algo" >
    ///     ALGORITHM
    ///     <br/>
    ///     The algorithm uses a <see cref="Stack{T}"/>:
    ///     <br/>    
    ///     - At the beginning the stack contains only a frame with the root node, no parent node nor incoming edge and
    ///       with a <see cref="bool"/> indicating the children for this node haven't been added to the stack yet;
    ///       <br/> 
    ///     - Then each frame at the top of the stack is popped out and processed, until the stack is empty.
    ///       <br/> 
    ///     - If the node being processed has the "children stacked" flag not set, all children are stacked up. The 
    ///       node itself is also stacked up, again, this time with the "children stacked" flag set.
    ///     - If the node being processed has the "children stacked" flag set, or is a leaf, it's yielded to the
    ///       output sequence, so that the client code implementing the visitor can lazily process the nodes.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Each of the n nodes and n - 1 edges of the tree is visited at most twice: the first time with the 
    ///       "children stacked" flag unset and a second time with the flag set. Leafs are only visited once, since 
    ///       they have no children and don't have to wait for their children to be visited.
    ///       <br/>
    ///     - <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> can also increase time and space complexity, 
    ///       especially if it perform an actual sorting of nodes. For example, if the sorter takes n * log(n) time
    ///       <br/>
    ///     - The <see cref="IEnumerable{T}"/> emitted by <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> is 
    ///       reversed to be pushed onto the stack in the right order, and that takes additional O(n - 1) total space, 
    ///       since there are n - 1 edges, which are 1-to-1 with nodes in the tree.
    ///       <br/>
    ///     - Each frame processing of a node with the "children stacked" flag set takes constant time (e.g.to check 
    ///       traversal order) and space (e.g. to extract parent node, incoming edge and node itself from the frame and
    ///       to build a <see cref="TreeTraversalContext{TEdge, TNode}"/> object for the visit).
    ///       <br/>
    ///     - Time Complexity is O(n * Ts) in total, where Ts is the amortized Time Complexity of 
    ///       <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> per edge/node. Taking into account the visit of
    ///       each emitted node, Time Complexity is O(n * Ts * Tv), where Tv is the Time Complexity of the visitor per 
    ///       node.
    ///       <br/>
    ///     - Space Complexity is O(n * Ss) in total, where Ss is the amortized Space Complexity of 
    ///       <see cref="TreeTraversal{TEdge, TNode}.ChildrenSorter"/> per edge/node. Taking into account the visit of
    ///       each emitted node, Space Complexity is O(n * (Ss + Sv)), where Sv is the Space Complexity of the visitor 
    ///       per node.
    ///     </para>
    /// </remarks>
    public override IEnumerable<TreeTraversalVisit<TEdge, TNode>> Visit(TNode node)
    {
        var stack = new Stack<StackFrame> { };
        stack.Push(new (default, default, node, false, 0));
        while (stack.Count > 0)
        {
            if (ProcessStack(stack) is TreeTraversalVisit<TEdge, TNode> visit)
                yield return visit;
        }
    }

    private TreeTraversalVisit<TEdge, TNode>? ProcessStack(Stack<StackFrame> stack)
    {
        var (parentNode, incomingEdge, node, childrenStacked, level) = stack.Pop();

        if (node.IsLeaf())
        {
            return TraversalOrder switch
            {
                TreeTraversalOrder.ParentFirst or TreeTraversalOrder.ChildrenFirst => 
                    new(node, new(parentNode, incomingEdge, level)),
                _ => 
                    throw new NotSupportedException($"{nameof(TreeTraversalOrder)} {TraversalOrder} is not supported"),
            };
        }

        if (!childrenStacked)
        {
            switch (TraversalOrder)
            {
                case TreeTraversalOrder.ParentFirst:
                    foreach (var child in ChildrenSorter(new(node, new(parentNode, incomingEdge, level))).Reverse())
                        stack.Push(new(node, child.Key, child.Value, false, level + 1));
                    stack.Push(new(parentNode, incomingEdge, node, true, level));

                    break;

                case TreeTraversalOrder.ChildrenFirst:
                    stack.Push(new(parentNode, incomingEdge, node, true, level));
                    foreach (var child in ChildrenSorter(new(node, new(parentNode, incomingEdge, level))).Reverse())
                        stack.Push(new(node, child.Key, child.Value, false, level + 1));

                    break;

                default:
                    throw new NotSupportedException($"{nameof(TreeTraversalOrder)} {TraversalOrder} is not supported");
            }

            return null;
        }

        return new(node, new(parentNode, incomingEdge, level));
    }
}
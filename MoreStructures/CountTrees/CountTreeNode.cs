using MoreStructures.RecImmTrees;
using MoreStructures.Utilities;

namespace MoreStructures.CountTrees;

/// <summary>
/// An implementation of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/>, wrapping another
/// implementation of <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/>, and counting the total
/// number of descendands the wrapped node has below (node itself excluded).
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <param name="WrappedNode">The node being wrapped, and whose descendants are going to be counted.</param>
/// <remarks>
/// <para>
/// Due to records semantics, and the use of value readonly dictionaries, two <see cref="CountTreeNode{TEdge, TNode}"/> 
/// instances wrapping the same underlying node, or two equivalent nodes, will be equal. 
/// </para>
/// <para>
/// Immutability of the wrapped tree is also required by the caching strategy applied by <see cref="DescendantsCount"/>.
/// </para>
/// </remarks>
public sealed record CountTreeNode<TEdge, TNode>(TNode WrappedNode)
    : IRecImmDictIndexedTreeNode<CountTreeEdge<TEdge, TNode>, CountTreeNode<TEdge, TNode>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    private IDictionary<CountTreeEdge<TEdge, TNode>, CountTreeNode<TEdge, TNode>>? _children;
    private int? _descendantsCount = null;

    /// <summary>
    /// A private readonly object used for synchronization across multiple calls on the same instance.
    /// </summary>
    /// <remarks>
    /// A <see cref="LockValueObject"/> is used not to break value equality of this record.
    /// </remarks>
    private readonly LockValueObject _lockObject = new();

    private record struct StackFrame(CountTreeNode<TEdge, TNode> Node, TNode WrappedNode, bool ChildrenProcessed);

    private void Compute()
    {
        lock (_lockObject)
        {
            var stack = new Stack<StackFrame>();
            stack.Push(new(this, WrappedNode, false));

            while (stack.Count > 0)
                ProcessStack(stack);
        }
    }

    private static void ProcessStack(Stack<StackFrame> stack)
    {
        var (node, wrappedNode, childrenProcessed) = stack.Pop();

        if (wrappedNode.IsLeaf())
        {
            var children = new Dictionary<CountTreeEdge<TEdge, TNode>, CountTreeNode<TEdge, TNode>> { };
            node._children = children.ToValueReadOnlyDictionary();
            node._descendantsCount = 0;
            return;
        }

        if (!childrenProcessed)
        {
            stack.Push(new(node, wrappedNode, true));

            var children = new Dictionary<CountTreeEdge<TEdge, TNode>, CountTreeNode<TEdge, TNode>> { };
            foreach (var (wrappedChildEdge, wrappedChildNode) in wrappedNode.Children)
            {
                var childEdge = new CountTreeEdge<TEdge, TNode>(wrappedChildEdge);
                var childNode = new CountTreeNode<TEdge, TNode>(wrappedChildNode);
                children[childEdge] = childNode;
                
                stack.Push(new(childNode, wrappedChildNode, false));
            }

            node._children = children.ToValueReadOnlyDictionary();
            return;
        }

        // At this point _children has been initialized in previous ProcessStack execution for the same node
        // and _descendantsCount has been set for all the children of the node (due to the fact that the node has
        // been re-pushed to the stack just before all its children).
        node._descendantsCount = node._children!.Sum(c => c.Value._descendantsCount!.Value + 1);
    }

    /// <inheritdoc/>
    public IDictionary<CountTreeEdge<TEdge, TNode>, CountTreeNode<TEdge, TNode>> Children 
    { 
        get
        {
            if (_children == null)
                Compute();

            return _children!; // Initialized in Compute
        }
    }

    /// <summary>
    /// The number of descendands below this node (node itself excluded).
    /// </summary>
    /// <remarks>
    /// Lazy evaluated and thread-safe. Once calculated, it is cached and returned.
    /// </remarks>
    public int DescendantsCount 
    {
        get 
        {
            if (_descendantsCount == null)
                Compute();

            return _descendantsCount!.Value; // Initialized in Compute
        }
    }
}

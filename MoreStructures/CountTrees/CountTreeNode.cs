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
/// <para id="info">
///     <para id="iterativity">
///     <see cref="CountTreeNode{TEdge, TNode}"/> structure construction and properties calculation are done lazily and 
///     fully iteratively, so the use of this structure is not limited by call stack depth but rather by the maximum 
///     size of the stack stored in the heap. Convenient with deep trees (i.e. trees having a height > ~1K nodes).
///     </para>
///     <para id="caching">
///     Once <see cref="Children"/> and <see cref="DescendantsCount"/> properties are calculated, they are cached to 
///     avoid multiple calculation. This is also one of the reasons why immutability of the wrapped tree is a 
///     requirement to use <see cref="CountTreeNode{TEdge, TNode}"/>.
///     </para>
///     <para id="complexity">
///     Time Complexity = O(n) and Space Complexity = O(n) where n = number of nodes in <see cref="WrappedNode"/> 
///     structure. Leafs are visited only once, intermediate nodes are visited (at most) twice.
///     </para>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var wrapped = new Node(1) 
/// { 
///     Children = new Dictionary&lt;Edge, Node&gt; 
///     {
///         [new(1)] = new(2),
///         [new(2)] = new(3)
///         {
///             ...
///         },
///         [new(5)] = new(6),
///     } 
/// };
/// 
/// var wrapping = new CountTreeNode&lt;Edge, Node&gt;(wrapped);
/// Assert.AreEqual(3, wrapping.Children.Count);
/// </code>
/// </example>
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
    /// <remarks>
    /// <inheritdoc cref="CountTreeNode{TEdge, TNode}" path="/remarks/para[@id='info']"/>
    /// </remarks>
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
    /// <inheritdoc cref="CountTreeNode{TEdge, TNode}" path="/remarks/para[@id='info']"/>
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

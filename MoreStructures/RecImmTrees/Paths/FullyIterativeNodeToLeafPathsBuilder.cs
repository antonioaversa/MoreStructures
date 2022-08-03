namespace MoreStructures.RecImmTrees.Paths;

/// <summary>
/// <inheritdoc/>
/// Iterative implementation.
/// </summary>
/// <remarks>
///     <inheritdoc cref="INodeToLeafPathsBuilder" path="/remarks"/>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     <inheritdoc cref="DocFragments" path="/remarks/para[@id='fully-iterative-advantages']"/>
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     The implementation uses a <see cref="Stack{T}"/>, onto which quadrules are stacked up: 
///     <br/>
///     (incoming edge, node, queue of subpaths of the parent node of node, queue of subpaths of the node itself).
///     <br/>
///     - A queue of subpaths of the root node is instantiated empty. It will collect the output 
///       <see cref="TreePath{TEdge, TNode}"/> instances.
///       <br/>
///     - Children (node and incoming edge) of the root node are pushed onto the stack in reverse order. The queue of
///       subpaths of the root is passed to each child. The queue of the child is set to <see langword="null"/>, since
///       children of the child haven't been processed yet (i.e. the child queue of subpath hasn't been populated yet).
///       <br/>
///     - Then the stack processing loop is performed. After every stack item is processed, the queue of subpaths of 
///       the root is unloaded, emitting <see cref="TreePath{TEdge, TNode}"/> instances from the collected 
///       <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/> of edges and nodes.
///       <br/>
///     <br/>
///     Stack processing iteration:
///     <br/>
///     - The frame at the top of the stack is popped.
///       <br/>
///     - If the node is a leaf, the node and its incoming edge are added to the queue of subpaths of the parent node.
///       <br/>
///     - If the node has children and the queue of subpaths of the node is set, children have been already processed. 
///       So as many paths are enqueued to the queue of subpaths of the parent node as paths in the queue of subpaths
///       of the node. Each subpath of the node is prepended with the node itself.
///       <br/>
///     - If the node has children and the queue of subpaths of the node is not set, children have not been processed
///       yet. So create a new queue for the subpaths of the node and push the node back to the stack, this time with
///       the queue of subpaths set. Then push all the children of the node to the stack (in reverse order), passing 
///       the queue of subpaths of the node as queue of subpaths of the parent, and the queue of subpaths of the child 
///       not set.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Each node is pushed to the stack and then processed at most twice (only once for leaves). So the maximum 
///       number of items in the stack is 2 * n, where n = number of nodes in the tree.
///       <br/>
///     - The total number of paths from root to leaf is equal to the total number of leaves of the tree, which can 
///       be at most n (if the root itself is a leaf), n - 1 (if the root is not a leaf and the tree is completely 
///       flat) or anything down to 1 (if the tree is as unbalanced as it can be).
///       <br/>
///     - Each stack processing iteration does constant time work: stack item deconstruction, check whether the node is
///       a leaf or not, instantiate a queue of subpaths of the node. Children are iterated, however, the total number 
///       of children iteration over all the nodes of the tree is equal to the number of edges, which is n - 1.
///       <br/>
///     - For each non-leaf node (and there can be O(n) of those), a queue of subpaths is instantiated and populated. 
///       Such queue can have O(n) items in it (for a flattish tree), each being a subpath of length O(n). This would
///       suggest a cubic complexity. However, there is at most a total of n root-to-leaf paths, each being at most of
///       length n, so overall Space Complexity is quadratic.
///       <br/>
///     - So Time Complexity is O(n) and Space Complexity is O(n^2).
///     </para>
/// </remarks>
public class FullyIterativeNodeToLeafPathsBuilder : INodeToLeafPathsBuilder
{
    private record struct StackFrame<TEdge, TNode>(
        TEdge Edge,
        TNode Node,
        Queue<IEnumerable<KeyValuePair<TEdge, TNode>>> ParentNodeSubpaths,
        Queue<IEnumerable<KeyValuePair<TEdge, TNode>>>? NodeSubpaths);

    /// <inheritdoc/>
    /// <inheritdoc cref="FullyIterativeNodeToLeafPathsBuilder" path="/remarks"/>
    public IEnumerable<TreePath<TEdge, TNode>> GetAllNodeToLeafPaths<TEdge, TNode>(TNode node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
    {
        var stack = new Stack<StackFrame<TEdge, TNode>>();
        var rootSubpaths = new Queue<IEnumerable<KeyValuePair<TEdge, TNode>>> { };

        foreach (var child in node.Children.Reverse())
        {
            stack.Push(new(child.Key, child.Value, rootSubpaths, null));
        }

        while (stack.Count > 0)
        {
            ProcessStack(stack);

            while (rootSubpaths.Count > 0)
                yield return new TreePath<TEdge, TNode>(rootSubpaths.Dequeue());
        }
    }

    private static void ProcessStack<TEdge, TNode>(Stack<StackFrame<TEdge, TNode>> stack)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
    {
        var (edge, node, parentNodeSubpaths, nodeSubpaths) = stack.Pop();

        if (node.IsLeaf())
        {
            parentNodeSubpaths.Enqueue(new [] { KeyValuePair.Create(edge, node) });
            return;
        }

        if (nodeSubpaths != null)
        {
            // Children have already been processed
            foreach (var nodeSubpath in nodeSubpaths)
                parentNodeSubpaths.Enqueue(nodeSubpath.Prepend(KeyValuePair.Create(edge, node)));
        }
        else
        {
            // Children have not been processed yet
            nodeSubpaths = new Queue<IEnumerable<KeyValuePair<TEdge, TNode>>> { };
            stack.Push(new(edge, node, parentNodeSubpaths, nodeSubpaths));
            foreach (var child in node.Children.Reverse())
            {
                stack.Push(new(child.Key, child.Value, nodeSubpaths, null));
            }
        }

    }
}

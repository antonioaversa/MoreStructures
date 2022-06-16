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
///     Implemented fully iteratively via a <see cref="Stack{T}"/>, so not limited by call stack depth but rather by 
///     the maximum size of the stack stored in the heap.
///     <br/>
///     Convenient with deep trees (i.e. trees having a height &gt; ~1K nodes).
///     </para>
/// </remarks>
public class FullyIterativeNodeToLeafPathsBuilder : INodeToLeafPathsBuilder
{
    private record struct StackFrame<TEdge, TNode>(
        TEdge Edge,
        TNode Node, 
        IList<IEnumerable<KeyValuePair<TEdge, TNode>>> ParentNodeSubpaths,
        IList<IEnumerable<KeyValuePair<TEdge, TNode>>>? NodeSubpaths);

    /// <inheritdoc/>
    /// <inheritdoc cref="FullyIterativeNodeToLeafPathsBuilder" path="/remarks"/>
    public IEnumerable<TreePath<TEdge, TNode>> GetAllNodeToLeafPaths<TEdge, TNode>(TNode node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
    {
        var stack = new Stack<StackFrame<TEdge, TNode>>();
        var rootSubpaths = new List<IEnumerable<KeyValuePair<TEdge, TNode>>> { };

        foreach (var child in node.Children.Reverse())
        {
            stack.Push(new(child.Key, child.Value, rootSubpaths, null));
        }

        while (stack.Count > 0)
        {
            ProcessStack(stack);
        }

        foreach (var rootSubpath in rootSubpaths)
            yield return new TreePath<TEdge, TNode>(rootSubpath);
    }

    private static void ProcessStack<TEdge, TNode>(Stack<StackFrame<TEdge, TNode>> stack)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
    {
        var (edge, node, parentNodeSubpaths, nodeSubpaths) = stack.Pop();

        if (node.IsLeaf())
        {
            parentNodeSubpaths.Add(new [] { KeyValuePair.Create(edge, node) });
            return;
        }

        if (nodeSubpaths != null)
        {
            // Children have already been processed
            foreach (var nodeSubpath in nodeSubpaths)
                parentNodeSubpaths.Add(nodeSubpath.Prepend(KeyValuePair.Create(edge, node)));
        }
        else
        {
            // Children have not been processed yet
            nodeSubpaths = new List<IEnumerable<KeyValuePair<TEdge, TNode>>> { };
            stack.Push(new(edge, node, parentNodeSubpaths, nodeSubpaths));
            foreach (var child in node.Children.Reverse())
            {
                stack.Push(new(child.Key, child.Value, nodeSubpaths, null));
            }
        }

    }
}

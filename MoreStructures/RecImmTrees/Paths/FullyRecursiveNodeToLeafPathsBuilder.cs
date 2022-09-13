namespace MoreStructures.RecImmTrees.Paths;

/// <summary>
///     <inheritdoc/>
///     <br/>
///     Recursive implementation.
/// </summary>
/// <remarks>
///     <inheritdoc cref="INodeToLeafPathsBuilder" path="/remarks"/>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Implemented fully recursively, so limited by stack depth and usable with tree of a "reasonable" height.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - The implementation iterates over the children, getting its node-to-leaf subpaths by calling 
///       <see cref="GetAllNodeToLeafPaths{TEdge, TNode}(TNode)"/> recursively.
///       <br/>
///     - Then, it prepends the child and its incoming edge to each subpath of each child.
///       <br/>
///     - If the node has no child, a singleton path is returned, containing only the child and its incoming edge.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Each node is processed once and the number of node-to-leaf paths returned has an upper bound on n = number of 
///       nodes in the tree. The length of each node-to-leaf path is also limited by n.
///       <br/>
///     - So, both Time and Space Complexity are O(n).
///     </para>
/// </remarks>
public class FullyRecursiveNodeToLeafPathsBuilder : INodeToLeafPathsBuilder
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <inheritdoc cref="FullyRecursiveNodeToLeafPathsBuilder" path="/remarks"/>
    public IEnumerable<TreePath<TEdge, TNode>> GetAllNodeToLeafPaths<TEdge, TNode>(TNode node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode> =>

        GetAllNodeToLeafPathsR<TEdge, TNode>(node).Select(pathNodes => new TreePath<TEdge, TNode>(pathNodes));

    private IEnumerable<IEnumerable<KeyValuePair<TEdge, TNode>>> GetAllNodeToLeafPathsR<TEdge, TNode>(TNode node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
    {
        foreach (var edgeAndChild in node.Children)
        {
            var childToLeafPaths = GetAllNodeToLeafPathsR<TEdge, TNode>(edgeAndChild.Value);
            var childPathNode = KeyValuePair.Create(edgeAndChild.Key, edgeAndChild.Value);
            if (childToLeafPaths.Any())
            {
                foreach (var childToLeafPath in childToLeafPaths)
                    yield return childToLeafPath.Prepend(childPathNode);
            }
            else
            {
                yield return new KeyValuePair<TEdge, TNode>[] { childPathNode };
            }
        }
    }
}

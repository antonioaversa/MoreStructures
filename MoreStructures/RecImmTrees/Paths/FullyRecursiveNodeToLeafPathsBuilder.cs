namespace MoreStructures.RecImmTrees.Paths;

/// <summary>
/// <inheritdoc/>
/// Recursive implementation.
/// </summary>
/// <remarks>
///     <inheritdoc cref="INodeToLeafPathsBuilder" path="/remarks"/>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Implemented fully recursively, so limited by stack depth and usable with tree of a "reasonable" height.
///     </para>
/// </remarks>
public class FullyRecursiveNodeToLeafPathsBuilder : INodeToLeafPathsBuilder
{
    /// <inheritdoc/>
    /// <inheritdoc cref="FullyRecursiveNodeToLeafPathsBuilder" path="/remarks"/>
    public IEnumerable<TreePath<TEdge, TNode>> GetAllNodeToLeafPaths<TEdge, TNode>(TNode node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
    {
        foreach (var edgeAndChild in node.Children)
        {
            var childToLeafPaths = GetAllNodeToLeafPaths<TEdge, TNode>(edgeAndChild.Value);
            if (childToLeafPaths.Any())
            {
                foreach (var childToLeafPath in childToLeafPaths)
                    yield return new TreePath<TEdge, TNode>(edgeAndChild.Key, edgeAndChild.Value)
                        .Concat(childToLeafPath);
            }
            else
            {
                yield return new TreePath<TEdge, TNode>(edgeAndChild.Key, edgeAndChild.Value);
            }
        }
    }
}

namespace MoreStructures.RecImmTrees.Paths;

/// <summary>
/// An algorithm building all <see cref="TreePath{TEdge, TNode}"/> from the provided 
/// <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> to a leaf.
/// </summary>
public interface INodeToLeafPathsBuilder
{
    /// <summary>
    /// Returns all <see cref="TreePath{TEdge, TNode}"/> from the provided <paramref name="node"/> to a leaf.
    /// </summary>
    /// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
    /// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
    /// <param name="node">The node, to start the structure traversal from.</param>
    /// <returns>A sequence of <see cref="TreePath{TEdge, TNode}"/>.</returns>
    /// <example>
    ///     Given the following tree structure:
    ///     <code>
    ///     0
    ///     |- 0 -> 1
    ///     |       |- 1 -> 2
    ///     |       |- 2 -> 3
    ///     |       |       |- 3 -> 4
    ///     |       |- 4 -> 5
    ///     |- 5 -> 6
    ///     |- 6 -> 7
    ///             |- 7 -> 8
    ///                     |- 8 -> 9
    ///                     |- 9 -> 10
    ///     </code>
    ///     
    ///     The method would return the following node-to-leaf paths:
    ///     <code>
    ///      (0 -> 1) => (1 -> 2)
    ///      (0 -> 1) => (2 -> 3) => (3 -> 4)
    ///      (0 -> 1) => (4 -> 5)
    ///      (5 -> 6)
    ///      (6 -> 7) => (7 -> 8) => (8 -> 9)
    ///      (6 -> 7) => (7 -> 8) => (9 -> 10)
    ///     </code>
    /// </example>
    IEnumerable<TreePath<TEdge, TNode>> GetAllNodeToLeafPaths<TEdge, TNode>(TNode node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>;
}

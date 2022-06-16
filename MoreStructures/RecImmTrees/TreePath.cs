using MoreStructures.RecImmTrees.Conversions;
using MoreStructures.Utilities;

namespace MoreStructures.RecImmTrees;

/// <summary>
/// An immutable sequence of <typeparamref name="TNode"/>, where each node is child of its predecessor and parent of 
/// its successor and where node relationships are stored in <typeparamref name="TEdge"/> instances.
/// </summary>
/// <param name="PathNodes">The sequence of nodes respecting the parent-child relationship.</param>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <remarks>
/// Immutability is guaranteed by using <see cref="ValueReadOnlyDictionary{TKey, TValue}"/>.
/// </remarks>
public record TreePath<TEdge, TNode>(IEnumerable<KeyValuePair<TEdge, TNode>> PathNodes)
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <summary>
    /// Builds an empty path, i.e. an empty sequence of nodes.
    /// </summary>
    public TreePath() : this(Enumerable.Empty<KeyValuePair<TEdge, TNode>>())
    {
    }

    /// <summary>
    /// Builds a path composed of a single node with its incoming edge.
    /// </summary>
    /// <param name="edge">The edge leading to the node.</param>
    /// <param name="node">The node defining the singleton path.</param>
    public TreePath(TEdge edge, TNode node) : this(Enumerable.Repeat(KeyValuePair.Create(edge, node), 1))
    {
    }

    /// <summary>
    /// Builds a path composed of the provided couples of edges and nodes.
    /// </summary>
    /// <param name="pathNodes">An array of couples (edge, node).</param>
    public TreePath(params (TEdge edge, TNode node)[] pathNodes)
        : this(pathNodes.Select(pathNode => KeyValuePair.Create(pathNode.edge, pathNode.node)))
    {
    }

    /// <summary>
    /// A readonly view of the private collection of path <typeparamref name="TNode"/> instances.
    /// </summary>
    public IEnumerable<KeyValuePair<TEdge, TNode>> PathNodes { get; } = 
        PathNodes.ToValueReadOnlyCollection();

    private static readonly IStringifier<TEdge, TNode> Stringifier =
        new FullyIterativeStringifier<TEdge, TNode>(r => string.Empty, (e, n) => $"{e}")
        {
            PathSeparator = " => ",
        };

    /// <summary>
    /// <inheritdoc/>
    /// Uses a <see cref="IStringifier{TEdge, TNode}"/> to generate the string.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public override string ToString() => Stringifier.Stringify(this);
}
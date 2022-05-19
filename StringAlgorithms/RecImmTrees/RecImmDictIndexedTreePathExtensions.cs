using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms.RecImmTrees;

/// <summary>
/// Extension methods for all <see cref="IRecImmDictIndexedTreePath{TEdge, TNode, TPath, TBuilder}"/> node concretions.
/// </summary>
public static class RecImmDictIndexedTreePathExtensions
{
    /// <summary>
    /// Builds a new path of nodes, appending the nodes of the second path to the first path.
    /// </summary>
    /// <param name="first">The path, to append nodes to.</param>
    /// <param name="second">The path, whose nodes have to be appended.</param>
    /// <returns>A new path, whose nodes are the concatenation of the nodes of the two paths.</returns>
    public static TPath Concat<TEdge, TNode, TPath, TBuilder>(
        this IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder> first,
        IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder> second)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
        where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>, new() =>
        new TBuilder().MultistepsPath(first.PathNodes.Concat(second.PathNodes));

    /// <summary>
    /// Append the provided node with its incoming edge to the provided path, bulding a new path.
    /// </summary>
    /// <param name="path">The path, to appended the node and the edge to.</param>
    /// <param name="edge">The edge, pointing to the node to be appended.</param>
    /// <param name="node">The node to be appended.</param>
    /// <returns>
    /// A new path, whose nodes are the concatenation of the nodes of the provided path and the one appended.
    /// </returns>
    public static TPath Append<TEdge, TNode, TPath, TBuilder>(
        this IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder> path,
        TEdge edge,
        TNode node)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
        where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>, new() =>
        new TBuilder().MultistepsPath(path.PathNodes.Concat(Enumerable.Repeat(KeyValuePair.Create(edge, node), 1)));
}

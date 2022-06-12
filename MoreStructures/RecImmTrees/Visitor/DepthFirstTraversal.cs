namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="TreeTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// Base class for all DFS strategies, i.e. all traversing strategies which goes in depth as far as possible 
/// along each path of the tree, only backtracking when a leaf is reached.
/// </summary>
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
///     <para>
///     A DFS visit strategy "parent first" would visit nodes and edges in either of the following ways, depending on
///     how children are sorted (lower-id edge first, lower-id edge last, median-id edge first, ...):
///     <br/>
///     - { (null, 0), (0, 1), (1, 2), (2, 3), (3, 4), (4, 5), (5, 6), (6, 7), (7, 8), (8, 9), (9, 10) }
///     <br/>
///     - { (null, 0), (6, 7), (7, 8), (9, 10), (8, 9), (5, 6), (0, 1), (4, 5), (2, 3), (3, 4), (1, 2) }
///     <br/>
///     - { (null, 0), (5, 6), (6, 7), (7, 8), (9, 10), (8, 9), (0, 1), (2, 3), (3, 4), (4, 5), (1, 2) }
///     <br/>
///     - ...
///     </para>
///     <para>
///     A DFS visit strategy "children first" would visit nodes and edges in either of the following ways, depending on
///     how children are sorted:
///     <br/>
///     - { (1, 2), (3, 4), (2, 3), (4, 5), (0, 1), (5, 6), (8, 9), (9, 10), (7, 8), (6, 7), (null, 0) }
///     <br/>
///     - { (9, 10), (8, 9), (7, 8), (6, 7), (5, 6), (4, 5), (3, 4), (2, 3), (1, 2), (0, 1), (null, 0) }
///     <br/>
///     - ...
///     </para>
/// </example>
public abstract class DepthFirstTraversal<TEdge, TNode>
    : TreeTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
}

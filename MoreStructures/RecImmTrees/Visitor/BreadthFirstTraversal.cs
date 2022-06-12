namespace MoreStructures.RecImmTrees.Visitor;

/// <inheritdoc cref="TreeTraversal{TEdge, TNode}" path="//*[not(self::summary or self::remarks)]"/>
/// <summary>
/// Base class for all BFT strategies, i.e. all traversing strategies which visit all the nodes at the current depth, 
/// along any path of the tree, before going deeper or shallower, exploring nodes with higher or lower depth.
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
///     A BFT visit strategy "parent first" would visit nodes and edges in either of the following ways, depending on
///     how children are sorted (lower-id edge first, lower-id edge last, median-id edge first, ...):
///     <br/>
///     - { (null, 0), (0, 1), (5, 6), (6, 7), (1, 2), (2, 3), (4, 5), (7, 8), (3, 4), (8, 9), (9, 10) }
///     <br/>
///     - { (null, 0), (6, 7), (5, 6), (0, 1), (7, 8), (4, 5), (2, 3), (1, 2), (9, 10), (8, 9), (3, 4) }
///     <br/>
///     - { (null, 0), (5, 6), (6, 7), (0, 1), (7, 8), (2, 3), (4, 5), (1, 2), (9, 10), (8, 9), (3, 4) }
///     <br/>
///     - ...
///     </para>
///     <para>
///     A BFT visit strategy "children first" would visit nodes and edges in either of the following ways, depending on
///     how children are sorted:
///     <br/>
///     - { (3, 4), (8, 9), (9, 10), (1, 2), (2, 3), (4, 5), (7, 8), (0, 1), (5, 6), (6, 7), (null, 0) }
///     <br/>
///     - { (9, 10), (8, 9), (3, 4), (7, 8), (4, 5), (2, 3), (1, 2), (6, 7), (5, 6), (0, 1), (null, 0) }
///     <br/>
///     - ...
///     </para>
/// </example>
public abstract class BreadthFirstTraversal<TEdge, TNode>
    : TreeTraversal<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
}

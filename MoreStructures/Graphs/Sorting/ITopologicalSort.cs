namespace MoreStructures.Graphs.Sorting;

/// <summary>
/// An algorithm performing a <b>topological sort</b> on the provided <see cref="IGraph"/>.
/// </summary>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     Given a DAG <c>G(V, E)</c>, where V is the set of vertices of G and E is the set of directed edges of G,
///     the topological sort of G, indicated here as TS, is a <see cref="IList{T}"/> of <see cref="int"/>, such that
///     <c>for all couples (v1, v2) of V^2, if there is a directed path from v1 to v2, then TS[v1] &lt; TS[v2]</c>. 
///     <br/>
///     The topological sort defines a <b>linear order</b> of the vertices V of the DAG G.
///     <br/>
///     The definition only makes sense when G is a DAG (Direct Acyclic Graph), because:
///     - a generic graph may have cycles, which make impossible to define a topological sort for any vertex on the 
///       cycle (since such vertex would be at the same time predecessor and successor of any other vertex of the 
///       cycle);
///       <br/>
///     - an undirected graph doesn't make distinction between the two different traversal directions of an edge, 
///       making each edge a cycle of two edges (1 edge if it is a loop).
///     </para>
/// </remarks>
public interface ITopologicalSort
{
    /// <summary>
    /// Performs the topological sort of the provided <paramref name="dag"/>.
    /// </summary>
    /// <param name="dag">The <see cref="IGraph"/> instance representing a DAG (Direct Acyclic Graph).</param>
    /// <returns>
    /// A list of the first n non-negative distinct integers, where n is the number of vertices in 
    /// <paramref name="dag"/>.
    /// </returns>
    /// <remarks>
    ///     <inheritdoc cref="ITopologicalSort"/>
    /// </remarks>
    IList<int> Sort(IGraph dag);
}

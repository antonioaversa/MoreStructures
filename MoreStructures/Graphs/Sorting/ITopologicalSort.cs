namespace MoreStructures.Graphs.Sorting;

/// <summary>
/// An algorithm performing a <b>topological sort</b> on the provided <see cref="IGraph"/>.
/// </summary>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     Given a DAG <c>G(V, E)</c>, where V is the set of vertices of G and E is the set of directed edges of G,
///     the topological sort of G, indicated here as TS, is a <see cref="IList{T}"/> of the first non-negative |V| 
///     <see cref="int"/> values, such that <c>for all couples (v1, v2) of V^2, if there is a directed path from v1 to 
///     v2, then TS[v1] &lt; TS[v2]</c>. 
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
///     <br/>
///     The topological sort is in general not unique, i.e. multiple orders are possible, which satisfy the definition
///     above. Look at the documentation of the <see cref="Sort(IGraph)"/> method for some examples.
///     </para>
/// </remarks>
public interface ITopologicalSort
{
    /// <summary>
    /// Performs the topological sort of the provided <paramref name="dag"/>.
    /// </summary>
    /// <param name="dag">The <see cref="IGraph"/> instance representing a DAG (Direct Acyclic Graph).</param>
    /// <returns>
    /// A list TS, of the first n non-negative distinct integers, where n is the number of vertices in 
    /// <paramref name="dag"/>. TS[i] represents the position of the vertex i in the topological sort.
    /// </returns>
    /// <remarks>
    ///     <inheritdoc cref="ITopologicalSort"/>
    /// </remarks>
    /// <example>
    /// The followin graph:
    /// <code>
    ///  0 --> 1 &lt;==&gt; 3
    /// | ^   ^     /
    /// | |  /     /
    /// | | /     /
    /// v |/     /
    ///  2 &lt;-----
    /// </code>
    /// is NOT a DAG and doesn't have any valid topological sort.
    /// <br/>
    /// <br/>
    /// The followin graph:
    /// <code>
    ///  0 --> 1 --&lt; 3
    ///  |          /
    ///  |         /
    ///  |        /
    ///  v       /
    ///  2 &lt;----
    /// </code>
    /// is a DAG and has a single valid topological sort order: <c>new[] { 0, 1, 3, 2 })</c>.
    /// <br/>
    /// <br/>
    /// The followin graph:
    /// <code>
    ///  0 --> 1 --&lt; 3
    ///  |
    ///  |
    ///  |
    ///  v
    ///  2
    /// </code>
    /// is a DAG and has two valid topological sort orders:
    /// <br/>
    /// - <c>new[] { 0, 1, 3, 2 })</c>;
    ///   <br/>
    /// - <c>new[] { 0, 2, 1, 3 })</c>.
    /// </example>
    IList<int> Sort(IGraph dag);
}

using MoreStructures.EdgeListGraphs;
using MoreStructures.Graphs;

namespace MoreStructures.AdjacencyListGraphs;

/// <summary>
/// A graph data structure, represented as an ordered list of neighborhoods: the i-th item of the list is the set of
/// ids of the vertices which are neighbors of the vertex with id i.
/// </summary>
/// <param name="Neighborhoods">
/// A list of sets of integers, each set representing the neighborhood of the corresponding vertex.
/// </param>
/// <remarks>
/// - If the graph can be considered undirected if all edges come in couples with both directions: i.e. when the 
///   neighborhoods list L is such that <c>if v2 belongs to L[v1], then v1 belongs to L[v2]</c>.
///   <br/>
/// - The size of this data structure is proportional to the number of edges of the graph, since a vertex has as many 
///   neighbors as edges connecting to other vertices (possibly including itself).
///   <br/>
/// - So, this graph representation is particularly useful when the number is edges is smaller or proportional to the 
///   number of vertices in the graph, i.e. when the graph is <b>sparse</b> (i.e. when e is O(v)).
///   <br/>
/// - It becomes an expensive representation when the graph is <b>dense</b> (i.e. when e is O(v^2)).
///   <br/>
/// - While having size proportional to the number of edges, <see cref="AdjacencyListGraph"/> is more convenient than 
///   <see cref="EdgeListGraph"/> to run neighborhood-based algorithms, such as discovery, because it makes easier 
///   and faster to get neighbors of a vertex.
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
/// is represented as <c>AdjacencyListGraph(new { new { 1, 2 }, new { 3 }, new { 0, 1 }, new { 1, 2 } })</c>.
/// </example>
public record AdjacencyListGraph(IList<ISet<int>> Neighborhoods) : IGraph
{
    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Because the neighborhoods list is indexed by the vertex id, the algorithm has just to perform a direct 
    ///       access to the <paramref name="start"/>-th item: that is the set of neighbors of the vertex with id 
    ///       <paramref name="start"/>.
    ///       <br/>
    ///     - When the value of <paramref name="takeIntoAccountEdgeDirection"/> is <see langword="false"/>, a lookup of
    ///       all neighborhoods is required.
    ///       <br/>
    ///     - In such case it would be better to have neighborhoods list already including bi-directional edges, and 
    ///       using <paramref name="takeIntoAccountEdgeDirection"/> = <see langword="true"/>. If not, the advantages of
    ///       having O(1) neighborhood lookup would be lost.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Direct access to the neighborhoods list is a constant time operation.
    ///       <br/>
    ///     - Therefore, when <paramref name="takeIntoAccountEdgeDirection"/> = <see langword="true"/>, Time and Space
    ///       Complexity (when enumerated) are O(avg_e), where avg_e is the average number of edges coming out of the 
    ///       <paramref name="start"/> vertex.
    ///       <br/>
    ///     - However, when <paramref name="takeIntoAccountEdgeDirection"/> = <see langword="false"/>, Time Complexity
    ///       becomes O(avg_e + v), where v is the number of vertices of the graph (i.e. the number of total 
    ///       neighborhoods defined).
    ///     </para>
    /// </remarks>
    public IEnumerable<(int vertex, (int edgeStart, int edgeEnd) edge)> GetAdjacentVerticesAndEdges(
        int start, bool takeIntoAccountEdgeDirection)
    {
        foreach (var end in Neighborhoods[start])
            yield return (end, (start, end));

        if (takeIntoAccountEdgeDirection)
            yield break;

        for (var i = 0; i < Neighborhoods.Count; i++)
        {
            if (i == start)
                continue;

            if (Neighborhoods[i].Contains(start))
                yield return (i, (i, start));
        }
    }
}

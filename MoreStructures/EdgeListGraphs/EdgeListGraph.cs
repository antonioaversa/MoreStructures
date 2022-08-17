using MoreStructures.AdjacencyListGraphs;
using MoreStructures.Graphs;

namespace MoreStructures.EdgeListGraphs;

/// <summary>
/// A graph data structure, represented as an unsorted list of unlabelled edges, connecting unlabelled vertices.
/// </summary>
/// <param name="NumberOfVertices">
/// The total number n of vertices in the graph, identified with ids from 0 to n - 1.
/// </param>
/// <param name="Edges">
/// The list of edges of the graph, each one represented as a couple of ids of the vertices which constitute the 
/// extremes of the edge. Edges can be considered as directional or not, depending on the scenario.
/// </param>
/// <remarks>
/// - If the edges are considered directional, i.e. (s, e) is considered as a different edge from (e, s), the resulting
///   graph is directed. Otherwise, the resulting graph is undirected.
///   <br/>
/// - The size of this data structure is proportional to the number of edges of the graph.
///   <br/>
/// - So, this graph representation is particularly useful when the number is edges is smaller or proportional to the 
///   number of vertices in the graph, i.e. when the graph is <b>sparse</b> (i.e. when e is O(v)).
///   <br/>
/// - It becomes an expensive representation when the graph is <b>dense</b> (i.e. when e is O(v^2)).
///   <br/>
/// - While having size proportional to the number of edges, <see cref="EdgeListGraph"/> is less convenient than 
///   <see cref="AdjacencyListGraph"/> to run neighborhood-based algorithms, such as discovery, because it makes more 
///   complex and slower to get neighbors of a vertex.
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
/// is represented as <c>EdgeListGraph(4, new { (0, 1), (0, 2), (1, 3), (2, 0), (2, 1), (3, 1), (3, 2) })</c>.
/// </example>
public record EdgeListGraph(int NumberOfVertices, IList<(int start, int end)> Edges) : IGraph
{
    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    /// In the <see cref="EdgeListGraph"/> representation, it's explicitely set in <see cref="NumberOfVertices"/>.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int GetNumberOfVertices() => NumberOfVertices;

    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Because the edge list is unsorted, and there is no "index" or additional data structure which can help
    ///       retrieving neighbors quickly, the algorithm has to linearly scan the entire edge list, looking for
    ///       neighbors.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Checking whether each edge is a neighbor or not only requires comparing the edge start, when taking into
    ///       account edge directions, or both start and end, when considering edges as undirected.
    ///       <br/>
    ///     - Both are constant time operations.
    ///       <br/>
    ///     - Therefore Time Complexity is O(e), where e is the number of edges, and Space Complexity is O(1).
    ///     </para>
    /// </remarks>
    public IEnumerable<(int vertex, (int edgeStart, int edgeEnd) edge)> GetAdjacentVerticesAndEdges(
        int start, bool takeIntoAccountEdgeDirection)
    {
        if (takeIntoAccountEdgeDirection)
        {
            foreach (var edge in Edges.Where(edge => edge.start == start).Select(edge => (edge.end, edge)))
                yield return edge;
            yield break;
        }

        foreach (var edge in Edges)
        {
            if (edge.start == start)
            {
                yield return (edge.end, edge);
                continue;
            }
            if (edge.end == start)
            {
                yield return (edge.start, edge);
            }
        }
    }
}

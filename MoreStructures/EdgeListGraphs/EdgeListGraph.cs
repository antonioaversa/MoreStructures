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
/// If the edges are considered directional, i.e. (s, e) is considered as a different edge from (e, s), the resulting
/// graph is directed. Otherwise, the resulting graph is undirected.
/// </remarks>
public record EdgeListGraph(int NumberOfVertices, IList<(int start, int end)> Edges) : IGraph
{
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

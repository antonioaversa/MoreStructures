namespace MoreStructures.Graphs;

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
/// - <b>This representation does support multigraphs</b>, i.e. graphs which can have multiple parallel edges 
///   between the same two vertices.
///   <br/>
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
    public IEnumerable<IGraph.Adjacency> GetAdjacentVerticesAndEdges(
        int start, bool takeIntoAccountEdgeDirection)
    {
        if (takeIntoAccountEdgeDirection)
        {
            var adjacencies = Edges
                .Where(edge => edge.start == start)
                .Select(edge => new IGraph.Adjacency(edge.end, edge.start, edge.end));
            foreach (var adjacency in adjacencies)
                yield return adjacency;
            yield break;
        }

        foreach (var edge in Edges)
        {
            if (edge.start == start)
            {
                yield return new(edge.end, edge.start, edge.end);
                continue;
            }
            if (edge.end == start)
            {
                yield return new(edge.start, edge.start, edge.end);
            }
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - An <see cref="IGraph"/> proxy is created, wrapping this instance of <see cref="IGraph"/>.
    ///       <br/>
    ///     - <see cref="IGraph.GetNumberOfVertices"/> is dispatched to the proxied graph.
    ///       <br/>
    ///     - <see cref="IGraph.GetAdjacentVerticesAndEdges(int, bool)"/> is directly implemented, accessing
    ///       <see cref="Edges"/> directly.
    ///       <br/>
    ///     - The implementation is very similar to the one of <see cref="EdgeListGraph"/>: the only difference
    ///       is that start and end vertices of each edge are inverted.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Since this method just creates a proxy, Time and Space Complexity are O(1).
    ///       <br/>
    ///     - All operations on the proxy have the same Time and Space Complexity as the corresponding methods in 
    ///       <see cref="EdgeListGraph"/>.
    ///     </para>
    /// </remarks>
    public IGraph Reverse() => new ReverseGraph(this);

    sealed private class ReverseGraph : ReverseProxyGraph<EdgeListGraph>
    {
        public ReverseGraph(EdgeListGraph graph) : base(graph)
        {
        }

        public override IEnumerable<IGraph.Adjacency> GetAdjacentVerticesAndEdges(
            int start, bool takeIntoAccountEdgeDirection)
        {
            var edges = Proxied.Edges;

            if (takeIntoAccountEdgeDirection)
            {
                var adjacencies = edges
                    .Where(edge => edge.end == start)
                    .Select(edge => new IGraph.Adjacency(edge.start, edge.end, edge.start));
                foreach (var adjacency in adjacencies)
                    yield return adjacency;
                yield break;
            }

            foreach (var edge in edges)
            {
                if (edge.end == start)
                {
                    yield return new(edge.start, edge.end, edge.start);
                }

                if (edge.start == start)
                {
                    yield return new(edge.end, edge.end, edge.start);
                    continue;
                }
            }
        }
    }
}

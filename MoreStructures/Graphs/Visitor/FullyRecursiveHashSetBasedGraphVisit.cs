namespace MoreStructures.Graphs.Visitor;

/// <summary>
/// A <see cref="IVisitStrategy"/> implementation which uses an <see cref="HashSet{T}"/> of <see cref="int"/> to store
/// already visited vertices, while visiting the graph, and performs the visit recursively.
/// </summary>
public class FullyRecursiveHashSetBasedGraphVisit : DirectionableVisit
{
    /// <summary>
    ///     <inheritdoc cref="FullyRecursiveHashSetBasedGraphVisit"/>
    /// </summary>
    /// <param name="directedGraph">
    ///     <inheritdoc/>
    /// </param>
    public FullyRecursiveHashSetBasedGraphVisit(bool directedGraph) : base(directedGraph)
    {
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    public override IEnumerable<int> DepthFirstSearch(IGraph graph)
    {
        var alreadyVisited = new HashSet<int>(); // Populated by RExplore
        var numberOfVertices = graph.GetNumberOfVertices();
        for (var vertex = 0; vertex < numberOfVertices; vertex++)
        {
            if (alreadyVisited.Contains(vertex))
                continue;

            foreach (var outputItem in RExplore(graph, alreadyVisited, vertex))
                yield return outputItem;
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="advantages">
    ///     ADVANTAGES AND DISADVANTAGES
    ///     <br/>
    ///     Implemented fully recursively, so limited by stack depth and usable with graphs of a "reasonable" size.
    ///     </para>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - A <see cref="HashSet{T}"/> of the ids of already visited vertices is instantiated to an empty set.
    ///       <br/>
    ///     - The set of already visited vertices is updated, adding the visited vertex, every time a visit is made.
    ///       <br/>
    ///     - The visit starts from the specified start vertex and looks for all the neighboring edges of such vertex
    ///       (taking into account the direction of the edge or not, depending on the specified parameters).
    ///       <br/>
    ///     - Only neighboring edges connecting the start vertex to a vertex not already visited are taken into account.
    ///       <br/>
    ///     - All other vertices are skipped, since those vertices, their neighborhood, neighbors of their neighborhood 
    ///       etc. have already been visited in previous steps of the recursive visit.
    ///       <br/>
    ///     - When the recursive visit terminates, all vertices directly or indirectly connected to the start vertex S
    ///       (i.e. all vertices V for which there is a path of edges e1, e2, ..., en, connecting S to V) will 
    ///       have been visited.
    ///       <br/>
    ///     - Vertices which are not connected to S (i.e. for which there is no path), are not included in the resulting
    ///       sequence of vertices.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Instantiating and adding items to the set of already visited vertices are constant-time operations.
    ///       <br/>
    ///     - The set of already visited vertices ensures that each node of the graph is visited at most once.
    ///       <br/>
    ///     - The complexity of retrieving the neighborhood of a vertex depends on the implementation of 
    ///       <see cref="IGraph.GetAdjacentVerticesAndEdges(int, bool)"/>, thus on the specific <see cref="IGraph"/>
    ///       implementation being used.
    ///       <br/>
    ///     - Checking whether a neighbor has already been visited is a O(1) operation, because the set used is an 
    ///       <see cref="HashSet{T}"/>.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(v * Ta) and Space Complexity is O(v * Sa), where e is the number of vertices
    ///       and Ta and Sa are the time and space cost of retrieving the neighborhood of a given vertex.
    ///     </para>
    /// </remarks>
    public override IEnumerable<int> Visit(IGraph graph, int start)
    {
        var alreadyVisited = new HashSet<int>(); // Populated by RExplore
        var lazyExploration = RExplore(graph, alreadyVisited, start);
        MoreLinq.MoreEnumerable.Consume(lazyExploration);
        return alreadyVisited;
    }

    private IEnumerable<int> RExplore(IGraph graph, HashSet<int> alreadyVisited, int start)
    {
        alreadyVisited.Add(start);
        yield return start;

        var unexploredVertices = graph
            .GetAdjacentVerticesAndEdges(start, DirectedGraph)
            .OrderBy(neighbor => neighbor.vertex);

        foreach (var (unexploredVertex, _) in unexploredVertices)
        {
            if (alreadyVisited.Contains(unexploredVertex))
                continue;

            foreach (var outputItem in RExplore(graph, alreadyVisited, unexploredVertex))
                yield return outputItem;
        }
    }
}

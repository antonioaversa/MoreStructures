namespace MoreStructures.Graphs.Visitor;

/// <summary>
/// A <see cref="IVisitStrategy"/> implementation which uses an <see cref="HashSet{T}"/> of <see cref="int"/> to store
/// already visited vertices, while visiting the graph, and performs the visit recursively.
/// </summary>
public class FullyRecursiveHashSetBasedGraphVisit : DirectionableVisit
{
    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc cref="FullyRecursiveHashSetBasedGraphVisit"/>
    /// </summary>
    public FullyRecursiveHashSetBasedGraphVisit(bool directedGraph) : base(directedGraph)
    {
    }

    /// <inheritdoc/>
    protected override IEnumerable<(int, int)> DepthFirstSearchAndConnectedComponentsOfGraph(IGraph graph)
    {
        var alreadyVisited = new HashSet<int>(); // Populated by RExplore
        var numberOfVertices = graph.GetNumberOfVertices();

        var currentConnectedComponent = 0;
        for (var vertex = 0; vertex < numberOfVertices; vertex++)
        {
            // Here RaiseAlreadyVisitedVertex should not be triggered, since the vertex is not been checked due to an
            // incoming edge from another vertex, but rather to explore vertices in connected components which haven't
            // been explored yet.
            if (alreadyVisited.Contains(vertex))
                continue;

            foreach (var outputItem in RDepthFirstSearchFromVertex(
                graph, alreadyVisited, vertex, currentConnectedComponent, null))
                yield return (outputItem, currentConnectedComponent);

            currentConnectedComponent++;
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
    ///     - An <see cref="HashSet{T}"/> of the ids of already visited vertices is instantiated to an empty set.
    ///       <br/>
    ///     - Vertices are iterated over, from the first (id = 0), to the last (id = v - 1, where v is the total number
    ///       of vertices).
    ///       <br/>
    ///     - The total number of vertices is retrieved via <see cref="IGraph.GetNumberOfVertices"/>.
    ///       <br/>
    ///     - If the vertex i has not already been visited (i.e. it appears in the <see cref="HashSet{T}"/>), it is
    ///       visited, with the same algorithm it would be visited by 
    ///       <see cref="DepthFirstSearchFromVertex(IGraph, int)"/>.
    ///       <br/>
    ///     - The order of visit is returned as a sequence of integers.
    ///       <br/>
    ///     - The set of already visited vertices is updated, adding the visited vertex, every time a visit is made, 
    ///       and it is shared by all visits of all vertices, whether they are connected to each other or not.
    ///       <br/>
    ///     - When the sequence of recursive visits terminates, all vertices of the graph will have been visited.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Instantiating and adding items to the set of already visited vertices are constant-time operations.
    ///       <br/>
    ///     - The set of already visited vertices ensures that each vertex of the graph is visited at most once.
    ///       <br/>
    ///     - To reach all vertices, the algorithm goes through all edges of the graph.
    ///       <br/>
    ///     - Because each vertex is visited at most once throughout the entire execution, edges are visited at most 
    ///       once, when edge direction is taken into account during the visit, and twice, when it is not.
    ///       <br/>
    ///     - The complexity of retrieving the total number of vertices depends on  
    ///       <see cref="IGraph.GetNumberOfVertices"/>. While being specific to the <see cref="IGraph"/>
    ///       implementation being used, all implementation provide O(1) runtime for such operation.
    ///       <br/>
    ///     - Checking whether a neighbor has already been visited is a O(1) operation, because the set used is an 
    ///       <see cref="HashSet{T}"/>.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(v * Ta + e) and Space Complexity is O(v * Sa + e), where 
    ///       v is the number of vertices, e is the number of edges and Ta and Sa are the time and space cost of 
    ///       retrieving the neighborhood of a given vertex.
    ///     </para>
    ///     <para id="complexity-and-events">
    ///     COMPLEXITY AND EVENTS
    ///     <br/>
    ///     - Event handlers are externally defined and have been considered O(1) in this analysis.
    ///       <br/>
    ///     - To include them in the analysis, it should be taken into account that 
    ///       <see cref="IVisitStrategy.VisitingVertex"/> and <see cref="IVisitStrategy.VisitedVertex"/> happen once 
    ///       per visited vertex, whereas <see cref="IVisitStrategy.AlreadyVisitedVertex"/> can happen globally as many
    ///       times as the number of edges.
    ///     </para>
    /// </remarks>
    public override IEnumerable<int> DepthFirstSearchOfGraph(IGraph graph) => base.DepthFirstSearchOfGraph(graph);

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
    ///     - The algorithm is a simple variation of <see cref="DepthFirstSearchOfGraph(IGraph)"/>.
    ///       <br/>
    ///     - All vertices from 0 to <see cref="IGraph.GetNumberOfVertices"/> - 1 are explored.
    ///       <br/>
    ///     - An <see cref="HashSet{T}"/> of already visited vertices is shared across all visits, to ensure that a
    ///       vertex is not visited twice.
    ///       <br/>
    ///     - A current value of the connected component is instantiated at 0 and incremented every time a new 
    ///       connected component is explored, i.e. every time the vertex i, of the top-level iteration, has not been
    ///       visited yet, meaning that none of the connected components explored so far contains it.
    ///       <br/>
    ///     - The resulting mapping between vertex id and connected component value is returned as result.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - The algorithm closely resembles <see cref="DepthFirstSearchOfGraph(IGraph)"/>, with the added complexity 
    ///       of instantiating and populating a <see cref="IDictionary{TKey, TValue}"/> of the mapping between vertices
    ///       and connected component labels.
    ///       <br/>
    ///     - Because the <see cref="IDictionary{TKey, TValue}"/> implementation used is a 
    ///       <see cref="Dictionary{TKey, TValue}"/>, which is hash-based, such additional operations are performed in
    ///       constant time.
    ///       <br/>
    ///     - Therefore the complexity of this method is the same as <see cref="DepthFirstSearchOfGraph(IGraph)"/>: 
    ///       Time Complexity is O(v * Ta + e) and Space Complexity is O(v * Sa + e), where v is the number of 
    ///       vertices, e is the number of edges and Ta and Sa are the time and space cost of retrieving the 
    ///       neighborhood of a given vertex.
    ///     </para>
    ///     <inheritdoc cref="DepthFirstSearchOfGraph" path="/remarks/para[@id='complexity-and-events']"/>
    /// </remarks>
    public override IDictionary<int, int> ConnectedComponents(IGraph graph) => base.ConnectedComponents(graph);

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
    ///     - Only neighboring edges connecting the start vertex to a vertex not already visited are taken into 
    ///       account.
    ///       <br/>
    ///     - However, to reach all relevant vertices, the algorithm may go through all edges of the graph.
    ///       <br/>
    ///     - All other vertices are skipped, since those vertices, their neighborhood, neighbors of their neighborhood 
    ///       etc. have already been visited in previous steps of the recursive visit.
    ///       <br/>
    ///     - When the recursive visit terminates, all vertices directly or indirectly connected to the start vertex S
    ///       (i.e. all vertices V for which there is a path of edges e1, e2, ..., en, connecting S to V) will 
    ///       have been visited.
    ///       <br/>
    ///     - Vertices which are not connected to S (i.e. for which there is no path), are not included in the 
    ///       resulting sequence of vertices.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Instantiating and adding items to the set of already visited vertices are constant-time operations.
    ///       <br/>
    ///     - The set of already visited vertices ensures that each vertex of the graph is visited at most once.
    ///       <br/>
    ///     - The complexity of retrieving the neighborhood of a vertex depends on the implementation of 
    ///       <see cref="IGraph.GetAdjacentVerticesAndEdges(int, bool)"/>, thus on the specific <see cref="IGraph"/>
    ///       implementation being used.
    ///       <br/>
    ///     - Checking whether a neighbor has already been visited is a O(1) operation, because the set used is an 
    ///       <see cref="HashSet{T}"/>.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(v * Ta + e) and Space Complexity is O(v * Sa + e), where v is the number 
    ///       of vertices, e is the number of edges and Ta and Sa are the time and space cost of retrieving the 
    ///       neighborhood of a given vertex.
    ///     </para>
    ///     <inheritdoc cref="DepthFirstSearchOfGraph" path="/remarks/para[@id='complexity-and-events']"/>
    /// </remarks>
    public override IEnumerable<int> DepthFirstSearchFromVertex(IGraph graph, int vertex)
    {
        var alreadyVisited = new HashSet<int>(); // Populated by RExplore
        var lazyExploration = RDepthFirstSearchFromVertex(
            graph, alreadyVisited, vertex, 0, null); // Single connected component "0"
        MoreLinq.MoreEnumerable.Consume(lazyExploration);
        return alreadyVisited;
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
    ///     - The algorithm closely resembles <see cref="DepthFirstSearchFromVertex(IGraph, int)"/> with one 
    ///       foundamental difference: <b>the recursive calls to neighbors of each vertex generate 
    ///       <see cref="IEnumerable{T}"/> which are iterated lazily and in parallel, one element of each 
    ///       <see cref="IEnumerator{T}"/> at a time</b>, up until all enumerators are done.
    ///       <br/>
    ///     - Let's take as an example a graph in which neighbors of vertex 0 are 1 and 2, neighbors of vertex 1 are 3 
    ///       and 4 and neighbors of vertex 2 are 5 and 4.
    ///       <br/>
    ///     - The visit of vertex 0 will first yield the vertex 0 itself.
    ///       <br/>
    ///     - It then yields the 1st element of the enumerable of BFS from vertex 1, which is the vertex 1 itself.
    ///       <br/>
    ///     - After that, it yields the 1st element of the enumerable of BFS from vertex 2, which is the vertex 2 
    ///       itself.
    ///       <br/>
    ///     - After that, since there are no other neighbors of 0, moves to the 2nd elements of each of the 
    ///       enumerators.
    ///       <br/>
    ///     - It yields the 2nd element of the enumerable of BFS from vertex 1, which is the vertex 3.
    ///       <br/>
    ///     - It then yields the 2nd element of the enumerable of BFS from vertex 2, which is the vertex 5.
    ///       <br/>
    ///     - Etc, until all enumerators are done (i.e. <see cref="System.Collections.IEnumerator.MoveNext"/> is
    ///       <see langword="false"/>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Same consideration about complexity as in <see cref="DepthFirstSearchFromVertex(IGraph, int)"/> apply.
    ///       What is different is just the order of visit, not edges or vertices visited.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(v * Ta + e) and Space Complexity is O(v * Sa + e), where v is the number 
    ///       of vertices, e is the number of edges and Ta and Sa are the time and space cost of retrieving the 
    ///       neighborhood of a given vertex.
    ///     </para>
    ///     <inheritdoc cref="DepthFirstSearchOfGraph" path="/remarks/para[@id='complexity-and-events']"/>
    /// </remarks>
    public override IEnumerable<int> BreadthFirstSearchFromVertex(IGraph graph, int vertex)
    {
        return BreadthFirstSearchFromVertices(graph, new[] { vertex });
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     The algorithm is very similar to <see cref="BreadthFirstSearchFromVertex(IGraph, int)"/>, with the only
    ///     difference that all vertices in the <paramref name="vertices"/> sequence are visited, in parallel, instead
    ///     of a single one.
    ///     <br/>
    ///     Because the vertices may or may not belong to different connected components, this algorithm returns "-1"
    ///     as connected component for all visited vertices.
    ///     <br/>
    ///     Time Complexity is O(v * Ta + e) and Space Complexity is O(v + e + Sa), exactly as for 
    ///     <see cref="BreadthFirstSearchFromVertex(IGraph, int)"/>, since in the worst case the entire graph has to be
    ///     explored.
    /// </remarks>
    public override IEnumerable<int> BreadthFirstSearchFromVertices(IGraph graph, IEnumerable<int> vertices)
    {
        var alreadyVisited = new HashSet<int>();
        var verticesEnumerators = vertices
            .Select(vertex => 
                RBreadthFirstSearchFromVertex(graph, vertex, alreadyVisited, -1, null, 0).GetEnumerator())
            .ToList();

        foreach (var (descendantVertex, _) in EnumerateInParallel(verticesEnumerators, 0))
            yield return descendantVertex;
    }

    private IEnumerable<int> RDepthFirstSearchFromVertex(
        IGraph graph, HashSet<int> alreadyVisited, int vertex, int connectedComponent, int? previousVertex)
    {
        RaiseVisitingVertex(new(vertex, connectedComponent, previousVertex));

        alreadyVisited.Add(vertex);
        yield return vertex;

        var neighbors = graph
            .GetAdjacentVerticesAndEdges(vertex, DirectedGraph)
            .OrderBy(neighbor => neighbor.Vertex);

        foreach (var (unexploredVertex, _, _) in neighbors)
        {
            if (alreadyVisited.Contains(unexploredVertex))
            {
                RaiseAlreadyVisitedVertex(new(unexploredVertex, connectedComponent, vertex));
                continue;
            }

            foreach (var outputItem in RDepthFirstSearchFromVertex(
                graph, alreadyVisited, unexploredVertex, connectedComponent, vertex))
                yield return outputItem;
        }

        RaiseVisitedVertex(new(vertex, connectedComponent, previousVertex));
    }

    private IEnumerable<(int value, int level)> RBreadthFirstSearchFromVertex(
        IGraph graph, int vertex, HashSet<int> alreadyVisited, int connectedComponent, int? previousVertex, int level)
    {
        if (alreadyVisited.Contains(vertex))
            yield break;

        RaiseVisitingVertex(new(vertex, connectedComponent, previousVertex));

        alreadyVisited.Add(vertex);
        yield return (vertex, level);

        var neighborsEnumerators = graph
            .GetAdjacentVerticesAndEdges(vertex, DirectedGraph)
            .OrderBy(neighbor => neighbor.Vertex)
            .Select(neighbor => RBreadthFirstSearchFromVertex(
                graph, neighbor.Vertex, alreadyVisited, connectedComponent, vertex, level + 1).GetEnumerator())
            .ToList();

        foreach (var descendantVertex in EnumerateInParallel(neighborsEnumerators, level + 1))
            yield return descendantVertex;

        RaiseVisitedVertex(new(vertex, connectedComponent, previousVertex));
    }

    private static IEnumerable<(int value, int level)> EnumerateInParallel(
        IList<IEnumerator<(int value, int level)>> enumerators, int level)
    {
        var enumeratorsMoveNext = new bool[enumerators.Count]; 
        var enumeratorsMoveNextCount = 0;
        for (var i = 0; i < enumerators.Count; i++)
        {
            enumeratorsMoveNext[i] = enumerators[i].MoveNext();
            if (enumeratorsMoveNext[i])
                enumeratorsMoveNextCount++;
        }

        var currentLevel = level;
        while (enumeratorsMoveNextCount > 0)
        {
            var skippedBecauseOfHigherLevel = 0;
            for (var i = 0; i < enumerators.Count; i++)
            {
                while (enumeratorsMoveNext[i] && enumerators[i].Current.level == currentLevel)
                {
                    yield return enumerators[i].Current;
                    enumeratorsMoveNext[i] = enumerators[i].MoveNext();

                    if (!enumeratorsMoveNext[i])
                        enumeratorsMoveNextCount--;
                }

                if (enumeratorsMoveNext[i] && enumerators[i].Current.level > currentLevel)
                    skippedBecauseOfHigherLevel++;
            }

            if (skippedBecauseOfHigherLevel == enumeratorsMoveNextCount)
                currentLevel++;
        }
    }
}

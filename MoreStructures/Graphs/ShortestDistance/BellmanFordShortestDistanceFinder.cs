using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Graphs.ShortestDistance;

using GraphDistances = IDictionary<(int, int), int>;
using BestPreviouses = Dictionary<int, (int distanceFromStart, int? previousVertex)>;

/// <summary>
/// An <see cref="IShortestDistanceFinder"/> implementation based on the Bellman-Ford algorithm.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Unlike <see cref="DijkstraShortestDistanceFinder"/>, this implementation doesn't require distances to be
///       non-negative and it also works in presence of negative cycles.
///       <br/>
///     - Unlike <see cref="DijkstraShortestDistanceFinder"/>, this algorithm doesn't require any external data 
///       structure (such as a <see cref="PriorityQueues.IPriorityQueue{T}"/> implementation: its runtime performance 
///       solely depends on the algorithm itself.
///       <br/>
///     - However, due to the generality of conditions in which it operates, it can't leverage the same performance as
///       the Dijkstra algorithm. So, if the graph doesn't have negative distances, or it can be reduce not to have
///       them, consider using <see cref="DijkstraShortestDistanceFinder"/> instead, for better performance.
///       <br/>
///     - In particolar it has a quadratic performance, instead of the linearithmic Time Complexity of Dijkstra on
///       graphs with non-negative distances or the linear complexity of "edge relaxation in topological order" on 
///       DAGs.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - As in the Dijkstra Algorithm, a dictionary BP, mapping each vertex to its currently known shortest distance 
///       from the start and the previous vertex in a path with such a distance is instantiated.
///       <br/>
///     - Then BP is initialized to only contains the start vertex s, which is a distance 0 from itself via an empty 
///       path.
///       <br/>
///     - After that, the main loop of algorithm is executed v times, where v is the number of edges in the graph.
///       <br/>
///     - At each iteration, all the edges in the graph are visited and <b>relaxed</b>.
///       <br/>
///     - Edge (u, v) relation is done in the following way: if the distance of u from s in BP is not defined, it is to
///       be considered as +Infinity. Therefore, there is no viable path from s to v via u, and no relation is 
///       possible via the edge (u, v).
///       <br/>
///     - If the distance BP[u].d of u from s in BP is defined instead, but the distance of v from s in BP is not,
///       the path going from s to v via u becomes the shortest knows, and is set into BP[v].
///       <br/>
///     - If both distances BP[u].d and BP[v].d of u and v from s in BP are defined, there are two possible cases.
///       <br/>
///     - Either BP[v].d is non-bigger than BP[u].d + the distance of (u, v), in which case the edge (u, v) won't
///       decrease the current estimate of the shortest path from s to v and won't be relaxed.
///       <br/>
///     - Or BP[v].d is strictly bigger, in which case the edge (u, v) does decrease the currently known shortest path 
///       from s to v, and will be relaxed, updating BP[v].
///       <br/>
///     - After v - 1 iterations, all edges are fully relaxed if there are no negative cycles. 
///       <br/>
///     - To check whether that's the case, a last, v-th iteration, is performed. If no edge is relaxed, there are no
///       negative cycles. Otherwise, there are. The set VR, of target vertices of edges relaxed at the v-th iteration,
///       is stored.
///       <br/>
///     - If there are negative cycles, each vertex v in VR, and each vertex reachable from v, has -Infinite distance
///       from the start. So a DFS is executed on the graph for each v in VR, and BP is updating, setting BP[v].d
///       to -Infinity and BP[v].previousVertex to null (since there is no actual finite shortest path).
///       <br/>
///     - If the end vertex is at a finite distance from the start, BP[e] contains such shortest distance, and the 
///       shortest path can be found by backtracking on previous pointers via BP, from e all the way back to s.
///       <br/>
///     - Otherwise -Infinity or +Infinity is returned, with an empty path, because either no path exists from the
///       start to the end, or a path exists, but the shortest is infinitely long.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - BP initialization is done in constant time.
///       <br/>
///     - The main loop of the algorithm is performed v times.
///       <br/>
///     - Each iteration checks and possibly relaxes all e edges of the graph. A single graph relation requires 
///       checking values in BP and edge distances, which are all constant-time operations.
///       <br/>
///     - Retrieving all e edges has a complexity which is specific to the <see cref="IGraph"/> implementation: in a
///       <see cref="EdgeListGraph"/> it is a O(1) operation, since edges are stored as a flat list.
///       <br/>
///     - In a <see cref="AdjacencyListGraph"/> it is a O(v) operation, since edges are stored in neighborhoods of the
///       v vertices.
///       <br/>
///     - In a <see cref="AdjacencyMatrixGraph"/> it is a O(v^2) operation, as it requires going through the matrix.
///       <br/>
///     - In case the presence of negative cycles is detected, up to r DFS explorations are performed, where r is the
///       number of vertices in VR (i.e. target of edges relaxed during the v-th iteration of the main loop).
///       <br/>
///     - In the worst case that means work proportional to v * (v + e), when r = v and assuming linear cost for DFS.
///       <br/>
///     - In case there are no negative cycles, up to v more iterations are performed to find the shortest path from
///       s to e.
///       <br/>
///     - In conclusion Time Complexity is O(v * (v * Tn + e)), where Tn is the time to retrieve the neighborhood of
///       a single vertex. Space Complexity is O(v + Sn), since BP contain at most v items, of constant size.
///     </para>
/// </remarks>
public class BellmanFordShortestDistanceFinder : IShortestDistanceFinder
{
    /// <summary>
    /// A building function able to instantiate the <see cref="IVisitStrategy"/> to be used to find all reachable
    /// vertices of vertices relaxed in the last iteration of the main loop of the Bellman-Ford algorithm, by running 
    /// a Depth First Searches from the start vertex via 
    /// <see cref="IVisitStrategy.DepthFirstSearchFromVertex(IGraph, int)"/>.
    /// </summary>
    public Func<IVisitStrategy> VisitStrategyBuilder { get; }

    /// <summary>
    ///     <inheritdoc cref="BellmanFordShortestDistanceFinder"/>
    /// </summary>
    /// <param name="visitStrategyBuilder">
    ///     <inheritdoc cref="VisitStrategyBuilder" path="/summary"/>
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="BellmanFordShortestDistanceFinder"/>
    /// </remarks>
    public BellmanFordShortestDistanceFinder(Func<IVisitStrategy> visitStrategyBuilder)
    {
        VisitStrategyBuilder = visitStrategyBuilder;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="BellmanFordShortestDistanceFinder"/>
    /// </remarks>
    public (int, IList<int>) Find(IGraph graph, GraphDistances distances, int start, int end)
    {
        var numberOfVertices = graph.GetNumberOfVertices();
        var bestPreviouses = new BestPreviouses { [start] = (0, null) };

        var verticesRelaxedInLastIteration = new HashSet<int>();
        for (var iteration = 1; iteration <= numberOfVertices; iteration++)
            RelaxEdges(graph, distances, numberOfVertices, bestPreviouses, iteration, verticesRelaxedInLastIteration);

        SetToMinusInfinity(graph, bestPreviouses, verticesRelaxedInLastIteration);

        if (!bestPreviouses.ContainsKey(end))
            return (int.MaxValue, Array.Empty<int>());

        var shortestDistance = bestPreviouses[end].distanceFromStart;
        if (shortestDistance == int.MinValue)
            return (int.MinValue, Array.Empty<int>());

        var shortestPath = BfsBasedShortestDistanceFinder.BuildShortestPath(end, bestPreviouses);

        return (shortestDistance, shortestPath);
    }

    private static void RelaxEdges(
        IGraph graph, GraphDistances distances, int numberOfVertices,
        BestPreviouses bestPreviouses, int iteration, HashSet<int> verticesRelaxedInLastIteration)
    {
        for (var source = 0; source < numberOfVertices; source++)
        {
            foreach (var (target, edgeStart, edgeEnd) in graph.GetAdjacentVerticesAndEdges(source, true))
            {
                if (!bestPreviouses.TryGetValue(source, out var sourceBest))
                    continue;

                var newTargetDistance = sourceBest.distanceFromStart + distances[(edgeStart, edgeEnd)];
                if (!bestPreviouses.TryGetValue(target, out var targetBest) ||
                    targetBest.distanceFromStart > newTargetDistance)
                {
                    if (iteration == numberOfVertices)
                        verticesRelaxedInLastIteration.Add(target);
                    else
                        bestPreviouses[target] = new(newTargetDistance, source);
                }
            }
        }
    }

    private void SetToMinusInfinity(IGraph graph, BestPreviouses bestPreviouses, 
        HashSet<int> verticesRelaxedInLastIteration)
    {
        if (verticesRelaxedInLastIteration.Count == 0)
            return;

        var visitor = VisitStrategyBuilder();
        foreach (var reachableVertex in visitor.BreadthFirstSearchFromVertices(graph, verticesRelaxedInLastIteration))
            bestPreviouses[reachableVertex] = new(int.MinValue, null);
    }
}

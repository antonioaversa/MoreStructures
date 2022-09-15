using MoreStructures.PriorityQueues;
using MoreStructures.PriorityQueues.Extensions;

namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// An <see cref="IShortestDistanceFinder"/> implementation based on the Dijkstra algorithm.
/// </summary>
/// <remarks>
///     <para id="requirements">
///     REQUIREMENTS
///     <br/>
///     - Dijkstra algorithm relies on the constraint that <b>edge distances are non-negative</b>. Therefore, there 
///       can't be negative loops and adding edges will always result in longer distances.
///       <br/>
///     - Moreover, given that the path P between two vertices u and v is optimal (i.e. the sum of distances over the
///       edges of the path is minimal), if w is a vertex in P, both the sub-path of P, P1, from u and w, and the 
///       sub-path P2, from w to v are optimal.
///       <br/>
///     - The algorithm takes advantage of that, by starting from the start vertex s and finding the shortest distance
///       for a single vertex v per iteration: the one minimizing the total distance from the start vertex, via any of
///       the vertices for which the shortest distance has already been calculated.
///       <br/>
///     - The vertex processed at each iteration v may not be in the optimal path from s to e. However, the optimal
///       path from s to v is surely optimal.
///       <br/>
///     - If e is reachable from s, it will be reached at some point in the traversal. Otherwise, the visit will stop
///       and a <see cref="int.MaxValue"/> distance will be returned instead.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - A dictionary BP, mapping each vertex to its currently known shortest distance from the start and the previous
///       vertex in a path with such a distance is instantiated and initialized to only contains the start vertex s, 
///       which is a distance 0 from itself via an empty path.
///       <br/>
///     - A set A of already added vertices is also instantiated and initialized to only contain s.
///       <br/>
///     - A priority queue PQ, storing ids of not yet added vertices by their known shortest total distance from s, is
///       also instantiated empty.
///       <br/>
///     - Then, the main loop of the algorithm is executed, until e is not in A.
///       <br/>
///     - Neighbors of the last vertex added to A, named hereafter lav, which are not yet in A, are discovered via 
///       <see cref="IGraph.GetAdjacentVerticesAndEdges(int, bool)"/> and iterated over.
///       <br/>
///     - If the distance d1, of a neighbor v from s via lav, is strictly smaller than the shortest distance from s to 
///       a known by BP, d2, both BP and PQ are updated with the new distance (PQ is updated via 
///       <see cref="UpdatablePriorityQueueExtensions.PushOrUpdate{T}(IUpdatablePriorityQueue{T}, T, int)"/>).
///       <br/>
///     - After all the neighbors of lav have been processed, the closest vertex to s non-in A is extracted via a 
///       <see cref="IPriorityQueue{T}.Pop"/> on PQ, if such a vertex exists.
///       <br/>
///     - If it does exist, such vertex is added to A and becomes the new lav. Otherwise, the loop is stopped.
///       <br/>
///     - After the loop terminates, if e is in BP, it means that a path, which is also shortest, from a to e has been
///       found, and can be reconstructed backwards by jumping links in BP.
///       <br/>
///     - Otherwise no path from s to e exists in the graph, and <see cref="int.MaxValue"/> and an empty path are 
///       returned as a result.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Initializations of the three data structures BP, A and PQ take a constant amount of work.
///       <br/>
///     - Each iteration in the main loop adds a new vertex to A, and only vertices not yet in A are processed.
///       <br/>
///     - Therefore, the number of iterations in the main loop is at most the number v of vertices in the graph.
///       <br/>
///     - At each iteration all edges outgoing from v are checked. 
///       <br/>
///     - Because in the worst case all vertices of the graph need to be explored, to find the shortest distance to e, 
///       the number of edges explored in total can be as high as the total number of edges e in the graph.
///       <br/>
///     - For each edge for which a shortest distance is found (in the worst case all e edges of the graph), both BP 
///       and PQ have to be updated with the new shortest distance.
///       <br/>
///     - Updating BP is done in O(1). Updating PQ, however, has a complexity which depends on the specific
///       <see cref="IUpdatablePriorityQueue{T}"/> implementation used.
///       <br/>
///     - As an example, if a <see cref="PriorityQueues.BinaryHeap.BinaryHeapPriorityQueue{T}"/> is used, updating PQ 
///       has logarithmic complexity over the number of items in PQ, which is at most v. So the processing of all 
///       edges, across all iterations, takes time proportional to e * log(v).
///       <br/>
///     - Moreover, after all neighbors of each vertex are processed, a <see cref="IPriorityQueue{T}.Pop"/> is done on 
///       PQ, to find the next vertex to add to A. This operation too is logarithmic with the number of items in PQ.
///       So the total cost of all pop operations, across all iterations, takes time proportional to v * log(v).
///       <br/>
///     - Therefore, when using a <see cref="PriorityQueues.BinaryHeap.BinaryHeapPriorityQueue{T}"/>, Time Complexity 
///       is O((v + e) * log(v)) and Space Complexity is O(v), since all structures contain at most v items, of 
///       constant size.
///       <br/>
///     - The Time Complexity may change when a different <see cref="IUpdatablePriorityQueue{T}"/> is used.
///       For instance, if a <see cref="PriorityQueues.FibonacciHeap.FibonacciHeapPriorityQueue{T}"/> is used, because
///       push and update operations are done in constant amortized time, the complexity is reduced to
///       O(e + v * log(v)), whereas when a <see cref="PriorityQueues.ArrayList.ArrayListPriorityQueue{T}"/> is used,
///       the complexity increases to O((v + e) * v).
///     </para>
/// </remarks>
public class DijkstraShortestDistanceFinder : IShortestDistanceFinder
{
    private Func<IUpdatablePriorityQueue<int>> PriorityQueueBuilder { get; }

    /// <inheritdoc cref="DijkstraShortestDistanceFinder"/>.
    /// <param name="priorityQueueBuilder">
    /// A builder of a <see cref="IUpdatablePriorityQueue{T}"/> of <see cref="int"/> values, used by the algorithm to
    /// store edges with priority from the closest to the start to the farthest.
    /// </param>
    public DijkstraShortestDistanceFinder(Func<IUpdatablePriorityQueue<int>> priorityQueueBuilder)
    {
        PriorityQueueBuilder = priorityQueueBuilder;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="DijkstraShortestDistanceFinder"/>
    /// </remarks>
    public (int, IList<int>) Find(IGraph graph, IDictionary<(int, int), int> distances, int start, int end)
    {
        var bestPreviouses = new Dictionary<int, (int distanceFromStart, int? previousVertex)>
        {
            [start] = (0, null),
        };
        var added = new HashSet<int>() { start };
        var vertexes = PriorityQueueBuilder();
        var lastAdded = start;
        while (lastAdded != end)
        {
            foreach (var (vertex, edgeStart, edgeEnd) in graph.GetAdjacentVerticesAndEdges(lastAdded, true))
            {
                if (added.Contains(vertex))
                    continue;

                var newDistance = bestPreviouses[lastAdded].distanceFromStart + distances[(edgeStart, edgeEnd)];
                if (!bestPreviouses.TryGetValue(vertex, out var bestPrevious) || 
                    bestPrevious.distanceFromStart > newDistance)
                {
                    bestPreviouses[vertex] = (newDistance, lastAdded);
                    vertexes.PushOrUpdate(vertex, -newDistance);
                }
            }

            if (vertexes.Count == 0)
                break;

            lastAdded = vertexes.Pop().Item;
            added.Add(lastAdded);
        }

        if (!bestPreviouses.ContainsKey(end))
            return (int.MaxValue, Array.Empty<int>());

        var shortestDistance = bestPreviouses[end].distanceFromStart;
        var shortestPath = BfsBasedShortestDistanceFinder.BuildShortestPath(end, bestPreviouses);

        return (shortestDistance, shortestPath);
    }
}

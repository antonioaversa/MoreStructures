using MoreStructures.PriorityQueues;
using MoreStructures.PriorityQueues.BinaryHeap;
using MoreStructures.PriorityQueues.Extensions;

namespace MoreStructures.Graphs.MinimumSpanningTree;

/// <summary>
/// A <see cref="IMstFinder"/> implementing Prim's algorithm.
/// </summary>
/// <remarks>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm closely resembles Dijkstra's algorithm for shortest distance finding. It is a gready algorithm
///       selecting a vertex and the corresponding edge at each iteration, and doing that efficiently thanks to the use
///       of a priority queue and a dictionary of mappings from vertices to edges of shortest distance.
///       <br/>
///     - The core idea is to split the vertices of the graph into two sets: the set A of vertices already added to the 
///       Minimum Spanning Tree MST, and the set NA of vertices which haven't been added yet to the MST.
///       <br/>
///     - At the beginning an arbitrary vertex (id = 0), is included in A and set as last added vertex LA, to bootstrap
///       the process. The set of edges representing the Minimum Spanning Tree, MST, is set to an empty set. The
///       dictionary mapping vertices to shortest distance edges BP is set to an empty dictionary. A min priority queue
///       of vertices, PQ, is also instantiated.
///       <br/>
///     - After that the main loop of the algorithm is executed, adding a single vertex LA to A and a single edge to 
///       MST at each iteration, and stopping only when A contains v vertices (i.e. when MST is spanning the entire 
///       graph).
///       <br/>
///     - At each iteration, all edges e, outgoing from and incoming into LA, and connecting LA to a vertex V not yet 
///       in A are checked.
///       <br/>
///     - If the distance of e, d(e), is strictly smaller than the smallest known edge for V, BP[V], BP is updated and
///       pushed or updated in PQ.
///       <br/>
///     - After all edges of LA are checked, the vertex V' connected to A via an edge E' of shortest distance is 
///       extracted from PQ, assigned to LA and added to A. E' is added to MST.
///       <br/>
///     - After v - 1 iterations, MST is returned as result.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - All data structures initialization (A, NA, PQ, BP, MST) takes constant amount of time.
///       <br/>
///     - During the v - 1 iterations, all edges are visited at most twice: once from the source side and once from the
///       target side.
///       <br/>
///     - While checking and updating BP and A take a constant amount of time, pushing or updating the priority in PQ
///       has a runtime which depends on the specific implementation of <see cref="IUpdatablePriorityQueue{T}"/> used.
///       <br/>
///     - The analysis of the impact of the priority queue on the overall performance of the algorithm is similar to 
///       the one done in <see cref="ShortestDistance.DijkstraShortestDistanceFinder"/>. Check that analysis for
///       further details.
///       <br/>
///     - In conclusion, Time Complexity is O(e + v * log(v)) with the best available implementation of an
///       <see cref="IUpdatablePriorityQueue{T}"/> and O((v + e) * e) with a naive implementation. 
///       <br/>
///     - Space Complexity is O(v), since all data structures contains O(v) items, all of constant size.
///     </para>
/// </remarks>
public class PrimMstFinder : IMstFinder
{
    private Func<IUpdatablePriorityQueue<int>> PriorityQueueBuilder { get; }

    /// <inheritdoc cref="PrimMstFinder"/>.
    /// <param name="priorityQueueBuilder">
    /// A builder of a <see cref="IUpdatablePriorityQueue{T}"/> of <see cref="int"/> values, used by the algorithm to
    /// store edges with priority from the closest to any of the vertices of the MST, to the farthest.
    /// </param>
    public PrimMstFinder(Func<IUpdatablePriorityQueue<int>> priorityQueueBuilder)
    {
        PriorityQueueBuilder = priorityQueueBuilder;
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// Finds the Minimum Spanning Tree (MST) of the provided <paramref name="graph"/> using the Prim's algorithm.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="PrimMstFinder"/>
    /// </remarks>
    public ISet<(int, int)> Find(IGraph graph, IGraphDistances distances)
    {
        var numberOfVertices = graph.GetNumberOfVertices();
        if (numberOfVertices == 0)
            return new HashSet<(int, int)>();

        var mst = new HashSet<(int, int)> { };

        var start = 0;
        var bestPreviouses = new Dictionary<int, (int edgeDistance, int edgeStart, int edgeEnd)>();

        var added = new HashSet<int> { start };
        var vertexes = PriorityQueueBuilder();
        var lastAdded = start;
        while (added.Count < numberOfVertices)
        {
            foreach (var (vertex, edgeStart, edgeEnd) in graph.GetAdjacentVerticesAndEdges(lastAdded, false))
            {
                if (added.Contains(vertex))
                    continue;

                var newDistance = distances[(edgeStart, edgeEnd)];
                if (!bestPreviouses.TryGetValue(vertex, out var bestPrevious) ||
                    bestPrevious.edgeDistance > newDistance)
                {
                    bestPreviouses[vertex] = (newDistance, edgeStart, edgeEnd);
                    vertexes.PushOrUpdate(vertex, -newDistance);
                }
            }

            if (vertexes.Count == 0)
                throw new InvalidOperationException("The graph is not connected.");

            lastAdded = vertexes.Pop().Item;
            added.Add(lastAdded);
            mst.Add((bestPreviouses[lastAdded].edgeStart, bestPreviouses[lastAdded].edgeEnd));
        }

        return mst;
    }
}

using MoreStructures.PriorityQueues;

namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// A <see cref="IShortestDistanceFinder"/> implementation based on a refinement of the Dijkstra algorithm, running
/// search in two parallel inversed flows: from the start vertext to the end vertex and viceversa.
/// </summary>
/// <remarks>
///     <para id="requirements">
///     REQUIREMENTS
///     <br/>
///     - Same as <see cref="DijkstraShortestDistanceFinder"/>.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm runs in parallel two searches: one from the start vertex s and the other one from the end 
///       vertex e.
///       <br/>
///     - The search from s is called <b>forward search</b> and is run on the provided directed graph, with the 
///       provided distance function, as it is done in a normal Dijkstra's algorithm execution.
///       <br/>
///     - The search from e is called <b>backward search</b> and is run on the reversed graph, with a reversed distance
///       function: <c>reversedDistance(edgeStart, edgeEnd) = distance(edgeEnd, edgeStart)</c>.
///       <br/>
///     - Each search has its own dictionary BP, set A, priority queue PQ and last added vertex LA, which are described
///       in detail in <see cref="DijkstraShortestDistanceFinder"/>. Data structures for the forward search are called
///       here BPf, Af, PQf and LAf. Data structures for the backward search are called here BPb, Ab, PQb, LAb.
///       <br/>
///     - After standard initialization of the two searches (which is different since they have different starting 
///       vertex, hence different A and BP initial content etc), the two searches are executed.
///       <br/>
///     - The "parallelism" is done by running a single step of one of the two searches at each iteration, and 
///       alternating between the two searches: first a step forward, then a step backward, then a step forward, etc.
///       <br/>
///     - The iterations continue until either a meeting point vertex v has been processed and added to both Af and Ab,
///       or both searches are done (i.e. no more edges to explore).
///       <br/>
///     - If the meeting point vertex v hasn't been found before the two searches would run out of edges to explore,
///       it means that e is not reachable from s, and <see cref="int.MaxValue"/> and an empty path are returned as 
///       result.
///       <br/>
///     - Otherwise, a path from s to e exists. However, such a path doesn't necessarily goes through v.
///       <br/>
///     - Therefore, the vertex u, of a shortest path from s to e and such that u is known to both BPf and BPb, with 
///       the correct  shortest path estimates in both directions, has to be found.
///       <br/>
///     - It can be proven that such a vertex u exists, which also belongs to Af or Ab or to both.
///       <br/>
///     - Therefore, all vertices in both BPf and BPf are scanned, and the one minimizing the sum of estimates, from s
///       and e respectively, is taken.
///       <br/>
///     - The full shortest path from s to e is then reconstructed by joining the two subpaths, going from s to u and
///       from u to e respectively.
///       <br/>
///     - The shortest path distance is simply the sum of shortest distances for u in BPf and BPb. 
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - The worst case analysis is the same as for <see cref="DijkstraShortestDistanceFinder"/>: in the worst 
///       scenario, all vertices have to be explored, before finding the meeting point or concluding that there is no
///       path from s to e.
///       <br/>
///     - Rebuilding the final path doesn't change the overall complexity, since calculating the intersection between
///       Af and Ab and finding the vertex minimizing the sum of estimates in BP are both linear time operations.
///       <br/>
///     - Therefore, Time Complexity is O((v + e) * v), O((v + e) * log(v)) or O(e + v * log(v)), depending on the
///       <see cref="IUpdatablePriorityQueue{T}"/> implementation chosen, as described in the documentation of 
///       <see cref="DijkstraShortestDistanceFinder"/>. Space Complexity remains O(v).
///       <br/>
///     - While it is true that running the search both from the start and from the end doesn't help asymptotic 
///       complexity in the worst case, it does help in the average case, i.e. on the expected number of iterations
///       before the algorithm stops adding vertices.
///       <br/>
///     - Intuitively, while a single run of Dijkstra's algorithm from s to e, at distance d = 2 * r from each other, 
///       has to explore an area of the graph proportional to Pi * d^2 = 4 * Pi * r^2, a bidirectional Dijkstra's
///       execution explores in average an area proportional to 2 * Pi * r^2. So, half of the area.
///     </para>
/// </remarks>
public class BidirectionalDijkstraShortestDistanceFinder : IShortestDistanceFinder
{
    private Func<IUpdatablePriorityQueue<int>> PriorityQueueBuilder { get; }

    /// <inheritdoc cref="BidirectionalDijkstraShortestDistanceFinder"/>.
    /// <param name="priorityQueueBuilder">
    /// A builder of a <see cref="IUpdatablePriorityQueue{T}"/> of <see cref="int"/> values, used by the algorithm to
    /// store edges with priority from the closest to the start, to the farthest.
    /// </param>
    public BidirectionalDijkstraShortestDistanceFinder(Func<IUpdatablePriorityQueue<int>> priorityQueueBuilder)
    {
        PriorityQueueBuilder = priorityQueueBuilder;
    }

    private sealed class Direction
    {
        public IGraph Graph { get; }
        public Func<int, int, int> DistancesFunc { get; }
        public BestPreviouses BestPreviouses { get; }
        public HashSet<int> Added { get; }
        public IUpdatablePriorityQueue<int> Vertexes { get; }

        public int LastAdded { get; private set; }
        public bool IsFinished { get; private set; }


        public Direction(
            IGraph graph, Func<int, int, int> distancesFunc, int endpoint, 
            Func<IUpdatablePriorityQueue<int>> priorityQueueBuilder)
        {
            Graph = graph;
            DistancesFunc = distancesFunc;
            BestPreviouses = new BestPreviouses(new() { [endpoint] = new(0, -1) });
            Added = new HashSet<int> { endpoint };
            Vertexes = priorityQueueBuilder();
            LastAdded = endpoint;
        }

        public int? OverlapsWith(Direction otherDirection)
        {
            if (Added.Contains(otherDirection.LastAdded))
                return otherDirection.LastAdded;

            if (otherDirection.Added.Contains(LastAdded))
                return LastAdded;
            
            return null;
        }

        public void RunStep()
        {
            if (IsFinished)
                return;

            ShortestDistanceFinderHelper.RelaxOutgoingEdgesOfVertex(
                Graph, DistancesFunc, BestPreviouses, Added, Vertexes, LastAdded);

            if (Vertexes.Count == 0)
            { 
                IsFinished = true;
                return;
            }

            LastAdded = Vertexes.Pop().Item;
            Added.Add(LastAdded);
        }

        public static int? FindMeetingPointVertexOnShortestPath(Direction first, Direction second) =>
            first.BestPreviouses.Values.Keys
                .Intersect(second.BestPreviouses.Values.Keys)
                .Cast<int?>()
                .MinBy(vertex => 
                    first.BestPreviouses.Values[vertex!.Value].DistanceFromStart + 
                    second.BestPreviouses.Values[vertex!.Value].DistanceFromStart);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="BidirectionalDijkstraShortestDistanceFinder"/>
    /// </remarks>
    public (int, IList<int>) Find(IGraph graph, IGraphDistances distances, int start, int end)
    {
        ShortestDistanceFinderHelper.ValidateParameters(graph, start, end);

        var distancesForwardFunc = (int edgeStart, int edgeEnd) => distances[(edgeStart, edgeEnd)];
        var forward = new Direction(graph, distancesForwardFunc, start, PriorityQueueBuilder);

        var reversedGraph = graph.Reverse();
        var distancesBackwardFunc = (int edgeStart, int edgeEnd) => distances[(edgeEnd, edgeStart)];
        var backward = new Direction(reversedGraph, distancesBackwardFunc, end, PriorityQueueBuilder);

        var forwardTurn = true;
        while (forward.OverlapsWith(backward) == null && !(forward.IsFinished && backward.IsFinished))
        {
            var direction = forwardTurn ? forward : backward;
            direction.RunStep();

            forwardTurn = !forwardTurn;
        }

        if (Direction.FindMeetingPointVertexOnShortestPath(forward, backward) is not int overlappingVertex)
            return (int.MaxValue, Array.Empty<int>());

        var forwardDistance = forward.BestPreviouses.Values[overlappingVertex].DistanceFromStart;
        var forwardPath = ShortestDistanceFinderHelper.BuildShortestPath(
            overlappingVertex, forward.BestPreviouses);

        var backwardDistance = backward.BestPreviouses.Values[overlappingVertex].DistanceFromStart;
        var backwardPath = ShortestDistanceFinderHelper.BuildShortestPath(
            overlappingVertex, backward.BestPreviouses, false);

        return (forwardDistance + backwardDistance, forwardPath.SkipLast(1).Concat(backwardPath).ToList());
    }
}

using MoreStructures.PriorityQueues;

namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// A <see cref="IShortestDistanceFinder"/> implementation based on the A* algorithm, which is a refinement of
/// the Dijkstra's algorithm, introducing a goal-oriented heuristic, driving the search in the direction of the end 
/// vertex.
/// </summary>
/// <remarks>
///     <para id="requirements">
///         <inheritdoc cref="DijkstraShortestDistanceFinder" path="/remarks/para[@id='requirements']"/>
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm is described in <see cref="DijkstraShortestDistanceFinder"/>, with the only difference that
///       edge distances are modified, based on a heuristic defined as a potential function.
///       <br/>
///     - New edge distance is defined as follow: given the potential function P, for each edge (u, v) in the graph, 
///       <c>d'(u, v) = d(u, v) + P(v) - P(u)</c>.
///       <br/>
///     - If P is defined correctly, P(u) and P(v) are good approximations of the distance of u and v from the end 
///       vertex e.
///       <br/> 
///     - If so, <c>P(v) - P(u)</c> will be negative if moving from u to v gets us closer to e and positive if it
///       gets us farther from it.
///       <br/>
///     - For this reason, given two vertices v' and v'' connected from u via <c>e' = (u, v')</c> and 
///       <c>e'' = (u, v'')</c>, and such that <c>d(e') = d(e'')</c>, if <c>P(v') &lt; P(v'')</c> then 
///       <c>d'(e') &lt; d''(e')</c>, so the algorithm will prefer e' over e'' during the exploration, as it seems to
///       be closer to e.
///       <br/>
///     - Because the algorithm stops when e is processed, if the algorithm visits e earlier than later, the algorithm 
///       will find the shortest path from s to e ealier than later.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - The complexity heavily depends on the accuracy of the potential function.
///       <br/>
///     - A good model of the average performance of the algorithm is very complicated to derive, since the heuristic 
///       can drive the exploration much quicker or slower towards the end vertex, depending on how the function is 
///       defined.
///       <br/>
///     - In general, potential functions which are closer to the actual shortest distance to the end vertex yield
///       better results. The farther they move from the ideal, the less optimized the exploration of the graph 
///       becomes.
///       <br/>
///     - Worst case remains as in <see cref="DijkstraShortestDistanceFinder"/>, where all vertices of the graph have
///       to be explored, for a path from the start to the end to be found (or prove there is no path, since start and
///       end lie in two different connected components).
///       <br/>
///     - Best case is when P is the shortest distance to e, in which case only the vertices of a shortest path from
///       s to e are visited (which is tipically a small fraction of the vertices of the graph, especially if the graph
///       is large). That is the bare minimum to find the shortest path from s to e.
///       <br/>
///     - Average case can even be worse than normal Dijkstra, if P is misleading, i.e. if it drives the exploration
///       away from e, rather than reducing the cost of edges which drives the exploration closer to e.
///       <br/>
///     - However, with a well defined P, close enough to the actual shortest distance, Time Complexity is between
///       O(e + v * log(v)) and O(h), where h is the highest number of edges of a shortest path from s to e.
///       <br/>
///     - Space Complexity is also between O(h) and O(v).
///     </para>
/// </remarks>
public class AStarShortestDistanceFinder : PotentialBasedShortestDistanceFinder
{
    /// <inheritdoc cref="AStarShortestDistanceFinder"/>
    /// <param name="priorityQueueBuilder">
    /// A builder of a <see cref="IUpdatablePriorityQueue{T}"/> of <see cref="int"/> values, used by the algorithm to
    /// store edges with priority from the closest to the start, to the farthest.
    /// </param>
    public AStarShortestDistanceFinder(Func<IUpdatablePriorityQueue<int>> priorityQueueBuilder)
        : base(new DijkstraShortestDistanceFinder(priorityQueueBuilder))
    {
    }
}

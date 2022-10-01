using MoreStructures.PriorityQueues;

namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// A <see cref="IPotentialBasedShortestDistanceFinder"/> implementation based on the bidirectional A* algorithm, 
/// which combines the goal-oriented heuristic of the A* algorithm and the double search approach of the bidirectional 
/// Dijkstra's algorithm.
/// </summary>
/// <remarks>
///     <para id="requirements">
///         <inheritdoc cref="AStarShortestDistanceFinder" path="/remarks/para[@id='requirements']"/>
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - Same as <see cref="AStarShortestDistanceFinder"/>, but with the use of the "bidirectional search approach"
///       used in <see cref="BidirectionalDijkstraShortestDistanceFinder"/> to run Dijkstra's algorithm.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - The bidirectional search cuts in half, in average, the exploration time of 
///       <see cref="AStarShortestDistanceFinder"/>.
///       <br/>
///     - That improves average runtime, but doesn't change worst case scenarios and best case scenarios, since in 
///       those scenarios the number of vertices to visit is fixed and equal to v and h, respectively, and irrespective
///       of whether the exploration is done unidirectionally or bidirectionally.
///       <br/>
///     - Therefore, Time Complexity is O(e + v * log(v)) in the worst case, O(h) in the best case and somewhere in 
///       between, still lower than <see cref="AStarShortestDistanceFinder"/>, in all other cases.
///       <br/>
///     - Space Complexity is between O(h) and O(v).
///     </para>
/// </remarks>
public class BidirectionalAStarShortestDistanceFinder : PotentialBasedShortestDistanceFinder
{
    /// <inheritdoc cref="BidirectionalAStarShortestDistanceFinder"/>
    /// <param name="priorityQueueBuilder">
    /// A builder of a <see cref="IUpdatablePriorityQueue{T}"/> of <see cref="int"/> values, used by the algorithm to
    /// store edges with priority from the closest to the start, to the farthest.
    /// </param>
    public BidirectionalAStarShortestDistanceFinder(Func<IUpdatablePriorityQueue<int>> priorityQueueBuilder)
        : base(new BidirectionalDijkstraShortestDistanceFinder(priorityQueueBuilder))
    {
    }
}

using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.PriorityQueues;

namespace MoreStructures.Graphs.ShortestDistanceTree;

/// <summary>
/// A <see cref="IShortestDistanceTreeFinder"/> implementation based on the Dijkstra algorithm.
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
///     - The algorithm closely resembles the one used in <see cref="DijkstraShortestDistanceFinder"/>, with the 
///       following differences. Check that algorithm for further information.
///       <br/>
///     - The main loop is only stopped when all vertices have been processed, instead of being stopped as soon as the
///       end vertex is found, and its shortest distance and path calculated.
///       <br/>
///     - The path from the start vertex is not reconstructed, since that would need to be done for each of the 
///       vertices of the graph, resulting in a worst case O(v^2) Space Complexity.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Same analysis as in <see cref="DijkstraShortestDistanceFinder"/>.
///       <br/>
///     - The fact that the algorithm doesn't stop when a specific vertex is found, but goes through all vertices, has
///       an impact on the average performance, but doesn't affect the worst case scenario, since in the worst case the
///       end vertex is the last vertex processed of the graph.
///       <br/>
///     - Therefore, as in single path Dijkstra's algorithm variant, Time Complexity is O(e + v * log(v)) with the
///       best available <see cref="IUpdatablePriorityQueue{T}"/> implementation 
///       (<see cref="PriorityQueues.FibonacciHeap.UpdatableFibonacciHeapPriorityQueue{T}"/>), O((e + v) * log(v)) when
///       using a <see cref="PriorityQueues.BinaryHeap.UpdatableBinaryHeapPriorityQueue{T}"/> and O((e + v) * v) when
///       using the naive implementation of <see cref="PriorityQueues.ArrayList.ArrayListPriorityQueue{T}"/>.
///       <br/>
///     - Space Complexity is O(v), since all auxiliary structures contain a number of items proportional to v.
///     </para>
/// </remarks>
public class DijkstraShortestDistanceTreeFinder : IShortestDistanceTreeFinder
{
    private Func<IUpdatablePriorityQueue<int>> PriorityQueueBuilder { get; }

    /// <inheritdoc cref="DijkstraShortestDistanceTreeFinder"/>.
    /// <param name="priorityQueueBuilder">
    /// A builder of a <see cref="IUpdatablePriorityQueue{T}"/> of <see cref="int"/> values, used by the algorithm to
    /// store edges with priority from the closest to the start, to the farthest.
    /// </param>
    public DijkstraShortestDistanceTreeFinder(Func<IUpdatablePriorityQueue<int>> priorityQueueBuilder)
    {
        PriorityQueueBuilder = priorityQueueBuilder;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="DijkstraShortestDistanceTreeFinder"/>
    /// </remarks>
    public BestPreviouses FindTree(IGraph graph, IGraphDistances distances, int start)
    {
        ShortestDistanceFinderHelper.ValidateParameters(graph, start, null);

        var bestPreviouses = new BestPreviouses(new() { [start] = new(0, -1) });
        var added = new HashSet<int>() { start };
        var vertexes = PriorityQueueBuilder();
        var numberOfVertices = graph.GetNumberOfVertices();
        var distancesFunc = (int edgeStart, int edgeEnd) => distances[(edgeStart, edgeEnd)];

        var lastAdded = start;
        while (added.Count < numberOfVertices)
        {
            ShortestDistanceFinderHelper.RelaxOutgoingEdgesOfVertex(
                graph, distancesFunc, bestPreviouses, added, vertexes, lastAdded);

            if (vertexes.Count == 0)
                break;

            lastAdded = vertexes.Pop().Item;
            added.Add(lastAdded);
        }

        return bestPreviouses;
    }
}

using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Graphs.ShortestDistanceTree;

using GraphDistances = IDictionary<(int, int), int>;

/// <summary>
/// A <see cref="IShortestDistanceFinder"/> implementation based on the Bellman-Ford algorithm.
/// </summary>
/// <remarks>
///     <para id="requirements">
///     REQUIREMENTS
///     <br/>
///     - Same as <see cref="BellmanFordShortestDistanceFinder"/>.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm closely resembles the one used in <see cref="BellmanFordShortestDistanceFinder"/>, with the 
///       only main difference that paths from the start vertex are reconstructed, since that would need to be done 
///       for each of the vertices of the graph, resulting in a worst case O(v^2) Space Complexity.
///       <br/>
///     - Unlike <see cref="DijkstraShortestDistanceTreeFinder"/>, which can improve w.r.t. 
///       <see cref="DijkstraShortestDistanceFinder"/> on the average runtime, while the worst case is still bound to
///       the total number of vertices in the graph, <see cref="BellmanFordShortestDistanceTreeFinder"/> cannot provide
///       the same average running time improvement over <see cref="BellmanFordShortestDistanceFinder"/>, since the
///       Bellman-Ford algorithm still requires v - 1 iterations to find optimal distances from the start vertex, and
///       one iteration more, to be able to identify negative cycles and set shortest distance to minus infinity, for 
///       all vertices reachable from a negative cycle.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Same analysis as in <see cref="BellmanFordShortestDistanceFinder"/>.
///       <br/>
///     - The fact that the algorithm doesn't reconstruct shortest paths doesn't reduce the overall complexity, which
///       is bound to the v iterations of the main loop of the algorithm, each one relaxing at most e edges.
///       <br/>
///     - Therefore, as in single path Bellman-Ford's algorithm variant, Time Complexity is O(v * (v * Tn + e)) and
///       Space Complexity is O(v + Sn), where Tn and Sn are the time and space to retrieve the neighborhood of a 
///       single vertex.
///     </para>
/// </remarks>
public class BellmanFordShortestDistanceTreeFinder : IShortestDistanceTreeFinder
{
    /// <summary>
    /// A building function able to instantiate the <see cref="IVisitStrategy"/> to be used to find all reachable
    /// vertices of vertices relaxed in the last iteration of the main loop of the Bellman-Ford algorithm, by running 
    /// a Depth First Searches from the start vertex via 
    /// <see cref="IVisitStrategy.DepthFirstSearchFromVertex(IGraph, int)"/>.
    /// </summary>
    public Func<IVisitStrategy> VisitStrategyBuilder { get; }

    /// <summary>
    ///     <inheritdoc cref="BellmanFordShortestDistanceTreeFinder"/>
    /// </summary>
    /// <param name="visitStrategyBuilder">
    ///     <inheritdoc cref="VisitStrategyBuilder" path="/summary"/>
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="BellmanFordShortestDistanceFinder"/>
    /// </remarks>
    public BellmanFordShortestDistanceTreeFinder(Func<IVisitStrategy> visitStrategyBuilder)
    {
        VisitStrategyBuilder = visitStrategyBuilder;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="BellmanFordShortestDistanceFinder"/>
    /// </remarks>
    public BestPreviouses FindTree(IGraph graph, GraphDistances distances, int start)
    {
        ShortestDistanceFinderHelper.ValidateParameters(graph, start, null);

        var numberOfVertices = graph.GetNumberOfVertices();
        var bestPreviouses = new BestPreviouses(new() { [start] = new(0, -1) });

        var verticesRelaxedInLastIteration = new HashSet<int>();
        for (var iteration = 1; iteration <= numberOfVertices; iteration++)
            RelaxEdges(graph, distances, numberOfVertices, bestPreviouses, iteration, verticesRelaxedInLastIteration);

        SetToMinusInfinity(graph, bestPreviouses, verticesRelaxedInLastIteration, VisitStrategyBuilder);
        return bestPreviouses;
    }

    private static void RelaxEdges(
        IGraph graph, GraphDistances distances, int numberOfVertices,
        BestPreviouses bestPreviouses, int iteration, HashSet<int> verticesRelaxedInLastIteration)
    {
        for (var source = 0; source < numberOfVertices; source++)
        {
            foreach (var (target, edgeStart, edgeEnd) in graph.GetAdjacentVerticesAndEdges(source, true))
            {
                if (!bestPreviouses.Values.TryGetValue(source, out var sourceBest))
                    continue;

                var newTargetDistance = sourceBest.DistanceFromStart + distances[(edgeStart, edgeEnd)];
                if (!bestPreviouses.Values.TryGetValue(target, out var targetBest) ||
                    targetBest.DistanceFromStart > newTargetDistance)
                {
                    if (iteration == numberOfVertices)
                        verticesRelaxedInLastIteration.Add(target);
                    else
                        bestPreviouses.Values[target] = new(newTargetDistance, source);
                }
            }
        }
    }

    private static void SetToMinusInfinity(IGraph graph, BestPreviouses bestPreviouses,
        HashSet<int> verticesRelaxedInLastIteration, Func<IVisitStrategy> visitStrategyBuilder)
    {
        if (verticesRelaxedInLastIteration.Count == 0)
            return;

        var visitor = visitStrategyBuilder();
        foreach (var reachableVertex in visitor.BreadthFirstSearchFromVertices(graph, verticesRelaxedInLastIteration))
            bestPreviouses.Values[reachableVertex] = new(int.MinValue, -1);
    }
}

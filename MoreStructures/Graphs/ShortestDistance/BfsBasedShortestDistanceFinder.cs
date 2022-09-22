using MoreLinq.Extensions;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// An <see cref="IShortestDistanceFinder"/> which runs a BFS on the provided graph from the start vertex, to find the
/// shortest distance and a shortest path to the end vertex.
/// </summary>
/// <remarks>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - First, a <see cref="IVisitStrategy"/> is instantiated via the <see cref="VisitStrategyBuilder"/> provided in
///       the constructor.
///       <br/>
///     - The <see cref="IVisitStrategy"/> instance will be used to run 
///       <see cref="IVisitStrategy.BreadthFirstSearchFromVertex"/> from the start vertex.
///       <br/>
///     - Unlike the Dijkstra algorithm, the BFS visit will traverse all vertices reachable from the start vertex by
///       breadth. Morever it never stops the BFS traversal, even when the current path can't possibly be shortest.
///       <br/>
///     - The visitor is instrumented with an event handler H, attached to both 
///       <see cref="IVisitStrategy.VisitingVertex"/> and <see cref="IVisitStrategy.AlreadyVisitedVertex"/>.
///       <br/>
///     - H updates a dictionary SPs mapping each vertex v to its currently known shortest path SP from the start 
///       vertex to v and the previous vertex of v in SP.
///       <br/>
///     - H also keeps a dictionary DSs mapping each vertex v to the <see cref="HashSet{T}"/> of its currently visited
///       downstream vertices, i.e. all vertices u visited from v during the BFS.
///       <br/>
///     - Every time H encounters a vertex v which is not yet in SPs (i.e. it doesn't have a registered shortest path
///       yet), it just adds it to SPs.
///       <br/>
///     - Every time H encounters a vertex v which is already is SPs (i.e. it has already a registered shortest path),
///       it adds it to SPs and then updates recursively the entries of SPs of all downstream vertices, using DSs.
///       <br/>
///     - After the BFS visit is concluded, there are two possible scenarios: either the end vertex has been discovered
///       by the visit or not.
///       <br/>
///     - If the end vertex has not been discovered, it means that there is no path at all from the start to the end
///       vertex. In that case, <see cref="int.MaxValue"/> and an emtpy <see cref="IList{T}"/> are returned as result.
///       <br/>
///     - If the end vertex has been discovered, <c>SPs[end]</c> gives the shortest distance. SPs can be navigated 
///       backwards, until the start vertex, to find the path with the shortest distance.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - While building the <see cref="IVisitStrategy"/> instance has a complexity that depends on the user-provided 
///       <see cref="VisitStrategyBuilder"/>, it doesn't generally depends on the size of the graph to be visited, and 
///       can be considered O(1) work.
///       <br/>
///     - Instrumenting the <see cref="IVisitStrategy"/> instance is also a constant-time operation.
///       <br/>
///     - Running the handler for every visited and already visited vertex, however, requires updating vertices 
///       downstream, every time an entry SPs is updated, i.e. when a smaller distance from the start is found. That
///       affects the performance of the BFS execution, increasing its Time Complexity.
///       <br/>
///     - Because the BFS doesn't stop when the end vertex is found (as there may be longer paths with shorter distance
///       from the start vertex), the downstream vertices to be recursively updated can cover then entire graph (i.e.
///       O(v) vertices).
///       <br/>
///     - The complexity of the BFS visit depends on the actual <see cref="IVisitStrategy"/> implementation used.
///       <br/>
///     - Rebuilding and reversing the path takes time and space proportional to the length of the path, which in the 
///       worst case can be O(v) where v is the number of vertices of the graph.
///       <br/>
///     - Therefore, Time Complexity is O(Tbfs * v + v) and Space Complexity is O(Sbfs + v^2).
///       <br/>
///     - If <see cref="FullyIterativeHashSetBasedGraphVisit"/> or <see cref="FullyRecursiveHashSetBasedGraphVisit"/>
///       visits are used, with an <see cref="IGraph"/> retrieving neighbors in constant time, Time Complexity becomes
///       O((v + e) * v) and O(v^2 + e).
///       <br/>
///     - On dense graphs, Time Complexity is O(v^3) and Space Complexity is O(v^2).
///     </para>
/// </remarks>
public class BfsBasedShortestDistanceFinder : IShortestDistanceFinder
{
    /// <summary>
    /// A building function able to instantiate the <see cref="IVisitStrategy"/> to be used to find the shortest 
    /// distance, by running a Breadth First Searches from the start vertex via 
    /// <see cref="IVisitStrategy.BreadthFirstSearchFromVertex(IGraph, int)"/>.
    /// </summary>
    public Func<IVisitStrategy> VisitStrategyBuilder { get; }

    /// <summary>
    ///     <inheritdoc cref="BfsBasedShortestDistanceFinder"/>
    /// </summary>
    /// <param name="visitStrategyBuilder">
    ///     <inheritdoc cref="VisitStrategyBuilder" path="/summary"/>
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="BfsBasedShortestDistanceFinder"/>
    /// </remarks>
    public BfsBasedShortestDistanceFinder(Func<IVisitStrategy> visitStrategyBuilder)
    {
        VisitStrategyBuilder = visitStrategyBuilder;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="BfsBasedShortestDistanceFinder"/>
    /// </remarks>
    public (int, IList<int>) Find(IGraph graph, IDictionary<(int, int), int> distances, int start, int end)
    {
        ShortestDistanceFinderHelper.ValidateParameters(graph, start, end);

        var bestPrevious = new Dictionary<int, (int distanceFromStart, int? previousVertex)>
        {
            [start] = (0, null),
        };
        var downstreamVertices = new Dictionary<int, HashSet<int>>();

        var visitor = VisitStrategyBuilder();
        visitor.VisitingVertex += 
            (o, e) => UpdateBestPreviousAndDownstreamVertices(o, e, distances, bestPrevious, downstreamVertices);
        visitor.AlreadyVisitedVertex += 
            (o, e) => UpdateBestPreviousAndDownstreamVertices(o, e, distances, bestPrevious, downstreamVertices);

        visitor
            .BreadthFirstSearchFromVertex(graph, start)
            .Consume();

        if (!bestPrevious.ContainsKey(end))
            return (int.MaxValue, Array.Empty<int>());

        var shortestDistance = bestPrevious[end].distanceFromStart;
        var shortestPath = BuildShortestPath(end, bestPrevious);

        return (shortestDistance, shortestPath);
    }

    private static void UpdateBestPreviousAndDownstreamVertices(
        object? sender, VisitEventArgs eventArgs, IDictionary<(int, int), int> distances, 
        Dictionary<int, (int distanceFromStart, int? previousVertex)> bestPrevious, 
        Dictionary<int, HashSet<int>> downstreamVertices)
    {
        var current = eventArgs.Vertex;
        if (eventArgs.PreviousVertex is not int previous)
            return;

        if (!downstreamVertices.ContainsKey(previous))
            downstreamVertices[previous] = new HashSet<int> { current };
        else
            downstreamVertices[previous].Add(current);

        var distanceFromStartToPrevious = bestPrevious[previous].distanceFromStart;
        var distanceFromPreviousToCurrent = distances[(previous, current)];
        var distanceFromStartToCurrent = distanceFromStartToPrevious + distanceFromPreviousToCurrent;

        if (!bestPrevious.ContainsKey(current))
        {
            bestPrevious[current] = (distanceFromStartToCurrent, previous);
        }
        else if (bestPrevious[current].distanceFromStart > distanceFromStartToCurrent)
        {
            bestPrevious[current] = (distanceFromStartToCurrent, previous);

            // Update all vertices downstream (if any), which have already entries in bestPrevious
            if (downstreamVertices.ContainsKey(current))
                foreach (var downstreamVertex in downstreamVertices[current])
                    UpdateBestPreviousAndDownstreamVertices(
                        sender, new(downstreamVertex, eventArgs.ConnectedComponent, current), 
                        distances, bestPrevious, downstreamVertices);
        }
    }

    internal static IList<int> BuildShortestPath(
        int end, 
        Dictionary<int, (int distanceFromStart, int? previousVertex)> bestPrevious)
    {
        var shortestPath = new List<int>();

        int? maybeCurrent = end;
        while (maybeCurrent is int current)
        {
            shortestPath.Add(current);
            maybeCurrent = bestPrevious[current].previousVertex;
        }

        shortestPath.Reverse();
        return shortestPath;
    }
}

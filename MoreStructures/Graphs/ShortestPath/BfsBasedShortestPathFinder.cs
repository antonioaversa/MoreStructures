using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Graphs.ShortestPath;

/// <summary>
/// An implementation of <see cref="IShortestPathFinder"/> which performs a BFS traversal, starting from the start 
/// vertex, to find the shortest path to the end vertex.
/// </summary>
public class BfsBasedShortestPathFinder : IShortestPathFinder
{
    /// <summary>
    /// A building function able to instantiate the <see cref="IVisitStrategy"/> to be used to find the shortest path, 
    /// by running a Breadth First Searches from the start vertex via 
    /// <see cref="IVisitStrategy.BreadthFirstSearchFromVertex(IGraph, int)"/>.
    /// </summary>

    public Func<IVisitStrategy> VisitStrategyBuilder { get; }

    /// <summary>
    ///     <inheritdoc cref="BfsBasedShortestPathFinder"/>
    /// </summary>
    /// <param name="visitStrategyBuilder">
    ///     <inheritdoc cref="VisitStrategyBuilder" path="/summary"/>
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="BfsBasedShortestPathFinder"/>
    /// </remarks>
    public BfsBasedShortestPathFinder(Func<IVisitStrategy> visitStrategyBuilder)
    {
        VisitStrategyBuilder = visitStrategyBuilder;
    }

    /// <inheritdoc/>
    public (int, IList<int>) Find(IGraph graph, int start, int end)
    {
        var visitor = VisitStrategyBuilder();
        
        var previousVertices = new Dictionary<int, int?>();
        visitor.VisitingVertex += (o, e) => previousVertices[e.Vertex] = e.PreviousVertex;

        var visit = visitor.BreadthFirstSearchFromVertex(graph, start);
        MoreLinq.MoreEnumerable.Consume(visit.TakeWhile(vertex => vertex != end));
        
        if (!previousVertices.ContainsKey(end))
            return (int.MaxValue, new List<int> { });

        // Rebuild path from previousVertices dictionary
        var path = new List<int>();
        int? maybeCurrentVertex = end;
        while (maybeCurrentVertex is int currentVertex)
        {
            path.Add(currentVertex);
            maybeCurrentVertex = previousVertices[currentVertex];
        }
        path.Reverse();

        return (path.Count - 1, path);
    }
}

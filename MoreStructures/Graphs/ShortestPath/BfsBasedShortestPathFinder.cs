using MoreLinq.Extensions;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Graphs.ShortestPath;

/// <summary>
/// An implementation of <see cref="IShortestPathFinder"/> which performs a BFS traversal, starting from the start 
/// vertex, to find the shortest path to the end vertex.
/// </summary>
/// <remarks>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - A <see cref="IVisitStrategy"/> is instantiated via the <see cref="VisitStrategyBuilder"/> provided in the 
///       constructor.
///       <br/>
///     - The <see cref="IVisitStrategy"/> instance is instrumented, by adding an event handler H to the 
///       <see cref="IVisitStrategy.VisitingVertex"/>.
///       <br/>
///     - The event handler H maps each visited vertex to its previous vertex (i.e. the vertex whose visit has trigger
///       the visit of this vertex), as stored in <see cref="VisitEventArgs.PreviousVertex"/> passed by the 
///       <see cref="IVisitStrategy.VisitingVertex"/> event, into a <see cref="IDictionary{TKey, TValue}"/> D.
///       <br/>
///     - A Breadth First Search from the start vertex is executed, running 
///       <see cref="IVisitStrategy.BreadthFirstSearchFromVertex"/>. The result of the visit is consumed (i.e. the 
///       visit is done) until either the end vertex is encountered, or when there are no more vertices reachable from 
///       the start vertex, which haven't been visited yet.
///       <br/>
///     - If the end vertex hasn't been discovered during the visit, it is not reachable. Therefore, a length of path 
///       equal to <see cref="int.MaxValue"/> and an empty path are returned.
///       <br/>
///     - If the end vertex has been reached, the end vertex has been visited and it is contained in D.
///       <br/>
///     - D is traversed backwards, from the end to the start vertex, rebuilding the (reversed) path.
///       <br/>
///     - Finally, the path is reversed and returned as result. 
///       <br/>
///     - Its length minus 1 represents the length of the path between start and end vertex.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - While building the <see cref="IVisitStrategy"/> instance has a complexity that depends on the user-provided 
///       <see cref="VisitStrategyBuilder"/>, it doesn't generally depends on the size of the graph to be visited, and 
///       can be considered O(1) work.
///       <br/>
///     - Instrumenting the <see cref="IVisitStrategy"/> instance and running the handler for every visited vertex are
///       also constant-time operations.
///       <br/>
///     - The complexity of the BFS visit depends on the actual <see cref="IVisitStrategy"/> implementation used.
///       <br/>
///     - Rebuilding and reversing the path takes time and space proportional to the length of the path, which in the 
///       worst case can be O(v) where v is the number of vertices of the graph.
///       <br/>
///     - Therefore, Time Complexity is O(Tbfs + v) and Space Complexity is O(Sbfs + v).
///       <br/>
///     - If <see cref="FullyIterativeHashSetBasedGraphVisit"/> or <see cref="FullyRecursiveHashSetBasedGraphVisit"/>
///       visits are used, with an <see cref="IGraph"/> retrieving neighbors in constant time, Time and Space 
///       Complexity become O(v + e).
///     </para>
/// </remarks>
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

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="BfsBasedShortestPathFinder"/>
    /// </remarks>
    public (int, IList<int>) Find(IGraph graph, int start, int end)
    {
        var visitor = VisitStrategyBuilder();

        var previousVertices = new Dictionary<int, int?>();
        visitor.VisitingVertex += (o, e) => previousVertices[e.Vertex] = e.PreviousVertex;

        visitor
            .BreadthFirstSearchFromVertex(graph, start)
            .TakeWhile(vertex => vertex != end)
            .Consume();

        if (!previousVertices.ContainsKey(end))
            return (int.MaxValue, new List<int> { });

        var path = BuildShortestPath(end, previousVertices);

        return (path.Count - 1, path);
    }

    private static IList<int> BuildShortestPath(int end, Dictionary<int, int?> previousVertices)
    {
        var path = new List<int>();
        int? maybeCurrentVertex = end;
        while (maybeCurrentVertex is int currentVertex)
        {
            path.Add(currentVertex);
            maybeCurrentVertex = previousVertices[currentVertex];
        }
        path.Reverse();
        return path;
    }
}

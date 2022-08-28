using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Graphs.Sorting;

/// <summary>
/// An <see cref="ITopologicalSort"/> implementation which assigns topological order to vertices by identifing a sink 
/// vertex at each iteration <b>in a deterministic way</b>, by running DFS once on the entire graph and storing the 
/// post order.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - This implementation is a conceptual improvement over both <see cref="AnyPathToSinkBasedTopologicalSort"/> and
///       <see cref="DfsOnEachVertexSinkBasedTopologicalSort"/>.
///       <br/>
///     - The optimization is based on the observation that a lot of the DFS traversal, done by 
///       <see cref="DfsOnEachVertexSinkBasedTopologicalSort"/> to populate its "reachability dictionary", is shared 
///       between many of the vertices connected together, and doesn't need to be repeated every time.
///       <br/>
///     - For example, in a simple graph <c>A1 -> A2 -> ... -> An</c>, the DFS on A1 and the DFS on A2 share the path 
///       <c>A2 -> ... -> An</c>, the DFS on A1 and A3 share the path <c>A3 -> ... -> An</c>, etc.
///       <br/>
///     - While <see cref="AnyPathToSinkBasedTopologicalSort"/> doesn't explicitely use a 
///       <see cref="IVisitStrategy.DepthFirstSearchFromVertex"/>, its strategy can be seen as a DFS traversal if
///       the initial vertex and the path followed at each iteration are taken consistently.
///       <br/>
///     - Therefore, this implementation is an improvement of both previously mentioned implementations.
///       <br/>
///     - Moreover, if the DFS traversal is deterministic, this algorithm also is.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - A full DFS traversal of the entire DAG is performed via 
///       <see cref="IVisitStrategy.DepthFirstSearchOfGraph(IGraph)"/> on a <see cref="IVisitStrategy"/> built by the 
///       <see cref="VisitStrategyBuilder"/> provided in the constructor.
///       <br/>
///     - Before running the DFS traversal, a post-visit event handler is attached to 
///       <see cref="IVisitStrategy.VisitedVertex"/>, running a post-order counter which is incremented at every visit.
///       <br/>
///     - The value of the post-order counter is stored in an array TS of v integers, where v is the number of vertices
///       in the graph, at a position defined by the id of the vertex.
///       <br/>
///     - Finally, the array TS is returned.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Running DFS for the entire graph via <see cref="IVisitStrategy.DepthFirstSearchFromVertex"/> is a
///       generally expensive operation, which has a Time and Space Complexity which depends on the specific 
///       <see cref="IVisitStrategy"/> used.
///       <br/>
///     - However, unlike in <see cref="DfsOnEachVertexSinkBasedTopologicalSort"/>, it is performed once, and not for
///       each vertex.
///       <br/>
///     - Instantiating the TS array is a O(v) operation, both in time and space.
///       <br/>
///     - Storing, updating and assigning the post-order counter to the right item of TS are all constant-time 
///       operations.
///       <br/>
///     - Therefore Time Complexity is O(Tdfs + v) and Space Complexity is O(Sdfs + v).
///       <br/>
///     - Using <see cref="FullyIterativeHashSetBasedGraphVisit"/> as <see cref="IVisitStrategy"/> and assuming 
///       <see cref="IGraph.GetAdjacentVerticesAndEdges(int, bool)"/> can be computed in constant time, Tdfs and Sdfs
///       becomes linear in v and e.
///       <br/>
///     - In this case, Time Complexity becomes O(v + e) and Space Complexity becomes O(v).
///     </para>
/// </remarks>
public class SingleDfsSinkBasedTopologicalSort : ITopologicalSort
{
    /// <summary>
    /// A visitor builder, to be used to build the <see cref="IVisitStrategy"/> instance needed to identify sink 
    /// vertices, by running a Depth First Search on the graph.
    /// </summary>
    public Func<IVisitStrategy> VisitStrategyBuilder { get; }

    /// <summary>
    ///     <inheritdoc cref="SingleDfsSinkBasedTopologicalSort"/>
    /// </summary>
    /// <param name="visitStrategyBuilder">
    ///     <inheritdoc cref="VisitStrategyBuilder" path="/summary"/>
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="SingleDfsSinkBasedTopologicalSort"/>
    /// </remarks>
    public SingleDfsSinkBasedTopologicalSort(Func<IVisitStrategy> visitStrategyBuilder)
    {
        VisitStrategyBuilder = visitStrategyBuilder;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="SingleDfsSinkBasedTopologicalSort"/>
    /// </remarks>
    public IList<int> Sort(IGraph dag)
    {
        // Find reachable nodes for each vertex
        var numberOfVertices = dag.GetNumberOfVertices();

        var topologicalSort = new int[numberOfVertices];
        var postOrder = numberOfVertices - 1;
        var visitStrategy = VisitStrategyBuilder();

        visitStrategy.VisitedVertex += (o, e) => topologicalSort[e.Vertex] = postOrder--;

        // Loop detection
        var currentPath = new HashSet<int>();
        visitStrategy.VisitingVertex += (o, e) => currentPath.Add(e.Vertex);
        visitStrategy.VisitedVertex += (o, e) => currentPath.Remove(e.Vertex);
        visitStrategy.AlreadyVisitedVertex += (o, e) =>
        {
            if (currentPath.Contains(e.Vertex))
                throw new InvalidOperationException($"The provided {nameof(dag)} is not a Direct Acyclic Graph");
        };
        
        MoreLinq.MoreEnumerable.Consume(visitStrategy.DepthFirstSearchOfGraph(dag));

        return topologicalSort;
    }
}

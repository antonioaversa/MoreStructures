namespace MoreStructures.Graphs.Visitor;

/// <summary>
/// An algorithm exploring <see cref="IGraph"/> instances.
/// </summary>
public interface IVisitStrategy
{
    /// <summary>
    /// Invoked when a node is about to be visited, by <see cref="DepthFirstSearchOfGraph(IGraph)"/>, 
    /// <see cref="ConnectedComponents(IGraph)"/>, <see cref="DepthFirstSearchFromVertex(IGraph, int)"/> or any other 
    /// method exploring vertices of the graph.
    /// </summary>
    /// <remarks>
    /// The event happens at most once per vertex per graph visit.
    /// <br/>/
    /// Check <see cref="VisitEventArgs"/> for the contextual information carried by the event.
    /// </remarks>
    event EventHandler<VisitEventArgs> VisitingVertex;

    /// <summary>
    /// Invoked just after a node has been visited, by <see cref="DepthFirstSearchOfGraph(IGraph)"/>, 
    /// <see cref="ConnectedComponents(IGraph)"/>, <see cref="DepthFirstSearchFromVertex(IGraph, int)"/> or any other 
    /// method exploring vertices of the graph.
    /// </summary>
    /// <remarks>
    /// The event happens at most once per vertex per graph visit.
    /// <br/>/
    /// Check <see cref="VisitEventArgs"/> for the contextual information carried by the event.
    /// </remarks>
    event EventHandler<VisitEventArgs> VisitedVertex;

    /// <summary>
    /// Invoked when a node would be about to be visited, but it has been already visited before, and won't be visited
    /// again.
    /// </summary>
    /// <remarks>
    /// The event may happen multiple times per vertex per graph visit and can signal the presence of one or more 
    /// cycles (if the connected component across visits is the same).
    /// <br/>
    /// Unlike <see cref="VisitingVertex"/> and <see cref="VisitedVertex"/>, the event may also not happen at all, for
    /// example in trees. 
    /// <br/>
    /// It surely is raised at least once in graphs with cycles. However, cycles are not required for this event to 
    /// happen: for example, a vertex is encountered multiple times in DAGs.
    /// <br/>/
    /// Check <see cref="VisitEventArgs"/> for the contextual information carried by the event (such as its connected 
    /// component and previous vertex).
    /// </remarks>
    event EventHandler<VisitEventArgs> AlreadyVisitedVertex;

    /// <summary>
    /// Explores the provided <paramref name="graph"/> entirely, returning its list of vertices in the order in which
    /// they have been explored.
    /// </summary>
    /// <param name="graph">
    /// The <see cref="IGraph"/> instance to be explored.
    /// <br/>
    /// Can be considered directed or undirected, depending on the actual exploration implementation and visit.
    /// </param>
    /// <returns>
    /// The sequence of vertices, lazily generated. Neighbors of the same vertex are visited by ascending id.
    /// </returns>
    IEnumerable<int> DepthFirstSearchOfGraph(IGraph graph);

    /// <summary>
    /// Explores the provided <paramref name="graph"/> entirely, via a Depth First Search, returning the mapping 
    /// between the id of each vertex and the label of its connected component.
    /// </summary>
    /// <param name="graph">
    /// The <see cref="IGraph"/> instance to be explored.
    /// <br/>
    /// Can be considered directed or undirected, depending on the actual exploration implementation and visit.
    /// <br/>
    /// However, connected components usually make sense for undirected graphs only, since a vertex can reach any other
    /// vertex of the same connected component only when edges can be traversed in both directions.
    /// </param>
    /// <returns>
    /// A dictionary, mapping the id of each vertex of the graph to the label of its connected component, which is a 
    /// non-negative integer.
    /// </returns>
    IDictionary<int, int> ConnectedComponents(IGraph graph);

    /// <summary>
    /// Runs a partial Depth First Search of the the provided <paramref name="graph"/>, returning the list of vertices 
    /// which are reachable from the vertex with id <paramref name="vertex"/>, along the edges of the graph, in the 
    /// order in which they have been explored.
    /// </summary>
    /// <param name="graph">
    /// The <see cref="IGraph"/> instance to be explored.
    /// Can be considered directed or undirected, depending on the actual exploration implementation and visit.
    /// </param>
    /// <param name="vertex">
    /// The <see cref="int"/> id identifying the vertex, from which the exploration has to be started.
    /// Different starting points will result into different sequences (by items and/or order) of vertices.
    /// </param>
    /// <returns>
    /// The sequence of vertices, lazily generated. Neighbors of the same vertex are visited by ascending id.
    /// </returns>
    IEnumerable<int> DepthFirstSearchFromVertex(IGraph graph, int vertex);

    /// <summary>
    /// Runs a partial Breadth First Search of the provided <paramref name="graph"/>, returning the list of vertices 
    /// which are reachable from the vertex with id <paramref name="vertex"/>, along the edges of the graph, in the 
    /// order in which they have been explored.
    /// </summary>
    /// <param name="graph">
    /// The <see cref="IGraph"/> instance to be explored.
    /// <br/>
    /// Can be considered directed or undirected, depending on the actual exploration implementation and visit.
    /// </param>
    /// <param name="vertex">
    /// The <see cref="int"/> id identifying the vertex, from which the exploration has to be started.
    /// Different starting points will result into different sequences (by items and/or order) of vertices.
    /// </param>
    /// <returns>
    /// The sequence of vertices, lazily generated. Neighbors of the same vertex are visited by ascending id.
    /// </returns>
    IEnumerable<int> BreadthFirstSearchFromVertex(IGraph graph, int vertex);
}

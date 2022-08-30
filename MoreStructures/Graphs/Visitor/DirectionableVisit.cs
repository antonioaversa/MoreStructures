namespace MoreStructures.Graphs.Visitor;

/// <summary>
/// Any A <see cref="IVisitStrategy"/> implementation which can perform the visit taking into account or not the
/// direction of the edges of the graph, based on the <see cref="DirectedGraph"/> property.
/// </summary>
public abstract class DirectionableVisit : IVisitStrategy
{
    /// <summary>
    /// Whether the provided <see cref="IGraph"/> should be considered as directed (i.e. edges should be 
    /// traversed from start to end) or inderected (i.e. edges should be traversed both from start to end and from
    /// end to start).
    /// </summary>
    public bool DirectedGraph { get; }

    /// <summary>
    ///     <inheritdoc cref="DirectionableVisit"/>
    /// </summary>
    /// <param name="directedGraph">
    ///     <inheritdoc cref="DirectedGraph" path="/summary"/>
    /// </param>
    protected DirectionableVisit(bool directedGraph)
    {
        DirectedGraph = directedGraph;
    }

    /// <inheritdoc/>
    public event EventHandler<VisitEventArgs> VisitingVertex = delegate { };

    /// <summary>
    /// Invoke the <see cref="VisitingVertex"/> event with the provided <paramref name="args"/>.
    /// </summary>
    /// <param name="args">The arguments to be provided, when raising the event.</param>
    protected virtual void RaiseVisitingVertex(VisitEventArgs args) => VisitingVertex.Invoke(this, args);

    /// <inheritdoc/>
    public event EventHandler<VisitEventArgs> VisitedVertex = delegate { };

    /// <summary>
    /// Invoke the <see cref="VisitedVertex"/> event with the provided <paramref name="args"/>.
    /// </summary>
    /// <param name="args">The arguments to be provided, when raising the event.</param>
    protected virtual void RaiseVisitedVertex(VisitEventArgs args) => VisitedVertex.Invoke(this, args);

    /// <inheritdoc/>
    public event EventHandler<VisitEventArgs> AlreadyVisitedVertex = delegate { };

    /// <summary>
    /// Invoke the <see cref="AlreadyVisitedVertex"/> event with the provided <paramref name="args"/>.
    /// </summary>
    /// <param name="args">The arguments to be provided, when raising the event.</param>
    protected virtual void RaiseAlreadyVisitedVertex(VisitEventArgs args) => AlreadyVisitedVertex.Invoke(this, args);

    /// <summary>
    /// Runs a Depth First Search and returns vertices and related connected components.
    /// </summary>
    /// <param name="graph">The <see cref="IGraph"/> instance to be explored.</param>
    /// <returns>
    /// A sequence of couples of <see cref="int"/>, the first being the vertex id and the second being the label of the
    /// connected component, the vertex belongs to.
    /// </returns>
    /// <remarks>
    /// Generalizes both <see cref="DepthFirstSearchOfGraph(IGraph)"/> and <see cref="ConnectedComponents(IGraph)"/>
    /// methods, which have very similar implementations. Check those two methods for further information.
    /// </remarks>
    protected abstract IEnumerable<(int, int)> DepthFirstSearchAndConnectedComponentsOfGraph(IGraph graph);

    /// <inheritdoc/>
    public virtual IEnumerable<int> DepthFirstSearchOfGraph(IGraph graph)
    {
        foreach (var (vertex, _) in DepthFirstSearchAndConnectedComponentsOfGraph(graph))
            yield return vertex;
    }

    /// <inheritdoc/>
    public virtual IDictionary<int, int> ConnectedComponents(IGraph graph)
    {
        var connectedComponents = new Dictionary<int, int>();
        foreach (var (vertex, connectedComponent) in DepthFirstSearchAndConnectedComponentsOfGraph(graph))
            connectedComponents[vertex] = connectedComponent;
        return connectedComponents;
    }

    /// <inheritdoc/>
    public abstract IEnumerable<int> DepthFirstSearchFromVertex(IGraph graph, int start);

    /// <inheritdoc/>
    public abstract IEnumerable<int> BreadthFirstSearchFromVertex(IGraph graph, int start);
}

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
    public abstract IEnumerable<int> DepthFirstSearch(IGraph graph);

    /// <inheritdoc/>
    public abstract IEnumerable<int> Visit(IGraph graph, int start);
}

namespace MoreStructures.EdgeListGraphs.Visitor;

/// <summary>
/// A <see cref="IVisitStrategy"/> implementation which uses an <see cref="HashSet{T}"/> of <see cref="int"/> to store
/// already visited vertices, while visiting the graph, and performs the visit recursively.
/// </summary>
public class FullyRecursiveHashSetBasedGraphVisit : IVisitStrategy
{
    /// <summary>
    /// Whether the provided <see cref="EdgeListGraph"/> should be considered as directed (i.e. edges should be 
    /// traversed from start to end) or inderected (i.e. edges should be traversed both from start to end and from
    /// end to start).
    /// </summary>
    public bool DirectedGraph { get; }

    /// <summary>
    ///     <inheritdoc cref="FullyRecursiveHashSetBasedGraphVisit"/>
    /// </summary>
    /// <param name="directedGraph">
    ///     <inheritdoc cref="DirectedGraph" path="/summary"/>
    /// </param>
    public FullyRecursiveHashSetBasedGraphVisit(bool directedGraph)
    {
        DirectedGraph = directedGraph;
    }

    /// <inheritdoc/>
    public IEnumerable<int> Visit(EdgeListGraph graph, int start)
    {
        var (_, edges) = graph;
        var alreadyVisited = new HashSet<int>();

        RExplore(edges, alreadyVisited, start);
        return alreadyVisited;
    }

    private void RExplore(IList<(int start, int end)> edges, HashSet<int> alreadyVisited, int start)
    {
        alreadyVisited.Add(start);

        Func<(int start, int end), int?> edgeSelector = DirectedGraph
            ? edge => 
                edge.start == start && !alreadyVisited.Contains(edge.end) ? edge.end : null
            : edge =>
                (edge.start == start && !alreadyVisited.Contains(edge.end)) 
                    ? edge.end 
                    : (edge.end == start && !alreadyVisited.Contains(edge.start)) 
                        ? edge.start 
                        : null;

        var unexploredVertices = edges.Select(edgeSelector).Where(uv => uv != null).Cast<int>();

        foreach (var unexploredVertex in unexploredVertices)
            RExplore(edges, alreadyVisited, unexploredVertex);
    }
}
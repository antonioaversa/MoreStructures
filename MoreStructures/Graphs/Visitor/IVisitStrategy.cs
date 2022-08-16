namespace MoreStructures.Graphs.Visitor;

/// <summary>
/// An algorithm exploring <see cref="IGraph"/> instances.
/// </summary>
public interface IVisitStrategy
{
    /// <summary>
    /// Explores the provided <paramref name="graph"/> entirely, returning its list of vertices.
    /// </summary>
    /// <param name="graph">
    /// The <see cref="IGraph"/> instance to be explored.
    /// Can be considered directed or undirected, depending on the actual exploration implementation and visit.
    /// </param>
    /// <returns>
    /// The sequence of vertices, lazily generated. Neighbors of the same vertex are visited by ascending id.
    /// </returns>
    IEnumerable<int> DepthFirstSearch(IGraph graph);

    /// <summary>
    /// Explores the provided <paramref name="graph"/>, returning the list of vertices which are reachable from the 
    /// vertex with id <paramref name="start"/>, along the edges of the graph.
    /// </summary>
    /// <param name="graph">
    /// The <see cref="IGraph"/> instance to be explored.
    /// Can be considered directed or undirected, depending on the actual exploration implementation and visit.
    /// </param>
    /// <param name="start">
    /// The <see cref="int"/> id identifying the vertex, from which the exploration has to be started.
    /// Different starting points will result into different sequences (by items and/or order) of vertices.
    /// </param>
    /// <returns>
    /// The sequence of vertices, lazily generated. Neighbors of the same vertex are visited by ascending id.
    /// </returns>
    IEnumerable<int> Visit(IGraph graph, int start);
}

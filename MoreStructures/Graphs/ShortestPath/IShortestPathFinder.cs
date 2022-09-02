namespace MoreStructures.Graphs.ShortestPath;

/// <summary>
/// An algorithm to find the shortest path (calculated as the number of edges forming the path) from a start vertex to
/// an end vertex in a directed graph.
/// </summary>
public interface IShortestPathFinder
{
    /// <summary>
    /// Finds the shortest path in the provided <paramref name="graph"/> from the <paramref name="start"/> vertex to 
    /// the <paramref name="end"/> vertex.
    /// </summary>
    /// <param name="graph">The graph to traverse.</param>
    /// <param name="start">The id of the vertex, to start traversal from.</param>
    /// <param name="end">The id of the vertex, to end traversal at.</param>
    /// <returns>
    /// A couple. The first item of the couple is the shortest distance, as number of edges of the shortest path 
    /// between <paramref name="start"/> and <paramref name="end"/>.
    /// <br/>
    /// The second item of the couple is the sequence of vertices of the <paramref name="graph"/>, identifying the 
    /// shortest path.
    /// </returns>
    (int, IList<int>) Find(IGraph graph, int start, int end);
}

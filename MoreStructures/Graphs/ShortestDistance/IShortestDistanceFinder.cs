namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// An algorithm that, given starting and ending vertices on a <b>weighted</b> directed graph, finds the <b>shortest 
/// distance</b> from the start to the end vertex, and a path on the graph having such a total distance.
/// </summary>
/// <remarks>
/// It differs from <see cref="ShortestPath.IShortestPathFinder"/> and its implementation because this class of 
/// algorithms takes into account <b>weight of edges</b> (i.e. distances between vertices).
/// <br/>
/// By contrast, the class of algorithm implementing <see cref="ShortestPath.IShortestPathFinder"/> only takes into 
/// account presence or absence of an edge: i.e. each edge is weighted "1", and vertex v is at distance 1 from vertex u
/// if there is a direct edge from v to u, and at distance infinity, if there is no path from v to u.
/// </remarks>
public interface IShortestDistanceFinder
{
    /// <summary>
    /// Finds the shortest distance and path in the provided <paramref name="graph"/> from the <paramref name="start"/> 
    /// vertex to the <paramref name="end"/> vertex, i.e. a path such that the sum of distances of all the edges on the
    /// path is non-bigger than the one of any other path from the provided <paramref name="start"/> and 
    /// <paramref name="end"/> vertices
    /// </summary>
    /// <param name="graph">The graph to traverse.</param>
    /// <param name="distances">A map between each edge of <paramref name="graph"/> and its distance.</param>
    /// <param name="start">The id of the vertex, to start traversal from.</param>
    /// <param name="end">The id of the vertex, to end traversal at.</param>
    /// <returns>
    /// A couple. The first item of the couple is the shortest distance, as the sum of the distance of each edge of the
    /// shortest path between <paramref name="start"/> and <paramref name="end"/>.
    /// <br/>
    /// The second item of the couple is the sequence of vertices of the <paramref name="graph"/>, identifying the 
    /// shortest path.
    /// </returns>
    (int, IList<int>) Find(IGraph graph, IGraphDistances distances, int start, int end);
}

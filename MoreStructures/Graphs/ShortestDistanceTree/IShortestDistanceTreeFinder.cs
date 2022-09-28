using MoreStructures.Graphs.ShortestDistance;

namespace MoreStructures.Graphs.ShortestDistanceTree;

/// <summary>
/// An algorithm that, given a start vertex on a <b>weighted</b> directed graph, finds the <b>shortest distance</b> 
/// from the start to any other vertex in the graph, and a path for each vertex of the graph having such a total 
/// distance.
/// </summary>
/// <remarks>
/// It differs from <see cref="ShortestDistance.IShortestDistanceFinder"/> and its implementation in this class of 
/// algorithms finds the shortest distance from all vertices of the graph, whereas 
/// <see cref="ShortestDistance.IShortestDistanceFinder"/> algorithms find the distance between the provided couple of
/// vertices only.
/// <br/>
/// <see cref="ShortestDistance.IShortestDistanceFinder"/> implementations can provide the best performance when the
/// shortest distance search is between two fixed points.
/// <br/>
/// If shortest distance has to be found from (or to) a given vertex to any other vertex, the class of algorithms
/// implementing <see cref="IShortestDistanceTreeFinder"/> can provide better performance, since all distances are
/// computed in a single execution of the algorithm.
/// </remarks>
public interface IShortestDistanceTreeFinder
{
    /// <summary>
    /// Finds the shortest distance and path in the provided <paramref name="graph"/> from the <paramref name="start"/> 
    /// vertex to each vertex of the <paramref name="graph"/>, i.e. a path such that the sum of distances of all the 
    /// edges on the path is non-bigger than the one of any other path from the provided <paramref name="start"/> to
    /// the vertex.
    /// </summary>
    /// <param name="graph">The graph to traverse.</param>
    /// <param name="distances">A map between each edge of <paramref name="graph"/> and its distance.</param>
    /// <param name="start">The id of the vertex, to start traversal from.</param>
    /// <returns>
    /// A <see cref="BestPreviouses"/> instance, wrapping a dictionary, mapping:
    /// <br/>
    /// - the id of each vertex v of the graph
    ///   <br/>
    /// - to the shortest distance S from <paramref name="start"/>,
    ///   <br/>
    /// - and to the id of the vertex preceeding v on a path from <paramref name="start"/> to v having total distance 
    ///   S.
    /// </returns>
    BestPreviouses Find(IGraph graph, IDictionary<(int, int), int> distances, int start);
}

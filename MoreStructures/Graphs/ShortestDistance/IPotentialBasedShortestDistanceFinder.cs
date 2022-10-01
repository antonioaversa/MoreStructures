namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// A <see cref="IShortestDistanceFinder"/> conceptual extension which introduces a <b>potential function</b> 
/// heuristic, to better drive the exploration of vertices of the graph, when searching for the end vertex.
/// </summary>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     In order for the potential function to be correct and possibly improve (and not decrease) algorithm 
///     performance, the function P should have a few properties. Some are mandatory, some are recommended. 
///     listed below.
///     <br/>
///     - It must be <b>deterministic</b>, i.e. an actual matematical function, yielding the same output for a
///       given input, every time it is called.
///       <br/>
///     - It must be <b>feasible</b>, i.e. it should not make distances negative when applied to edge distances on 
///       the graph, <u>if Dijkstra is used as shortest path finder</u>. If distances become negative, Dijkstra 
///       cannot be applied, as its main requirement would be broken.
///       Notice how it is not enough for the potential function to be non-negative, since the distance d(u, v) of 
///       an edge (u, v) of the graph is shifted by the potential difference, i.e. by P(v) - P(u), which can be 
///       negative and can make the overall edge cost, d(u, v) + P(v) - P(u), negative.
///       <br/>
///     - It should be <b>quick</b> to calculate the potential, not to have an impact on the overall runtime of the
///       algorithm. Tipically O(1).
///       <br/>
///     - It should be <b>minimal at the end vertex</b>, so that Dijkstra would be incentivized to go towards that 
///       direction, while exploring the graph.
///       <br/>
///     - It should become <b>higher as the distance from the end vertex increases</b>, so that Dijkstra would be 
///       disincentivized from getting farther from the end vertex.
///       <br/>
///     - It tipically is defined as <b>a lower bound for the shortest distance</b>: e.g. the euclidean distance on
///       the map in a road network.
///     </para>
/// </remarks>
public interface IPotentialBasedShortestDistanceFinder : IShortestDistanceFinder
{
    /// <inheritdoc cref="IShortestDistanceFinder.Find"/>
    /// <param name="graph">
    ///     <inheritdoc cref="IShortestDistanceFinder.Find"/>
    /// </param>
    /// <param name="distances">
    ///     <inheritdoc cref="IShortestDistanceFinder.Find"/>
    /// </param>
    /// <param name="potentials">
    /// A function mapping each vertex of the provided graph to a <see cref="int"/>, providing an heuristic for
    /// "how far" the vertex is from the end vertex, to drive the <see cref="IPotentialBasedShortestDistanceFinder"/> 
    /// towards an earlier, rather than late, discovery of the end vertex.
    /// <br/>
    /// Check the general documentation of <see cref="IPotentialBasedShortestDistanceFinder"/> for further information
    /// about how to define a correct and good potential function.
    /// </param>
    /// <param name="start">
    ///     <inheritdoc cref="IShortestDistanceFinder.Find"/>
    /// </param>
    /// <param name="end">
    ///     <inheritdoc cref="IShortestDistanceFinder.Find"/>
    /// </param>
    (int, IList<int>) Find(
        IGraph graph, IGraphDistances distances, Func<int, int> potentials, int start, int end);
}

namespace MoreStructures.Graphs.MinimumSpanningTree;

/// <summary>
/// An algorithm to find the Minimum Spanning Tree (MST) of an undirected, connected, weighted <see cref="IGraph"/>.
/// </summary>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     The <b>Minimum Spanning Tree</b> of an undirected, connected, weighted graph <c>G = (V, E)</c> with distances 
///     defined by the mapping d, shortened in MST, is a subgraph T of G which is:
///     <br/>
///     - spanning, i.e. it is composed of v vertices - all the vertices in G;
///       <br/>
///     - a tree, i.e. it is connected (single connected component) and composed of v - 1 edges;
///       <br/>
///     - of minimum total weight, i.e. the sum of d(e) for all e belonging to T is minimized, across all possible T.
///     <br/>
///     <br/>
///     The MST of a graph G can be considered as a "backbone" of G which minimizes cost of connection: T reaches all
///     vertices of G and it does so by being fully connected (every vertex is still reachable from every other vertex)
///     and of minimum cost (defined as minimum of the sum of the edges of the tree.
///     <br/>
///     The definition, as it is formulated, only applies to <b>undirected graphs</b>, since for directed graph the 
///     notion of connected components has to be extended to become meaningful (e.g. into "strongly connected 
///     components").
///     <br/>
///     The definition only applies to <b>connected graphs</b>, since a graph with multiple connected components cannot
///     have a single connected tree spanning the entire graph: the tree would need to have an edge connecting vertices 
///     of different connected components, but if that edge existed, the two connected components would be a single 
///     one.
///     <br/>
///     When a graph G has m connected components, those components can be discovered by running DFS on G and labelling
///     each connected component with ids from 0 to m-1. Each group of vertices G[0], ..., G[m-1] with related edges 
///     would represent a connected sub-graph of G, having a MST T[i], where two T[i] and T[j] would be completely
///     separated.
///     <br/>
///     The definition only applies to <b>weighted graphs</b>, since an unweighted graph would not allow to give a 
///     meaningful metric definition for the "weight" to a spanning tree. "Number of edges" cannot be used as a metric,
///     because all spanning tree of a graph of v vertices has exactly v nodes and v - 1 edges, so all spanning tree
///     would end up having the same weight.
///     </para>
/// </remarks>
public interface IMstFinder
{
    /// <summary>
    /// Finds the Minimum Spanning Tree (MST) of the provided <paramref name="graph"/>.
    /// </summary>
    /// <param name="graph">The <see cref="IGraph"/>, to find the MST of.</param>
    /// <param name="distances">
    /// The dictionary mapping each edge of <paramref name="graph"/> to its weight, which represents the "distance"
    /// from the start vertex of the edge to the end vertex.
    /// </param>
    /// <returns>The set of edges, in the form (source, target), identifying the MST.</returns>
    /// <remarks>
    ///     <inheritdoc cref="IMstFinder"/>
    /// </remarks>
    public ISet<(int, int)> Find(IGraph graph, IDictionary<(int, int), int> distances);
}

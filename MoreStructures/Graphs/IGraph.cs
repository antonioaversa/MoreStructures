namespace MoreStructures.Graphs;

/// <summary>
/// A graph data structure, directed or undirected.
/// </summary>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     A graph is a data structure composed of two types of objects: <g>vertices</g> and <g>edges</g>.
///     <br/>
///     Both vertices and edges can carry labels, weights, costs, or any other piece of information, specific to the
///     scenario in which they are used.
///     <br/>
///     More precisely, a graph is conceptually defined as a couple of two collections:
///     <br/>
///     - a collection of vertices,
///       <br/>
///     - and a collection of edges, connecting such vertices, directionally or undirectionally.
///       <br/>
///     The actual underlying representation of a graph instance depends on the specific <see cref="IGraph"/> used.
///     </para>
/// </remarks>
public interface IGraph
{
    /// <summary>
    /// Returns the total number of vertices of the graph.
    /// </summary>
    /// <returns>A non-negative integer.</returns>
    int GetNumberOfVertices();

    /// <summary>
    /// Returns the vertices of the graph which are neighbor of the <paramref name="start"/> vertex, each vertex 
    /// together with its incoming or outgoing edge, linking the vertex to the <paramref name="start"/> vertex.
    /// </summary>
    /// <param name="start">The id of the vertex, to look for neighbors of.</param>
    /// <param name="takeIntoAccountEdgeDirection">
    /// Whether to consider the direction of edges, when looking for neighbors.
    /// </param>
    /// <returns>
    /// A sequence of couples: the first item being the id of the neighboring vertex V found, and the second item being
    /// the edge which connects the <paramref name="start"/> vertex to V, or viceversa. The order is <b>undefined</b>, 
    /// and depends on the implementation.
    /// </returns>
    IEnumerable<(int vertex, (int edgeStart, int edgeEnd) edge)> GetAdjacentVerticesAndEdges(
        int start, bool takeIntoAccountEdgeDirection);
}

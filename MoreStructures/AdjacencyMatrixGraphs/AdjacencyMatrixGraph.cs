using MoreStructures.Graphs;

namespace MoreStructures.AdjacencyMatrixGraphs;

/// <summary>
/// A graph data structure, represented as a matrix: the (i, j) element of the matrix is true if the vertex with id i
/// is neighbor of the vertex with id j, and false otherwise.
/// </summary>
/// <param name="AdjacencyMatrix">
/// A square matrix of boolean, each value representing whether a vertex is neighborhood of another one.
/// </param>
/// <remarks>
/// - If the graph can be considered undirected if all edges come in couples with both directions: i.e. the matrix is
///   simmetrix, i.e. <c>M[i, j] == M[j, i] for all (i, j)</c>.
///   <br/>
/// - The size of this data structure is proportional to the square of the number of vertices of the graph.
///   <br/>
/// - So, this graph representation is particularly useful when the number is edges is proportional to the square of 
///   the number of vertices v in the graph, and O(v) retrieval of the incoming and outgoing edges is required.
///   <br/>
/// - Performance is O(v), whether <c>takeIntoAccountEdgeDirection</c> is true or not.
///   <br/>
/// - Notice that O(v) is worse than O(avg_e), where avg_e is the average number of edges coming out of a vertex, for
///   sparse graphs, and comparable for dense graphs.
/// </remarks>
/// <example>
/// The followin graph:
/// <code>
///  0 --> 1 &lt;==&gt; 3
/// | ^   ^     /
/// | |  /     /
/// | | /     /
/// v |/     /
///  2 &lt;-----
/// </code>
/// is represented as <c>AdjacencyMatrixGraph(new {{ F, T, T, F }, { F, F, F, T }, { T, T, F, F }, {F, T, T, F }})</c>.
/// </example>
public record AdjacencyMatrixGraph(bool[,] AdjacencyMatrix) : IGraph
{
    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    /// In the <see cref="AdjacencyMatrixGraph"/> representation, corresponds to edge of the square matrix.
    /// </remarks>
    public int GetNumberOfVertices() => AdjacencyMatrix.GetLength(0);

    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Unlike the adjacency list representation, the matrix representation allows to access neighborhoods based 
    ///       on both outgoing and incoming edges of a given vertex (the first is a row, the second is a column).
    ///       <br/>
    ///     - Therefore, unlike the adjacency list representation, when the value of 
    ///       <paramref name="takeIntoAccountEdgeDirection"/> is <see langword="false"/>, a lookup of all neighborhoods
    ///       defined in the matrix (i.e. a full matrix lookup) is not required.
    ///       <br/>
    ///     - Instead, a single additional lookup of the neighborhood of incoming edges, is required, in addition to 
    ///       the lookup of the of the neighborhood of outgoing edges.
    ///       <br/>
    ///     - Notice that, while in the adjacency list representation the neighborhood precisely contains the number of
    ///       neighboring vertices, avg_e, in the adjacency matrix representation the neighborhood is in the form of a
    ///       boolean array of v items, where v is the number of vertices of the graph.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Direct accesses to the two neighborhoods of interest are constant time operations, since it is about
    ///       retrieving a row and a column given their index, respectively.
    ///       <br/>
    ///     - The matrix is a square matrix of v rows and columns, so each of the neighborhoods to check has v 
    ///       elements.
    ///       <br/>
    ///     - Each neighborhood has to be linearly scanned, looking for <see langword="true"/> values.
    ///       <br/>
    ///     - Therefore, Time and Space Complexity (when enumerated) are O(v).
    ///     </para>
    /// </remarks>
    public IEnumerable<(int vertex, (int edgeStart, int edgeEnd) edge)> GetAdjacentVerticesAndEdges(
        int start, bool takeIntoAccountEdgeDirection)
    {
        for (var j = 0; j < AdjacencyMatrix.GetLength(1); j++)
        {
            if (AdjacencyMatrix[start, j])
                yield return (j, (start, j));
        }

        if (takeIntoAccountEdgeDirection)
            yield break;

        for (var i = 0; i < AdjacencyMatrix.GetLength(0); i++)
        {
            if (AdjacencyMatrix[i, start])
                yield return (i, (i, start));
        }
    }
}

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
    /// <inheritdoc/>
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

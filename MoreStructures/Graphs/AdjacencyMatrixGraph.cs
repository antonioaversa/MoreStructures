namespace MoreStructures.Graphs;

/// <summary>
/// A graph data structure, represented as a matrix: the (i, j) element of the matrix is true if the vertex with id i
/// is neighbor of the vertex with id j, and false otherwise.
/// </summary>
/// <param name="AdjacencyMatrix">
/// A square matrix of boolean, each value representing whether a vertex is neighborhood of another one.
/// </param>
/// <remarks>
/// - <b>This representation doesn't support multigraphs</b>, i.e. graphs which can have multiple parallel edges 
///   between the same two vertices.
///   <br/>
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
///   <br/>
/// - This representation is particularly convenient when used as a directed graph and traversal has often to be done 
///   in reversed direction, since <see cref="Reverse"/> is an O(1) operation (it just builds a proxy to the original
///   graph) and <see cref="GetAdjacentVerticesAndEdges(int, bool)"/> has comparable O(v) complexities when traversing 
///   edges according to their direction or in any direction.
///   <br/>
/// - Notice that <see cref="AdjacencyListGraph"/> has better runtime (O(avg_e)) when edges are traversed according to
///   their direction, and worse runtime (O(avg_e + v)) when edges are traversed in any direction. 
///   <br/>
/// - <see cref="EdgeListGraph"/> has consistent runtime in both traversal (O(e)), but e is O(v^2) in dense graphs,
///   leading to sensibly worse performance in such scenarios.
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
    /// In the <see cref="AdjacencyMatrixGraph"/> representation, corresponds to the edge of the square matrix.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int GetNumberOfVertices() => AdjacencyMatrix.GetLength(0);

    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Iterates over all the cells of the adjacency matrix M.
    ///       <br/>
    ///     - For each adjacency M[u, v] set in M, the edge (u, v) is returned.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - The adjacency matrix has v rows and v columns, where v is the number of vertices in the graph.
    ///       <br/>
    ///     - Therefore Time Complexity is O(v^2). Space Complexity is O(1), since the iteration uses a constant 
    ///       amount of space.
    ///     </para>
    /// </remarks>
    public IEnumerable<(int edgeStart, int edgeEnd)> GetAllEdges() =>
        from u in Enumerable.Range(0, AdjacencyMatrix.GetLength(0))
        from v in Enumerable.Range(0, AdjacencyMatrix.GetLength(1))
        where AdjacencyMatrix[u, v]
        select (u, v);

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
    public IEnumerable<IGraph.Adjacency> GetAdjacentVerticesAndEdges(
        int start, bool takeIntoAccountEdgeDirection)
    {
        for (var j = 0; j < AdjacencyMatrix.GetLength(1); j++)
        {
            if (AdjacencyMatrix[start, j])
                yield return new(j, start, j);
        }

        if (takeIntoAccountEdgeDirection)
            yield break;

        for (var i = 0; i < AdjacencyMatrix.GetLength(0); i++)
        {
            if (AdjacencyMatrix[i, start])
                yield return new(i, i, start);
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - An <see cref="IGraph"/> proxy is created, wrapping this instance of <see cref="IGraph"/>.
    ///       <br/>
    ///     - <see cref="IGraph.GetNumberOfVertices"/> is dispatched to the proxied graph.
    ///       <br/>
    ///     - <see cref="IGraph.GetAdjacentVerticesAndEdges(int, bool)"/> is directly implemented, accessing
    ///       <see cref="AdjacencyMatrix"/> directly.
    ///       <br/>
    ///     - The implementation is very similar to the one of <see cref="AdjacencyMatrixGraph"/>: the only difference
    ///       is that columns and rows are inverted.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Since this method just creates a proxy, Time and Space Complexity are O(1).
    ///       <br/>
    ///     - All operations on the proxy have the same Time and Space Complexity as the corresponding methods in 
    ///       <see cref="AdjacencyMatrixGraph"/>.
    ///     </para>
    /// </remarks>
    public IGraph Reverse() => new ReverseGraph(this);

    sealed private class ReverseGraph : ReverseProxyGraph<AdjacencyMatrixGraph>
    {
        public ReverseGraph(AdjacencyMatrixGraph graph) : base(graph)
        {
        }

        public override IEnumerable<IGraph.Adjacency> GetAdjacentVerticesAndEdges(
            int start, bool takeIntoAccountEdgeDirection)
        {
            var adjacencyMatrix = Proxied.AdjacencyMatrix;

            for (var i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                if (adjacencyMatrix[i, start])
                    yield return new(i, start, i);
            }

            if (takeIntoAccountEdgeDirection)
                yield break;

            for (var j = 0; j < adjacencyMatrix.GetLength(1); j++)
            {
                if (adjacencyMatrix[start, j])
                    yield return new(j, j, start);
            }
        }
    }
}

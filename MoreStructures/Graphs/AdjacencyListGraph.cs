namespace MoreStructures.Graphs;

/// <summary>
/// A graph data structure, represented as an ordered list of neighborhoods: the i-th item of the list is the set of
/// ids of the vertices which are neighbors of the vertex with id i.
/// </summary>
/// <param name="Neighborhoods">
/// A list of sets of integers, each set representing the neighborhood of the corresponding vertex.
/// </param>
/// <remarks>
/// - <b>This representation doesn't support multigraphs</b>, i.e. graphs which can have multiple parallel edges 
///   between the same two vertices.
///   <br/>
/// - If the graph can be considered undirected if all edges come in couples with both directions: i.e. when the 
///   neighborhoods list L is such that <c>if v2 belongs to L[v1], then v1 belongs to L[v2]</c>.
///   <br/>
/// - The size of this data structure is proportional to the number of edges of the graph, since a vertex has as many 
///   neighbors as edges connecting to other vertices (possibly including itself).
///   <br/>
/// - So, this graph representation is particularly useful when the number is edges is smaller or proportional to the 
///   number of vertices in the graph, i.e. when the graph is <b>sparse</b> (i.e. when e is O(v)).
///   <br/>
/// - It becomes an expensive representation when the graph is <b>dense</b> (i.e. when e is O(v^2)).
///   <br/>
/// - While having size proportional to the number of edges, <see cref="AdjacencyListGraph"/> is more convenient than 
///   <see cref="EdgeListGraph"/> to run neighborhood-based algorithms, such as discovery, because it makes easier 
///   and faster to get neighbors of a vertex.
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
/// is represented as <c>AdjacencyListGraph(new { new { 1, 2 }, new { 3 }, new { 0, 1 }, new { 1, 2 } })</c>.
/// </example>
public record AdjacencyListGraph(IList<ISet<int>> Neighborhoods) : IGraph
{
    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    /// In the <see cref="AdjacencyListGraph"/> representation, corresponds to number of neighborhoods.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int GetNumberOfVertices() => Neighborhoods.Count;

    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Iterates over all the neighborhoods.
    ///       <br/>
    ///     - For each neighbor v of the vertex u, returns the edge (u, v).
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - There are v neighbors in total (as many as the number of vertices in the graph).
    ///       <br/>
    ///     - The total number of neighbors across all neighborhoods is e (as many as the number of edges in the 
    ///       graph).
    ///       <br/>
    ///     - Therefore Time Complexity is O(v + e). Space Complexity is O(1), since the iteration uses a constant 
    ///       amount of space.
    ///     </para>
    /// </remarks>
    public IEnumerable<(int edgeStart, int edgeEnd)> GetAllEdges() => 
        Neighborhoods.SelectMany((v, i) => Enumerable.Repeat(i, v.Count).Zip(v).Select(c => c));

    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Because the neighborhoods list is indexed by the vertex id, the algorithm has just to perform a direct 
    ///       access to the <paramref name="start"/>-th item: that is the set of neighbors of the vertex with id 
    ///       <paramref name="start"/>.
    ///       <br/>
    ///     - When the value of <paramref name="takeIntoAccountEdgeDirection"/> is <see langword="false"/>, a lookup of
    ///       all neighborhoods is required.
    ///       <br/>
    ///     - In such case it would be better to have neighborhoods list already including bi-directional edges, and 
    ///       using <paramref name="takeIntoAccountEdgeDirection"/> = <see langword="true"/>. If not, the advantages of
    ///       having O(1) neighborhood lookup would be lost.
    ///       <br/>
    ///     - If bi-directional edges are not included in the graph, and many calls to this method need to be called
    ///       with <paramref name="takeIntoAccountEdgeDirection"/> = <see langword="true"/>, consider reversing the
    ///       graph using <see cref="Reverse"/>, and keep one structure for direct lookup and the other for reversed
    ///       lookup.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Direct access to the neighborhoods list is a constant time operation.
    ///       <br/>
    ///     - Therefore, when <paramref name="takeIntoAccountEdgeDirection"/> = <see langword="true"/>, Time and Space
    ///       Complexity (when enumerated) are O(avg_e), where avg_e is the average number of edges coming out of the 
    ///       <paramref name="start"/> vertex.
    ///       <br/>
    ///     - However, when <paramref name="takeIntoAccountEdgeDirection"/> = <see langword="false"/>, Time Complexity
    ///       becomes O(avg_e + v), where v is the number of vertices of the graph (i.e. the number of total 
    ///       neighborhoods defined).
    ///     </para>
    /// </remarks>
    public IEnumerable<IGraph.Adjacency> GetAdjacentVerticesAndEdges(
        int start, bool takeIntoAccountEdgeDirection)
    {
        foreach (var end in Neighborhoods[start])
            yield return new(end, start, end);

        if (takeIntoAccountEdgeDirection)
            yield break;

        for (var i = 0; i < Neighborhoods.Count; i++)
        {
            if (i == start)
                continue;

            if (Neighborhoods[i].Contains(start))
                yield return new(i, i, start);
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]" />
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Because <see cref="AdjacencyListGraph"/> has O(avg_e + v) Time Complexity when edges have to be traversed
    ///       in reverse, rather than O(avg_e), which is much smaller on large graphs, a proxy to the original data
    ///       structure is not used.
    ///       <br/>
    ///     - Instead, reversed neighborhoods RV are calculated, by iterating over all neighbors u of the neighborhood 
    ///       N[v] of each vertex v of this graph: if u belongs to N[v] then v is added to RV[u], initially set to an
    ///       empty <see cref="HashSet{T}"/>.
    ///       <br/>
    ///     - Finally a new <see cref="AdjacencyListGraph"/> is built out of RV and returned as result.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Unlike in <see cref="AdjacencyMatrixGraph.Reverse"/>, a brand new structure is built, and proxies are not
    ///       used at all.
    ///       <br/>
    ///     - There are as many neighborhoods and reversed neighbors as vertices in the graph.
    ///       <br/>
    ///     - The cost of going through all the neighbors in all the neighborhoods is proportional to the number of 
    ///       edges in the graph.
    ///       <br/>
    ///     - Therefore Time and Space Complexity are O(v + e), where v is the number of vertices and e the number of
    ///       edges.
    ///     </para>
    /// </remarks>
    public IGraph Reverse()
    {
        var numberOfVertices = Neighborhoods.Count;
        var reversedNeighborhoods = new ISet<int>[numberOfVertices];

        for (var vertex = 0; vertex < numberOfVertices; vertex++)
            reversedNeighborhoods[vertex] = new HashSet<int>();

        for (var vertex = 0; vertex < numberOfVertices; vertex++)
            foreach (var neighbor in Neighborhoods[vertex])
                reversedNeighborhoods[neighbor].Add(vertex);

        return new AdjacencyListGraph(reversedNeighborhoods);
    }
}

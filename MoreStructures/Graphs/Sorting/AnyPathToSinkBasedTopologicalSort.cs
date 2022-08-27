namespace MoreStructures.Graphs.Sorting;

/// <summary>
/// An <see cref="ITopologicalSort"/> implementation which assigns topological order to vertices by identifing a sink 
/// vertex at each iteration <b>in a non-deterministic way</b>, picking some start vertex and following some path to a
/// sink.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Compared to <see cref="DfsOnEachVertexSinkBasedTopologicalSort"/>, it has better runtime (quadratic, rather
///       than cubic, over the number of vertices on the graph).
///       <br/>
///     - However, the algorithm picks a vertex and follows a path at random (using the LINQ method 
///       <see cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/> on a <see cref="HashSet{T}"/>, which 
///       is not sorted and doesn't provide any guarantee about ordering).
///       <br/>
///     - Therefore, unlike <see cref="DfsOnEachVertexSinkBasedTopologicalSort"/>, the topological order produced when 
///       multiple results are possible, is not defined.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The number of vertices v in the DAG is computed via <see cref="IGraph.GetNumberOfVertices"/>.
///       <br/>
///     - Then, a set of the integers from 0 to v-1 is instantiated, keeping the remaining vertices not sorted yet, 
///       together with an array TS of v integers, to accomodate the topological sort value for each of the vertices.
///       <br/>
///     - Then, the main loop of the algorithm is executed, while there are still unsorted vertices.
///       <br/>
///     - Any of the sorted vertices v is taken, and a directed path v is followed until a sink vertex s is found.
///       <br/>
///     - If, while following a path, a loop is detected (i.e. a vertex is visited again), an exception is thrown.
///       <br/>
///     - Otherwise, the found sink can be assigned the highest unassigned sort value (v-1 down to 0), and the loop
///       repeats.
///       <br/>
///     - When all vertices have been sorted, TS can be returned as result.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Getting the number of vertices is a constant-time operation in most <see cref="IGraph"/> implementations.
///       <br/>
///     - Instantiating the set of unsorted vertices, instantiating the output array, and getting the first available 
///       vertex from the set of unsorted vertices are all O(v) operations, both in time and space.
///       <br/>
///     - The main loop of the algorithm is executed v times.
///       <br/>
///     - Each iteration can potentially explore the entire graph, so a path of O(v) vertices and O(v) edges.
///       <br/>
///     - Therefore, Time Complexity is O(v^2) and Space Complexity is O(v).
///     </para>
/// </remarks>
public class AnyPathToSinkBasedTopologicalSort : ITopologicalSort
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="AnyPathToSinkBasedTopologicalSort"/>
    /// </remarks>
    public IList<int> Sort(IGraph dag)
    {
        var numberOfVertices = dag.GetNumberOfVertices();
        var remainingVertices = Enumerable.Range(0, numberOfVertices).ToHashSet();

        var topologicalSort = new int[numberOfVertices];
        while (remainingVertices.FirstOrDefault(-1) is int vertex && vertex >= 0)
        {
            // Follow a path until the end to find a sink
            var verticesOnPath = new HashSet<int> { vertex };

            var currentVertex = vertex;
            while (currentVertex >= 0)
            {
                var nextVertex = dag
                    .GetAdjacentVerticesAndEdges(currentVertex, true)
                    .Select(a => a.Vertex)
                    .Where(v => v != currentVertex && remainingVertices.Contains(v))
                    .FirstOrDefault(-1);

                if (nextVertex < 0)
                    break; // currentVertex is a sink

                if (verticesOnPath.Contains(nextVertex))
                    throw new InvalidOperationException($"The provided {nameof(dag)} is not a Direct Acyclic Graph");

                verticesOnPath.Add(nextVertex);
                currentVertex = nextVertex;
            }

            topologicalSort[currentVertex] = remainingVertices.Count - 1;
            remainingVertices.Remove(currentVertex);
        }

        return topologicalSort;
    }
}
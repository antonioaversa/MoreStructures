using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Graphs.StronglyConnectedComponents;

/// <summary>
/// A simple implementation of <see cref="ISccFinder"/>.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - This is the easiest, most straighforward implementation of <see cref="ISccFinder"/>.
///       <br/>
///     - It has, however, a cubic runtime for dense graphs, which is less than ideal.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - First a dictionary R of reachable vertices is built, mapping each of the vertices of the graph to an
///       <see cref="HashSet{T}"/> of vertices reachable from it (including the vertex itself).
///       <br/>
///     - The dictionary is populated by running <see cref="IVisitStrategy.DepthFirstSearchFromVertex"/> on each
///       vertex, via the <see cref="VisitStrategy"/> provided in the constructor.
///       <br/>
///     - After that, all vertices of the graph are iterated over.
///       <br/>
///     - A SCC array, of as many items as the number of vertices in the graph, is initialized. A current SCC counter 
///       is set to 0 and an <see cref="HashSet{T}"/> P of already processed vertices is initialized to an empty set.
///       <br/>
///     - For each vertex i, all reachable vertices j, except the ones in P, in R[i] are checked.
///       <br/>
///     - Vertices j such that R[j] contains i (including i itself, given that i always belongs to R[i]) are all 
///       assigned the same SCC.
///       <br/>
///     - When a vertex j is assigned a SCC, it is also added to P, so that it is not checked again in following
///       iterations.
///       <br/>
///     - After all vertices reachable from i have been checked, the current SCC counter is incremented and the current
///       iteration finishes.
///       <br/>
///     - Once all vertices have been processed, the resulting SCC array is returned as result.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - This algorithm creates a dictionary of reachable vertices, pretty much like 
///       <see cref="Sorting.DfsOnEachVertexSinkBasedTopologicalSort"/>.
///       <br/>
///     - Similar considerations about the complexity of setting up the dictionary apply to this algorithm.
///       <br/>
///     - The main nested loops of the algorithm check each couple of a vertex and a reachable vertex from it.
///       <br/>
///     - The number of such couples is O(v^2), in graph where vertices are connected to O(v) other vertices.
///       <br/>
///     - Checking whether a vertex has already been processed, or whether it is reachable/reached from another vertex
///       are constant-time operations.
///       <br/>
///     - Therefore Time Complexity is O(v * Tdfs + v^2) and Space Complexity is O(v^2 + Sdfs), where Tdfs and Sdfs
///       are Time and Space Complexity of running a DFS from a vertex.
///       <br/>
///     - Time Complexity is O(v^3) in graphs where every vertex is connected to every other vertex.
///     </para>
/// </remarks>
public class NaiveSccFinder : ISccFinder
{
    /// <summary>
    /// The visitor to be used to find SCC, by running Depth First Searches on each vertex.
    /// </summary>
    public IVisitStrategy VisitStrategy { get; }

    /// <summary>
    ///     <inheritdoc cref="NaiveSccFinder"/>
    /// </summary>
    /// <param name="visitStrategy">
    ///     <inheritdoc cref="VisitStrategy" path="/summary"/>
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="NaiveSccFinder"/>
    /// </remarks>
    public NaiveSccFinder(IVisitStrategy visitStrategy)
    {
        VisitStrategy = visitStrategy;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="NaiveSccFinder" path="/remarks"/>
    /// </remarks>
    public IList<int> Find(IGraph graph)
    {
        var numberOfVertices = graph.GetNumberOfVertices();
        var verticesReachableFromVertices = Enumerable
            .Range(0, numberOfVertices)
            .Select(vertex => VisitStrategy.DepthFirstSearchFromVertex(graph, vertex).ToHashSet())
            .ToList();

        var scc = new int[numberOfVertices];
        var currentScc = 0;
        var processedVertices = new HashSet<int>();
        for (var i = 0; i < numberOfVertices; i++)
        {
            if (processedVertices.Contains(i))
                continue;

            var verticesSameScc = verticesReachableFromVertices[i]
                .Except(processedVertices)
                .Where(j => verticesReachableFromVertices[j].Contains(i));

            foreach (var j in verticesSameScc)
            {
                scc[j] = currentScc;
                processedVertices.Add(j);
            }

            currentScc++;
        }

        return scc;
    }
}
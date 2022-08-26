using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Graphs.Sorting;

/// <summary>
/// An <see cref="ITopologicalSort"/> implementation which assigns topological order to vertices by identifing a sink 
/// vertex at each iteration.
/// </summary>
/// <remarks>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - First, all reachable vertices, from each vertex of the graph, are identified, running a 
///       <see cref="IVisitStrategy.DepthFirstSearchFromVertex(MoreStructures.Graphs.IGraph, int)"/> via the 
///       <see cref="VisitStrategy"/> provided in the constructor.
///       <br/>
///     - Reachable vertices from each vertex are stored into a <see cref="Dictionary{TKey, TValue}"/> D, mapping the
///       <see cref="int"/> id of the vertex i to an <see cref="HashSet{T}"/> of vertices reachable from i (including
///       i itself).
///       <br/>
///     - Then, an array of v integers, named here TS, where v is the number of vertices in the graph, is instantiated,
///       to store the topological sort of the provided DAG. 
///       <br/>
///     - An <see cref="HashSet{T}"/> of vertices already processed (and removed from D) is also instantiated to an 
///       empty set.
///       <br/>
///     - Finally the main loop of the algorithm is repeated, until D remains empty.
///       <br/>
///     - At each iteration, the sink vertex with the biggest index is found. 
///       <br/>
///     - The sink is found by looking for vertices from which no other vertices are reachable, other than the vertex 
///       itself and possibly the already processed ones (which are "virtually" removed from the graph).
///       <br/>
///     - If such a sink is not found, it means that the remaining graph contains a cycle, because there is at least a
///       vertex in the graph (otherwise the main loop condition would have been false) and for each vertex i there is 
///       a vertex j such that a directed path from i to j, i ~~> j, exists. So an 
///       <see cref="InvalidOperationException"/> is thrown as topological sort only makes sense for DAGs.
///       <br/>
///     - If such a sink is found instead, it is returned as (v - 1 - i)-th item of TS, where i is the 0-based running
///       index of the main loop of the algorithm, incremented by 1 at each iteration.
///       <br/>
///     - Finally the array TS is returned.
///     </para>
///     <para id="complexity">
///     <br/>
///     - Getting the number of vertices via <see cref="IGraph.GetNumberOfVertices"/> is a constant-time operation in
///       any classical implementation of <see cref="IGraph"/>.
///       <br/>
///     - Running DFS for each vertex of the graph via <see cref="IVisitStrategy.DepthFirstSearchFromVertex"/> is a
///       generally expensive operation, which has a Time and Space Complexity which depends on the specific 
///       <see cref="IVisitStrategy"/> used.
///       <br/>
///     - Instantiating the TS array also requires resetting all its v items.
///       <br/>
///     - The main loop of the algorithm, executed v times, requires finding the last sink vertex, which is an 
///       operation quadratic in complexity. All other operations require constant time and space.
///       <br/>
///     - Therefore Time Complexity is O(v * Tdfs + v^3) and Space Complexity is O(v^2 + Sdfs).
///       <br/>
///     - Using <see cref="FullyIterativeHashSetBasedGraphVisit"/> as <see cref="VisitStrategy"/> and assuming 
///       <see cref="IGraph.GetAdjacentVerticesAndEdges(int, bool)"/> can be computed in constant time, Tdfs and Sdfs
///       becomes linear in v and e.
///       <br/>
///     - In this case, Time Complexity becomes O(v * (v + e) + v^3) which can be simplified as O(v^3), since e can 
///       be at most O(v^2) (in graphs where every vertex is connected to every other vertex).
///     </para>
/// </remarks>
public class SinkBasedTopologicalSort : ITopologicalSort
{
    /// <summary>
    /// The visitor to be used to identify sink vertices, by running Depth First Searches on the graph.
    /// </summary>
    public IVisitStrategy VisitStrategy { get; }

    /// <summary>
    ///     <inheritdoc cref="SinkBasedTopologicalSort"/>
    /// </summary>
    /// <param name="visitStrategy">
    ///     <inheritdoc cref="VisitStrategy" path="/summary"/>
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="SinkBasedTopologicalSort"/>
    /// </remarks>
    public SinkBasedTopologicalSort(IVisitStrategy visitStrategy)
    {
        VisitStrategy = visitStrategy;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    public IList<int> Sort(IGraph dag)
    {
        // Find reachable nodes for each vertex
        var numberOfVertices = dag.GetNumberOfVertices();
        var verticesReachableFromVertices = new Dictionary<int, HashSet<int>>(
            from vertex in Enumerable.Range(0, numberOfVertices)
            let verticesReachableFromVertex = VisitStrategy.DepthFirstSearchFromVertex(dag, vertex).ToHashSet()
            select KeyValuePair.Create(vertex, verticesReachableFromVertex));

        var topologicalSort = new int[numberOfVertices];
        var removedVertices = new HashSet<int>();
        while (verticesReachableFromVertices.Count > 0)
        {
            var lastSink = (
                from verticesReachableFromVertex in verticesReachableFromVertices
                let vertex = verticesReachableFromVertex.Key
                where !verticesReachableFromVertex.Value.Except(removedVertices).Except(new[] { vertex }).Any() // Sink
                select vertex)
                .DefaultIfEmpty(-1)
                .Max();

            if (lastSink < 0)
                throw new InvalidOperationException($"The provided {nameof(dag)} is not a Direct Acyclic Graph");

            topologicalSort[lastSink] = numberOfVertices - 1 - removedVertices.Count;
            removedVertices.Add(lastSink);
            verticesReachableFromVertices.Remove(lastSink);
        }

        return topologicalSort;
    }
}
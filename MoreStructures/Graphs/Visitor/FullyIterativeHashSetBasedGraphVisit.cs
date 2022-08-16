namespace MoreStructures.Graphs.Visitor;

/// <summary>
/// A <see cref="IVisitStrategy"/> implementation which uses an <see cref="HashSet{T}"/> of <see cref="int"/> to store
/// already visited vertices, while visiting the graph, and performs the visit iteratively, using a 
/// <see cref="Queue{T}"/>.
/// </summary>
public class FullyIterativeHashSetBasedGraphVisit : DirectionableVisit
{
    /// <summary>
    ///     <inheritdoc cref="FullyIterativeHashSetBasedGraphVisit"/>
    /// </summary>
    /// <param name="directedGraph">
    ///     <inheritdoc/>
    /// </param>
    public FullyIterativeHashSetBasedGraphVisit(bool directedGraph) : base(directedGraph)
    {
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    public override IEnumerable<int> DepthFirstSearch(IGraph graph)
    {
        var queue = new Queue<int>();
        var alreadyVisited = new HashSet<int>(); // Populated by ProcessQueue
        var numberOfVertices = graph.GetNumberOfVertices();
        for (var vertex = 0; vertex < numberOfVertices; vertex++)
        {
            queue.Enqueue(vertex);
            while (queue.Count > 0)
            {
                var maybeOutputItem = ProcessQueue(graph, queue, alreadyVisited);
                if (maybeOutputItem != null)
                    yield return maybeOutputItem.Value;
            }
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="advantages">
    ///     ADVANTAGES AND DISADVANTAGES
    ///     <br/>
    ///     <inheritdoc cref="DocFragments" path="/remarks/para[@id='fully-iterative-advantages']"/>
    ///     </para>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - The algorithm closely resembles to 
    ///       <see cref="FullyRecursiveHashSetBasedGraphVisit.Visit(MoreStructures.Graphs.IGraph, int)"/>, the only 
    ///       difference being that the state of the visit is stored into a <see cref="Queue{T}"/>, instantiated in the
    ///       heap, rather than in the call stack.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - As for <see cref="FullyRecursiveHashSetBasedGraphVisit.Visit(MoreStructures.Graphs.IGraph, int)"/>, 
    ///       Time Complexity is O(v * Ta) and Space Complexity is O(v * Sa), where e is the number of vertices and Ta and 
    ///       Sa are the time and space cost of retrieving the neighborhood of a given vertex.
    ///     </para>
    /// </remarks>
    public override IEnumerable<int> Visit(IGraph graph, int start)
    {
        var queue = new Queue<int>();
        var alreadyVisited = new HashSet<int>(); // Populated by ProcessQueue
        queue.Enqueue(start);
        while (queue.Count > 0)
            ProcessQueue(graph, queue, alreadyVisited);

        return alreadyVisited;
    }

    private int? ProcessQueue(IGraph graph, Queue<int> queue, HashSet<int> alreadyVisited)
    {
        var node = queue.Dequeue();

        if (alreadyVisited.Contains(node))
            return null;

        alreadyVisited.Add(node);
        var unexploredVertices = graph
            .GetAdjacentVerticesAndEdges(node, DirectedGraph)
            .Where(neighbor => !alreadyVisited.Contains(neighbor.vertex))
            .OrderBy(neighbor => neighbor.vertex)
            .ToHashSet();

        foreach (var (vertex, _) in unexploredVertices)
            queue.Enqueue(vertex);

        return node;
    }
}
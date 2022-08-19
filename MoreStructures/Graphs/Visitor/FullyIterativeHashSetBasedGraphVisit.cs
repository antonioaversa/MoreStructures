namespace MoreStructures.Graphs.Visitor;

/// <summary>
/// A <see cref="IVisitStrategy"/> implementation which uses an <see cref="HashSet{T}"/> of <see cref="int"/> to store
/// already visited vertices, while visiting the graph, and performs the visit iteratively, using a 
/// <see cref="Stack{T}"/>.
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
    /// <remarks>
    ///     <para id="advantages">
    ///     ADVANTAGES AND DISADVANTAGES
    ///     <br/>
    ///     Implemented fully recursively, so limited by stack depth and usable with graphs of a "reasonable" size.
    ///     </para>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - The total number of vertices of the graph is found via <see cref="IGraph.GetNumberOfVertices"/>.
    ///       <br/>
    ///     - If such number is v, vertices are identified by integers from 0 to v - 1.
    ///       <br/>
    ///     - Iterate over all vertices, visiting all other vertices reachable from the current vertex i, in the same 
    ///       way visit is performed by <see cref="IVisitStrategy.Visit(IGraph, int)"/>.
    ///       <br/>
    ///     - All visits share the same <see cref="HashSet{T}"/> of visited vertices, so that, if a vertex has been
    ///       visited before, directly or via another vertex, it's not visited again.
    ///       <br/>
    ///     - Every time a new vertex is visited, such vertex is yielded into the output sequence.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - The complexity of retrieving the total number of vertices depends on  
    ///       <see cref="IGraph.GetNumberOfVertices"/>. While being specific to the <see cref="IGraph"/>
    ///       implementation being used, all implementation provide O(1) runtime for such operation.
    ///       <br/>
    ///     - The <see cref="HashSet{T}"/> shared across all visits ensures that each vertex is visited at most once.
    ///       <br/>
    ///     - Because each vertex is visited at most once throughout the entire execution, edges are visited at most 
    ///       once, when edge direction is taken into account during the visit, and twice, when it is not.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(v * Ta + e) and Space Complexity is O(v * Sa + e), where 
    ///       v is the number of vertices, e is the number of edges and Ta and Sa are the time and space cost of 
    ///       retrieving the neighborhood of a given vertex.
    ///     </para>
    /// </remarks>
    public override IEnumerable<int> DepthFirstSearch(IGraph graph)
    {
        var stack = new Stack<int>();
        var alreadyVisited = new HashSet<int>(); // Populated by ProcessStack
        var numberOfVertices = graph.GetNumberOfVertices();
        for (var vertex = 0; vertex < numberOfVertices; vertex++)
        {
            stack.Push(vertex);
            while (stack.Count > 0)
            {
                var maybeOutputItem = ProcessStack(graph, stack, alreadyVisited);
                if (maybeOutputItem != null)
                    yield return maybeOutputItem.Value;
            }
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    public override IDictionary<int, int> ConnectedComponents(IGraph graph)
    {
        var stack = new Stack<int>();
        var alreadyVisited = new HashSet<int>(); // Populated by ProcessStack
        var numberOfVertices = graph.GetNumberOfVertices();

        var connectedComponents = new Dictionary<int, int>();
        var currentConnectedComponent = 0;
        for (var vertex = 0; vertex < numberOfVertices; vertex++)
        {
            stack.Push(vertex);
            bool currentConnectedComponentUsed = false;
            while (stack.Count > 0)
            {
                var maybeOutputItem = ProcessStack(graph, stack, alreadyVisited);
                if (maybeOutputItem != null)
                {
                    connectedComponents[maybeOutputItem.Value] = currentConnectedComponent;
                    currentConnectedComponentUsed = true;
                }
            }

            if (currentConnectedComponentUsed)
                currentConnectedComponent++;
        }

        return connectedComponents;
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
    ///       difference being that the state of the visit is stored into a <see cref="Stack{T}"/>, instantiated in the
    ///       heap, rather than in the call stack.
    ///       <br/>
    ///     - A <see cref="Stack{T}"/> is used, and not a <see cref="Queue{T}"/>, to preserve the order of visit.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - As for <see cref="FullyRecursiveHashSetBasedGraphVisit.Visit(MoreStructures.Graphs.IGraph, int)"/>, 
    ///       Time Complexity is O(v * Ta) and Space Complexity is O(v * Sa), where v is the number of vertices and 
    ///       Ta and Sa are the time and space cost of retrieving the neighborhood of a given vertex.
    ///     </para>
    /// </remarks>
    public override IEnumerable<int> Visit(IGraph graph, int start)
    {
        var stack = new Stack<int>();
        var alreadyVisited = new HashSet<int>(); // Populated by ProcessStack
        stack.Push(start);
        while (stack.Count > 0)
            ProcessStack(graph, stack, alreadyVisited);

        return alreadyVisited;
    }

    private int? ProcessStack(IGraph graph, Stack<int> stack, HashSet<int> alreadyVisited)
    {
        var node = stack.Pop();

        if (alreadyVisited.Contains(node))
            return null;

        alreadyVisited.Add(node);
        var unexploredVertices = graph
            .GetAdjacentVerticesAndEdges(node, DirectedGraph)
            .Where(neighbor => !alreadyVisited.Contains(neighbor.vertex))
            .OrderByDescending(neighbor => neighbor.vertex)
            .ToHashSet();

        foreach (var (vertex, _) in unexploredVertices)
            stack.Push(vertex);

        return node;
    }
}
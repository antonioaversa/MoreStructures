using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Graphs.StronglyConnectedComponents;

/// <summary>
/// An efficient implementation of <see cref="ISccFinder"/>, which runs a single DFS on the inverted graph to calculate
/// post orders, and then uses the post orders to identify sink SCC.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTANGES AND DISADVANTAGES
///     <br/>
///     - Compared to the simple approach of <see cref="NaiveSccFinder"/>, this implementation requires more complex
///       operations, such as reversing the graph, generating and storing post order values.
///       <br/>
///     - However, it has better runtime, approaching optimality, since it runs DFS only once, and not for every vertex
///       of the graph.
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm is based on the observation that the DFS on any vertex of a sink SCC is the sink SCC itself.
///       <br/>
///     - So the goal at each iteration is to efficiently find a vertex which belongs to a sink SCC and run DFS on it.
///       <br/>
///     - Such a vertex v can be found by running a single DFS on the reverse graph (graph with the same vertices and
///       reversed orientation of edges) and taking the vertex with the highest post order value.
///       <br/>
///     - After having identified v, DFS is run on v to find the sink SCC S, v is in. 
///       <br/>
///     - All found vertices are assigned the same SCC S: a list of v items SCCs is initialized to store the result and
///       SCCs[i] is set to S to indicate the vertex i is in the SCC with label S.
///       <br/>
///     - Then the operation is repeated, identifing the vertex v with the highest post order value, among the vertices
///       of the graph which haven't been assigned a SCC yet.
///       <br/>
///     - When there are no vertices left, the SCCs list is returned as result.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - <see cref="IGraph.Reverse"/> performance depends on the specific implementation of the graph.
///       <br/>
///     - Building and instrumenting the required <see cref="IVisitStrategy"/> instances and storing the post order 
///       do not depend on the side of the input.
///       <br/>
///     - The output has size equal to the number of vertices in the graph.
///       <br/>
///     - DFS, which is at the core of this algorithm, is run in two stages.
///       <br/>
///     - In the first stage it is executed just once, on the entire graph, calling 
///       <see cref="IVisitStrategy.DepthFirstSearchOfGraph"/>.
///       <br/>
///     - In the second stage it is executed iteratively, on vertices not assigned an SCC yet, this time calling 
///       <see cref="IVisitStrategy.DepthFirstSearchFromVertex"/>.
///       <br/>
///     - Because at each iteration chosen vertex belongs to a sink SCC (due to the fact that the vertex with the
///       highest remaining post order is selected), each vertex of the graph is explored at most once thoughout the
///       entire second stage.
///       <br/>
///     - Therefore, Time Complexity is O(Trvs + Tdfs) and Space Complexity is O(Srvs + Sdfs), where Trvs and Srvs are
///       the time and space required to reverse the provided <see cref="IGraph"/> and Tdfs and Sdfs are the time and 
///       space required to run DFS via the <see cref="IVisitStrategy"/> provided by 
///       <see cref="VisitStrategyBuilder"/>.
///       <br/>
///     - In typical implementations of <see cref="IGraph"/> and <see cref="IVisitStrategy"/>, Tdfs >= Trvs, and Tdfs
///       is O(v + e), which makes Time Complexity equals to O(v * Ta + e). 
///       <br/>
///     - Space Complexity is generally at least O(v + e + Sa), if <see cref="IGraph.Reverse"/> just builds a proxy of 
///       the provided graph, but can be O(v^2) for example if <see cref="IGraph"/> builds a separated transposed copy
///       of the adjacency matrix when reversing, and v^2 > v + e (very sparse graph).
///     </para>
/// </remarks>
public class SinkSccBasedSccFinder : ISccFinder
{
    /// <summary>
    /// A building function able to instantiate the <see cref="IVisitStrategy"/> to be used to run Depth First Searches
    /// of the entire graph and from vertex, via <see cref="IVisitStrategy.DepthFirstSearchOfGraph(IGraph)"/> and
    /// <see cref="IVisitStrategy.DepthFirstSearchFromVertex(IGraph, int)"/> respectively.
    /// </summary>
    public Func<IVisitStrategy> VisitStrategyBuilder { get; }

    /// <summary>
    ///     <inheritdoc cref="SinkSccBasedSccFinder"/>
    /// </summary>
    /// <param name="visitStrategyBuilder">
    ///     <inheritdoc cref="VisitStrategyBuilder" path="/summary"/>
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="SinkSccBasedSccFinder"/>
    /// </remarks>
    public SinkSccBasedSccFinder(Func<IVisitStrategy> visitStrategyBuilder)
    {
        VisitStrategyBuilder = visitStrategyBuilder;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="SinkSccBasedSccFinder"/>
    /// </remarks>
    public IList<int> Find(IGraph graph)
    {
        var reverseGraph = graph.Reverse();
        var reverseVisitStrategy = VisitStrategyBuilder();

        var numberOfVertices = graph.GetNumberOfVertices();
        var verticesInPostOrder = new int[numberOfVertices];
        var postOrderValue = 0;
        reverseVisitStrategy.VisitedVertex += (o, e) => verticesInPostOrder[postOrderValue++] = e.Vertex;
        MoreLinq.MoreEnumerable.Consume(reverseVisitStrategy.DepthFirstSearchOfGraph(reverseGraph));

        var directVisitStrategy = VisitStrategyBuilder();

        var scc = new int[numberOfVertices];
        for (int i = 0; i < numberOfVertices; i++)
            scc[i] = -1;

        var currentScc = 0;
        for (var i = numberOfVertices - 1; i >= 0; i--)
        {
            var vertex = verticesInPostOrder[i];

            if (scc[vertex] >= 0)
                continue;

            foreach (var reachableVertex in directVisitStrategy.DepthFirstSearchFromVertex(graph, vertex))
                scc[reachableVertex] = currentScc;

            currentScc++;
        }

        return scc;
    }
}
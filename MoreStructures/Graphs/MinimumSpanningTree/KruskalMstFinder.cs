using MoreStructures.DisjointSets;
using MoreStructures.Lists.Sorting;

namespace MoreStructures.Graphs.MinimumSpanningTree;

/// <summary>
/// A <see cref="IMstFinder"/> based on the Kruskal's algorithm.
/// </summary>
/// <remarks>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - First, all edges of the graph are sorted in ascending order, using the <see cref="Sorter"/> provided in the
///       constructor.
///       <br/>
///     - Then, set of edges MST, which will contain the result, is initialized to an empty set (i.e. no edges included
///       in the spanning tree, before the main loop).
///       <br/>
///     - A <see cref="IDisjointSet"/> DS is also initialized to contain the v values from 0 to v - 1, each value 
///       representing a vertex of the graph and at the beginning disjoint from all others. 
///       <br/>
///     - The disjoint set DS will be used as a <b>cycle detector</b>, i.e. as a data structure able to determine 
///       nearly in O(1) (more precisely in O(log*(n)) whether adding an edge to MST would result in a cycle.
///       <br/>
///     - Then, the main loop of the algorithm is executed, until MST contains exactly v - 1 edges in it (any spanning
///       tree has to have exactly v vertices and v - 1 edges, otherwise it would not be spanning, or it would not be 
///       a tree).
///       <br/>
///     - At each iteration, the next minimum edge e is added to MST, if it doesn't introduce a cycle. Otherwise, it is
///       discarded,
///       <br/>
///     - To verify whether e = (u, v) introduces a cycle in MST or not, it checks whether v and u are already 
///       connected in DS (via <see cref="IDisjointSet.AreConnected(int, int)"/>), i.e. whether they belong to the same
///       set. 
///       <br/>
///     - If so, it means that there is already a path (sequence of edges) in MST connecting u and v. Therefore the
///       introduction of the edge e = (u, v) would give a second path from u to v, hence a cycle.
///       <br/>
///     - Otherwise, e can safely be added to MST with no introduction of a cycle. The vertices u and v are also united
///       in DS, to reflect the addition of e into MST, in DS as well.
///       <br/>
///     - When MST contains v - 1 edges, the number of disjoint sets in DS is checked: if it is 1, it means that all
///       vertices in the graph have been connected by MST, and MST is indeed a Minimum Spanning Tree.
///       <br/>
///     - If it is not the case, and there is more than a single disjoint set, an 
///       <see cref="InvalidOperationException"/> is thrown, since the graph is not connected, and a single tree 
///       spanning the entire graph cannot be constructed.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - The algorithm makes use of an <see cref="IInPlaceSorting"/> algorithm, to sort all the edges of the graph in
///       ascending order by their distance. 
///       <br/>
///     - Since sorting edges is required by the algorithm, the overall complexity is lower bounded by 
///       O(e * log(e)) = O(e * log(v)). 
///       <br/>
///     - Such complexity can only be reduced when specific assumptions can be made, on the distance values of the 
///       edges (e.g. if integer and within a small range, a non-comparison based, linear search, such as Counting 
///       Sort, may be used).
///       <br/>
///     - The algorithm also makes use of a <see cref="IDisjointSet"/> data structure, to detect loops after the 
///       potential introduction of each single edge of the graph.
///       <br/>
///     - For that reason, for the algorithm to be efficient, when building the Minimum Spanning Tree, the disjoint
///       set should be able to verify connection (via <see cref="IDisjointSet.AreConnected(int, int)"/>) and to
///       merge vertices (via <see cref="IDisjointSet.Union(int, int)"/>) in (nearly) constant time.
///       <br/>
///     - While this is actually possible, by using a <see cref="PathCompressionWeightedQuickUnionDisjointSet"/>, the
///       overall complexity is dominated by the edge sorting, which is in general linearithmic over the edges.
///       <br/>
///     - For these reasons, Time Complexity is O(e * log(v)). Space Complexity is O(v + e), since large-enough data 
///       structures are required to store sorted edges and disjoint sets of vertices.
///     </para>
/// </remarks>
public class KruskalMstFinder : IMstFinder
{
    /// <summary>
    /// The <see cref="IInPlaceSorting"/> algorithm to be used to sort the edges of the graph.
    /// </summary>
    public IInPlaceSorting Sorter { get; }

    /// <summary>
    /// A build of <see cref="IDisjointSet"/> instances, used by the algorithm to detect potential cycles in the MST.
    /// </summary>
    public Func<int, IDisjointSet> DisjointSetBuilder { get; }

    /// <inheritdoc cref="KruskalMstFinder"/>
    /// <param name="sorter">
    ///     <inheritdoc cref="Sorter" path="/summary"/>
    /// </param>
    /// <param name="disjointSetBuilder">
    ///     <inheritdoc cref="DisjointSetBuilder" path="/summary"/>
    /// </param>
    public KruskalMstFinder(IInPlaceSorting sorter, Func<int, IDisjointSet> disjointSetBuilder)
    {
        Sorter = sorter;
        DisjointSetBuilder = disjointSetBuilder;
    }

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// Finds the Minimum Spanning Tree (MST) of the provided <paramref name="graph"/> using the Kruskal's algorithm.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="KruskalMstFinder"/>
    /// </remarks>
    public ISet<(int, int)> Find(IGraph graph, IGraphDistances distances)
    {
        var edges = graph.GetAllEdges().ToList();
        Sorter.Sort(edges, new EdgesComparer(distances));

        var numberOfVertices = graph.GetNumberOfVertices();
        var cycleDetector = DisjointSetBuilder(numberOfVertices);
        var mst = new HashSet<(int, int)> { };
        var currentEdgeIndex = 0;
        while (mst.Count < numberOfVertices - 1 && currentEdgeIndex < edges.Count)
        {
            var edge = edges[currentEdgeIndex++];
            if (cycleDetector.AreConnected(edge.edgeStart, edge.edgeEnd))
                continue;

            mst.Add(edge);
            cycleDetector.Union(edge.edgeStart, edge.edgeEnd);
        }

        if (cycleDetector.SetsCount > 1)
            throw new InvalidOperationException("The graph is not connected.");

        return mst;
    }

    private sealed class EdgesComparer : IComparer<(int, int)>
    {
        private IGraphDistances Distances { get; }

        public EdgesComparer(IGraphDistances distances)
        {
            Distances = distances;
        }

        public int Compare((int, int) x, (int, int) y) => Comparer<int>.Default.Compare(Distances[x], Distances[y]);
    }
}

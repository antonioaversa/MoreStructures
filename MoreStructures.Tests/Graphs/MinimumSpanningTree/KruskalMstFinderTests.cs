using MoreStructures.DisjointSets;
using MoreStructures.Graphs;
using MoreStructures.Graphs.MinimumSpanningTree;
using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Graphs.MinimumSpanningTree;

[TestClass]
public class KruskalMstFinderTests : MstFinderTests
{
    public KruskalMstFinderTests() 
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new KruskalMstFinder(
                new HeapSort(),
                numberOfVertices => new PathCompressionWeightedQuickUnionDisjointSet(numberOfVertices)))
    {
    }
}

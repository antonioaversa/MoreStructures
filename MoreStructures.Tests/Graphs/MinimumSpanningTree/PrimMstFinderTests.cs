using MoreStructures.Graphs;
using MoreStructures.Graphs.MinimumSpanningTree;
using MoreStructures.PriorityQueues.BinaryHeap;

namespace MoreStructures.Tests.Graphs.MinimumSpanningTree;

[TestClass]
public class PrimMstFinderTests : MstFinderTests
{
    public PrimMstFinderTests()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new PrimMstFinder(() => new UpdatableBinaryHeapPriorityQueue<int>()))
    {
    }
}

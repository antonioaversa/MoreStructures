using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

[TestClass]
public class BidirectionalAStarShortestDistanceFinderTests_WithoutHeuristic : DijkstraShortestDistanceFinderTests
{
    public BidirectionalAStarShortestDistanceFinderTests_WithoutHeuristic()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new BidirectionalAStarShortestDistanceFinder(() => new UpdatableBinomialHeapPriorityQueue<int>()))
    {
    }
}

[TestClass]
public class BidirectionalAStarShortestDistanceFinderTests_WithHeuristic : PotentialBasedShortestDistanceFinderTests
{
    public BidirectionalAStarShortestDistanceFinderTests_WithHeuristic()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new BidirectionalAStarShortestDistanceFinder(() => new UpdatableBinomialHeapPriorityQueue<int>()))
    {
    }
}

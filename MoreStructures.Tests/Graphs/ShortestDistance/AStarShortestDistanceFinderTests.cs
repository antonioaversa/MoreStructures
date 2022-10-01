using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

[TestClass]
public class AStarShortestDistanceFinderTests_WithoutHeuristic : DijkstraShortestDistanceFinderTests
{
    public AStarShortestDistanceFinderTests_WithoutHeuristic()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new AStarShortestDistanceFinder(() => new UpdatableBinomialHeapPriorityQueue<int>()))
    {
    }
}

[TestClass]
public class AStarShortestDistanceFinderTests_WithHeuristic : PotentialBasedShortestDistanceFinderTests
{
    public AStarShortestDistanceFinderTests_WithHeuristic()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new AStarShortestDistanceFinder(() => new UpdatableBinomialHeapPriorityQueue<int>()))
    {
    }
}

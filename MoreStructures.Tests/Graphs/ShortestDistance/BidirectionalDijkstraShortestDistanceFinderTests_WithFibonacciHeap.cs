using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.PriorityQueues.FibonacciHeap;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

[TestClass]
public class BidirectionalDijkstraShortestDistanceFinderTests_WithFibonacciHeap : DijkstraShortestDistanceFinderTests
{
    public BidirectionalDijkstraShortestDistanceFinderTests_WithFibonacciHeap()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new BidirectionalDijkstraShortestDistanceFinder(() => new UpdatableFibonacciHeapPriorityQueue<int>()))
    {
    }
}


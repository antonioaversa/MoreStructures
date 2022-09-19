using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.PriorityQueues.ArrayList;
using MoreStructures.PriorityQueues.BinaryHeap;
using MoreStructures.PriorityQueues.BinomialHeap;
using MoreStructures.PriorityQueues.FibonacciHeap;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

[TestClass]
public class DijkstraShortestDistanceFinderTests_WithArrayList : ShortestDistanceFinderTests
{
    public DijkstraShortestDistanceFinderTests_WithArrayList()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder(() => new ArrayListPriorityQueue<int>()))
    {
    }
}

[TestClass]
public class DijkstraShortestDistanceFinderTests_WithBinaryHeap : ShortestDistanceFinderTests
{
    public DijkstraShortestDistanceFinderTests_WithBinaryHeap()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder(() => new UpdatableBinaryHeapPriorityQueue<int>()))
    {
    }
}

[TestClass]
public class DijkstraShortestDistanceFinderTests_WithBinomialHeap : ShortestDistanceFinderTests
{
    public DijkstraShortestDistanceFinderTests_WithBinomialHeap()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder(() => new UpdatableBinomialHeapPriorityQueue<int>()))
    {
    }
}

[TestClass]
public class DijkstraShortestDistanceFinderTests_WithFibonacciHeap : ShortestDistanceFinderTests
{
    public DijkstraShortestDistanceFinderTests_WithFibonacciHeap()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder(() => new UpdatableFibonacciHeapPriorityQueue<int>()))
    {
    }
}

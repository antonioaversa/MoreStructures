using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.PriorityQueues.ArrayList;
using MoreStructures.PriorityQueues.BinaryHeap;
using MoreStructures.PriorityQueues.BinomialHeap;
using MoreStructures.PriorityQueues.FibonacciHeap;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

[TestClass]
public abstract class DijkstraShortestDistanceFinderTests : ShortestDistanceFinderTests
{
    protected DijkstraShortestDistanceFinderTests(
        Func<int, IList<(int, int)>, IGraph> graphBuilder, Func<IShortestDistanceFinder> finderBuilder) 
        : base(graphBuilder, finderBuilder)
    {
    }

    [DataRow("3 V, negative 3-C", 3,
        new[] { 0, 1, 2 },
        new[] { 1, 2, 0 },
        new[] { 1, -4, 2 }, 0, 2)]
    [DataRow("6 V, source to 1-chain and 3-chain merging to sink", 6,
        new[] { 0, 0, 1, 2, 4, 5 },
        new[] { 1, 2, 3, 4, 5, 1 },
        new[] { 9, -1, 1, 1, 1, 1 }, 0, 3)]
    [DataRow("9 V, source to sink, same source to 1-chain and 3-chain merging to 3-chain to sink", 9,
        new[] { 0, 0, 0, 1, 2, 3, 4, 5, 6, 7 },
        new[] { 1, 2, 8, 3, 4, 6, 5, 1, 7, 8 },
        new[] { 9, 1, -7, 1, 1, 1, 1, 1, 0, 0 }, 0, 8)]
    [DataTestMethod]
    public void Find_RaisesExceptionWhenNegativeEdgesAreEncountered(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances, int start, int end)
    {
        Assert.ThrowsException<InvalidOperationException>(() =>
            TestGraph(
                graphDescription, numberOfVertices, starts, ends, distances, start, end, -1, Array.Empty<int>()));
    }
}

[TestClass]
public class DijkstraShortestDistanceFinderTests_WithArrayList : DijkstraShortestDistanceFinderTests
{
    public DijkstraShortestDistanceFinderTests_WithArrayList()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder(() => new ArrayListPriorityQueue<int>()))
    {
    }
}

[TestClass]
public class DijkstraShortestDistanceFinderTests_WithBinaryHeap : DijkstraShortestDistanceFinderTests
{
    public DijkstraShortestDistanceFinderTests_WithBinaryHeap()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder(() => new UpdatableBinaryHeapPriorityQueue<int>()))
    {
    }
}

[TestClass]
public class DijkstraShortestDistanceFinderTests_WithBinomialHeap : DijkstraShortestDistanceFinderTests
{
    public DijkstraShortestDistanceFinderTests_WithBinomialHeap()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder(() => new UpdatableBinomialHeapPriorityQueue<int>()))
    {
    }
}

[TestClass]
public class DijkstraShortestDistanceFinderTests_WithFibonacciHeap : DijkstraShortestDistanceFinderTests
{
    public DijkstraShortestDistanceFinderTests_WithFibonacciHeap()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder(() => new UpdatableFibonacciHeapPriorityQueue<int>()))
    {
    }
}


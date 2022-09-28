using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.Graphs.ShortestDistanceTree;
using MoreStructures.PriorityQueues.ArrayList;

namespace MoreStructures.Tests.Graphs.ShortestDistanceTree;

[TestClass]
public class DijkstraShortestDistanceTreeFinderTests : ShortestDistanceTreeFinderTests
{
    public DijkstraShortestDistanceTreeFinderTests()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder(() => new ArrayListPriorityQueue<int>()),
            () => new DijkstraShortestDistanceTreeFinder(() => new ArrayListPriorityQueue<int>()))
    {
    }

    [DataRow("3 V, negative 3-C", 3,
        new[] { 0, 1, 2 },
        new[] { 1, 2, 0 },
        new[] { 1, -4, 2 }, 0)]
    [DataRow("6 V, source to 1-chain and 3-chain merging to sink", 6,
        new[] { 0, 0, 1, 2, 4, 5 },
        new[] { 1, 2, 3, 4, 5, 1 },
        new[] { 9, -1, 1, 1, 1, 1 }, 0)]
    [DataRow("9 V, source to sink, same source to 1-chain and 3-chain merging to 3-chain to sink", 9,
        new[] { 0, 0, 0, 1, 2, 3, 4, 5, 6, 7 },
        new[] { 1, 2, 8, 3, 4, 6, 5, 1, 7, 8 },
        new[] { 9, 1, -7, 1, 1, 1, 1, 1, 0, 0 }, 0)]
    [DataTestMethod]
    public void Find_RaisesExceptionWhenNegativeEdgesAreEncountered(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances, int start)
    {
        Assert.ThrowsException<InvalidOperationException>(() =>
            TestGraph(graphDescription, numberOfVertices, starts, ends, distances, start));
    }
}
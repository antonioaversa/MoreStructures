using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestPath;

namespace MoreStructures.Tests.Graphs.ShortestPath;

public abstract class ShortestPathFinderTests
{
    protected Func<int, IList<(int, int)>, IGraph> GraphBuilder { get; }
    protected Func<IShortestPathFinder> FinderBuilder { get; }

    protected ShortestPathFinderTests(
        Func<int, IList<(int, int)>, IGraph> graphBuilder, Func<IShortestPathFinder> finderBuilder)
    {
        GraphBuilder = graphBuilder;
        FinderBuilder = finderBuilder;
    }

    [DataRow("1 V, 1 isolated", 1, new int[] { }, new int[] { }, 0, 0,
        0, new[] { 0 })]
    [DataRow("2 V, 2 isolated", 2, new int[] { }, new int[] { }, 0, 1,
        int.MaxValue, new int[] { })]
    [DataRow("2 V, source to sink", 2, new[] { 0 }, new[] { 1 }, 0, 1,
        1, new[] { 0, 1 })]
    [DataRow("2 V, sink from source", 2, new[] { 1 }, new[] { 0 }, 0, 1,
        int.MaxValue, new int[] { })]
    [DataRow("2 V, 2-L, source to sink", 2, new[] { 0, 0, 1 }, new[] { 0, 1, 1 }, 0, 1,
        1, new[] { 0, 1 })]
    [DataRow("3 V, source to 2-chain and 1-chain, sharing leaf", 3, new[] { 0, 0, 1 }, new[] { 1, 2, 2 }, 0, 2,
        1, new[] { 0, 2 })]
    [DataRow("5 V, source to 2-chain and 1-chain, sharing leaf", 5, new[] { 0, 0, 1, 2, 4 }, new[] { 1, 2, 4, 3, 3 }, 
        0, 3,
        2, new[] { 0, 2, 3 })]
    [DataRow("9 V, source to 2-chain and 1-chain merging to intermediate, to 2-chain and 1-chain merging to leaf",
        9, new[] { 0, 0, 1, 2, 3, 4, 4, 5, 6, 8 }, new[] { 1, 2, 4, 3, 4, 5, 6, 8, 7, 7 }, 0, 7,
        4, new[] { 0, 1, 4, 6, 7 })]
    [DataTestMethod]
    public void Find_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int start, int end,
        int expectedDistance, int[] expectedPath)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var finder = FinderBuilder();
        var (distance, path) = finder.Find(graph, start, end);
        Assert.AreEqual(expectedDistance, distance);
        Assert.IsTrue(
            expectedPath.SequenceEqual(path),
            $"{graphDescription} - Expected [{string.Join(", ", expectedPath)}], Actual: [{string.Join(", ", path)}]");
    }
}

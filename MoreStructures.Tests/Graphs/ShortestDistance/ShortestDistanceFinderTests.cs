using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

public abstract class ShortestDistanceFinderTests
{
    protected Func<int, IList<(int, int)>, IGraph> GraphBuilder { get; }
    protected Func<IShortestDistanceFinder> FinderBuilder { get; }

    protected ShortestDistanceFinderTests(
        Func<int, IList<(int, int)>, IGraph> graphBuilder, Func<IShortestDistanceFinder> finderBuilder)
    {
        GraphBuilder = graphBuilder;
        FinderBuilder = finderBuilder;
    }

    [DataRow("3 V, source to 2-chain and 1-chain, sharing leaf 1", 3,
        new[] { 0, 0, 1 },
        new[] { 1, 2, 2 },
        new[] { 2, 3, 2 })]
    [DataTestMethod]
    public void Find_ThrowsExceptionWithInvalidStartOrEnd(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances)
    {
        Assert.ThrowsException<ArgumentException>(() => 
            TestGraph(
                graphDescription, numberOfVertices, starts, ends, distances, -1, 1, -1, Array.Empty<int>()));
        Assert.ThrowsException<ArgumentException>(() =>
            TestGraph(
                graphDescription, numberOfVertices, starts, ends, distances, -1, 1, -1, Array.Empty<int>()));
        Assert.ThrowsException<ArgumentException>(() =>
            TestGraph(
                graphDescription, numberOfVertices, starts, ends, distances, 1, -1, -1, Array.Empty<int>()));
        Assert.ThrowsException<ArgumentException>(() =>
            TestGraph(
                graphDescription, numberOfVertices, starts, ends, distances, 1, 3, -1, Array.Empty<int>()));
    }

    [DataRow("1 V, 1 isolated", 1, new int[] { }, new int[] { }, new int[] { }, 0, 0,
        0, new[] { 0 })]
    [DataRow("2 V, 2 isolated", 2, new int[] { }, new int[] { }, new int[] { }, 0, 1,
        int.MaxValue, new int[] { })]
    [DataRow("2 V, source to sink", 2, new[] { 0 }, new[] { 1 }, new[] { 5 }, 0, 1,
        5, new[] { 0, 1 })]
    [DataRow("2 V, sink from source", 2, new[] { 1 }, new[] { 0 }, new[] { 5 }, 0, 1,
        int.MaxValue, new int[] { })]
    [DataRow("2 V, 2-L, source to sink", 2, 
        new[] { 0, 0, 1 }, 
        new[] { 0, 1, 1 }, 
        new[] { 0, 4, 2 }, 0, 1,
        4, new[] { 0, 1 })]
    [DataRow("3 V, source to 2-chain and 1-chain, sharing leaf 1", 3, 
        new[] { 0, 0, 1 }, 
        new[] { 1, 2, 2 }, 
        new[] { 2, 3, 2 }, 0, 2,
        3, new[] { 0, 2 })]
    [DataRow("3 V, source to 2-chain and 1-chain, sharing leaf 2", 3, 
        new[] { 0, 0, 1 }, 
        new[] { 1, 2, 2 },
        new[] { 1, 3, 1 }, 0, 2,
        2, new[] { 0, 1, 2 })]
    [DataRow("5 V, source to 2-chain and 1-chain, sharing leaf 1", 5, 
        new[] { 0, 0, 1, 2, 4 }, 
        new[] { 1, 2, 4, 3, 3 },
        new[] { 3, 1, 2, 5, 2 }, 0, 3,
        6, new[] { 0, 2, 3 })]
    [DataRow("5 V, source to 2-chain and 1-chain, sharing leaf 2", 5, 
        new[] { 0, 0, 1, 2, 4 }, 
        new[] { 1, 2, 4, 3, 3 },
        new[] { 3, 1, 1, 5, 1 }, 0, 3,
        5, new[] { 0, 1, 4, 3 })]
    [DataRow("5 V, source to 4-chain with edge bypassing middle vertex", 5,
        new[] { 0, 1, 1, 2, 3 },
        new[] { 1, 2, 3, 3, 4 },
        new[] { 1, 2, 3, 2, 1 }, 0, 4,
        5, new[] { 0, 1, 3, 4 })]
    [DataRow("5 V, source to 4-chain with edge bypassing 3 vertices in the middle", 5,
        new[] { 0, 0, 1, 2, 3 },
        new[] { 1, 4, 2, 3, 4 },
        new[] { 1, 5, 2, 2, 1 }, 0, 4,
        5, new[] { 0, 4 })]
    [DataRow("5 V, 2 connected components: source to sink and source to intermediate to sink", 5,
        new[] { 0, 2, 3 },
        new[] { 1, 3, 4 },
        new[] { 1, 1, 1 }, 0, 4,
        int.MaxValue, new int[] { })]
    [DataRow("6 V, 5-C of vertices at distance 0, each to sink", 6,
        new[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 },
        new[] { 1, 5, 2, 5, 3, 5, 4, 5, 0, 5 },
        new[] { 0, 6, 0, 4, 0, 7, 0, 2, 0, 6 }, 0, 5,
        2, new[] { 0, 1, 2, 3, 5 })]
    [DataRow("6 V, source to 1-chain and 3-chain merging to sink", 6,
        new[] { 0, 0, 1, 2, 4, 5 },
        new[] { 1, 2, 3, 4, 5, 1 },
        new[] { 9, 1, 1, 1, 1, 1 }, 0, 3,
        5, new[] { 0, 2, 4, 5, 1, 3 })]
    [DataRow("6 V, source to 4-chain and 2-chain merging to sink, sink meeting point found going forward", 6,
        new[] { 0, 0, 1, 2, 3, 4 },
        new[] { 1, 2, 5, 3, 4, 5 },
        new[] { 1, 10, 5, 1, 1, 1 }, 0, 5,
        6, new[] { 0, 1, 5 })]
    [DataRow("6 V, source to 4-chain and 2-chain merging to sink, source meeting point found going backward", 6,
        new[] { 0, 0, 1, 2, 3, 4 },
        new[] { 1, 2, 5, 3, 4, 5 },
        new[] { 5, 1, 1, 1, 1, 10 }, 0, 5,
        6, new[] { 0, 1, 5 })]
    [DataRow("7 V, source to sink, same source to 1-chain and 3-chain merging to vertex to sink", 7,
        new[] { 0, 0, 0, 1, 2, 3, 4, 5 },
        new[] { 1, 2, 6, 3, 4, 6, 5, 1 },
        new[] { 9, 1, 7, 1, 1, 1, 1, 1 }, 0, 6,
        6, new[] { 0, 2, 4, 5, 1, 3, 6 })]
    [DataRow("9 V, source to sink, same source to 1-chain and 3-chain merging to 3-chain to sink", 9,
        new[] { 0, 0, 0, 1, 2, 3, 4, 5, 6, 7 },
        new[] { 1, 2, 8, 3, 4, 6, 5, 1, 7, 8 },
        new[] { 9, 1, 7, 1, 1, 1, 1, 1, 0, 0 }, 0, 8,
        6, new[] { 0, 2, 4, 5, 1, 3, 6, 7, 8 })]
    [DataRow("9 V, source to 2-chain and 1-chain merging to intermediate, to 2-chain and 1-chain merging to leaf 1", 9, 
        new[] { 0, 0, 1, 2, 3, 4, 4, 5, 6, 8 }, 
        new[] { 1, 2, 4, 3, 4, 5, 6, 8, 7, 7 },
        new[] { 2, 1, 1, 3, 0, 1, 2, 3, 1, 0 }, 0, 7,
        6, new[] { 0, 1, 4, 6, 7 })]
    [DataRow("9 V, source to 2-chain and 1-chain merging to intermediate, to 2-chain and 1-chain merging to leaf 2", 9,
        new[] { 0, 0, 1, 2, 3, 4, 4, 5, 6, 8 },
        new[] { 1, 2, 4, 3, 4, 5, 6, 8, 7, 7 },
        new[] { 2, 1, 3, 3, 0, 1, 2, 3, 1, 0 }, 0, 7,
        7, new[] { 0, 2, 3, 4, 6, 7 })]
    [DataRow("9 V, source to chains merging to intermediate, to chains merging to leaf, with shortcut to leaf 1", 9,
        new[] { 0, 0, 1, 2, 3, 3, 4, 4, 4, 5, 6, 8 },
        new[] { 1, 2, 4, 3, 4, 7, 3, 5, 6, 8, 7, 7 },
        new[] { 2, 1, 1, 3, 0, 2, 0, 1, 2, 3, 1, 0 }, 0, 7,
        5, new[] { 0, 1, 4, 3, 7 })]
    [DataRow("11 V, source to chains merging to intermediate, to chains merging to intermediate to leaf 1", 11,
        new[] { 0, 0, 0, 0, 1, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
        new[] { 1, 2, 3, 5, 8, 9, 1, 4, 1, 6, 7, 1, 10, 8 },
        new[] { 9, 1, 1, 1, 1, 0, 7, 1, 5, 1, 1, 3, 0, 0 }, 0, 10,
        6, new[] { 0, 5, 6, 7, 1, 9, 8, 10 })]
    [DataTestMethod]
    public void Find_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances, int start, int end,
        int expectedDistance, int[] expectedPath)
    {
        TestGraph(
            graphDescription, numberOfVertices, starts, ends, distances, start, end, expectedDistance, expectedPath);
    }

    protected void TestGraph(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances, int start, int end,
        int expectedDistance, int[] expectedPath)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var distancesDict = starts.Zip(ends).Zip(distances).ToDictionary(t => t.First, t => t.Second);

        var finder = FinderBuilder();
        var (distance, path) = finder.Find(graph, distancesDict, start, end);
        var message =
            $"{graphDescription} - Expected [{string.Join(", ", expectedPath)}], Actual: [{string.Join(", ", path)}]";
        Assert.AreEqual(expectedDistance, distance, message);
        Assert.IsTrue(expectedPath.SequenceEqual(path), message);
    }
}

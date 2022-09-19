using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

[TestClass]
public class BellmanFordShortestDistanceFinderTests : ShortestDistanceFinderTests
{
    public BellmanFordShortestDistanceFinderTests()
    : base(
        (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
        () => new BellmanFordShortestDistanceFinder(() => new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }

    [DataRow("2 V, source to sink", 2,
        new[] { 0 },
        new[] { 1 },
        new[] { -1 }, 0, 1,
        -1, new[] { 0, 1 })]
    [DataRow("3 V, source to sink and isolated", 3,
        new[] { 0 },
        new[] { 1 },
        new[] { -1 }, 0, 2,
        int.MaxValue, new int[] { })]
    [DataRow("3 V, source and intermediate to sink 1", 3,
        new[] { 0, 0, 2 },
        new[] { 1, 2, 1 },
        new[] { -1, 0, 0 }, 0, 1,
        -1, new[] { 0, 1 })]
    [DataRow("3 V, source and intermediate to sink 2", 3,
        new[] { 0, 0, 2 },
        new[] { 1, 2, 1 },
        new[] { 0, 1, -2 }, 0, 1,
        -1, new[] { 0, 2, 1 })]
    [DataRow("3 V, source and intermediate to sink 3", 3,
        new[] { 0, 0, 2 },
        new[] { 1, 2, 1 },
        new[] { 0, -1, 2 }, 0, 1,
        0, new[] { 0, 1 })]
    [DataRow("4 V, source and positive 2-C to sink", 4,
        new[] { 0, 0, 1, 2, 2 },
        new[] { 1, 3, 2, 1, 3 },
        new[] { -1, 1, -2, 3, 3 }, 0, 3,
        0, new[] { 0, 1, 2, 3 })]
    [DataRow("5 V, source and positive 3-C with nested 2-C to sink", 5,
        new[] { 0, 0, 1, 2, 2, 3, 3, 3 },
        new[] { 1, 4, 2, 3, 4, 1, 2, 4 },
        new[] { 2, 1, -1, -2, -1, 4, 3, 0 }, 0, 4,
        -1, new[] { 0, 1, 2, 3, 4 })]
    [DataTestMethod]
    public void Find_IsCorrectWithNegativeValues(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances, int start, int end,
        int expectedDistance, int[] expectedPath)
    {
        TestGraph(
            graphDescription, numberOfVertices, starts, ends, distances, start, end, expectedDistance, expectedPath);
    }

    [DataRow("2 V, 1 2-C 1", 2,
        new[] { 0, 1 },
        new[] { 1, 0 },
        new[] { -1, -1 }, 0, 0,
        int.MinValue, new int[] { })]
    [DataRow("2 V, 1 2-C 2", 2,
        new[] { 0, 1 },
        new[] { 1, 0 },
        new[] { -1, -1 }, 0, 1,
        int.MinValue, new int[] { })]
    [DataRow("2 V, 1 2-C 3", 2,
        new[] { 0, 1 },
        new[] { 1, 0 },
        new[] { 1, -2 }, 0, 1,
        int.MinValue, new int[] { })]
    [DataRow("3 V, source to intermediate to 2-C", 3,
        new[] { 0, 1, 2 },
        new[] { 1, 2, 1 },
        new[] { 1, 2, -3 }, 0, 2,
        int.MinValue, new int[] { })]
    [DataRow("4 V, source to sink and 2-C", 4,
        new[] { 0, 0, 2, 3 },
        new[] { 1, 2, 3, 2 },
        new[] { 1, 1, 1, -2 }, 0, 1,
        1, new[] { 0, 1 })]
    [DataRow("4 V, source to 2-C, both to sink", 4,
        new[] { 0, 0, 2, 3, 3 },
        new[] { 1, 2, 3, 2, 1 },
        new[] { 1, 1, 1, -2, 1000 }, 0, 1,
        int.MinValue, new int[] { })]
    [DataTestMethod]
    public void Find_IsCorrectWithNegativeCycles(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances, int start, int end,
        int expectedDistance, int[] expectedPath)
    {
        TestGraph(
            graphDescription, numberOfVertices, starts, ends, distances, start, end, expectedDistance, expectedPath);
    }
}
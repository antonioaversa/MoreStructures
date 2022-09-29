using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.Graphs.ShortestDistanceTree;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.ShortestDistanceTree;

[TestClass]
public class BellmanFordShortestDistanceTreeFinderTests : ShortestDistanceTreeFinderTests
{
    public BellmanFordShortestDistanceTreeFinderTests()
    : base(
        (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
        () => new BellmanFordShortestDistanceFinder(() => new FullyIterativeHashSetBasedGraphVisit(true)),
        () => new BellmanFordShortestDistanceTreeFinder(() => new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }

    [DataRow("5 V, source and positive 3-C with nested 2-C to sink", 5,
        new[] { 0, 0, 1, 2, 2, 3, 3, 3 },
        new[] { 1, 4, 2, 3, 4, 1, 2, 4 },
        new[] { 2, 1, -1, -2, -1, 4, 3, 0 })]
    [DataTestMethod]
    public void Find_IsCorrectWithNegativeValues(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances)
    {
        for (var start = 0; start < numberOfVertices; start++)
            TestGraph(graphDescription, numberOfVertices, starts, ends, distances, start);
    }

    [DataRow("4 V, source to 2-C, both to sink", 4,
        new[] { 0, 0, 2, 3, 3 },
        new[] { 1, 2, 3, 2, 1 },
        new[] { 1, 1, 1, -2, 1000 })]
    [DataTestMethod]
    public void Find_IsCorrectWithNegativeCycles(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances)
    {
        for (var start = 0; start < numberOfVertices; start++)
            TestGraph(graphDescription, numberOfVertices, starts, ends, distances, start);
    }
}

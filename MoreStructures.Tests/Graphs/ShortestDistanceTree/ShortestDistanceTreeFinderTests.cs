using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.Graphs.ShortestDistanceTree;

namespace MoreStructures.Tests.Graphs.ShortestDistanceTree;

public abstract class ShortestDistanceTreeFinderTests
{
    protected Func<int, IList<(int, int)>, IGraph> GraphBuilder { get; }
    protected Func<IShortestDistanceFinder> SinglePathFinderBuilder { get; }
    protected Func<IShortestDistanceTreeFinder> FinderBuilder { get; }

    protected ShortestDistanceTreeFinderTests(
        Func<int, IList<(int, int)>, IGraph> graphBuilder,
        Func<IShortestDistanceFinder> singlePathFinderBuilder,
        Func<IShortestDistanceTreeFinder> finderBuilder)
    {
        GraphBuilder = graphBuilder;
        SinglePathFinderBuilder = singlePathFinderBuilder;
        FinderBuilder = finderBuilder;
    }

    [DataRow("11 V, source to chains merging to intermediate, to chains merging to intermediate to leaf 1", 11,
        new[] { 0, 0, 0, 0, 1, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
        new[] { 1, 2, 3, 5, 8, 9, 1, 4, 1, 6, 7, 1, 10, 8 },
        new[] { 9, 1, 1, 1, 1, 0, 7, 1, 5, 1, 1, 3, 0, 0 })]
    [DataTestMethod]
    public void Find_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances)
    {
        for (var start = 0; start < numberOfVertices; start++)
            TestGraph(
                graphDescription, numberOfVertices, starts, ends, distances, start);
    }

    protected void TestGraph(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances, int start)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var distancesDict = starts.Zip(ends).Zip(distances).ToDictionary(t => t.First, t => t.Second);

        var finder = FinderBuilder();
        var bestPreviouses = finder.Find(graph, distancesDict, start).Values;
        var bestDistances =
            from bp in bestPreviouses
            orderby bp.Key
            select bp.Value.DistanceFromStart;

        var singlePathFinder = SinglePathFinderBuilder();
        var expectedBestDistances = Enumerable
            .Range(0, numberOfVertices)
            .Select(v => singlePathFinder.Find(graph, distancesDict, start, v).Item1)
            .Where(d => d < int.MaxValue);

        var message =
            $"{graphDescription} - Expected [{string.Join(", ", expectedBestDistances)}], " +
            $"Actual: [{string.Join(", ", bestDistances)}]";
        Assert.IsTrue(expectedBestDistances.SequenceEqual(bestDistances), message);

        for (var vertex = 0; vertex < numberOfVertices; vertex++)
        {
            var path = new LinkedList<int>();
            var current = vertex;
            while (current >= 0 && bestPreviouses.TryGetValue(current, out var bestPrevious))
            {
                path.AddFirst(current);
                current = bestPrevious.PreviousVertex;
            }

            if (path.First == null)
                continue;

            Assert.IsTrue(path.First.Value == start);

            var expectedPathDistance = path.Zip(path.Skip(1)).Sum(e => distancesDict[e]);
            Assert.AreEqual(bestPreviouses[vertex].DistanceFromStart, expectedPathDistance);
        }
    }
}

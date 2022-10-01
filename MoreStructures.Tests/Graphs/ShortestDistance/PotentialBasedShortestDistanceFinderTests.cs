using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

public abstract class PotentialBasedShortestDistanceFinderTests
{
    protected Func<int, IList<(int, int)>, IGraph> GraphBuilder { get; }
    protected Func<IPotentialBasedShortestDistanceFinder> FinderBuilder { get; }

    protected PotentialBasedShortestDistanceFinderTests(
        Func<int, IList<(int, int)>, IGraph> graphBuilder, Func<IPotentialBasedShortestDistanceFinder> finderBuilder)
    {
        GraphBuilder = graphBuilder;
        FinderBuilder = finderBuilder;
    }

    [DataRow("7 V, source to sink, same source to 1-chain and 3-chain merging to vertex to sink", 7,
        new[] { 0, 0, 0, 1, 2, 3, 4, 5 },
        new[] { 1, 2, 6, 3, 4, 6, 5, 1 },
        new[] { 27, 3, 21, 3, 3, 3, 3, 3 }, 
        new int[]
        {
            9, 8, 7, 6, 5, 4, 3, // Bad potentials: non-sensible values
            1, 1, 1, 1, 1, 1, 1, // Bad potentials: all equal values
            0, 3, 1, 4, 2, 3, 3, // Approx euclidean potentials calculated from vertex 0
            3, 0, 3, 1, 2, 1, 1, // Approx euclidean potentials calculated from vertex 1
            1, 3, 2, 1, 3, 2, 4, // Approx euclidean potentials calculated from vertex 2
        })]
    [DataTestMethod]
    public void Find_IsCorrect(
        string graphDescription, int numberOfVertices, int[] starts, int[] ends, int[] distances, int[] potentials)
    {
        var graph = GraphBuilder(numberOfVertices, starts.Zip(ends).ToList());
        var graphDistances = new DictionaryAdapterGraphDistances(
            starts.Zip(ends).Zip(distances).ToDictionary(t => t.First, t => t.Second));

        var finder = FinderBuilder();

        for (var start = 0; start < numberOfVertices; start++)
        {
            for (var end = 0; end < numberOfVertices; end++)
            {
                for (var i = 0; i < potentials.Length; i += numberOfVertices)
                {
                    var (distanceWithHeuristic, pathWithHeuristic) =
                        finder.Find(graph, graphDistances, v => potentials[i + v], start, end);
                    var (distanceWithoutHeuristic, pathWithoutHeuristic) =
                        finder.Find(graph, graphDistances, start, end);
                    Assert.AreEqual(distanceWithoutHeuristic, distanceWithHeuristic, graphDescription);
                    Assert.IsTrue(pathWithoutHeuristic.SequenceEqual(pathWithHeuristic), graphDescription);
                }
            }
        }
    }
}

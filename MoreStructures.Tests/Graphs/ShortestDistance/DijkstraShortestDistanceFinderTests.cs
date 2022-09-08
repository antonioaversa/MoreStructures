using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

[TestClass]
public class DijkstraShortestDistanceFinderTests : ShortestDistanceFinderTests
{
    public DijkstraShortestDistanceFinderTests()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new DijkstraShortestDistanceFinder())
    {
    }
}

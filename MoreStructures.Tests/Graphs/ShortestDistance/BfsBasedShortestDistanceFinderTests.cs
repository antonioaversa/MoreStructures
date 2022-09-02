using MoreStructures.Graphs;
using MoreStructures.Graphs.ShortestDistance;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.ShortestDistance;

[TestClass]
public class BfsBasedShortestDistanceFinderTests : ShortestDistanceFinderTests
{
    public BfsBasedShortestDistanceFinderTests()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new BfsBasedShortestDistanceFinder(
                () => new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}
using MoreStructures.EdgeListGraphs;
using MoreStructures.Graphs.ShortestPath;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.ShortestPath;

[TestClass]
public class BfsBasedShortestPathFinderTests : ShortestPathFinderTests
{
    public BfsBasedShortestPathFinderTests() 
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new BfsBasedShortestPathFinder(
                () => new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}

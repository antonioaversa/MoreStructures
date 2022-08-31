using MoreStructures.Graphs;
using MoreStructures.Graphs.StronglyConnectedComponents;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.StronglyConnectedComponents;

[TestClass]
public class SinkSccBasedSccFinderTests : SccFinderTests
{
    public SinkSccBasedSccFinderTests()
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new SinkSccBasedSccFinder(
                () => new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}



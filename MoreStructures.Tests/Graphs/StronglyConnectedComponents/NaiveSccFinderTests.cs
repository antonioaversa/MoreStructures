using MoreStructures.Graphs;
using MoreStructures.Graphs.StronglyConnectedComponents;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.StronglyConnectedComponents;

[TestClass]
public class NaiveSccFinderTests : SccFinderTests
{
    public NaiveSccFinderTests() 
        : base(
            (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
            () => new NaiveSccFinder(new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}



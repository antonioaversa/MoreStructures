using MoreStructures.Graphs;

namespace MoreStructures.Tests.Graphs;

[TestClass]
public class EdgeListGraphTests : GraphTests
{
    public EdgeListGraphTests() : base(
        (numberOfVertices, edges) =>
            new EdgeListGraph(numberOfVertices, edges))
    {
    }
}
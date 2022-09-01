using MoreStructures.Graphs;

namespace MoreStructures.Tests.Graphs;

[TestClass]
public class AdjacencyListGraphTests : GraphTests
{
    public AdjacencyListGraphTests() : base(
        (numberOfVertices, edges) => 
            new AdjacencyListGraph(GraphTestUtils.BuildNeighborhoods(numberOfVertices, edges)))
    {
    }
}

using MoreStructures.Graphs;

namespace MoreStructures.Tests.Graphs;

[TestClass]
public class AdjacencyMatrixGraphTests : GraphTests
{
    public AdjacencyMatrixGraphTests() : base(
    (numberOfVertices, edges) =>
        new AdjacencyMatrixGraph(GraphTestUtils.BuildMatrix(numberOfVertices, edges)))
    {
    }
}
using MoreStructures.Graphs;
using MoreStructures.Graphs.Sorting;

namespace MoreStructures.Tests.Graphs.Sorting;

[TestClass]
public class AnyPathToSinkBasedTopologicalSortTests_WithEdgeListGraph : TopologicalSortTests
{
    public AnyPathToSinkBasedTopologicalSortTests_WithEdgeListGraph() : base(
        (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
        new AnyPathToSinkBasedTopologicalSort())
    {
    }
}

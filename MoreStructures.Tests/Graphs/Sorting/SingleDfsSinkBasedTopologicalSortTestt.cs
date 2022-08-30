using MoreStructures.EdgeListGraphs;
using MoreStructures.Graphs.Sorting;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Sorting;

[TestClass]
public class SingleDfsSinkBasedTopologicalSortTests_WithEdgeListGraph : TopologicalSortTests
{
    public SingleDfsSinkBasedTopologicalSortTests_WithEdgeListGraph() : base(
        (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
        new SingleDfsSinkBasedTopologicalSort(() => new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}

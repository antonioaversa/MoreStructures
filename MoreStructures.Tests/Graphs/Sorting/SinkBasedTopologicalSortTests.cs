using MoreStructures.AdjacencyListGraphs;
using MoreStructures.AdjacencyMatrixGraphs;
using MoreStructures.EdgeListGraphs;
using MoreStructures.Graphs.Sorting;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Sorting;

[TestClass]
public class SinkBasedTopologicalSortTests_WithEdgeListGraph : TopologicalSortTests
{
    public SinkBasedTopologicalSortTests_WithEdgeListGraph() : base(
        (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges), 
        new SinkBasedTopologicalSort(new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}

[TestClass]
public class SinkBasedTopologicalSortTests_WithAdjacencyListGraph : TopologicalSortTests
{
    public SinkBasedTopologicalSortTests_WithAdjacencyListGraph() : base(
        (numberOfVertices, edges) => new AdjacencyListGraph(
            GraphTestUtils.BuildNeighborhoods(numberOfVertices, edges)),
        new SinkBasedTopologicalSort(new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}

[TestClass]
public class SinkBasedTopologicalSortTests_WithAdjacencyMatrixGraph : TopologicalSortTests
{
    public SinkBasedTopologicalSortTests_WithAdjacencyMatrixGraph() : base(
        (numberOfVertices, edges) => new AdjacencyMatrixGraph(
            GraphTestUtils.BuildMatrix(numberOfVertices, edges)),
        new SinkBasedTopologicalSort(new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}

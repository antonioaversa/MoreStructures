using MoreStructures.AdjacencyListGraphs;
using MoreStructures.AdjacencyMatrixGraphs;
using MoreStructures.EdgeListGraphs;
using MoreStructures.Graphs.Sorting;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Sorting;

[TestClass]
public class DfsOnEachVertexSinkBasedTopologicalSortTests_WithEdgeListGraph : TopologicalSortTests
{
    public DfsOnEachVertexSinkBasedTopologicalSortTests_WithEdgeListGraph() : base(
        (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges), 
        new DfsOnEachVertexSinkBasedTopologicalSort(new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}

[TestClass]
public class DfsOnEachVertexSinkBasedTopologicalSortTests_WithAdjacencyListGraph : TopologicalSortTests
{
    public DfsOnEachVertexSinkBasedTopologicalSortTests_WithAdjacencyListGraph() : base(
        (numberOfVertices, edges) => new AdjacencyListGraph(
            GraphTestUtils.BuildNeighborhoods(numberOfVertices, edges)),
        new DfsOnEachVertexSinkBasedTopologicalSort(new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}

[TestClass]
public class DfsOnEachVertexSinkBasedTopologicalSortTests_WithAdjacencyMatrixGraph : TopologicalSortTests
{
    public DfsOnEachVertexSinkBasedTopologicalSortTests_WithAdjacencyMatrixGraph() : base(
        (numberOfVertices, edges) => new AdjacencyMatrixGraph(
            GraphTestUtils.BuildMatrix(numberOfVertices, edges)),
        new DfsOnEachVertexSinkBasedTopologicalSort(new FullyIterativeHashSetBasedGraphVisit(true)))
    {
    }
}

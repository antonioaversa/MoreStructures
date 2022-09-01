using MoreStructures.Graphs;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Visitor;

[TestClass]
public class FullyIterativeHashSetBasedGraphVisitTests_WithEdgeListGraph : VisitStrategyTests
{
    public FullyIterativeHashSetBasedGraphVisitTests_WithEdgeListGraph() : base(
        (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
        directedGraph => new FullyIterativeHashSetBasedGraphVisit(directedGraph))
    {
    }
}

[TestClass]
public class FullyRecursiveHashSetBasedGraphVisitTests_WithAdjacencyListGraph : VisitStrategyTests
{
    public FullyRecursiveHashSetBasedGraphVisitTests_WithAdjacencyListGraph() : base(
        (numberOfVertices, edges) => new AdjacencyListGraph(
            GraphTestUtils.BuildNeighborhoods(numberOfVertices, edges)),
        directedGraph => new FullyRecursiveHashSetBasedGraphVisit(
            directedGraph))
    {
    }
}

[TestClass]
public class FullyRecursiveHashSetBasedGraphVisitTests_WithAdjacencyMatrixGraph : VisitStrategyTests
{
    public FullyRecursiveHashSetBasedGraphVisitTests_WithAdjacencyMatrixGraph() : base(
        (numberOfVertices, edges) => new AdjacencyMatrixGraph(
            GraphTestUtils.BuildMatrix(numberOfVertices, edges)),
        directedGraph => new FullyRecursiveHashSetBasedGraphVisit(directedGraph))
    {
    }
}
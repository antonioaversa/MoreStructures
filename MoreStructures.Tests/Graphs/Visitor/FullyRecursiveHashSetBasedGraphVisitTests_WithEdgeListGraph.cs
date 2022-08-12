using MoreStructures.EdgeListGraphs;
using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Visitor;

[TestClass]
public class FullyRecursiveHashSetBasedGraphVisitTests_WithEdgeListGraph : VisitStrategyTests
{
    public FullyRecursiveHashSetBasedGraphVisitTests_WithEdgeListGraph() : base(
        (numberOfVertices, edges) => new EdgeListGraph(numberOfVertices, edges),
        directedGraph => new FullyRecursiveHashSetBasedGraphVisit(directedGraph))
    {
    }
}

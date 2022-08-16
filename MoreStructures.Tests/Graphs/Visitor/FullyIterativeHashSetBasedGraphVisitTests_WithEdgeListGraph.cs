using MoreStructures.EdgeListGraphs;
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
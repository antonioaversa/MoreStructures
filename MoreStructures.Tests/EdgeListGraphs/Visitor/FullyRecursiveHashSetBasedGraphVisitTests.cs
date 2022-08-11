using MoreStructures.EdgeListGraphs.Visitor;

namespace MoreStructures.Tests.EdgeListGraphs.Visitor;

[TestClass]
public class FullyRecursiveHashSetBasedGraphVisitTests : VisitStrategyTests
{
    public FullyRecursiveHashSetBasedGraphVisitTests() : base(
        directedGraph => new FullyRecursiveHashSetBasedGraphVisit(directedGraph))
    {
    }
}


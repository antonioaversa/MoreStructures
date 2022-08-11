using MoreStructures.Graphs.Visitor;

namespace MoreStructures.Tests.Graphs.Visitor;

[TestClass]
public class FullyRecursiveHashSetBasedGraphVisitTests : VisitStrategyTests
{
    public FullyRecursiveHashSetBasedGraphVisitTests() : base(
        directedGraph => new FullyRecursiveHashSetBasedGraphVisit(directedGraph))
    {
    }
}


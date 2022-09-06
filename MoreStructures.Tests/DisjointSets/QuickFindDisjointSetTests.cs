using MoreStructures.DisjointSets;

namespace MoreStructures.Tests.DisjointSets;

[TestClass]
public class QuickFindDisjointSetTests : DisjointSetTests
{
    public QuickFindDisjointSetTests() : base(valuesCount => new QuickFindDisjointSet(valuesCount))
    {
    }
}

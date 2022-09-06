using MoreStructures.DisjointSets;

namespace MoreStructures.Tests.DisjointSets;

[TestClass]
public class QuickUnionDisjointSetTests : DisjointSetTests
{
    public QuickUnionDisjointSetTests() : base(valuesCount => new QuickUnionDisjointSet(valuesCount))
    {
    }
}

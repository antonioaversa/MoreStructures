using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.DisjointSets;

namespace MoreStructures.Tests.DisjointSets;

[TestClass]
public class QuickFindDisjointSetTests : DisjointSetTests
{
    public QuickFindDisjointSetTests() : base(valuesCount => new QuickFindDisjointSet(valuesCount))
    {
    }

    [TestMethod]
    public void Ctor_SetsValuesCountAccordingly()
    {
        var disjointSet = Builder(4);
        Assert.AreEqual(4, disjointSet.ValuesCount);
    }

    [TestMethod]
    public void Ctor_SetsSetsCountAccordingly()
    {
        var disjointSet = Builder(4);
        Assert.AreEqual(4, disjointSet.SetCount);
    }

    [TestMethod]
    public void Ctor_RaisesExceptionOnInvalidValuesCount()
    {
        Assert.ThrowsException<ArgumentException>(() => Builder(-1));
        Assert.ThrowsException<ArgumentException>(() => Builder(-100));
    }
}
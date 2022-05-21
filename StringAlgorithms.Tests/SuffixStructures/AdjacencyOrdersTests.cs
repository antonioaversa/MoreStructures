using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;

namespace StringAlgorithms.Tests.SuffixStructures;

[TestClass]
public class AdjacencyOrdersTests
{
    [TestMethod]
    public void FlagsConsistency()
    {
        Assert.AreEqual(AdjacencyOrders.None, AdjacencyOrders.None & AdjacencyOrders.Before);
        Assert.AreEqual(AdjacencyOrders.None, AdjacencyOrders.None & AdjacencyOrders.After);
        Assert.AreEqual(AdjacencyOrders.Before, AdjacencyOrders.None | AdjacencyOrders.Before);
        Assert.AreEqual(AdjacencyOrders.After, AdjacencyOrders.None | AdjacencyOrders.After);
        Assert.AreEqual(AdjacencyOrders.BeforeOrAfter, AdjacencyOrders.None | AdjacencyOrders.BeforeOrAfter);
        Assert.AreEqual(AdjacencyOrders.BeforeOrAfter, AdjacencyOrders.Before | AdjacencyOrders.After);
        Assert.AreEqual(AdjacencyOrders.None, AdjacencyOrders.Before & AdjacencyOrders.After);
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixStructures;
using MoreStructures.SuffixTrees;

namespace MoreStructures.Tests.SuffixStructures
{
    [TestClass]
    public class SuffixStructuresEdgeExtensionsTests
    {
        [TestMethod]
        public void IsAdjacentTo_IsCorrect()
        {
            Assert.IsTrue(new SuffixTreeEdge(0, 1).IsAdjacentTo(new(1, 2), AdjacencyOrders.Before));
            Assert.IsFalse(new SuffixTreeEdge(0, 1).IsAdjacentTo(new(1, 2), AdjacencyOrders.After));
            Assert.IsTrue(new SuffixTreeEdge(0, 1).IsAdjacentTo(new(1, 2), AdjacencyOrders.BeforeOrAfter));

            Assert.IsFalse(new SuffixTreeEdge(1, 2).IsAdjacentTo(new(0, 1), AdjacencyOrders.Before));
            Assert.IsTrue(new SuffixTreeEdge(1, 2).IsAdjacentTo(new(0, 1), AdjacencyOrders.After));
            Assert.IsTrue(new SuffixTreeEdge(1, 2).IsAdjacentTo(new(0, 1), AdjacencyOrders.BeforeOrAfter));
        }
    }
}

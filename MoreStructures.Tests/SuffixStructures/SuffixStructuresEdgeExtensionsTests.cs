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

        [TestMethod]
        public void ContainsIndex_ThrowsExceptionOnInvalidIndex()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(0, 1).ContainsIndex(-4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(0, 1).ContainsIndex(-1));
        }

        [TestMethod]
        public void ContainsIndex_IsCorrect()
        {
            Assert.IsFalse(new SuffixTreeEdge(0, 0).ContainsIndex(0));
            Assert.IsFalse(new SuffixTreeEdge(0, 0).ContainsIndex(1));
            Assert.IsTrue(new SuffixTreeEdge(0, 1).ContainsIndex(0));
            Assert.IsFalse(new SuffixTreeEdge(0, 1).ContainsIndex(1));
            Assert.IsTrue(new SuffixTreeEdge(0, 2).ContainsIndex(1));
            Assert.IsTrue(new SuffixTreeEdge(7, 3).ContainsIndex(7));
            Assert.IsTrue(new SuffixTreeEdge(7, 3).ContainsIndex(9));
            Assert.IsFalse(new SuffixTreeEdge(7, 3).ContainsIndex(10));
        }

        [TestMethod]
        public void ContainsIndexesNonBiggerThan_ThrowsExceptionOnInvalidIndex()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new SuffixTreeEdge(0, 1).ContainsIndexesNonBiggerThan(-4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new SuffixTreeEdge(0, 1).ContainsIndexesNonBiggerThan(-1));
        }

        [TestMethod]
        public void ContainsIndexesNonBiggerThan_IsCorrect()
        {
            Assert.IsTrue(new SuffixTreeEdge(0, 0).ContainsIndexesNonBiggerThan(0));
            Assert.IsTrue(new SuffixTreeEdge(0, 0).ContainsIndexesNonBiggerThan(1));
            Assert.IsTrue(new SuffixTreeEdge(0, 1).ContainsIndexesNonBiggerThan(0));
            Assert.IsTrue(new SuffixTreeEdge(0, 1).ContainsIndexesNonBiggerThan(1));
            Assert.IsTrue(new SuffixTreeEdge(0, 2).ContainsIndexesNonBiggerThan(1));
            Assert.IsFalse(new SuffixTreeEdge(7, 3).ContainsIndexesNonBiggerThan(6));
            Assert.IsTrue(new SuffixTreeEdge(7, 3).ContainsIndexesNonBiggerThan(7));
            Assert.IsTrue(new SuffixTreeEdge(7, 3).ContainsIndexesNonBiggerThan(9));
            Assert.IsTrue(new SuffixTreeEdge(7, 3).ContainsIndexesNonBiggerThan(10));
        }
    }
}

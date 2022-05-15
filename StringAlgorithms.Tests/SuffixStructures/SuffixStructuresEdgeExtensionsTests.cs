using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;
using StringAlgorithms.SuffixTrees;

namespace StringAlgorithms.Tests.SuffixStructures
{
    [TestClass]
    public class SuffixStructuresEdgeExtensionsTests
    {
        [TestMethod]
        public void IsAdjacentTo_IsCorrect()
        {
            Assert.IsTrue(new SuffixTreeEdge(0, 1).IsAdjacentTo(new(1, 2), AdjacencyOrder.Before));
            Assert.IsFalse(new SuffixTreeEdge(0, 1).IsAdjacentTo(new(1, 2), AdjacencyOrder.After));
            Assert.IsTrue(new SuffixTreeEdge(0, 1).IsAdjacentTo(new(1, 2), AdjacencyOrder.BeforeOrAfter));

            Assert.IsFalse(new SuffixTreeEdge(1, 2).IsAdjacentTo(new(0, 1), AdjacencyOrder.Before));
            Assert.IsTrue(new SuffixTreeEdge(1, 2).IsAdjacentTo(new(0, 1), AdjacencyOrder.After));
            Assert.IsTrue(new SuffixTreeEdge(1, 2).IsAdjacentTo(new(0, 1), AdjacencyOrder.BeforeOrAfter));
        }
    }
}

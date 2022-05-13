using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringAlgorithms.SuffixTrees.Tests;

[TestClass]
public class SuffixTreeEdgeTests
{
    [TestMethod]
    public void IsAdjacentTo_Correctness()
    {
        Assert.IsTrue(new SuffixTreeEdge(0, 1).IsAdjacentTo(new (1, 2), SuffixTreeEdge.AdjacencyOrder.Before));
        Assert.IsFalse(new SuffixTreeEdge(0, 1).IsAdjacentTo(new(1, 2), SuffixTreeEdge.AdjacencyOrder.After));
        Assert.IsTrue(new SuffixTreeEdge(0, 1).IsAdjacentTo(new(1, 2), SuffixTreeEdge.AdjacencyOrder.BeforeOrAfter));

        Assert.IsFalse(new SuffixTreeEdge(1, 2).IsAdjacentTo(new(0, 1), SuffixTreeEdge.AdjacencyOrder.Before));
        Assert.IsTrue(new SuffixTreeEdge(1, 2).IsAdjacentTo(new(0, 1), SuffixTreeEdge.AdjacencyOrder.After));
        Assert.IsTrue(new SuffixTreeEdge(1, 2).IsAdjacentTo(new(0, 1), SuffixTreeEdge.AdjacencyOrder.BeforeOrAfter));
    }

    [TestMethod]
    public void Of()
    {
        Assert.IsTrue(new SuffixTreeEdge(0, 0).Of(new("", '$')) == "");
        Assert.IsTrue(new SuffixTreeEdge(0, 1).Of(new("", '$')) == "$");
        Assert.IsTrue(new SuffixTreeEdge(0, 0).Of(new("a", '$')) == "");
        Assert.IsTrue(new SuffixTreeEdge(0, 1).Of(new("a", '$')) == "a");
        Assert.IsTrue(new SuffixTreeEdge(0, 2).Of(new("a", '$')) == "a$");
    }

}

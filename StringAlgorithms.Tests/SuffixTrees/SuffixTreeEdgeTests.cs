using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StringAlgorithms.SuffixTrees.Tests;

[TestClass]
public class SuffixTreeEdgeTests
{
    [TestMethod]
    public void Ctor_ValidIndex()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(-1, 1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(0, -1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(0, 0));
    }

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
    public void Of_InboundIndexes()
    {
        Assert.IsTrue(new SuffixTreeEdge(0, 1).Of(new("", '$')) == "$");
        Assert.IsTrue(new SuffixTreeEdge(0, 1).Of(new("a", '$')) == "a");
        Assert.IsTrue(new SuffixTreeEdge(0, 2).Of(new("a", '$')) == "a$");
    }

    [TestMethod]
    public void Of_OutOfBoundsIndexes()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(1, 1).Of(new("", '$')));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(2, 1).Of(new("a", '$')));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(2, 2).Of(new("a", '$')));
    }

}

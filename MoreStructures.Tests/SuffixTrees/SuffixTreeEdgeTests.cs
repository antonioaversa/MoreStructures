using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixTrees;
using System;

namespace MoreStructures.Tests.SuffixTrees;

[TestClass]
public class SuffixTreeEdgeTests
{
    [TestMethod]
    public void Ctor_ValidIndex()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(-1, 1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(0, -1));
    }

    [TestMethod]
    public void Ctor_ZeroLengthIsValid()
    {
        var edge = new SuffixTreeEdge(0, 0);
        Assert.AreEqual(0, edge.Length);
    }

    [TestMethod]
    public void CompareTo_IsCorrect()
    {
        Assert.IsTrue(new SuffixTreeEdge(0, 1).CompareTo(new SuffixTreeEdge(0, 1)) == 0);
        Assert.IsTrue(new SuffixTreeEdge(0, 1).CompareTo(new SuffixTreeEdge(0, 2)) < 0);
        Assert.IsTrue(new SuffixTreeEdge(0, 2).CompareTo(new SuffixTreeEdge(0, 1)) > 0);
        Assert.IsTrue(new SuffixTreeEdge(0, 1).CompareTo(new SuffixTreeEdge(1, 1)) < 0);
        Assert.IsTrue(new SuffixTreeEdge(0, 2).CompareTo(new SuffixTreeEdge(1, 1)) < 0);
        Assert.IsTrue(new SuffixTreeEdge(1, 1).CompareTo(new SuffixTreeEdge(0, 1)) > 0);
        Assert.IsTrue(new SuffixTreeEdge(1, 1).CompareTo(new SuffixTreeEdge(0, 2)) > 0);
        Assert.ThrowsException<ArgumentException>(() =>
            new SuffixTreeEdge(0, 1).CompareTo(null));
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

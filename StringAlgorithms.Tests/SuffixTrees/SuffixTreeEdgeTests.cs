using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixTrees;
using System;

namespace StringAlgorithms.Tests.SuffixTrees;

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

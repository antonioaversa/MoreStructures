using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StringAlgorithms.SuffixTries.Tests;

[TestClass]
public class SuffixTrieEdgeTests
{
    [TestMethod]
    public void Ctor_ValidIndex()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTrieEdge(-1));
    }

    [TestMethod]
    public void Of_InboundIndexes()
    {
        Assert.IsTrue(new SuffixTrieEdge(0).Of(new("a", '$')) == "a");
        Assert.IsTrue(new SuffixTrieEdge(1).Of(new("a", '$')) == "$");
    }

    [TestMethod]
    public void Of_OutOfBoundsIndexes()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTrieEdge(1).Of(new("", '$')));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTrieEdge(2).Of(new("a", '$')));
    }
}

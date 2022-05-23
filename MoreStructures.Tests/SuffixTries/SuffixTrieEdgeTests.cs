using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixTries;
using System;

namespace MoreStructures.Tests.SuffixTries;

[TestClass]
public class SuffixTrieEdgeTests
{
    [TestMethod]
    public void Ctor_ValidIndex()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTrieEdge(-1));
    }

    [TestMethod]
    public void CompareTo_IsCorrect()
    {
        Assert.IsTrue(new SuffixTrieEdge(0).CompareTo(new SuffixTrieEdge(0)) == 0);
        Assert.IsTrue(new SuffixTrieEdge(0).CompareTo(new SuffixTrieEdge(1)) < 0);
        Assert.IsTrue(new SuffixTrieEdge(1).CompareTo(new SuffixTrieEdge(0)) > 0);
        Assert.ThrowsException<ArgumentException>(() =>
            new SuffixTrieEdge(1).CompareTo(null));
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

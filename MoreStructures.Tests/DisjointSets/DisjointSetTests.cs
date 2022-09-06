using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.DisjointSets;

namespace MoreStructures.Tests.DisjointSets;

public abstract class DisjointSetTests
{
    protected Func<int, IDisjointSet> Builder { get; }

    protected DisjointSetTests(Func<int, IDisjointSet> builder)
    {
        Builder = builder;
    }

    [TestMethod]
    public void Ctor_SetsValuesCountAccordingly()
    {
        var disjointSet = Builder(4);
        Assert.AreEqual(4, disjointSet.ValuesCount);
    }

    [TestMethod]
    public void Ctor_SetsSetsCountAccordingly()
    {
        var disjointSet = Builder(4);
        Assert.AreEqual(4, disjointSet.SetsCount);
    }

    [TestMethod]
    public void Ctor_RaisesExceptionOnInvalidValuesCount()
    {
        Assert.ThrowsException<ArgumentException>(() => Builder(-1));
        Assert.ThrowsException<ArgumentException>(() => Builder(-100));
    }

    [TestMethod]
    public void SetsCount_IsCorrect()
    {
        var disjointSet = Builder(10);
        Assert.AreEqual(10, disjointSet.SetsCount);
        disjointSet.Union(0, 1);
        Assert.AreEqual(9, disjointSet.SetsCount);
        disjointSet.Union(0, 1);
        Assert.AreEqual(9, disjointSet.SetsCount);
        disjointSet.Union(1, 2);
        Assert.AreEqual(8, disjointSet.SetsCount);
        disjointSet.Union(0, 2);
        Assert.AreEqual(8, disjointSet.SetsCount);
    }

    [TestMethod]
    public void SetCounts_WithTransitivity()
    {
        var disjointSet = Builder(10);
        Assert.AreEqual(10, disjointSet.SetsCount);
        disjointSet.Union(1, 2);
        disjointSet.Union(3, 1);
        disjointSet.Union(3, 4);
        disjointSet.Union(5, 1);
        disjointSet.Union(2, 0);
        Assert.AreEqual(5, disjointSet.SetsCount);
    }

    [TestMethod]
    public void AreConnected_RaisesExceptionOnInvalidValues()
    {
        var disjointSet = Builder(10);
        Assert.ThrowsException<ArgumentException>(() => disjointSet.AreConnected(0, -1));
        Assert.ThrowsException<ArgumentException>(() => disjointSet.AreConnected(-1, 0));
        Assert.ThrowsException<ArgumentException>(() => disjointSet.AreConnected(10, 2));
        Assert.ThrowsException<ArgumentException>(() => disjointSet.AreConnected(2, 10));
    }

    [TestMethod]
    public void AreConnected_RaisesExceptionOnEmptyQueue()
    {
        var emptyDisjointSet = Builder(0);
        Assert.ThrowsException<InvalidOperationException>(() => emptyDisjointSet.AreConnected(0, 0));
    }

    [TestMethod]
    public void AreConnected_IsReflexive()
    {
        var disjointSet = Builder(10);
        Assert.IsTrue(disjointSet.AreConnected(3, 3));
        Assert.IsTrue(disjointSet.AreConnected(9, 9));
    }
    
    [TestMethod]
    public void AreConnected_IsSymmetric()
    {
        var disjointSet = Builder(10);
        disjointSet.Union(3, 4);
        Assert.IsTrue(disjointSet.AreConnected(3, 4));
        Assert.IsTrue(disjointSet.AreConnected(4, 3));
        Assert.IsFalse(disjointSet.AreConnected(0, 3));
        Assert.IsFalse(disjointSet.AreConnected(3, 0));
    }

    [TestMethod]
    public void AreConnected_IsTransitive()
    {
        var disjointSet = Builder(10);
        Assert.IsFalse(disjointSet.AreConnected(0, 1));
        Assert.IsFalse(disjointSet.AreConnected(0, 9));
        disjointSet.Union(0, 9);
        Assert.IsTrue(disjointSet.AreConnected(0, 9));
        Assert.IsFalse(disjointSet.AreConnected(0, 8));
        disjointSet.Union(8, 9);
        Assert.IsTrue(disjointSet.AreConnected(0, 8));
        Assert.IsTrue(disjointSet.AreConnected(8, 9));
    }

    [TestMethod]
    public void Find_ThrowsExceptionOnInvalidValue()
    {
        var disjointSet = Builder(10);
        Assert.ThrowsException<ArgumentException>(() => disjointSet.Find(-1));
        Assert.ThrowsException<ArgumentException>(() => disjointSet.Find(10));
        Assert.ThrowsException<ArgumentException>(() => disjointSet.Find(100));
    }

    [TestMethod]
    public void Find_ThrowsExceptionOnEmptyQueue()
    {
        var emptyDisjointSet = Builder(0);
        Assert.ThrowsException<InvalidOperationException>(() => emptyDisjointSet.Find(0));
    }

    [TestMethod]
    public void Find_ReturnsDifferentSetIdsForNonConnectedValues()
    {
        var disjointSet = Builder(10);
        Assert.IsFalse(disjointSet.AreConnected(2, 3));
        Assert.AreNotEqual(disjointSet.Find(2), disjointSet.Find(3));
    }

    [TestMethod]
    public void Find_ReturnsSameSetIdForConnectedValues()
    {
        var disjointSet = Builder(10);
        disjointSet.Union(2, 3);
        Assert.AreEqual(disjointSet.Find(2), disjointSet.Find(3));
    }

    [TestMethod]
    public void Find_IsReflexive()
    {
        var disjointSet = Builder(10);
        for (var i = 0; i < disjointSet.ValuesCount; i++)
            Assert.AreEqual(disjointSet.Find(i), disjointSet.Find(i));
    }

    [TestMethod]
    public void Find_IsSymmetric()
    {
        var disjointSet = Builder(10);
        disjointSet.Union(2, 3);
        Assert.AreEqual(disjointSet.Find(3), disjointSet.Find(2));
    }

    [TestMethod]
    public void Find_IsTransitive()
    {
        var disjointSet = Builder(10);
        disjointSet.Union(2, 3);
        disjointSet.Union(3, 5);
        Assert.AreEqual(disjointSet.Find(2), disjointSet.Find(5));
    }

    [TestMethod]
    public void Union_IsIdempotent()
    {
        var disjointSet = Builder(10);
        disjointSet.Union(2, 3);
        Assert.IsTrue(disjointSet.AreConnected(2, 3));
        disjointSet.Union(2, 3);
        Assert.IsTrue(disjointSet.AreConnected(2, 3));
    }

    [TestMethod]
    public void Union_ThrowsExceptionOnInvalidValues()
    {
        var disjointSet = Builder(10);
        Assert.ThrowsException<ArgumentException>(() => disjointSet.Union(0, -1));
        Assert.ThrowsException<ArgumentException>(() => disjointSet.Union(-1, 0));
        Assert.ThrowsException<ArgumentException>(() => disjointSet.Union(10, 2));
        Assert.ThrowsException<ArgumentException>(() => disjointSet.Union(2, 10));
    }

    [TestMethod]
    public void Union_ThrowsExceptionOnEmptyQueue()
    {
        var emptyDisjointSet = Builder(0);
        Assert.ThrowsException<InvalidOperationException>(() => emptyDisjointSet.Union(0, 0));
    }

    [TestMethod]
    public void Union_BehavesCorrectlyWithCycles()
    {
        var disjointSet = Builder(10);
        disjointSet.Union(2, 3);
        disjointSet.Union(3, 2);
        Assert.IsTrue(disjointSet.AreConnected(2, 3));
        disjointSet.Union(2, 3);
        disjointSet.Union(3, 4);
        disjointSet.Union(4, 2);
        Assert.IsTrue(disjointSet.AreConnected(2, 4));
        Assert.IsTrue(disjointSet.AreConnected(4, 2));
        Assert.IsTrue(disjointSet.AreConnected(2, 3));
        Assert.IsTrue(disjointSet.AreConnected(3, 2));
    }
}

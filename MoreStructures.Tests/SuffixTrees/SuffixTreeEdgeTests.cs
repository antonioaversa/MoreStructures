using MoreStructures.SuffixTrees;

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

    [TestMethod]
    public void OfRotated_InboundIndexes()
    {
        Assert.IsTrue(new SuffixTreeEdge(0, 1).OfRotated(new("$", '$')) == "$");
        Assert.IsTrue(new SuffixTreeEdge(0, 1).OfRotated(new("$abcd", '$')) == "$");
        Assert.IsTrue(new SuffixTreeEdge(0, 2).OfRotated(new("$abcd", '$')) == "$a");
    }

    [TestMethod]
    public void OfRotated_OutOfBoundsIndexes()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(1, 1).OfRotated(new("$", '$')));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(2, 1).OfRotated(new("$a", '$')));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SuffixTreeEdge(2, 2).OfRotated(new("$ab", '$')));
    }

    [TestMethod]
    public void ToString_OfEquivalentInstancesAreTheSame()
    {
        var edgeStr1 = new SuffixTreeEdge(1, 2).ToString();
        var edgeStr2 = new SuffixTreeEdge(1, 2).ToString();
        Assert.AreEqual(edgeStr2, edgeStr1);
    }

    [TestMethod]
    public void ToString_WithDifferentStartOrLengthAreDifferent()
    {
        var edgeStr1 = new SuffixTreeEdge(1, 2).ToString();
        var edgeStr2 = new SuffixTreeEdge(1, 3).ToString();
        Assert.AreNotEqual(edgeStr2, edgeStr1);

        var edgeStr3 = new SuffixTreeEdge(2, 2).ToString();
        Assert.AreNotEqual(edgeStr3, edgeStr1);
    }
}

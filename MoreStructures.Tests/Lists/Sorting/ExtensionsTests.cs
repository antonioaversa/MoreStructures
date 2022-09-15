using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

[TestClass]
public class ExtensionsTests
{
    [TestMethod]
    public void IsSorted_WithComparable_IsCorrectWithNumbers()
    {
        Assert.IsTrue(Array.Empty<int>().IsSorted());
        Assert.IsTrue(new List<int> { }.IsSorted());
        Assert.IsTrue(new[] { 1 }.IsSorted());
        Assert.IsFalse(new[] { 3, 2, 4, 5 }.IsSorted());
        Assert.IsTrue(new[] { 1, 2, 4, 5 }.IsSorted());
        Assert.IsTrue(new[] { 1.0, 1.0, 4.0, 5.0 }.IsSorted());
        Assert.IsFalse(new[] { 1.0, -1.0 }.IsSorted());
        Assert.IsTrue(new[] { -1.0, 1.0 }.IsSorted());
        Assert.IsFalse(new[] { 1.0f, 1.0f, -4.0f, 5.0f }.IsSorted());
        Assert.IsTrue(Enumerable.Range(100, 0).ToList().IsSorted());
    }

    [TestMethod]
    public void IsSorted_WithComparable_IsCorrectWithBool()
    {
        Assert.IsTrue(new[] { false, false, true }.IsSorted());
        Assert.IsFalse(new[] { true, false, true }.IsSorted());
        Assert.IsTrue(new[] { true, true, true }.IsSorted());
    }

    [TestMethod]
    public void IsSorted_WithComparable_IsCorrectWithChars()
    {
        Assert.IsTrue(new[] { 'a', 'a', 'b' }.IsSorted());
        Assert.IsTrue(new[] { 'a', 'b', 'c' }.IsSorted());
        Assert.IsTrue(new[] { 'A', 'a', 'b' }.IsSorted());
        Assert.IsFalse(new[] { 'a', 'c', 'b' }.IsSorted());
        Assert.IsFalse(new[] { 'a', 'c', 'A' }.IsSorted());
    }

    [TestMethod]
    public void IsSorted_WithComparer_IsCorrect()
    {
        var invertedComparer = new InvertedComparer<int>();
        Assert.IsTrue(new[] { 5, 3, -1 }.IsSorted(invertedComparer));
        Assert.IsFalse(new[] { 1, 2, 3 }.IsSorted(invertedComparer));
    }

    private sealed class InvertedComparer<T> : IComparer<T>
        where T : IComparable<T>
    {
        public int Compare(T? x, T? y)
        {
            if (x == null) return y == null ? 0 : -1;
            if (y == null) return 1;
            return -x.CompareTo(y);
        }
    }
}

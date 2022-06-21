namespace MoreStructures.Tests;

[TestClass]
public class VirtuallyRotatedTextWithTerminatorTests
{
    private static void AssertEnumeratorSequence<T>(IEnumerator<T> enumerator, params T[] values)
    {
        foreach (var value in values)
        {
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(value, enumerator.Current);
        }
        Assert.IsFalse(enumerator.MoveNext());
    }

    private static void AssertEnumeratorSequence(System.Collections.IEnumerator enumerator, params object[] values)
    {
        foreach (var value in values)
        {
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(value, enumerator.Current);
        }
        Assert.IsFalse(enumerator.MoveNext());
    }

    [TestMethod]
    public void GetEnumerator_IteratesCorrectly()
    {
        var text = new RotatedTextWithTerminator("abad$");
        var vrtext = new VirtuallyRotatedTextWithTerminator(text, 0);
        using var vrtextEnumerator = vrtext.GetEnumerator();
        AssertEnumeratorSequence(vrtextEnumerator, 'a', 'b', 'a', 'd', text.Terminator);
    }

    [TestMethod]
    public void GetEnumerator_ResetsCorrectly()
    {
        var text = new RotatedTextWithTerminator("abad$");
        var vrtext = new VirtuallyRotatedTextWithTerminator(text, 0);
        using var vrtextEnumerator = vrtext.GetEnumerator();
        AssertEnumeratorSequence(vrtextEnumerator, 'a', 'b', 'a', 'd', text.Terminator);

        vrtextEnumerator.Reset();
        AssertEnumeratorSequence(vrtextEnumerator, 'a', 'b', 'a', 'd', text.Terminator);
    }

    [TestMethod]
    public void GetEnumerator_CurrentRaisesExceptionBeforeAndAfter()
    {
        var text = new RotatedTextWithTerminator("abad$");
        var vrtext = new VirtuallyRotatedTextWithTerminator(text, 0);
        using var vrtextEnumerator = vrtext.GetEnumerator();
        Assert.ThrowsException<InvalidOperationException>(() => vrtextEnumerator.Current);
        for (int i = 0; i < text.Length + 1; i++)
            vrtextEnumerator.MoveNext();
        Assert.ThrowsException<InvalidOperationException>(() => vrtextEnumerator.Current);
    }

    [TestMethod]
    public void GetEnumerator_AsIEnumerable_IteratesCorrectly()
    {
        var text = new RotatedTextWithTerminator("abad$");
        var vrtext = new VirtuallyRotatedTextWithTerminator(text, 0) as System.Collections.IEnumerable;
        var vrtextEnumerator = vrtext.GetEnumerator();
        AssertEnumeratorSequence(vrtextEnumerator, 'a', 'b', 'a', 'd', text.Terminator);
    }

    [TestMethod]
    public void GetEnumerator_AsIEnumerable_ResetsCorrectly()
    {
        var text = new RotatedTextWithTerminator("abad$");
        var vrtext = new VirtuallyRotatedTextWithTerminator(text, 0) as System.Collections.IEnumerable;
        var vrtextEnumerator = vrtext.GetEnumerator();
        AssertEnumeratorSequence(vrtextEnumerator, 'a', 'b', 'a', 'd', text.Terminator);

        vrtextEnumerator.Reset();
        AssertEnumeratorSequence(vrtextEnumerator, 'a', 'b', 'a', 'd', text.Terminator);
    }

    [TestMethod]
    public void Indexer_FollowsRotation()
    {
        Assert.AreEqual('a', b("abc$", 0)[0]);
        Assert.AreEqual('$', b("abc$", 1)[0]);
        Assert.AreEqual('b', b("abc$", -1)[0]);
        Assert.AreEqual('a', b("abc$", 0)[4]);
        Assert.AreEqual('a', b("abc$", 1)[5]);
        Assert.AreEqual('$', b("abc$", 0)[^1]);
        Assert.AreEqual('c', b("abc$", 1)[^1]);
    }

    [TestMethod]
    public void CompareTo_IsCorrect()
    {
        Assert.ThrowsException<ArgumentNullException>(() => b("abc$", 0).CompareTo(null));
        Assert.AreEqual(0, b("abc$", 0).CompareTo(b("abc$", 0)));
        Assert.AreEqual(0, b("abc$", 0).CompareTo(b("abc$", 4)));
        Assert.AreEqual(0, b("abc$", 0).CompareTo(b("abc$", -4)));
        Assert.AreEqual(0, b("abc$", 0).CompareTo(b("abc$", 8)));
        Assert.AreEqual(0, b("abc$", 0).CompareTo(b("abc$", -8)));

        Assert.AreEqual(0, b("$abc", 0).CompareTo(b("abc$", 1)));
        Assert.AreEqual(0, b("bc$a", 0).CompareTo(b("abc$", -1)));
        Assert.AreEqual(0, b("c$ab", 0).CompareTo(b("abc$", 2)));
        Assert.AreEqual(0, b("c$ab", 0).CompareTo(b("abc$", -2)));

        Assert.AreEqual(-1, b("ab$", 0).CompareTo(b("ab$c", 0)));
        Assert.AreEqual(1, b("ab$c", 0).CompareTo(b("ab$", 0)));
        Assert.AreEqual(-1, b("abc$", 0).CompareTo(b("abd$", 0)));
    }

    private static VirtuallyRotatedTextWithTerminator b(string text, int rotation, char terminator = '$') =>
        new(new(text, terminator), rotation);
}

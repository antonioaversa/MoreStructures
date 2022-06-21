using MoreStructures.Utilities;

namespace MoreStructures.Tests;

[TestClass]
public class RotatedTextWithTerminatorTests
{
    [ExcludeFromCodeCoverage(Justification = "Mock structure only partially used")]
    private record HardcodedSelector(string Selection) : TextWithTerminator.ISelector
    {
        public string Of(TextWithTerminator text) => Selection;
        public string OfRotated(RotatedTextWithTerminator text) => Selection;
    }

    [ExcludeFromCodeCoverage(Justification = "Mock structure only partially used")]
    private record FirstCharSelector() : TextWithTerminator.ISelector
    {
        public string Of(TextWithTerminator text) => string.Concat(text[0..1]);
        public string OfRotated(RotatedTextWithTerminator text) => string.Concat(text[0..1]);
    }

    [TestMethod]
    public void Ctor_Preconditions()
    {
        Assert.IsNotNull(new RotatedTextWithTerminator("a$a", '$'));
        Assert.ThrowsException<ArgumentException>(() => new RotatedTextWithTerminator("a", '$'));
        Assert.ThrowsException<ArgumentException>(() => new RotatedTextWithTerminator("a$$a", '$'));
    }

    [TestMethod]
    public void Equals_IsByValueWithStrings()
    {
        var text1 = new RotatedTextWithTerminator("a$");
        var text2 = new RotatedTextWithTerminator("a$");
        Assert.AreEqual(text1, text2);
    }

    [TestMethod]
    public void Equals_IsByValueWithListOfChar()
    {
        var text1 = new RotatedTextWithTerminator("a$".ToList());
        var text2 = new RotatedTextWithTerminator("a$".ToList());
        Assert.AreEqual(text1, text2);
    }

    private static IEnumerable<char> SomeChars()
    {
        yield return 'a';
        yield return '$';
    }

    [TestMethod]
    public void Equals_IsByValueWithEnumerableOfChar()
    {
        var text1 = new RotatedTextWithTerminator(SomeChars());
        var text2 = new RotatedTextWithTerminator(SomeChars());
        Assert.AreEqual(text1, text2);
    }

    [TestMethod]
    public void Indexer_WithSelector()
    {
        Assert.AreEqual("b", new RotatedTextWithTerminator("c$ab", '$')[new HardcodedSelector("b")]);
        Assert.AreEqual("c", new RotatedTextWithTerminator("c$ab", '$')[new FirstCharSelector()]);
    }

    [TestMethod]
    public void Indexer_WithRangeOfString()
    {
        Assert.AreEqual("c".AsValue(), new RotatedTextWithTerminator("c$ab", '$')[0..1].AsValue());
        Assert.AreEqual(string.Empty.AsValue(), new RotatedTextWithTerminator("c$ab", '$')[0..0].AsValue());
    }

    [TestMethod]
    public void Indexer_WithRangeOfEnumerable()
    {
        Assert.AreEqual("c$ab".AsValue(), new RotatedTextWithTerminator("c$ab".ToArray(), '$')[0..].AsValue());
        Assert.AreEqual("c$".AsValue(), new RotatedTextWithTerminator("c$ab".ToArray(), '$')[..^2].AsValue());
    }

    [TestMethod]
    public void Indexer_WithIndexOfString()
    {
        Assert.AreEqual('c', new RotatedTextWithTerminator("c$ab", '$')[0]);
        Assert.AreEqual('b', new RotatedTextWithTerminator("c$ab", '$')[3]);
    }

    [TestMethod]
    public void Indexer_WithIndexOfEnumerable()
    {
        Assert.AreEqual('c', new RotatedTextWithTerminator("c$ab".ToArray(), '$')[0]);
        Assert.AreEqual('b', new RotatedTextWithTerminator("c$ab".ToArray(), '$')[3]);
    }

    [TestMethod]
    public void Length_IsCorrect()
    {
        Assert.AreEqual(4, new RotatedTextWithTerminator("c$ab", '$').Length);
        Assert.AreEqual(1, new RotatedTextWithTerminator("$", '$').Length);
    }

    [TestMethod]
    public void StartsWith_IsCorrectWithString()
    {
        Assert.IsTrue(new RotatedTextWithTerminator("a$", '$').StartsWith("a"));
    }

    [TestMethod]
    public void StartsWith_IsCorrectWithEnumerable()
    {
        Assert.IsTrue(new RotatedTextWithTerminator("a$".ToArray(), '$').StartsWith("a"));
    }

    [TestMethod]
    public void EndsWith_IsCorrectWithString()
    {
        Assert.IsTrue(new RotatedTextWithTerminator("a$", '$').EndsWith("$"));
    }

    [TestMethod]
    public void EndsWith_IsCorrectWithEnumerable()
    {
        Assert.IsTrue(new RotatedTextWithTerminator("a$".ToArray(), '$').EndsWith("$"));
    }

    [TestMethod]
    public void GetEnumerator_Generic_IsCorrect()
    {
        var text = new RotatedTextWithTerminator("c$ab");
        var enumerator = text.GetEnumerator();
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[0], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[1], enumerator.Current);
        Assert.AreEqual(text.Terminator, enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[2], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[3], enumerator.Current);
    }

    [TestMethod]
    public void GetEnumerator_Generic_WorksWithLinq()
    {
        var text = new RotatedTextWithTerminator("c$ab");
        Assert.AreEqual($"c$a", string.Concat(from c in text where c != 'b' select c));
    }

    [TestMethod]
    public void GetEnumerator_NonGeneric_IsCorrect()
    {
        var text = new RotatedTextWithTerminator("c$ab");
        var enumerator = ((System.Collections.IEnumerable)text).GetEnumerator();
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[0], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[1], enumerator.Current);
        Assert.AreEqual(text.Terminator, enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[2], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[3], enumerator.Current);
    }

}

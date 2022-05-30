using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MoreStructures.Utilities;

namespace MoreStructures.Tests;

[TestClass]
public class TextWithTerminatorTests
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
    public void Ctor_Preconditions_WithValidateInput()
    {
        Assert.IsNotNull(new TextWithTerminator("a", '$'));
        Assert.ThrowsException<ArgumentException>(() => new TextWithTerminator("a", 'a'));
        Assert.ThrowsException<ArgumentException>(() => new TextWithTerminator("a$a", '$'));
    }

    [TestMethod]
    public void Ctor_BrokenInvariant_WithoutValidateInput()
    {
        var text1 = new TextWithTerminator("a", 'a', false);
        Assert.IsTrue(text1.Text.Contains(text1.Terminator));
        var text2 = new TextWithTerminator("a$a", '$', false);
        Assert.IsTrue(text2.Text.Contains(text2.Terminator));
    }

    [TestMethod]
    public void Indexer_WithSelector()
    {
        Assert.AreEqual("b", new TextWithTerminator("abc", '$')[new HardcodedSelector("b")]);
        Assert.AreEqual("a", new TextWithTerminator("abc", '$')[new FirstCharSelector()]);
    }

    [TestMethod]
    public void Indexer_WithRange()
    {
        Assert.AreEqual("a".AsValue(), new TextWithTerminator("abc", '$')[0..1].AsValue());
        Assert.AreEqual(string.Empty.AsValue(), new TextWithTerminator("abc", '$')[0..0].AsValue());
        Assert.AreEqual("abc$".AsValue(), new TextWithTerminator("abc", '$')[0..].AsValue());
        Assert.AreEqual("ab".AsValue(), new TextWithTerminator("abc", '$')[..^2].AsValue());
    }

    [TestMethod]
    public void Indexer_WithIndex()
    {
        Assert.AreEqual('a', new TextWithTerminator("abc", '$')[0]);
        Assert.AreEqual('$', new TextWithTerminator("abc", '$')[3]);
    }

    [TestMethod]
    public void Length_IsCorrect()
    {
        Assert.AreEqual(4, new TextWithTerminator("abc", '$').Length);
        Assert.AreEqual(1, new TextWithTerminator("", '$').Length);
    }

    [TestMethod]
    public void StartsWith_IsCorrect()
    {
        Assert.IsTrue(new TextWithTerminator("a", '$').StartsWith("a"));
    }

    [TestMethod]
    public void EndsWith_IsCorrect()
    {
        Assert.IsTrue(new TextWithTerminator("a", '$').EndsWith("$"));
    }

    [TestMethod]
    public void GetEnumerator_Generic_IsCorrect()
    {
        var text = new TextWithTerminator("abc");
        var enumerator = text.GetEnumerator();
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[0], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[1], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[2], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text.Terminator, enumerator.Current);
    }

    [TestMethod]
    public void GetEnumerator_Generic_WorksWithLinq()
    {
        var text = new TextWithTerminator("abc");
        Assert.AreEqual($"ac{text.Terminator}", string.Concat(from c in text where c != 'b' select c));
    }

    [TestMethod]
    public void GetEnumerator_NonGeneric_IsCorrect()
    {
        var text = new TextWithTerminator("abc");
        var enumerator = ((System.Collections.IEnumerable)text).GetEnumerator();
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[0], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[1], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text[2], enumerator.Current);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(text.Terminator, enumerator.Current);
    }

}

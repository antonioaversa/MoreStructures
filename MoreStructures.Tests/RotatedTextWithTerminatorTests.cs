using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
    public void Indexer_WithSelector()
    {
        Assert.AreEqual("b", new RotatedTextWithTerminator("c$ab", '$')[new HardcodedSelector("b")]);
        Assert.AreEqual("c", new RotatedTextWithTerminator("c$ab", '$')[new FirstCharSelector()]);
    }

    [TestMethod]
    public void Indexer_WithRange()
    {
        Assert.AreEqual("c".AsValueEnumerable(), new RotatedTextWithTerminator("c$ab", '$')[0..1]);
        Assert.AreEqual(string.Empty.AsValueEnumerable(), new RotatedTextWithTerminator("c$ab", '$')[0..0]);
        Assert.AreEqual("c$ab".AsValueEnumerable(), new RotatedTextWithTerminator("c$ab", '$')[0..]);
        Assert.AreEqual("c$".AsValueEnumerable(), new RotatedTextWithTerminator("c$ab", '$')[..^2]);
    }

    [TestMethod]
    public void Indexer_WithIndex()
    {
        Assert.AreEqual('c', new RotatedTextWithTerminator("c$ab", '$')[0]);
        Assert.AreEqual('b', new RotatedTextWithTerminator("c$ab", '$')[3]);
    }

    [TestMethod]
    public void Length_IsCorrect()
    {
        Assert.AreEqual(4, new RotatedTextWithTerminator("c$ab", '$').Length);
        Assert.AreEqual(1, new RotatedTextWithTerminator("$", '$').Length);
    }

    [TestMethod]
    public void StartsWith_IsCorrect()
    {
        Assert.IsTrue(new RotatedTextWithTerminator("a$", '$').StartsWith("a"));
    }

    [TestMethod]
    public void EndsWith_IsCorrect()
    {
        Assert.IsTrue(new RotatedTextWithTerminator("a$", '$').EndsWith("$"));
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

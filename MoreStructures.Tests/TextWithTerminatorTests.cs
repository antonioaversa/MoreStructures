using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MoreStructures.Tests
{
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
            public string Of(TextWithTerminator text) => text[0..1];
            public string OfRotated(RotatedTextWithTerminator text) => text[0..1];
        }

        [TestMethod]
        public void Ctor_Preconditions()
        {
            Assert.IsNotNull(new TextWithTerminator("a", '$'));
            Assert.ThrowsException<ArgumentException>(() => new TextWithTerminator("a", 'a'));
            Assert.ThrowsException<ArgumentException>(() => new TextWithTerminator("a$a", '$'));
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
            Assert.AreEqual("a", new TextWithTerminator("abc", '$')[0..1]);
            Assert.AreEqual(string.Empty, new TextWithTerminator("abc", '$')[0..0]);
            Assert.AreEqual("abc$", new TextWithTerminator("abc", '$')[0..]);
            Assert.AreEqual("ab", new TextWithTerminator("abc", '$')[..^2]);
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
            Assert.AreEqual($"ac{text.Terminator}", string.Join(string.Empty, from c in text where c != 'b' select c));
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
}

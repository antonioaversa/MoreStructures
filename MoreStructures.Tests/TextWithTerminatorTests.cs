using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MoreStructures.Tests
{
    [TestClass]
    public class TextWithTerminatorTests
    {
        private record HardcodedSelector(string Selection) : TextWithTerminator.ISelector
        {
            public string Of(TextWithTerminator text) => Selection;
        }

        private record FirstCharSelector() : TextWithTerminator.ISelector
        {
            public string Of(TextWithTerminator text) => text[0..1];
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

    }
}

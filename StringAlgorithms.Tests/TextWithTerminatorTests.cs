using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StringAlgorithms.Tests
{
    [TestClass]
    public class TextWithTerminatorTests
    {
        [TestMethod]
        public void Ctor_Preconditions()
        {
            Assert.IsNotNull(new TextWithTerminator("a", '$'));
            Assert.ThrowsException<ArgumentException>(() => new TextWithTerminator("a", 'a'));
            Assert.ThrowsException<ArgumentException>(() => new TextWithTerminator("a$a", '$'));
        }

        [TestMethod]
        public void AsString_StartsWithText()
        {
            Assert.IsTrue(new TextWithTerminator("a", '$').AsString.StartsWith("a"));
        }

        [TestMethod]
        public void AsString_EndsWithTerminator()
        {
            Assert.IsTrue(new TextWithTerminator("a", '$').AsString.EndsWith("$"));
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;

namespace MoreStructures.Tests.Utilities
{
    [TestClass] 
    public class CharOrTerminatorComparerTests
    {
        [TestMethod]
        public void Compare_IsCorrect_WithTerminatorLowerInASCII()
        {
            var comparer = new CharOrTerminatorComparer('$');
            Assert.AreEqual(-1, comparer.Compare('a', 'b'));
            Assert.AreEqual(0, comparer.Compare('a', 'a'));
            Assert.AreEqual(1, comparer.Compare('b', 'a'));
            Assert.AreEqual(1, comparer.Compare('a', '$'));
            Assert.AreEqual(-1, comparer.Compare('$', 'b'));
            Assert.AreEqual(0, comparer.Compare('$', '$'));
        }

        [TestMethod]
        public void Compare_IsCorrect_WithTerminatorHigherInASCII()
        {
            var comparer = new CharOrTerminatorComparer('s');
            Assert.AreEqual(-1, comparer.Compare('a', 'b'));
            Assert.AreEqual(0, comparer.Compare('a', 'a'));
            Assert.AreEqual(1, comparer.Compare('b', 'a'));
            Assert.AreEqual(1, comparer.Compare('a', 's'));
            Assert.AreEqual(-1, comparer.Compare('s', 'b'));
            Assert.AreEqual(0, comparer.Compare('s', 's'));
        }
    }
}

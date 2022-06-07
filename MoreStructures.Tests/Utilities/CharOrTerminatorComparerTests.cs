using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;

namespace MoreStructures.Tests.Utilities;

[TestClass] 
public class CharOrTerminatorComparerTests
{
    [TestMethod]
    public void Equals_ByValue()
    {
        Assert.AreEqual(CharOrTerminatorComparer.Build('a'), CharOrTerminatorComparer.Build('a'));
        Assert.AreNotEqual(CharOrTerminatorComparer.Build('a'), CharOrTerminatorComparer.Build('b'));
        Assert.IsFalse(CharOrTerminatorComparer.Build('a').Equals(null));
    }

    [TestMethod]
    public void GetHashCode_ByValue()
    {
        Assert.AreEqual(
            CharOrTerminatorComparer.Build('a').GetHashCode(), 
            CharOrTerminatorComparer.Build('a').GetHashCode());
    }

    [TestMethod]
    public void Terminator_IsSet()
    {
        Assert.AreEqual('a', CharOrTerminatorComparer.Build('a').Terminator);
    }

    [TestMethod]
    public void Compare_IsCorrect_WithTerminatorLowerInASCII()
    {
        var comparer = CharOrTerminatorComparer.Build('$');
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
        var comparer = CharOrTerminatorComparer.Build('s');
        Assert.AreEqual(-1, comparer.Compare('a', 'b'));
        Assert.AreEqual(0, comparer.Compare('a', 'a'));
        Assert.AreEqual(1, comparer.Compare('b', 'a'));
        Assert.AreEqual(1, comparer.Compare('a', 's'));
        Assert.AreEqual(-1, comparer.Compare('s', 'b'));
        Assert.AreEqual(0, comparer.Compare('s', 's'));
    }
}

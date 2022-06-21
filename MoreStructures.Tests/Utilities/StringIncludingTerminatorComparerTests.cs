using MoreStructures.Utilities;

namespace MoreStructures.Tests;

[TestClass]
public class StringIncludingTerminatorComparerTests
{
    [TestMethod]
    public void Equals_ByValue()
    {
        Assert.AreEqual(StringIncludingTerminatorComparer.Build('a'), StringIncludingTerminatorComparer.Build('a'));
        Assert.AreNotEqual(StringIncludingTerminatorComparer.Build('a'), StringIncludingTerminatorComparer.Build('b'));
        Assert.IsFalse(StringIncludingTerminatorComparer.Build('a').Equals(null));
    }

    [TestMethod]
    public void GetHashCode_ByValue()
    {
        Assert.AreEqual(
            StringIncludingTerminatorComparer.Build('a').GetHashCode(),
            StringIncludingTerminatorComparer.Build('a').GetHashCode());
    }

    [TestMethod]
    public void Terminator_IsSet()
    {
        Assert.AreEqual('a', StringIncludingTerminatorComparer.Build('a').Terminator);
    }

    [TestMethod]
    public void Compare_IsCorrect_WithNoTerminatorIncluded()
    {
        var comparer = StringIncludingTerminatorComparer.Build('$');

        Assert.IsTrue(comparer.Compare(string.Empty, string.Empty) == 0);
        Assert.IsTrue(comparer.Compare("a", string.Empty) > 0);
        Assert.IsTrue(comparer.Compare(string.Empty, "a") < 0);
        Assert.IsTrue(comparer.Compare("a", null) > 0);
        Assert.IsTrue(comparer.Compare(null, "a") < 0);
        Assert.IsTrue(comparer.Compare(null, string.Empty) < 0);
        Assert.IsTrue(comparer.Compare(string.Empty, null) > 0);

        Assert.IsTrue(comparer.Compare("aab", "aaa") > 0);
        Assert.IsTrue(comparer.Compare("aa", "aaa") < 0);
        Assert.IsTrue(comparer.Compare("aaa", "aaa") == 0);
    }

    [TestMethod]
    public void Compare_IsCorrect_WithTerminatorIncludedLowerInASCII()
    {
        var comparer = StringIncludingTerminatorComparer.Build('$');

        Assert.IsTrue(comparer.Compare(string.Empty, "$") < 0);
        Assert.IsTrue(comparer.Compare("$", string.Empty) > 0);
        Assert.IsTrue(comparer.Compare("$", "a") < 0);
        Assert.IsTrue(comparer.Compare("a", "$") > 0);
        Assert.IsTrue(comparer.Compare("$", null) > 0);
        Assert.IsTrue(comparer.Compare(null, "$") < 0);
        Assert.IsTrue(comparer.Compare("$", "$") == 0);
        Assert.IsTrue(comparer.Compare("$a", "$b") < 0);
        Assert.IsTrue(comparer.Compare("$b", "$a") > 0);
        Assert.IsTrue(comparer.Compare("$aab", "$aaa") > 0);
    }

    [TestMethod]
    public void Compare_IsCorrect_WithTerminatorIncludedHigherInASCII()
    {
        var comparer = StringIncludingTerminatorComparer.Build('z');

        Assert.IsTrue(comparer.Compare(string.Empty, "z") < 0);
        Assert.IsTrue(comparer.Compare("z", string.Empty) > 0);
        Assert.IsTrue(comparer.Compare("z", "a") < 0);
        Assert.IsTrue(comparer.Compare("a", "z") > 0);
        Assert.IsTrue(comparer.Compare("z", null) > 0);
        Assert.IsTrue(comparer.Compare(null, "z") < 0);
        Assert.IsTrue(comparer.Compare("z", "z") == 0);
        Assert.IsTrue(comparer.Compare("za", "zb") < 0);
        Assert.IsTrue(comparer.Compare("zb", "za") > 0);
        Assert.IsTrue(comparer.Compare("zaab", "zaaa") > 0);
    }
}

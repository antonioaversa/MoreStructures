using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;

namespace MoreStructures.Tests;

[TestClass]
public class StringIncludingTerminatorComparerTests
{
    [TestMethod]
    public void Compare_IsCorrect_WithNoTerminatorIncluded()
    {
        var comparer = new StringIncludingTerminatorComparer('$');

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
        var comparer = new StringIncludingTerminatorComparer('$');

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
        var comparer = new StringIncludingTerminatorComparer('z');

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

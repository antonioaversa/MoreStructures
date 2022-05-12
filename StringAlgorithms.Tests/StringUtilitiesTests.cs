using Microsoft.VisualStudio.TestTools.UnitTesting;
using static StringAlgorithms.StringUtilities;

namespace StringAlgorithms.Tests;

[TestClass]
public class StringUtilitiesTests
{
    [TestMethod]
    public void LongestPrefixInCommon_Correctness()
    {
        Assert.AreEqual(0, LongestPrefixInCommon("a", "b"));
        Assert.AreEqual(0, LongestPrefixInCommon("a", ""));
        Assert.AreEqual(0, LongestPrefixInCommon("", "a"));
        Assert.AreEqual(1, LongestPrefixInCommon("a", "a"));
        Assert.AreEqual(1, LongestPrefixInCommon("aa", "a"));
        Assert.AreEqual(1, LongestPrefixInCommon("a", "aa"));
        Assert.AreEqual(2, LongestPrefixInCommon("aab", "aa"));
        Assert.AreEqual(2, LongestPrefixInCommon("aab", "aac"));
        Assert.AreEqual(2, LongestPrefixInCommon("aab", "aacbaab"));
        Assert.AreEqual(3, LongestPrefixInCommon("aabc", "aabaabc"));
    }
}

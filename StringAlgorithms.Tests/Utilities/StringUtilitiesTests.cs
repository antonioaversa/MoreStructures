using Microsoft.VisualStudio.TestTools.UnitTesting;
using static StringAlgorithms.Utilities.StringUtilities;

namespace StringAlgorithms.Tests.Utilities;

[TestClass]
public class StringUtilitiesTests
{
    [TestMethod]
    public void LongestPrefixInCommon_IsCorrect()
    {
        Assert.AreEqual(0, LongestPrefixInCommon("", ""));
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

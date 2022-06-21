using static MoreStructures.Utilities.StringUtilities;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class StringUtilitiesTests
{

    [DataRow("", "", 0)]
    [DataRow("", "", 0)]
    [DataRow("a", "b", 0)]
    [DataRow("a", "", 0)]
    [DataRow("", "a", 0)]
    [DataRow("a", "a", 1)]
    [DataRow("aa", "a", 1)]
    [DataRow("a", "aa", 1)]
    [DataRow("aab", "aa", 2)]
    [DataRow("aab", "aac", 2)]
    [DataRow("aab", "aacbaab", 2)]
    [DataRow("aabc", "aabaabc", 3)]
    [DataTestMethod]
    public void LongestPrefixInCommon_IsCorrect(string first, string second, int expected)
    {
        Assert.AreEqual(expected, LongestPrefixInCommon(first, second));
        Assert.AreEqual(expected, LongestPrefixInCommon(first.ToCharArray(), second.ToCharArray()));
        Assert.AreEqual(expected, LongestPrefixInCommon(first.ToList(), second.ToList()));
    }
}

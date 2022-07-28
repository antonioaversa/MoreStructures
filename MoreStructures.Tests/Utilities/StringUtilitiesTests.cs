using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public void LongestCommonPrefix_IsCorrect(string first, string second, int expected)
    {
        // Without indexes
        Assert.AreEqual(expected, LongestCommonPrefix(first, second));
        Assert.AreEqual(expected, LongestCommonPrefix(first.ToCharArray(), second.ToCharArray()));
        Assert.AreEqual(expected, LongestCommonPrefix(first.ToList(), second.ToList()));

        // With indexes
        foreach (var i in Enumerable.Range(0, first.Length))
        {
            foreach (var j in Enumerable.Range(0, second.Length))
            {
                var result1 = LongestCommonPrefix(first, i, second, j);
                Assert.AreEqual(first[i..(i+result1)], second[j..(j+result1)]);

                var result2 = LongestCommonPrefix(first.Skip(i), second.Skip(j));
                Assert.AreEqual(result2, result1);

                if (i + result1 < first.Length - 1 || j + result1 < second.Length - 1)
                    Assert.AreNotEqual(first[i..], second[j..]);
            }
        }
    }

    [TestMethod]
    public void LongestCommonPrefix_RaisesExceptionWithInvalidIndexes()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => LongestCommonPrefix("abc", -1, "abd", 0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => LongestCommonPrefix("abc", 3, "abd", 0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => LongestCommonPrefix("abc", 0, "abd", -1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => LongestCommonPrefix("abc", 0, "abd", 3));
    }
}

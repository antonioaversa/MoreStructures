using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.KnuthMorrisPratt.Matching;

namespace MoreStructures.Tests.KnuthMorrisPratt.Matching;

[TestClass]
public class MatcherTests
{
    [DataRow("abcdabcaba", "a", new int[] { 0, 4, 7, 9 })]
    [DataRow("abcdabcaba", "ab", new int[] { 0, 4, 7 })]
    [DataRow("abcdabcaba", "abc", new int[] { 0, 4 })]
    [DataRow("abcdabcaba", "abcd", new int[] { 0 })]
    [DataRow("abcdabcaba", "abcde", new int[] { })]
    [DataRow("abcdabcaba", "e", new int[] { })]
    [DataTestMethod]
    public void Match_IsCorrect(string text, string pattern, int[] startIndexes)
    {
        var matches = Matcher.Match(text, pattern, '$', false).ToList();
        Assert.IsTrue(matches.All(m => m.Success));
        Assert.IsTrue(startIndexes.SequenceEqual(matches.Select(m => m.Begin)));
        Assert.IsTrue(matches.All(m => m.MatchedChars == pattern.Length));

        var matchesWithValidation = Matcher.Match(text, pattern, '$', true).ToList();
        Assert.IsTrue(matches.SequenceEqual(matchesWithValidation));
    }

    [TestMethod]
    public void Match_ReturnsNoMatchWithEmptyText()
    {
        Assert.IsFalse(Matcher.Match(string.Empty, "a", '$').Any());
    }

    [TestMethod]
    public void Match_ThrowsExceptionWithEmptyPattern()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            Matcher.Match("a string", "", '$').ToList());
    }

    [TestMethod]
    public void Match_DoesntValidateTextAndPatternAgainstSeparatorByDefault()
    {
        Assert.IsTrue(Matcher.Match("a$a", "a", '$').Count() >= 0);
        Assert.IsTrue(Matcher.Match("a$a", "b", '$').Count() >= 0);
        Assert.IsTrue(Matcher.Match("a", "a$", '$').Count() >= 0);
        Assert.IsTrue(Matcher.Match("a", "b$", '$').Count() >= 0);
        Assert.IsTrue(Matcher.Match("a$a", "a$a", '$').Count() >= 0);
        Assert.IsTrue(Matcher.Match("a$a", "b$b", '$').Count() >= 0);
    }

    [TestMethod]
    public void Match_RaisesExceptionWhenTextAndPatternAreValidatedAgainstSeparator()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            Matcher.Match("a$a", "a", '$', true).ToList());
        Assert.ThrowsException<ArgumentException>(() =>
            Matcher.Match("a$a", "b", '$', true).ToList());
        Assert.ThrowsException<ArgumentException>(() =>
            Matcher.Match("a", "a$", '$', true).ToList());
        Assert.ThrowsException<ArgumentException>(() =>
            Matcher.Match("a", "b$", '$', true).ToList());
        Assert.ThrowsException<ArgumentException>(() =>
            Matcher.Match("a$a", "a$a", '$', true).ToList());
        Assert.ThrowsException<ArgumentException>(() =>
            Matcher.Match("a$a", "b$b", '$', true).ToList());
    }
}

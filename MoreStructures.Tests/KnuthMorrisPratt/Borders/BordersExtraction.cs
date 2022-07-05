namespace MoreStructures.Tests.KnuthMorrisPratt.Borders;

public abstract class BordersExtractionTests
{
    protected IBordersExtraction BordersExtraction { get; }

    protected BordersExtractionTests(IBordersExtraction bordersExtraction)
    {
        BordersExtraction = bordersExtraction;
    }

    [DataRow(new string[] { }, "")]
    [DataRow(new string[] { }, "ab")]
    [DataRow(new string[] { "a" }, "aa")]
    [DataRow(new string[] { "a" }, "aba")]
    [DataRow(new string[] { "aa", "a" }, "aaa")]
    [DataRow(new string[] { "a" }, "abba")]
    [DataRow(new string[] { "aba", "a" }, "ababa")]
    [DataRow(new string[] { "aabaa", "aa", "a"}, "aabaabaa")]
    [DataRow(new string[] { "abcabc", "abc" }, "abcabcabc")]
    [DataTestMethod]
    public void GetAllBordersByDescLength_IsCorrect(string[] expectedResult, string text)
    {
        var result = BordersExtraction.GetAllBordersByDescLength(text);
        Assert.IsTrue(expectedResult.SequenceEqual(result), 
            $"Expected [{string.Join(", ", expectedResult)}], Got: [{string.Join(", ", result)}]");
    }
}

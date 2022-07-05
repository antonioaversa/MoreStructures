using MoreStructures.KnuthMorrisPratt.PrefixFunction;

namespace MoreStructures.Tests.KnuthMorrisPratt.PrefixFunction;

public abstract class PrefixFunctionCalculatorTests
{
    protected IPrefixFunctionCalculator PrefixFunctionCalculator { get; }

    protected PrefixFunctionCalculatorTests(IPrefixFunctionCalculator prefixFunctionCalculator)
    {
        PrefixFunctionCalculator = prefixFunctionCalculator;
    }

    [DataRow(new int[] { }, "")]
    [DataRow(new int[] { 0, 0 }, "ab")]
    [DataRow(new int[] { 0, 1 }, "aa")]
    [DataRow(new int[] { 0, 0, 1 }, "aba")]
    [DataRow(new int[] { 0, 1, 2 }, "aaa")]
    [DataRow(new int[] { 0, 0, 0, 1 }, "abba")]
    [DataRow(new int[] { 0, 0, 1, 2, 3 }, "ababa")]
    [DataRow(new int[] { 0, 1, 0, 1, 2, 3, 4, 5 }, "aabaabaa")]
    [DataRow(new int[] { 0, 0, 0, 1, 2, 3, 4, 5, 6 }, "abcabcabc")]
    [DataTestMethod]
    public void GetAllBordersByDescLength_IsCorrect(int[] expectedResult, string text)
    {
        var result = PrefixFunctionCalculator.GetValues(text);
        Assert.IsTrue(expectedResult.SequenceEqual(result), 
            $"Expected [{string.Join(", ", expectedResult)}], Got: [{string.Join(", ", result)}]");
    }
}

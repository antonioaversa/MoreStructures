using MoreStructures.SuffixArrays;
using MoreStructures.SuffixArrays.LongestCommonPrefix;

namespace MoreStructures.Tests.SuffixArrays.LongestCommonPrefix;

public abstract class LcpArrayBuilderTests
{
    public record LcpArrayBuilderBuilderInput(TextWithTerminator Text, SuffixArray SuffixArray);

    protected Func<LcpArrayBuilderBuilderInput, ILcpArrayBuilder> BuilderBuilder { get; }

    protected LcpArrayBuilderTests(Func<LcpArrayBuilderBuilderInput, ILcpArrayBuilder> builderBuilder)
    {
        BuilderBuilder = builderBuilder;
    }

    [DataRow("", new[] { 0 }, new int[] { })]
    [DataRow("a", new[] { 1, 0 }, new[] { 0 })]
    [DataRow("aa", new[] { 2, 1, 0 }, new[] { 0, 1 })]
    [DataRow("aaa", new[] { 3, 2, 1, 0 }, new[] { 0, 1, 2 })]
    [DataRow("aba", new[] { 3, 2, 0, 1 }, new[] { 0, 1, 0 })]
    [DataRow("abc", new[] { 3, 0, 1, 2 }, new[] { 0, 0, 0 })]
    [DataRow("abaabaac", new[] { 8, 2, 5, 0, 3, 6, 1, 4, 7 }, new[] { 0, 2, 1, 4, 1, 0, 3, 0 })]
    [DataTestMethod]
    public void Build_IsCorrect(string textContent, int[] suffixArray, int[] expectedResult)
    {
        var text = new TextWithTerminator(textContent, '$');
        var builder = BuilderBuilder(new(text, new(suffixArray)));
        var result = builder.Build().Lengths;
        Assert.IsTrue(
            expectedResult.SequenceEqual(result),
            $"Expected: [{string.Join(",", expectedResult)}], Actual: [{string.Join(", ", result)}]");
    }
}

using MoreStructures.SuffixArrays.Builders;
using MoreStructures.SuffixStructures;

namespace MoreStructures.Tests.SuffixArrays.Builders;

[TestClass]
public abstract class SuffixArrayBuilderTests<TEdge, TNode>
    where TEdge : ISuffixStructureEdge<TEdge, TNode>
    where TNode : ISuffixStructureNode<TEdge, TNode>
{
    protected Func<string, ISuffixArrayBuilder> BuilderBuilder { get; }

    protected SuffixArrayBuilderTests(Func<string, ISuffixArrayBuilder> builderBuilder)
    {
        BuilderBuilder = builderBuilder;
    }

    [DataRow("", new int[] { 0 })]
    [DataRow("ab", new int[] { 2, 0, 1 })]
    [DataRow("ba", new int[] { 2, 1, 0 })]
    [DataRow("aaa", new int[] { 3, 2, 1, 0 })]
    [DataRow("abaabaac", new[] { 8, 2, 5, 0, 3, 6, 1, 4, 7 })]
    [DataRow("ababcacaa", new int[] { 9, 8, 7, 0, 2, 5, 1, 3, 6, 4 })]
    [DataRow("aabaacaaadba", new int[] { 12, 11, 6, 0, 3, 7, 1, 4, 8, 10, 2, 5, 9 })]
    [DataTestMethod]
    public void Build_IsCorrect(string text, int[] expectedResult)
    {
        var builder = BuilderBuilder(text);
        var result = builder.Build();
        Assert.IsTrue(expectedResult.SequenceEqual(result.Indexes), 
            $"Expected: [{string.Join(", ", expectedResult)}], Actual: [{string.Join(", ", result.Indexes)}]");
    }
}

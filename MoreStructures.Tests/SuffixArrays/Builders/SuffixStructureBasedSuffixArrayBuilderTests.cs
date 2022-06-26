using MoreStructures.SuffixArrays.Builders;
using MoreStructures.SuffixStructures.Builders;
using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTrees.Builders;
using MoreStructures.SuffixTries;
using MoreStructures.SuffixTries.Builders;

namespace MoreStructures.Tests.SuffixArrays.Builders;

[TestClass]
public class SuffixTreeBasedSuffixArrayBuilderTests : SuffixArrayBuilderTests<SuffixTreeEdge, SuffixTreeNode>
{
    private static readonly IBuilder<SuffixTreeEdge, SuffixTreeNode> SuffixTreeBuilder = 
        new UkkonenSuffixTreeBuilder();

    public SuffixTreeBasedSuffixArrayBuilderTests()
        : base(BuildSuffixArrayBuilder)
    {
    }

    private static ISuffixArrayBuilder<SuffixTreeEdge, SuffixTreeNode> BuildSuffixArrayBuilder(string textContent)
    {
        var text = new TextWithTerminator(textContent);
        return new SuffixStructureBasedSuffixArrayBuilder<SuffixTreeEdge, SuffixTreeNode>(
            text, SuffixTreeBuilder.BuildTree(text));
    }
}

[TestClass]
public class SuffixTrieBasedSuffixArrayBuilderTests : SuffixArrayBuilderTests<SuffixTrieEdge, SuffixTrieNode>
{
    private static readonly IBuilder<SuffixTrieEdge, SuffixTrieNode> SuffixTrieBuilder =
        new NaivePartiallyRecursiveSuffixTrieBuilder();

    public SuffixTrieBasedSuffixArrayBuilderTests()
        : base(BuildSuffixArrayBuilder)
    {
    }

    private static ISuffixArrayBuilder<SuffixTrieEdge, SuffixTrieNode> BuildSuffixArrayBuilder(string textContent)
    {
        var text = new TextWithTerminator(textContent);
        return new SuffixStructureBasedSuffixArrayBuilder<SuffixTrieEdge, SuffixTrieNode>(
            text, SuffixTrieBuilder.BuildTree(text));
    }
}

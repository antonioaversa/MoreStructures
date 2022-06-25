using MoreStructures.SuffixArrays.Builders;
using MoreStructures.SuffixStructures.Builders;
using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTrees.Builders;

namespace MoreStructures.Tests.SuffixArrays.Builders;

[TestClass]
public class SuffixStructureBasedSuffixArrayBuilderTests : SuffixArrayBuilderTests<SuffixTreeEdge, SuffixTreeNode>
{
    private static readonly IBuilder<SuffixTreeEdge, SuffixTreeNode> SuffixTreeBuilder = 
        new UkkonenSuffixTreeBuilder();

    public SuffixStructureBasedSuffixArrayBuilderTests()
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

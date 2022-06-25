using MoreStructures.SuffixArrays.Builders;
using MoreStructures.SuffixTrees;

namespace MoreStructures.Tests.SuffixArrays.Builders;

[TestClass]
public class NaiveSuffixArrayBuilderTests : SuffixArrayBuilderTests<SuffixTreeEdge, SuffixTreeNode>
{
    public NaiveSuffixArrayBuilderTests() 
        : base(s => new NaiveSuffixArrayBuilder<SuffixTreeEdge, SuffixTreeNode>(new(s)))
    {
    }
}

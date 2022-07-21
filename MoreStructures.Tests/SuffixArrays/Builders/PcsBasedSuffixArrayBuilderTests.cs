using MoreStructures.SuffixArrays.Builders;
using MoreStructures.SuffixTrees;

namespace MoreStructures.Tests.SuffixArrays.Builders;

[TestClass]
public class PcsBasedSuffixArrayBuilderTests : SuffixArrayBuilderTests<SuffixTreeEdge, SuffixTreeNode>
{
    public PcsBasedSuffixArrayBuilderTests()
    : base(s => new PcsBasedSuffixArrayBuilder(
        new(s), new Dictionary<char, int> { ['$'] = 0, ['a'] = 1, ['b'] = 2, ['c'] = 3, ['d'] = 4 }))
    {
    }
}
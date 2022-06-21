using MoreStructures.SuffixTrees.Builders;

namespace MoreStructures.Tests.SuffixTrees.Builders;

[TestClass]
public class UkkonenSuffixTreeBuilderTests : SuffixTreeBuilderTests
{
    public UkkonenSuffixTreeBuilderTests() 
        : base(new UkkonenSuffixTreeBuilder())
    {
    }
}

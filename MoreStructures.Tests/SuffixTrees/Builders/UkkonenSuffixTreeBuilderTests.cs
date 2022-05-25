using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixTrees.Builders;
using MoreStructures.SuffixStructures.Builders;

namespace MoreStructures.Tests.SuffixTrees.Builders;

[TestClass]
public class UkkonenSuffixTreeBuilderTests : SuffixTreeBuilderTests
{
    public UkkonenSuffixTreeBuilderTests() 
        : base(new UkkonenSuffixTreeBuilder())
    {
    }
}

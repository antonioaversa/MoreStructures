using MoreStructures.SuffixArrays.Builders;
using MoreStructures.SuffixArrays.LongestCommonPrefix;
using MoreStructures.SuffixTrees.Builders;

namespace MoreStructures.Tests.SuffixTrees.Builders;

[TestClass]
public class SuffixAndLcpArraysBasedSuffixTreeBuilderTests : SuffixTreeBuilderTests
{
    public SuffixAndLcpArraysBasedSuffixTreeBuilderTests()
        : base(new SuffixAndLcpArraysBasedSuffixTreeBuilder(
            text =>
            {
                var suffixArray = new NaiveSuffixArrayBuilder(text).Build();
                var lcpArray = new KasaiLcpArrayBuilder(text, suffixArray).Build();
                return (suffixArray, lcpArray);
            }))
    {
    }
}

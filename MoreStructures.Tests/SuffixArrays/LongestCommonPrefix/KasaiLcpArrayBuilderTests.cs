using MoreStructures.SuffixArrays.LongestCommonPrefix;

namespace MoreStructures.Tests.SuffixArrays.LongestCommonPrefix;

[TestClass]
public class KasaiLcpArrayBuilderTests : LcpArrayBuilderTests
{
    public KasaiLcpArrayBuilderTests() : base(labbi => new KasaiLcpArrayBuilder(labbi.Text, labbi.SuffixArray))
    {
    }
}
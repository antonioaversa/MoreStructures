using MoreStructures.SuffixArrays.LongestCommonPrefix;

namespace MoreStructures.Tests.SuffixArrays.LongestCommonPrefix;

[TestClass]
public class NaiveLcpArrayBuilderTests : LcpArrayBuilderTests
{
    public NaiveLcpArrayBuilderTests() : base(labbi => new NaiveLcpArrayBuilder(labbi.Text, labbi.SuffixArray))
    {
    }
}
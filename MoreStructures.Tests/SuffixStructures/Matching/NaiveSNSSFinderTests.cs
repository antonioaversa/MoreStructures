using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixStructures.Matching;

namespace MoreStructures.Tests.SuffixTrees.Matching;

[TestClass]
public class NaiveSNSSFinderTests : SNSSFinderTests
{
    public NaiveSNSSFinderTests()
        : base(new NaiveSNSSFinder())
    {
    }
}
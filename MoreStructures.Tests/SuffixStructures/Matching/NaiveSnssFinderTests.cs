using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixStructures.Matching;

namespace MoreStructures.Tests.SuffixTrees.Matching;

[TestClass]
public class NaiveSnssFinderTests : SnssFinderTests
{
    public NaiveSnssFinderTests()
        : base(new NaiveSnssFinder())
    {
    }
}
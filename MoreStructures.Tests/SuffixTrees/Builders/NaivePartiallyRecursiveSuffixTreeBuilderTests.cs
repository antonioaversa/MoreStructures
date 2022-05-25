using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixTrees.Builders;

namespace MoreStructures.Tests.SuffixTrees.Builders;

[TestClass]
public class NaivePartiallyRecursiveSuffixTreeBuilderTests : SuffixTreeBuilderTests
{
    public NaivePartiallyRecursiveSuffixTreeBuilderTests() 
        : base(new NaivePartiallyRecursiveSuffixTreeBuilder())
    {
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixTries.Builders;

namespace MoreStructures.Tests.SuffixTries.Builders;

[TestClass]
public class NaivePartiallyRecursiveSuffixTrieBuilderTests : SuffixTrieBuilderTests
{
    public NaivePartiallyRecursiveSuffixTrieBuilderTests() 
        : base(new NaivePartiallyRecursiveSuffixTrieBuilder())
    {
    }
}

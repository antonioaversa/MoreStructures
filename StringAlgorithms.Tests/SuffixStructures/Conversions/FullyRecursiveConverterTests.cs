using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures.Conversions;

namespace StringAlgorithms.Tests.SuffixStructures.Conversions;

[TestClass]
public class FullyRecursiveConverterTests : ConverterTests
{
    public FullyRecursiveConverterTests()
        : base(new FullyRecursiveConverter())
    {
    }
}

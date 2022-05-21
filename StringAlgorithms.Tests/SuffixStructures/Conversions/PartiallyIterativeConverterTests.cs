using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures.Conversions;

namespace StringAlgorithms.Tests.SuffixStructures.Conversions;

[TestClass]
public class PartiallyIterativeConverterTests : ConverterTests
{
    public PartiallyIterativeConverterTests() : base(new PartiallyIterativeConverter())
    {
    }
}

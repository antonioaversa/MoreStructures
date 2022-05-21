using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures.Conversions;

namespace StringAlgorithms.Tests.SuffixStructures.Conversions;

/// <summary>
/// Runs tests defined in <see cref="ConverterTests"/> for <see cref="PartiallyIterativeConverter"/>.
/// </summary>
[TestClass]
public class PartiallyIterativeConverterTests : ConverterTests
{
    public PartiallyIterativeConverterTests() : base(new PartiallyIterativeConverter())
    {
    }
}

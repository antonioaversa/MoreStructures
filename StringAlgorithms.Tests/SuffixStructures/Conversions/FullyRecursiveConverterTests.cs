using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures.Conversions;

namespace StringAlgorithms.Tests.SuffixStructures.Conversions;

/// <summary>
/// Runs tests defined in <see cref="ConverterTests"/> for <see cref="FullyRecursiveConverter"/>.
/// </summary>
[TestClass]
public class FullyRecursiveConverterTests : ConverterTests
{
    public FullyRecursiveConverterTests() : base(new FullyRecursiveConverter())
    {
    }
}

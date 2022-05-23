using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixStructures.Conversions;

namespace MoreStructures.Tests.SuffixStructures.Conversions;

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

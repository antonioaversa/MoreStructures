using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures.Transformations;

namespace StringAlgorithms.Tests.SuffixStructures.Transformations;

// FIXME: [TestClass]
public class FullyRecursiveSuffixStructuresConverterTests : SuffixStructuresConverterTests
{
    public FullyRecursiveSuffixStructuresConverterTests()
        : base(new FullyRecursiveSuffixStructuresConverter())
    {
    }
}

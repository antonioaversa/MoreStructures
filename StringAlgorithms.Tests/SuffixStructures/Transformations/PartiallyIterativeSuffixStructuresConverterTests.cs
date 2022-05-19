using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures.Transformations;

namespace StringAlgorithms.Tests.SuffixStructures.Transformations;

[TestClass]
public class PartiallyIterativeSuffixStructuresConverterTests : SuffixStructuresConverterTests
{
    public PartiallyIterativeSuffixStructuresConverterTests()
        : base(new PartiallyIterativeSuffixStructuresConverter())
    {
    }
}

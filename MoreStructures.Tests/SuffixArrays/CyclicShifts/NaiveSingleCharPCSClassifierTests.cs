using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class NaiveSingleCharPCSClassifierTests : SingleCharPCSClassifierTests
{
    public NaiveSingleCharPCSClassifierTests()
    : base(input => new NaiveSingleCharPCSClassifier(input))
    {
    }
}
using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class NaiveSingleCharPcsClassifierTests : SingleCharPcsClassifierTests
{
    public NaiveSingleCharPcsClassifierTests()
    : base(cbi => new NaiveSingleCharPcsClassifier(cbi.Input, cbi.InputWithTerminator))
    {
    }
}
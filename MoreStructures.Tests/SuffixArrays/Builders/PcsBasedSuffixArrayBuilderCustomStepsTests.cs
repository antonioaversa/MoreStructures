using MoreStructures.Strings.Sorting;
using MoreStructures.SuffixArrays.Builders;
using MoreStructures.SuffixArrays.CyclicShifts;
using MoreStructures.SuffixTrees;

namespace MoreStructures.Tests.SuffixArrays.Builders;

[TestClass]
public class PcsBasedSuffixArrayBuilderCustomStepsTests : SuffixArrayBuilderTests<SuffixTreeEdge, SuffixTreeNode>
{
    public PcsBasedSuffixArrayBuilderCustomStepsTests()
    : base(s => new PcsBasedSuffixArrayBuilder(
        new(s),
        input =>
            new CountingSortCharsSorter(
                new Dictionary<char, int> { ['$'] = 0, ['a'] = 1, ['b'] = 2, ['c'] = 3, ['d'] = 4 }), 
        (input, order) => 
            new OrderBasedSingleCharPcsClassifier(input, order),
        (pcsLength, order, eqClasses) => 
            new CountingSortDoubleLengthPcsSorter(pcsLength, order, eqClasses),
        (pcsLength, eqClassesPcsHalfLength, order) => 
            new EqClassesBasedDoubleLengthPcsClassifier(pcsLength, eqClassesPcsHalfLength, order)))
    {
    }
}
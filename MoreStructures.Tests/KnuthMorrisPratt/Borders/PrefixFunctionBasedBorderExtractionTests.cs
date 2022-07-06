using MoreStructures.KnuthMorrisPratt.Borders;
using MoreStructures.KnuthMorrisPratt.PrefixFunction;

namespace MoreStructures.Tests.KnuthMorrisPratt.Borders;

[TestClass]
public class PrefixFunctionBasedBorderExtractionTests : BordersExtractionTests
{
    public PrefixFunctionBasedBorderExtractionTests() : base(
        new PrefixFunctionBasedBorderExtraction(
            new NaivePrefixFunctionCalculator(
                new NaiveBordersExtraction())))
    {
    }
}
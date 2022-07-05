using MoreStructures.KnuthMorrisPratt.Borders;
using MoreStructures.KnuthMorrisPratt.PrefixFunction;

namespace MoreStructures.Tests.KnuthMorrisPratt.PrefixFunction;

[TestClass]
public class NaivePrefixFunctionCalculatorTests : PrefixFunctionCalculatorTests
{
    public NaivePrefixFunctionCalculatorTests() : base(
        new NaivePrefixFunctionCalculator(new NaiveBordersExtraction()))
    {
    }
}
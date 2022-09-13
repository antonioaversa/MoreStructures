using MoreStructures.KnuthMorrisPratt.PrefixFunction;

namespace MoreStructures.Tests.KnuthMorrisPratt.PrefixFunction;

[TestClass]
public class FastPrefixFunctionCalculatorTests : PrefixFunctionCalculatorTests
{
    public FastPrefixFunctionCalculatorTests() : base(
        new FastPrefixFunctionCalculator())
    {
    }
}
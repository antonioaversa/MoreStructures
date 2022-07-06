using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.KnuthMorrisPratt.Borders;

namespace MoreStructures.Tests.KnuthMorrisPratt.Borders;

[TestClass]
public class NaiveBordersExtractionTests : BordersExtractionTests
{
    public NaiveBordersExtractionTests() : base(new NaiveBordersExtraction())
    {
    }
}

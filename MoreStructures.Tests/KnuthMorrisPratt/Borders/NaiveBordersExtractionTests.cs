namespace MoreStructures.Tests.KnuthMorrisPratt.Borders;

[TestClass]
public class NaiveBordersExtractionTests : BordersExtractionTests
{
    public NaiveBordersExtractionTests() : base(new NaiveBordersExtraction())
    {
    }
}
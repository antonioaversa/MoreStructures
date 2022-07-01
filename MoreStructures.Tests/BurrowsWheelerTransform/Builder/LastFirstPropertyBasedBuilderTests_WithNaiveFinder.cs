using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

[TestClass]
public class LastFirstPropertyBasedBuilderTests_WithNaiveFinder : BuilderTests
{
    public LastFirstPropertyBasedBuilderTests_WithNaiveFinder() : base(
        new LastFirstPropertyBasedBuilder() 
        { 
            FirstLastFinderBuilder = lastBWMColumn => 
                new NaiveFinder(lastBWMColumn, BWTransform.QuickSort(lastBWMColumn).sortedText),
        })
    {
    }
}

using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder.LastFirstFinders;

[TestClass]
public class PrecomputedFinderTests : LastFirstFinderTests
{
    public PrecomputedFinderTests() 
        : base(lastBWMColumn => new PrecomputedFinder(lastBWMColumn, BWTransform.QuickSort(lastBWMColumn).sortedText))
    {
    }
}

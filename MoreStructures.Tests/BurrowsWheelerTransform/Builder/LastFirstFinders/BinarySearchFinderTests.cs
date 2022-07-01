using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder.LastFirstFinders;

[TestClass]
public class BinarySearchFinderTests : LastFirstFinderTests
{
    public BinarySearchFinderTests() 
        : base(lastBWMColumn => new BinarySearchFinder(lastBWMColumn, BWTransform.QuickSort(lastBWMColumn).sortedText))
    {
    }
}

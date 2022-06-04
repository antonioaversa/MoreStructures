using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

[TestClass]
public class LastFirstPropertyBasedBuilderTests_WithBinarySearchFinder : BuilderTests
{
    public LastFirstPropertyBasedBuilderTests_WithBinarySearchFinder() : base(
        new LastFirstPropertyBasedBuilder() 
        { 
            FirstLastFinderBuilder = lastBWMColumn => 
                new BinarySearchFinder(lastBWMColumn, BWTransform.QuickSort),
        })
    {
    }
}

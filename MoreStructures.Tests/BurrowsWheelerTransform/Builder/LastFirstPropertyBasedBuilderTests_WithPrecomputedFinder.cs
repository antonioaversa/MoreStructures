using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

[TestClass]
public class LastFirstPropertyBasedBuilderTests_WithPrecomputedFinder : BuilderTests
{
    public LastFirstPropertyBasedBuilderTests_WithPrecomputedFinder() : base(
        new LastFirstPropertyBasedBuilder() 
        { 
            FirstLastFinderBuilder = lastBWMColumn => new PrecomputedFinder(lastBWMColumn, BWTransform.QuickSort),
        })
    {
    }
}

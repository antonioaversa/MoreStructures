using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

[TestClass]
public class LastFirstPropertyBasedBuilderWithPrecomputedFinderTests : BuilderTests
{
    public LastFirstPropertyBasedBuilderWithPrecomputedFinderTests() : base(
        new LastFirstPropertyBasedBuilder() 
        { 
            FirstLastFinderBuilder = lastBWMColumn => new PrecomputedFinder(lastBWMColumn),
        })
    {
    }
}

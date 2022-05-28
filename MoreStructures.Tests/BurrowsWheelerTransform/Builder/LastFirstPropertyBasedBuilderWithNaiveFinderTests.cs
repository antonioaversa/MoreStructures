using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

[TestClass]
public class LastFirstPropertyBasedBuilderWithNaiveFinderTests : BuilderTests
{
    public LastFirstPropertyBasedBuilderWithNaiveFinderTests() : base(
        new LastFirstPropertyBasedBuilder() 
        { 
            FirstLastFinderBuilder = lastBWMColumn => new NaiveLastFirstFinder(lastBWMColumn),
        })
    {
    }
}

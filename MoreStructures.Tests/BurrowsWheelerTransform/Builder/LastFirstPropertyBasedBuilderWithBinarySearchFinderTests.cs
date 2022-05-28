using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

[TestClass]
public class LastFirstPropertyBasedBuilderWithBinarySearchFinderTests : BuilderTests
{
    public LastFirstPropertyBasedBuilderWithBinarySearchFinderTests() : base(
        new LastFirstPropertyBasedBuilder() 
        { 
            FirstLastFinderBuilder = lastBWMColumn => new BinarySearchFinder(lastBWMColumn),
        })
    {
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder.LastFirstFinders;

[TestClass]
public class PrecomputedFinderTests : LastFirstFinderTests
{
    public PrecomputedFinderTests() 
        : base(lastBWTColumn => new PrecomputedFinder(lastBWTColumn, BWTransform.QuickSort))
    {
    }
}

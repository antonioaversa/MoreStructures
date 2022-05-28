using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder.LastFirstFinders;

[TestClass]
public class NaiveFinderTests : LastFirstFinderTests
{
    public NaiveFinderTests() : base(lastBWTColumn => new NaiveFinder(lastBWTColumn))
    {
    }
}

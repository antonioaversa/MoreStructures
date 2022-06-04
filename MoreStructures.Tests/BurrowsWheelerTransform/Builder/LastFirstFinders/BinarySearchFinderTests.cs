using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder.LastFirstFinders;

[TestClass]
public class BinarySearchFinderTests : LastFirstFinderTests
{
    public BinarySearchFinderTests() 
        : base(lastBWTColumn => new BinarySearchFinder(lastBWTColumn, BWTransform.QuickSort))
    {
    }
}

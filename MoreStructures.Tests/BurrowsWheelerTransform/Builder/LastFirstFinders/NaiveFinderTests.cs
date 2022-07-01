using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder.LastFirstFinders;

[TestClass]
public class NaiveFinderTests : LastFirstFinderTests
{
    public NaiveFinderTests() : base(
        lastBWMColumn => new NaiveFinder(lastBWMColumn, BWTransform.QuickSort(lastBWMColumn).sortedText))
    {
    }

    [TestMethod]
    public void Ctor_SetsBWTAndSortedBWT()
    {
        var finder1 = new NaiveFinder(new("bb$aba"), BWTransform.QuickSort(new("bb$aba")).sortedText);
        Assert.AreEqual(new RotatedTextWithTerminator("bb$aba"), finder1.BWT);

        var finder2 = new NaiveFinder(new("bb$aba"), new RotatedTextWithTerminator("aabb$"));
        Assert.AreEqual(new RotatedTextWithTerminator("bb$aba"), finder2.BWT);
        Assert.AreEqual(new RotatedTextWithTerminator("aabb$"), finder2.SortedBWT);
    }
}

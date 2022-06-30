using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders;
using MoreStructures.BurrowsWheelerTransform.Matching.Comparers;
using MoreStructures.SuffixArrays;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching.Comparers;

[TestClass]
public class IndexModKPartialSuffixArrayAgainstPatternComparerTests
{
    [TestMethod]
    public void Compare_IsCorrect()
    {
        var text = new TextWithTerminator("panamabananas");
        var bwtBuilder = new LastFirstPropertyBasedBuilder();
        var bwt = bwtBuilder.BuildTransform(text).Content;
        var bwtSorter = BWTransform.QuickSort;
        var partialSuffixArray = new IndexModKPartialSuffixArray(5, 
            new Dictionary<int, int> { [1] = 5, [11] = 10, [12] = 0 });

        var comparer1 = new IndexModKPartialSuffixArrayAgainstPatternComparer(
            text, partialSuffixArray, "ana", bwt, bwtSorter);

        Assert.IsTrue(comparer1.Compare(0, 0) < 0);
        Assert.IsTrue(comparer1.Compare(0, 3) < 0);

        Assert.IsTrue(comparer1.Compare(1, 0) < 0);
        Assert.IsTrue(comparer1.Compare(1, 3) < 0);
        Assert.IsTrue(comparer1.Compare(1, 5) < 0);

        Assert.IsTrue(comparer1.Compare(3, 0) == 0);
        Assert.IsTrue(comparer1.Compare(3, 3) == 0);
        Assert.IsTrue(comparer1.Compare(3, 5) == 0);

        Assert.IsTrue(comparer1.Compare(4, 0) == 0);
        Assert.IsTrue(comparer1.Compare(4, 3) == 0);
        Assert.IsTrue(comparer1.Compare(4, 5) == 0);

        Assert.IsTrue(comparer1.Compare(5, 0) == 0);
        Assert.IsTrue(comparer1.Compare(5, 3) == 0);
        Assert.IsTrue(comparer1.Compare(5, 5) == 0);

        Assert.IsTrue(comparer1.Compare(6, 0) > 0);
        Assert.IsTrue(comparer1.Compare(6, 3) > 0);
        Assert.IsTrue(comparer1.Compare(6, 5) > 0);
    }
}

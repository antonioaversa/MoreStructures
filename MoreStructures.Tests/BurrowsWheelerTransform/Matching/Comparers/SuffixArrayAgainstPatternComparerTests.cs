using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Matching.Comparers;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Matching.Comparers;

[TestClass] 
public class SuffixArrayAgainstPatternComparerTests
{
    [TestMethod]
    public void Compare_IsCorrect()
    {
        var comparer1 = new SuffixArrayAgainstPatternComparer(
            new("mississippi"), new(Enumerable.Range(0, 12)), "issi");
        
        Assert.IsTrue(comparer1.Compare(0, 0) > 0);
        Assert.IsTrue(comparer1.Compare(0, 3) > 0);
        
        Assert.IsTrue(comparer1.Compare(1, 0) == 0);
        Assert.IsTrue(comparer1.Compare(1, 3) == 0);
        Assert.IsTrue(comparer1.Compare(1, 5) == 0);
        
        Assert.IsTrue(comparer1.Compare(2, 0) > 0);
        Assert.IsTrue(comparer1.Compare(2, 3) > 0);
        Assert.IsTrue(comparer1.Compare(2, 5) > 0);

        Assert.IsTrue(comparer1.Compare(4, 0) == 0);
        Assert.IsTrue(comparer1.Compare(4, 3) == 0);
        Assert.IsTrue(comparer1.Compare(4, 5) == 0);

        Assert.IsTrue(comparer1.Compare(9, 0) > 0);
        Assert.IsTrue(comparer1.Compare(9, 3) > 0);
        Assert.IsTrue(comparer1.Compare(9, 5) > 0);

        var comparer2 = new SuffixArrayAgainstPatternComparer(
            new("issi"), new(Enumerable.Range(0, 12)), "omissis");

        Assert.IsTrue(comparer2.Compare(0, 0) < 0);
        Assert.IsTrue(comparer2.Compare(1, 0) > 0);
        Assert.IsTrue(comparer2.Compare(2, 0) > 0);
        Assert.IsTrue(comparer2.Compare(3, 0) < 0);

        var comparer3 = new SuffixArrayAgainstPatternComparer(
            new("issi"), new(Enumerable.Range(0, 12)), "issi");

        Assert.IsTrue(comparer3.Compare(0, 0) == 0);
        Assert.IsTrue(comparer3.Compare(1, 0) > 0);
        Assert.IsTrue(comparer3.Compare(2, 0) > 0);
        Assert.IsTrue(comparer3.Compare(3, 0) < 0);
    }
}

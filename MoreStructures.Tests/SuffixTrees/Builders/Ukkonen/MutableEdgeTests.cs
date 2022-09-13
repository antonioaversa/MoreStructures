using MoreStructures.SuffixTrees.Builders.Ukkonen;

namespace MoreStructures.Tests.SuffixTrees.Builders.Ukkonen;

[TestClass]
public class MutableEdgeTests
{
    [TestMethod]
    public void Equals_BasedOnStartAndEndValue()
    {
        var e1 = new MutableEdge(2, new(3));
        var e2 = new MutableEdge(2, new(3));
        Assert.AreEqual(e1, e2);

        var e3 = new MutableEdge(3, new(3));
        Assert.AreNotEqual(e1, e3);

        var e4 = new MutableEdge(2, new(4));
        Assert.AreNotEqual(e1, e4);
    }

    [TestMethod]
    public void GetHashCode_DoesntChangeWhenMovingEnd()
    {
        var me = new MovingEnd(3);
        var e1 = new MutableEdge(2, me);
        var hc1 = e1.GetHashCode();
        me.Value++;
        var hc2 = e1.GetHashCode();
        Assert.AreEqual(hc1, hc2);
    }
}

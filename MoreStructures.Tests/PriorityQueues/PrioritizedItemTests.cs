using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

[TestClass]
public class PrioritizedItemTests
{
    [TestMethod]
    public void CompareTo_IsCorrect()
    {
        Assert.IsTrue(new PrioritizedItem<int>(3, 2, 0).CompareTo(new PrioritizedItem<int>(3, 3, 1)) < 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 2, 0).CompareTo(new PrioritizedItem<int>(3, 4, 1)) < 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 2, 1).CompareTo(new PrioritizedItem<int>(3, 3, 0)) < 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 2, 1).CompareTo(new PrioritizedItem<int>(3, 2, 0)) < 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 2, 0).CompareTo(new PrioritizedItem<int>(4, 3, 0)) < 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 2, 0).CompareTo(new PrioritizedItem<int>(4, 3, 1)) < 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 3, 1).CompareTo(new PrioritizedItem<int>(3, 3, 0)) < 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(3, 3, 0)) == 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(3, 3, 1)) > 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(3, 2, 0)) > 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(3, 2, 1)) > 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(4, 2, 0)) > 0);
        Assert.IsTrue(new PrioritizedItem<int>(3, 4, 0).CompareTo(new PrioritizedItem<int>(4, 2, 0)) > 0);
    }

    [TestMethod]
    public void CompareTo_TakesIntoAccountEras()
    {
        var pi1 = new PrioritizedItem<bool>(true, 2, 0, new(0));
        var pi2 = new PrioritizedItem<bool>(true, 2, 0, new(0));
        Assert.IsTrue(pi1.CompareTo(pi2) == 0);

        var pi3 = new PrioritizedItem<bool>(true, 2, 0, new(1));
        Assert.IsTrue(pi1.CompareTo(pi3) > 0);
        Assert.IsTrue(pi3.CompareTo(pi1) < 0);

        var pi4 = new PrioritizedItem<bool>(true, 2, 1, new(0));
        Assert.IsTrue(pi4.CompareTo(pi1) < 0);
        Assert.IsTrue(pi4.CompareTo(pi3) > 0);

        var pi5 = new PrioritizedItem<bool>(true, 2, 1, new(1));
        Assert.IsTrue(pi5.CompareTo(pi3) < 0);

        var pi6 = new PrioritizedItem<bool>(true, 2, 1, new(-1));
        Assert.IsTrue(pi6.CompareTo(pi1) > 0);
    }
}

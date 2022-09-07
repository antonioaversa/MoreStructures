using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

[TestClass]
public class PrioritizedItemTests
{
    [TestMethod]
    public void CompareTo_IsCorrect()
    {
        Assert.AreEqual(-1, new PrioritizedItem<int>(3, 2, 0).CompareTo(new PrioritizedItem<int>(3, 3, 1)));
        Assert.AreEqual(-1, new PrioritizedItem<int>(3, 2, 1).CompareTo(new PrioritizedItem<int>(3, 3, 0)));
        Assert.AreEqual(-1, new PrioritizedItem<int>(3, 2, 1).CompareTo(new PrioritizedItem<int>(3, 2, 0)));
        Assert.AreEqual(-1, new PrioritizedItem<int>(3, 2, 0).CompareTo(new PrioritizedItem<int>(4, 3, 0)));
        Assert.AreEqual(-1, new PrioritizedItem<int>(3, 2, 0).CompareTo(new PrioritizedItem<int>(4, 3, 1)));
        Assert.AreEqual(-1, new PrioritizedItem<int>(3, 3, 1).CompareTo(new PrioritizedItem<int>(3, 3, 0)));
        Assert.AreEqual(0, new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(3, 3, 0)));
        Assert.AreEqual(1, new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(3, 3, 1)));
        Assert.AreEqual(1, new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(3, 2, 0)));
        Assert.AreEqual(1, new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(3, 2, 1)));
        Assert.AreEqual(1, new PrioritizedItem<int>(3, 3, 0).CompareTo(new PrioritizedItem<int>(4, 2, 0)));
    }
}

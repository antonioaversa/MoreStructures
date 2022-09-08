using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

public abstract class PeekKthPriorityQueueTests
{
    protected Func<IPeekKthPriorityQueue<int>> IntQueueBuilder { get; }

    protected PeekKthPriorityQueueTests(Func<IPeekKthPriorityQueue<int>> intQueueBuilder)
    {
        IntQueueBuilder = intQueueBuilder;
    }

    [TestMethod]
    public void PeekKth_ThrowsExceptionOnInvalidK()
    {
        var queue = IntQueueBuilder();
        Assert.ThrowsException<ArgumentException>(() => queue.PeekKth(-1));
        Assert.ThrowsException<ArgumentException>(() => queue.PeekKth(-10));
    }

    [TestMethod]
    public void PeekKth_IsCorrect()
    {
        var queue = IntQueueBuilder();
        Assert.IsNull(queue.PeekKth(0));
        Assert.IsNull(queue.PeekKth(10));
        queue.Push(2, 3);
        Assert.AreEqual(queue.Peek(), queue.PeekKth(0));
        Assert.IsNull(queue.PeekKth(1));
        queue.Push(3, 2);
        Assert.AreEqual(queue.Peek(), queue.PeekKth(0));
        Assert.AreEqual(new PrioritizedItem<int>(3, 2, 1), queue.PeekKth(1));
        Assert.IsNull(queue.PeekKth(2));
        queue.Push(4, 0);
        queue.Push(5, 1);
        queue.Push(6, 4);
        Assert.AreEqual(new PrioritizedItem<int>(2, 3, 0), queue.PeekKth(1));
        Assert.AreEqual(new PrioritizedItem<int>(5, 1, 3), queue.PeekKth(3));
        Assert.IsNull(queue.PeekKth(5));
        queue.Pop();
        Assert.AreEqual(new PrioritizedItem<int>(4, 0, 2), queue.PeekKth(3));
        queue.Pop();
        Assert.IsNull(queue.PeekKth(3));
        Assert.AreEqual(new PrioritizedItem<int>(3, 2, 1), queue.PeekKth(0));
    }

    [TestMethod]
    public void PeekKth_IsStable()
    {
        var queue = IntQueueBuilder();
        queue.Push(6, 2); // 0
        queue.Push(2, 3); // 1
        queue.Push(2, 3); // 2
        queue.Push(3, 3); // 3
        queue.Push(4, 3); // 4
        queue.Push(3, 3); // 5
        queue.Push(1, 2); // 6
        Assert.AreEqual(queue.Peek(), queue.PeekKth(0));
        Assert.AreEqual(new PrioritizedItem<int>(2, 3, 2), queue.PeekKth(1));
        Assert.AreEqual(new PrioritizedItem<int>(3, 3, 3), queue.PeekKth(2));
        Assert.AreEqual(new PrioritizedItem<int>(3, 3, 5), queue.PeekKth(4));
        queue.Pop();
        queue.Pop();
        Assert.AreEqual(new PrioritizedItem<int>(4, 3, 4), queue.PeekKth(1));
        Assert.AreEqual(new PrioritizedItem<int>(3, 3, 5), queue.PeekKth(2));
        Assert.AreEqual(new PrioritizedItem<int>(1, 2, 6), queue.PeekKth(4));
        Assert.IsNull(queue.PeekKth(5));
        for (var i = 0; i < 3; i++) 
            queue.Pop();
        Assert.AreEqual(new PrioritizedItem<int>(6, 2, 0), queue.PeekKth(0));
        Assert.AreEqual(new PrioritizedItem<int>(1, 2, 6), queue.PeekKth(1));
        queue.Pop();
        queue.Pop();
        Assert.IsNull(queue.PeekKth(0));
    }
}

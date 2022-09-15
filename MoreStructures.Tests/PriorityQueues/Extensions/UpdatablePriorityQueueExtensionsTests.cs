using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues;
using MoreStructures.PriorityQueues.ArrayList;
using MoreStructures.PriorityQueues.BinaryHeap;
using MoreStructures.PriorityQueues.Extensions;

namespace MoreStructures.Tests.PriorityQueues.Extensions;

public abstract class UpdatablePriorityQueueExtensionsTests
{
    protected Func<IUpdatablePriorityQueue<int>> IntQueueBuilder { get; }

    protected UpdatablePriorityQueueExtensionsTests(Func<IUpdatablePriorityQueue<int>> intQueueBuilder)
    {
        IntQueueBuilder = intQueueBuilder;
    }

    [TestMethod]
    public void PushOrUpdate_IsCorrect()
    {
        var queue = IntQueueBuilder();

        // Pushes
        queue.PushOrUpdate(1, 2);
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(new PrioritizedItem<int>(1, 2, 0), queue.Peek());
        queue.PushOrUpdate(0, 3);
        Assert.AreEqual(2, queue.Count);
        Assert.AreEqual(new PrioritizedItem<int>(0, 3, 1), queue.Peek());
        queue.PushOrUpdate(2, 1);
        Assert.AreEqual(new PrioritizedItem<int>(0, 3, 1), queue.Peek());

        // Updates
        queue.PushOrUpdate(2, 4);
        Assert.AreEqual(new PrioritizedItem<int>(2, 4, 3), queue.Peek());
        queue.PushOrUpdate(1, 5);
        Assert.AreEqual(new PrioritizedItem<int>(1, 5, 4), queue.Peek());
        queue.PushOrUpdate(1, 3);
        Assert.AreEqual(new PrioritizedItem<int>(2, 4, 3), queue.Peek());
    }

    [TestMethod]
    public void PopAll_ExtractsAllItems()
    {
        var queue = IntQueueBuilder();
        queue.Push(1, 5);
        queue.Push(2, 1);
        queue.Push(3, 3);
        queue.Push(4, 4);
        queue.Push(0, 6);
        queue.Push(1, 2);
        var itemsCount = queue.Count;
        var items = queue.PopAll().ToList();
        Assert.AreEqual(itemsCount, items.Count);
        Assert.IsTrue(
            new[] { (0, 6), (1, 5), (4, 4), (3, 3), (1, 2), (2, 1) }.SequenceEqual(
                items.Select(pi => (pi.Item, pi.Priority))));
        Assert.AreEqual(0, queue.Count);
    }
}

[TestClass]
public class UpdatablePriorityQueueExtensionsTests_WithArrayListPriorityQueue
    : UpdatablePriorityQueueExtensionsTests
{
    public UpdatablePriorityQueueExtensionsTests_WithArrayListPriorityQueue()
        : base(() => new ArrayListPriorityQueue<int>())
    {
    }
}

[TestClass]
public class UpdatablePriorityQueueExtensionsTests_WithUpdatableBinaryHeapPriorityQueue
    : UpdatablePriorityQueueExtensionsTests
{
    public UpdatablePriorityQueueExtensionsTests_WithUpdatableBinaryHeapPriorityQueue() 
        : base(() => new UpdatableBinaryHeapPriorityQueue<int>())
    {
    }
}
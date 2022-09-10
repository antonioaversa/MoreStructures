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
}

[TestClass]
public class UpdatablePriorityQueueExtensionsTests_ArrayListBasedPriorityQueue
    : UpdatablePriorityQueueExtensionsTests
{
    public UpdatablePriorityQueueExtensionsTests_ArrayListBasedPriorityQueue()
        : base(() => new ArrayListBasedPriorityQueue<int>())
    {
    }
}

[TestClass]
public class UpdatablePriorityQueueExtensionsTests_WithUpdatableHeapBasedPriorityQueue
    : UpdatablePriorityQueueExtensionsTests
{
    public UpdatablePriorityQueueExtensionsTests_WithUpdatableHeapBasedPriorityQueue() 
        : base(() => new UpdatableHeapBasedPriorityQueue<int>())
    {
    }
}
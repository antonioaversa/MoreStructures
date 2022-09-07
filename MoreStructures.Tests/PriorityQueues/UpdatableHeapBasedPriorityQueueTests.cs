using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

[TestClass]
public class UpdatableHeapBasedPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public UpdatableHeapBasedPriorityQueueTests_AsBasicQueue() : base(
        () => new UpdatableHeapBasedPriorityQueue<int>(),
        () => new UpdatableHeapBasedPriorityQueue<RefType>(),
        () => new UpdatableHeapBasedPriorityQueue<ValType>())
    {
    }
}


[TestClass]
public class UpdatableHeapBasedPriorityQueueTests_AsUpdatableQueue : UpdatablePriorityQueueTests
{
    public UpdatableHeapBasedPriorityQueueTests_AsUpdatableQueue() : base(
        () => new UpdatableHeapBasedPriorityQueue<int>(),
        () => new UpdatableHeapBasedPriorityQueue<RefType>(),
        () => new UpdatableHeapBasedPriorityQueue<ValType>())
    {
    }
}

using MoreStructures.PriorityQueues.BinaryHeap;

namespace MoreStructures.Tests.PriorityQueues.BinaryHeap;

[TestClass]
public class UpdatableBinaryHeapPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public UpdatableBinaryHeapPriorityQueueTests_AsBasicQueue() : base(
        () => new UpdatableBinaryHeapPriorityQueue<int>(),
        () => new UpdatableBinaryHeapPriorityQueue<RefType>(),
        () => new UpdatableBinaryHeapPriorityQueue<ValType>())
    {
    }
}


[TestClass]
public class UpdatableBinaryHeapPriorityQueueTests_AsUpdatableQueue : UpdatablePriorityQueueTests
{
    public UpdatableBinaryHeapPriorityQueueTests_AsUpdatableQueue() : base(
        () => new UpdatableBinaryHeapPriorityQueue<int>(),
        () => new UpdatableBinaryHeapPriorityQueue<RefType>(),
        () => new UpdatableBinaryHeapPriorityQueue<ValType>())
    {
    }
}

[TestClass]
public class UpdatableBinaryHeapPriorityQueueTests_AsMergeableAndUpdatableQueue 
    : MergeableAndUpdatablePriorityQueueTests<UpdatableBinaryHeapPriorityQueue<int>>
{
    public UpdatableBinaryHeapPriorityQueueTests_AsMergeableAndUpdatableQueue() : base(
        () => new UpdatableBinaryHeapPriorityQueue<int>())
    {
    }
}


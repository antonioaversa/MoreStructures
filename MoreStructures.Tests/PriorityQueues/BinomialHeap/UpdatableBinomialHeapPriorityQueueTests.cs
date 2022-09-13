using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.Tests.PriorityQueues.BinomialHeap;

[TestClass]
public class UpdatableBinomialHeapPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public UpdatableBinomialHeapPriorityQueueTests_AsBasicQueue() : base(
        () => new UpdatableBinomialHeapPriorityQueue<int>(),
        () => new UpdatableBinomialHeapPriorityQueue<RefType>(),
        () => new UpdatableBinomialHeapPriorityQueue<ValType>())
    {
    }
}

[TestClass]
public class UpdatableBinomialHeapPriorityQueueTests_AsUpdatableQueue : UpdatablePriorityQueueTests
{
    public UpdatableBinomialHeapPriorityQueueTests_AsUpdatableQueue() : base(
        () => new UpdatableBinomialHeapPriorityQueue<int>(),
        () => new UpdatableBinomialHeapPriorityQueue<RefType>(),
        () => new UpdatableBinomialHeapPriorityQueue<ValType>())
    {
    }
}


using MoreStructures.PriorityQueues.BinaryHeap;

namespace MoreStructures.Tests.PriorityQueues.BinaryHeap;

[TestClass]
public class BinaryHeapPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public BinaryHeapPriorityQueueTests_AsBasicQueue() : base(
        () => new BinaryHeapPriorityQueue<int>(),
        () => new BinaryHeapPriorityQueue<RefType>(),
        () => new BinaryHeapPriorityQueue<ValType>())
    {
    }
}

[TestClass]
public class BinaryHeapPriorityQueueTests_AsPeekKthQueue : PeekKthPriorityQueueTests
{
    public BinaryHeapPriorityQueueTests_AsPeekKthQueue() : base(
        () => new BinaryHeapPriorityQueue<int>())
    {
    }
}

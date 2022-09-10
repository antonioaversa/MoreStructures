using MoreStructures.PriorityQueues.BinaryHeap;

namespace MoreStructures.Tests.PriorityQueues.BinaryHeap;

[TestClass]
public class HeapBasedPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public HeapBasedPriorityQueueTests_AsBasicQueue() : base(
        () => new HeapBasedPriorityQueue<int>(),
        () => new HeapBasedPriorityQueue<RefType>(),
        () => new HeapBasedPriorityQueue<ValType>())
    {
    }
}

[TestClass]
public class HeapBasedPriorityQueueTests_AsPeekKthQueue : PeekKthPriorityQueueTests
{
    public HeapBasedPriorityQueueTests_AsPeekKthQueue() : base(
        () => new HeapBasedPriorityQueue<int>())
    {
    }
}

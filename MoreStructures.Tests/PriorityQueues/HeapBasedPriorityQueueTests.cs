using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

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

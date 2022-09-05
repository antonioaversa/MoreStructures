using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

[TestClass]
public class HeapBasedPriorityQueueTests : PriorityQueueTests
{
    public HeapBasedPriorityQueueTests() : base(
        () => new HeapBasedPriorityQueue<int>(),
        () => new HeapBasedPriorityQueue<RefType>(),
        () => new HeapBasedPriorityQueue<ValType>())
    {
    }
}

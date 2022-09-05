using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

[TestClass]
public class UpdatableHeapBasedPriorityQueueTests : UpdatablePriorityQueueTests
{
    public UpdatableHeapBasedPriorityQueueTests() : base(
        () => new UpdatableHeapBasedPriorityQueue<int>(),
        () => new UpdatableHeapBasedPriorityQueue<RefType>(),
        () => new UpdatableHeapBasedPriorityQueue<ValType>())
    {
    }
}

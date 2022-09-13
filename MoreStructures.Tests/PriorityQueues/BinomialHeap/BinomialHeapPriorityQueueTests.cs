using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.Tests.PriorityQueues.BinomialHeap;

[TestClass]
public class BinomialHeapPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public BinomialHeapPriorityQueueTests_AsBasicQueue() : base(
        () => new BinomialHeapPriorityQueue<int>(),
        () => new BinomialHeapPriorityQueue<RefType>(),
        () => new BinomialHeapPriorityQueue<ValType>())
    {
    }
}

using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.Tests.PriorityQueues.FibonacciHeap;

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




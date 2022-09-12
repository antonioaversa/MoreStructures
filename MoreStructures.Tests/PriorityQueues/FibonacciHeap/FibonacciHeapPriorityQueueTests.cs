using MoreStructures.PriorityQueues.FibonacciHeap;

namespace MoreStructures.Tests.PriorityQueues.FibonacciHeap;

[TestClass]
public class FibonacciHeapPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public FibonacciHeapPriorityQueueTests_AsBasicQueue() : base(
        () => new FibonacciHeapPriorityQueue<int>(),
        () => new FibonacciHeapPriorityQueue<RefType>(),
        () => new FibonacciHeapPriorityQueue<ValType>())
    {
    }
}




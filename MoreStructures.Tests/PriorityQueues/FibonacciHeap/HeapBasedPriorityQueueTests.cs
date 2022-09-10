using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues.FibonacciHeap;

namespace MoreStructures.Tests.PriorityQueues.FibonacciHeap;

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




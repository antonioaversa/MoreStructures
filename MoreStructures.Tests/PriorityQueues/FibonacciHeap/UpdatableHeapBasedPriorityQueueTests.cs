using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues.FibonacciHeap;

namespace MoreStructures.Tests.PriorityQueues.FibonacciHeap;

[TestClass]
public class UpdatableHeapBasedPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public UpdatableHeapBasedPriorityQueueTests_AsBasicQueue() : base(
        () => new UpdatableHeapBasedPriorityQueue<int>(),
        () => new UpdatableHeapBasedPriorityQueue<RefType>(),
        () => new UpdatableHeapBasedPriorityQueue<ValType>())
    {
    }
}


[TestClass]
public class UpdatableHeapBasedPriorityQueueTests_AsUpdatableQueue : UpdatablePriorityQueueTests
{
    public UpdatableHeapBasedPriorityQueueTests_AsUpdatableQueue() : base(
        () => new UpdatableHeapBasedPriorityQueue<int>(),
        () => new UpdatableHeapBasedPriorityQueue<RefType>(),
        () => new UpdatableHeapBasedPriorityQueue<ValType>())
    {
    }

    [TestMethod]
    public void UpdatePriority_TriggersAChainOfLosers()
    {
        var queue = IntUpdatableQueueBuilder();
        for (var i = 0; i < 1024; i++)
            queue.Push(i, i);
        queue.Pop();

        // After 2^10 pushes, the heap is rebalanced by a single pop into a forest of 9 binomial trees.
        // Due to the way the merging is performed, the last root of the forest has value 511 = 2^9 -1.
        // The "last chain" (last -> last -> last...) from 511 is 255 -> 127 -> ... -> 3 -> 1 -> 0.
        // So the chain of 9 nodes from 2^8 - 1 = 255 down to 2^0 - 1 = 0.
        // The "2-nd chain" (2nd last -> 2nd last -> 2nd last...) from 511 is 383 -> 193 -> ... -> 2.
        // So the chain of 8 nodes from 2^8 + 2^7 - 1 = 383 down to 2^1 + 2^0 - 1 = 2.

        // Updating the priority of each of the nodes in the "2-nd chain" to higher values flags all the nodes in the
        // "last chain" from the first (511) to 3 as loser.
        for (var i = 8; i > 0; i--)
            queue.UpdatePriority((int)(Math.Pow(2, i) + Math.Pow(2, i - 1) - 1), 1024);

        // So updating the priority of "1" to a higher value will promote "1" as a root and trigger a chain reaction of
        // losers being update to being root nodes.
        queue.UpdatePriority(1, 1024);
    }

}

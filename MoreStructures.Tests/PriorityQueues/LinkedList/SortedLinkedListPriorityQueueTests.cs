using MoreStructures.PriorityQueues;
using MoreStructures.PriorityQueues.LinkedList;

namespace MoreStructures.Tests.PriorityQueues.LinkedList;

[TestClass]
public class SortedLinkedListPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public SortedLinkedListPriorityQueueTests_AsBasicQueue() : base(
        () => new SortedLinkedListPriorityQueue<int>(),
        () => new SortedLinkedListPriorityQueue<RefType>(),
        () => new SortedLinkedListPriorityQueue<ValType>())
    {
    }

    [TestMethod]
    public void Ctor_WithEnumerable()
    {
        static IEnumerable<PrioritizedItem<int>> GetNumbers()
        {
            yield return new(2, 1, 0);
            yield return new(2, 3, 1);
            yield return new(3, -1, 2);
            yield return new(1, 4, 3);
        }

        var queue = new SortedLinkedListPriorityQueue<int>(GetNumbers());
        Assert.AreEqual(4, queue.Count);

        Assert.IsTrue(GetNumbers().OrderByDescending(e => e.Priority).Select(e => e.Item).SequenceEqual(queue));
    }

    [TestMethod]
    public void Ctor_WithProvidedLinkedList()
    {
        var backingList = new LinkedList<PrioritizedItem<int>>(
            new PrioritizedItem<int>[] { new(2, 1, 0), new(2, 3, 1), new(3, -1, 2), new(1, 4, 3) });
        var queue = new SortedLinkedListPriorityQueue<int>(backingList);
        Assert.AreEqual(4, queue.Count);
        queue.Push(4, 5);
        Assert.AreEqual(5, backingList.Count);
        queue.Peek();
        Assert.AreEqual(5, backingList.Count);
        queue.Pop();
        Assert.AreEqual(4, backingList.Count);
    }
}

[TestClass]
public class SortedLinkedListPriorityQueueTests_AsUpdatableQueue : UpdatablePriorityQueueTests
{
    public SortedLinkedListPriorityQueueTests_AsUpdatableQueue() : base(
        () => new SortedLinkedListPriorityQueue<int>(),
        () => new SortedLinkedListPriorityQueue<RefType>(),
        () => new SortedLinkedListPriorityQueue<ValType>())
    {
    }
}

[TestClass]
public class SortedLinkedListPriorityQueueTests_AsPeekKthQueue : PeekKthPriorityQueueTests
{
    public SortedLinkedListPriorityQueueTests_AsPeekKthQueue() : base(
        () => new SortedLinkedListPriorityQueue<int>())
    {
    }
}

[TestClass]
public class SortedLinkedListPriorityQueueTests_AsMergeableAndUpdatable
    : MergeableAndUpdatablePriorityQueueTests<SortedLinkedListPriorityQueue<int>>
{
    public SortedLinkedListPriorityQueueTests_AsMergeableAndUpdatable() : base(
        () => new SortedLinkedListPriorityQueue<int>())
    {
    }
}

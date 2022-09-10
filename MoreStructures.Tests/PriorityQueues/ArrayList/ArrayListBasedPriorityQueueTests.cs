using MoreStructures.PriorityQueues;
using MoreStructures.PriorityQueues.ArrayList;

namespace MoreStructures.Tests.PriorityQueues.ArrayList;

[TestClass]
public class ArrayListBasedPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public ArrayListBasedPriorityQueueTests_AsBasicQueue() : base(
        () => new ArrayListBasedPriorityQueue<int>(),
        () => new ArrayListBasedPriorityQueue<RefType>(),
        () => new ArrayListBasedPriorityQueue<ValType>())
    {
    }

    [TestMethod]
    public void Ctor_WithEnumerable()
    {
        IEnumerable<PrioritizedItem<int>> GetNumbers()
        {
            yield return new(2, 1, 0);
            yield return new(2, 3, 1);
            yield return new(3, -1, 2);
            yield return new(1, 4, 3);
        }

        var queue = new ArrayListBasedPriorityQueue<int>(GetNumbers());
        Assert.AreEqual(4, queue.Count);

        Assert.IsTrue(GetNumbers().OrderByDescending(e => e.Priority).Select(e => e.Item).SequenceEqual(queue));
    }

    [TestMethod]
    public void Ctor_WithProvidedList()
    {
        var backingList = new List<PrioritizedItem<int>> { new(2, 1, 0), new(2, 3, 1), new(3, -1, 2), new(1, 4, 3) };
        var queue = new ArrayListBasedPriorityQueue<int>(backingList);
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
public class ArrayListBasedPriorityQueueTests_AsUpdatableQueue : UpdatablePriorityQueueTests
{
    public ArrayListBasedPriorityQueueTests_AsUpdatableQueue() : base(
        () => new ArrayListBasedPriorityQueue<int>(),
        () => new ArrayListBasedPriorityQueue<RefType>(),
        () => new ArrayListBasedPriorityQueue<ValType>())
    {
    }
}

[TestClass]
public class ArrayListBasedPriorityQueueTests_AsPeekKthQueue : PeekKthPriorityQueueTests
{
    public ArrayListBasedPriorityQueueTests_AsPeekKthQueue() : base(
        () => new ArrayListBasedPriorityQueue<int>())
    {
    }

    [TestMethod]
    public void PeekKth_WorstQuickFindPivot()
    {
        var queue = IntQueueBuilder();
        var numberOfItems = 10;
        for (var i = numberOfItems - 1; i >= 0; i--)
            queue.Push(i, i);
        for (var i = 0; i < numberOfItems; i++)
            Assert.AreEqual(numberOfItems - 1 - i, queue.PeekKth(i)?.Item);
    }
}

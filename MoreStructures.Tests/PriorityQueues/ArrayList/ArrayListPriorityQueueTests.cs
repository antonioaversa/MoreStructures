﻿using MoreStructures.PriorityQueues;
using MoreStructures.PriorityQueues.ArrayList;

namespace MoreStructures.Tests.PriorityQueues.ArrayList;

[TestClass]
public class ArrayListPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public ArrayListPriorityQueueTests_AsBasicQueue() : base(
        () => new ArrayListPriorityQueue<int>(),
        () => new ArrayListPriorityQueue<RefType>(),
        () => new ArrayListPriorityQueue<ValType>())
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

        var queue = new ArrayListPriorityQueue<int>(GetNumbers());
        Assert.AreEqual(4, queue.Count);

        Assert.IsTrue(GetNumbers().OrderByDescending(e => e.Priority).Select(e => e.Item).SequenceEqual(queue));
    }

    [TestMethod]
    public void Ctor_WithProvidedList()
    {
        var backingList = new List<PrioritizedItem<int>> { new(2, 1, 0), new(2, 3, 1), new(3, -1, 2), new(1, 4, 3) };
        var queue = new ArrayListPriorityQueue<int>(backingList);
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
public class ArrayListPriorityQueueTests_AsUpdatableQueue : UpdatablePriorityQueueTests
{
    public ArrayListPriorityQueueTests_AsUpdatableQueue() : base(
        () => new ArrayListPriorityQueue<int>(),
        () => new ArrayListPriorityQueue<RefType>(),
        () => new ArrayListPriorityQueue<ValType>())
    {
    }
}

[TestClass]
public class ArrayListPriorityQueueTests_AsPeekKthQueue : PeekKthPriorityQueueTests
{
    public ArrayListPriorityQueueTests_AsPeekKthQueue() : base(
        () => new ArrayListPriorityQueue<int>())
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

[TestClass]
public class ArrayListPriorityQueueTests_AsMergeableAndUpdatable 
    : MergeableAndUpdatablePriorityQueueTests<ArrayListPriorityQueue<int>>
{
    public ArrayListPriorityQueueTests_AsMergeableAndUpdatable() : base(
        () => new ArrayListPriorityQueue<int>())
    {
    }
}

using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

[TestClass]
public class ArrayListBasedPriorityQueueTests : UpdatablePriorityQueueTests
{
    public ArrayListBasedPriorityQueueTests() : base(
        () => new ArrayListBasedPriorityQueue<int>(),
        () => new ArrayListBasedPriorityQueue<RefType>(),
        () => new ArrayListBasedPriorityQueue<ValType>())
    {
    }

    [TestMethod]
    public void Ctor_WithEnumerable()
    {
        IEnumerable<ItemAndPriority<int>> GetNumbers()
        {
            yield return new(2, 1);
            yield return new(2, 3);
            yield return new(3, -1);
            yield return new(1, 4);
        }

        var queue = new ArrayListBasedPriorityQueue<int>(GetNumbers());
        Assert.AreEqual(4, queue.Count);

        Assert.IsTrue(GetNumbers().OrderByDescending(e => e.Priority).Select(e => e.Item).SequenceEqual(queue));
    }

    [TestMethod]
    public void Ctor_WithProvidedList()
    {
        var backingList = new List<ItemAndPriority<int>> { new(2, 1), new(2, 3), new(3, -1), new(1, 4) };
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

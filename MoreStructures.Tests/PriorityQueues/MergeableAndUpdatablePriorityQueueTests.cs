using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

public abstract class MergeableAndUpdatablePriorityQueueTests<TPriorityQueue>
    : MergeablePriorityQueueTests<TPriorityQueue>
    where TPriorityQueue : IMergeablePriorityQueue<int, TPriorityQueue>, IUpdatablePriorityQueue<int>
{
    protected MergeableAndUpdatablePriorityQueueTests(Func<TPriorityQueue> builder) : base(builder)
    {
    }

    [TestMethod]
    public void Merge_QueueUpdatesKeepWorkingAfter()
    {
        var source = IntBuilder();
        source.Push(2, 0);
        source.Push(2, -4);
        source.Push(3, -2);
        
        var target = IntBuilder();
        target.Push(1, 2);
        target.Push(2, -4);

        source.MergeFrom(target);

        Assert.AreEqual(0, target.GetPrioritiesOf(1).Count());
        Assert.AreEqual(0, target.GetPrioritiesOf(2).Count());

        AssertClearedQueueKeepsWorking(target);
    }

    [TestMethod]
    public void Clear_QueueUpdatesKeepWorkingAfter()
    {
        var queue = IntBuilder();
        queue.Push(1, 2);
        queue.Push(2, -4);
        Assert.IsTrue(queue.GetPrioritiesOf(1).SequenceEqual(new[] { 2 }));
        Assert.IsTrue(queue.GetPrioritiesOf(2).SequenceEqual(new[] { -4 }));
        
        queue.Clear();
        AssertClearedQueueKeepsWorking(queue);
    }

    private static void AssertClearedQueueKeepsWorking(TPriorityQueue queue)
    {
        queue.Push(1, 1);
        Assert.IsTrue(queue.GetPrioritiesOf(1).SequenceEqual(new[] { 1 }));

        queue.UpdatePriority(1, 0);
        Assert.IsTrue(queue.GetPrioritiesOf(1).SequenceEqual(new[] { 0 }));

        queue.Push(2, 2);
        Assert.IsTrue(queue.GetPrioritiesOf(2).SequenceEqual(new[] { 2 }));

        queue.Pop();
        Assert.AreEqual(0, queue.GetPrioritiesOf(2).Count());

        queue.Pop();
        Assert.AreEqual(0, queue.GetPrioritiesOf(1).Count());

        queue.Push(1, 0);
        queue.Push(2, -1);
        var removedItem1 = queue.Remove(1);
        Assert.AreEqual(1, removedItem1!.Value.Item);
        Assert.AreEqual(0, removedItem1!.Value.Priority);
        Assert.IsNull(queue.Remove(1));

        var removedItem2 = queue.Remove(2);
        Assert.AreEqual(2, removedItem2!.Value.Item);
        Assert.AreEqual(-1, removedItem2!.Value.Priority);
        Assert.IsNull(queue.Remove(2));
    }
}


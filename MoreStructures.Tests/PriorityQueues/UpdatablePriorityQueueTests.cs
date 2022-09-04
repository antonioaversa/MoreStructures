using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

public abstract class UpdatablePriorityQueueTests : PriorityQueueTests
{
    protected Func<IUpdatablePriorityQueue<int>> IntUpdatableQueueBuilder { get; }
    protected Func<IUpdatablePriorityQueue<RefType>> RefTypeUpdatableQueueBuilder { get; }
    protected Func<IUpdatablePriorityQueue<ValType>> ValTypeUpdatableQueueBuilder { get; }

    protected UpdatablePriorityQueueTests(
        Func<IUpdatablePriorityQueue<int>> intUpdatableQueueBuilder, 
        Func<IUpdatablePriorityQueue<RefType>> refTypeUpdatableQueueBuilder, 
        Func<IUpdatablePriorityQueue<ValType>> valTypeUpdatableQueueBuilder) 
        : base(intUpdatableQueueBuilder, refTypeUpdatableQueueBuilder, valTypeUpdatableQueueBuilder)
    {
        IntUpdatableQueueBuilder = intUpdatableQueueBuilder;
        RefTypeUpdatableQueueBuilder = refTypeUpdatableQueueBuilder;
        ValTypeUpdatableQueueBuilder = valTypeUpdatableQueueBuilder;
    }

    [TestMethod]
    public void GetHighestPriorityOf_IsCorrect()
    {
        var queue = IntUpdatableQueueBuilder();
        Assert.IsNull(queue.GetHighestPriorityOf(3));
        queue.Push(3, 2);
        Assert.AreEqual(2, queue.GetHighestPriorityOf(3));
        queue.Push(4, 0);
        Assert.AreEqual(2, queue.GetHighestPriorityOf(3));
        Assert.AreEqual(0, queue.GetHighestPriorityOf(4));
        queue.Push(3, 3);
        Assert.AreEqual(3, queue.GetHighestPriorityOf(3));
        queue.Push(3, 1);
        Assert.AreEqual(3, queue.GetHighestPriorityOf(3));
        queue.Pop();
        Assert.AreEqual(2, queue.GetHighestPriorityOf(3));
        queue.Pop();
        Assert.AreEqual(1, queue.GetHighestPriorityOf(3));
        Assert.AreEqual(0, queue.GetHighestPriorityOf(4));
        queue.Pop();
        queue.Pop();
        Assert.IsNull(queue.GetHighestPriorityOf(3));
        Assert.IsNull(queue.GetHighestPriorityOf(4));
    }

    [TestMethod]
    public void GetPrioritiesOf_IsCorrect()
    {
        var queue = IntUpdatableQueueBuilder();
        Assert.IsTrue(Array.Empty<int>().SequenceEqual(queue.GetPrioritiesOf(3)));
        queue.Push(2, 3);
        Assert.IsTrue(Array.Empty<int>().SequenceEqual(queue.GetPrioritiesOf(3)));
        queue.Push(3, 4);
        Assert.IsTrue(new int[] { 4 }.SequenceEqual(queue.GetPrioritiesOf(3)));
        queue.Push(3, 0);
        Assert.IsTrue(new int[] { 4, 0 }.SequenceEqual(queue.GetPrioritiesOf(3)));
        queue.Push(3, 5);
        Assert.IsTrue(new int[] { 5, 4, 0 }.SequenceEqual(queue.GetPrioritiesOf(3)));
        queue.Push(3, 2);
        Assert.IsTrue(new int[] { 5, 4, 2, 0 }.SequenceEqual(queue.GetPrioritiesOf(3)));
        queue.Pop();
        Assert.IsTrue(new int[] { 4, 2, 0 }.SequenceEqual(queue.GetPrioritiesOf(3)));
        queue.Pop();
        Assert.IsTrue(new int[] { 2, 0 }.SequenceEqual(queue.GetPrioritiesOf(3)));
        Assert.IsTrue(new int[] { 3 }.SequenceEqual(queue.GetPrioritiesOf(2)));
        queue.Pop();
        Assert.IsTrue(new int[] { 2, 0 }.SequenceEqual(queue.GetPrioritiesOf(3)));
        Assert.IsTrue(Array.Empty<int>().SequenceEqual(queue.GetPrioritiesOf(2)));
        queue.Pop();
        Assert.IsTrue(new int[] { 0 }.SequenceEqual(queue.GetPrioritiesOf(3)));
        queue.Pop();
        Assert.IsTrue(Array.Empty<int>().SequenceEqual(queue.GetPrioritiesOf(3)));
    }

    [TestMethod]
    public void UpdatePriority_RaisesExceptionOnItemNonPresent()
    {
        var queue = IntUpdatableQueueBuilder();
        Assert.ThrowsException<InvalidOperationException>(() => queue.UpdatePriority(3, 1));
        queue.Push(3, 2);
        Assert.ThrowsException<InvalidOperationException>(() => queue.UpdatePriority(2, 3));
    }

    [TestMethod]
    public void UpdatePriority_IsStableWithInt()
    {
        var queue = IntUpdatableQueueBuilder();
        queue.Push(3, 2);
        queue.Push(3, 4);
        queue.UpdatePriority(3, 1);
        Assert.AreEqual(new ItemAndPriority<int>(3, 4), queue.Peek());
        queue.UpdatePriority(3, 3);
        Assert.AreEqual(new ItemAndPriority<int>(3, 4), queue.Peek());
        queue.Pop();
        Assert.AreEqual(new ItemAndPriority<int>(3, 3), queue.Peek());
        queue.Pop();
        Assert.AreEqual(0, queue.Count);
    }

    [TestMethod]
    public void UpdatePriority_IsStableWithReferenceTypes()
    {
        var queue = RefTypeUpdatableQueueBuilder();
        var o1 = new RefType(1, new object());
        var o2 = new RefType(2, new object());
        var o3 = new RefType(3, new object());

        queue.Push(o1, 0);
        queue.Push(o2, 0);
        queue.Push(o3, 0);

        queue.UpdatePriority(o1, 1);
        queue.UpdatePriority(o2, 3);
        queue.UpdatePriority(o3, 2);

        Assert.IsTrue(ReferenceEquals(o2, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o3, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o1, queue.Pop().Item));

        queue.Push(o2, 5);
        queue.Push(o1, 2);
        queue.Push(o1, 2);
        queue.Push(o1, 4);
        queue.Push(o3, 3);

        Assert.IsTrue(ReferenceEquals(o2, queue.Peek().Item));
        queue.UpdatePriority(o1, 6);
        Assert.IsTrue(ReferenceEquals(o1, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o2, queue.Peek().Item));
        queue.UpdatePriority(o2, 1);
        Assert.IsTrue(ReferenceEquals(o1, queue.Peek().Item));
        Assert.AreEqual(4, queue.Peek().Priority);
    }

    [TestMethod]
    public void UpdatePriority_IsCorrect()
    {
        var queue = IntUpdatableQueueBuilder();
        queue.Push(3, 2);
        queue.UpdatePriority(3, 1);
        Assert.AreEqual(new ItemAndPriority<int>(3, 1), queue.Peek());
        queue.UpdatePriority(3, 4);
        Assert.AreEqual(new ItemAndPriority<int>(3, 4), queue.Peek());
        queue.Push(4, 2);
        Assert.AreEqual(new ItemAndPriority<int>(3, 4), queue.Peek());
        queue.Push(5, 5);
        Assert.AreEqual(new ItemAndPriority<int>(5, 5), queue.Peek());
        queue.UpdatePriority(5, 3);
        Assert.AreEqual(new ItemAndPriority<int>(3, 4), queue.Peek());
        queue.Pop();
        Assert.AreEqual(new ItemAndPriority<int>(5, 3), queue.Peek());
        queue.Pop();
        Assert.AreEqual(new ItemAndPriority<int>(4, 2), queue.Peek());
    }

    [TestMethod]
    public void Remove_IsCorrect()
    {
        var queue = IntUpdatableQueueBuilder();
        Assert.IsNull(queue.Remove(3));
        queue.Push(3, 2);
        Assert.AreEqual(new ItemAndPriority<int>(3, 2), queue.Remove(3));
        queue.Push(3, 2);
        queue.Push(3, 4);
        queue.Push(3, 3);
        Assert.AreEqual(new ItemAndPriority<int>(3, 4), queue.Remove(3));
        Assert.AreEqual(2, queue.Count);
        Assert.AreEqual(new ItemAndPriority<int>(3, 3), queue.Remove(3));
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(new ItemAndPriority<int>(3, 2), queue.Remove(3));
        Assert.IsNull(queue.Remove(3));

        queue.Push(4, 7);
        queue.Push(5, 7);
        Assert.AreEqual(new ItemAndPriority<int>(4, 7), queue.Remove(4));
        Assert.AreEqual(new ItemAndPriority<int>(5, 7), queue.Peek());
    }

    [TestMethod]
    public void Remove_WorksWithReferenceTypes()
    {
        var queue = RefTypeUpdatableQueueBuilder();
        var o1 = new object();
        var o2 = new object();
        queue.Push(new RefType(1, o1), 2);
        queue.Push(new RefType(2, o1), 4);
        queue.Push(new RefType(1, o2), 3);
        Assert.AreEqual(new ItemAndPriority<RefType>(new(2, o1), 4), queue.Remove(new RefType(2, o1)));
        Assert.AreEqual(new ItemAndPriority<RefType>(new(1, o1), 2), queue.Remove(new RefType(1, o1)));
        Assert.AreEqual(new ItemAndPriority<RefType>(new(1, o2), 3), queue.Remove(new RefType(1, o2)));
    }

    [TestMethod]
    public void Remove_WorksWithValueTypes()
    {
        var queue = ValTypeUpdatableQueueBuilder();
        queue.Push(new ValType(1, "a"), 2);
        queue.Push(new ValType(2, "a"), 4);
        queue.Push(new ValType(1, "b"), 3);
        Assert.AreEqual(new ItemAndPriority<ValType>(new(2, "a"), 4), queue.Remove(new ValType(2, "a")));
        Assert.AreEqual(new ItemAndPriority<ValType>(new(1, "a"), 2), queue.Remove(new ValType(1, "a")));
        Assert.AreEqual(new ItemAndPriority<ValType>(new(1, "b"), 3), queue.Remove(new ValType(1, "b")));

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

public abstract class PriorityQueueTests
{
    protected record RefType(int Field1, object Field2);
    protected record struct ValType(int Field1, string Field2);

    protected Func<IPriorityQueue<int>> IntQueueBuilder { get; }
    protected Func<IPriorityQueue<RefType>> RefTypeQueueBuilder { get; }
    protected Func<IPriorityQueue<ValType>> ValTypeQueueBuilder { get; }

    protected PriorityQueueTests(
        Func<IPriorityQueue<int>> intQueueBuilder,
        Func<IPriorityQueue<RefType>> refTypeQueueBuilder, 
        Func<IPriorityQueue<ValType>> valTypeQueueBuilder)
    {
        IntQueueBuilder = intQueueBuilder;
        RefTypeQueueBuilder = refTypeQueueBuilder;
        ValTypeQueueBuilder = valTypeQueueBuilder;
    }

    [TestMethod]
    public void Pop_RaisesExceptionOnEmptyQueue()
    {
        var queue = IntQueueBuilder();
        Assert.ThrowsException<InvalidOperationException>(() => queue.Pop());
        queue.Push(0, 2);
        queue.Pop();
        Assert.ThrowsException<InvalidOperationException>(() => queue.Pop());
    }

    [TestMethod]
    public void Pop_RespectsPriorities()
    {
        var queue = IntQueueBuilder();
        queue.Push(0, 2);
        Assert.AreEqual(new ItemAndPriority<int>(0, 2), queue.Pop());
        queue.Push(0, 2);
        queue.Push(1, 1);
        Assert.AreEqual(new ItemAndPriority<int>(0, 2), queue.Pop());
        Assert.AreEqual(new ItemAndPriority<int>(1, 1), queue.Pop());
        queue.Push(0, 2);
        queue.Push(1, 0);
        queue.Push(2, 1);
        Assert.AreEqual(new ItemAndPriority<int>(0, 2), queue.Pop());
        Assert.AreEqual(new ItemAndPriority<int>(2, 1), queue.Pop());
        Assert.AreEqual(new ItemAndPriority<int>(1, 0), queue.Pop());
    }

    [TestMethod]
    public void PushAndPop_WorkWithReferenceTypes()
    {
        var queue = RefTypeQueueBuilder();
        var o1 = new RefType(1, new object());
        queue.Push(o1, 0);
        Assert.IsTrue(ReferenceEquals(o1, queue.Pop().Item));

        var o2 = new RefType(2, new object());
        var o3 = new RefType(3, new object());
        queue.Push(o1, 0);
        queue.Push(o2, 2);
        queue.Push(o3, 1);
        Assert.IsTrue(ReferenceEquals(o2, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o3, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o1, queue.Pop().Item));
    }

    [TestMethod]
    public void PushAndPop_WorkWithValueTypes()
    {
        var queue = ValTypeQueueBuilder();
        var o1 = new ValType(1, "a");
        queue.Push(o1, 0);
        Assert.AreEqual(o1, queue.Pop().Item);

        var o2 = new ValType(2, "a");
        var o3 = new ValType(3, "b");
        queue.Push(o1, 0);
        queue.Push(o2, 2);
        queue.Push(o3, 1);
        Assert.AreEqual(o2, queue.Pop().Item);
        Assert.AreEqual(o3, queue.Pop().Item);
        Assert.AreEqual(o1, queue.Pop().Item);
    }

    [TestMethod]
    public void PushAndPop_AreStable()
    {
        var queue = RefTypeQueueBuilder();
        var o1 = new RefType(1, new object());
        var o2 = new RefType(2, new object());
        var o3 = new RefType(3, new object());

        queue.Push(o1, 0);
        queue.Push(o2, 0);
        queue.Push(o3, 0);
        Assert.IsTrue(ReferenceEquals(o1, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o2, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o3, queue.Pop().Item));

        queue.Push(o1, 0);
        queue.Push(o2, 1);
        queue.Push(o3, 0);
        queue.Push(o2, 1);
        queue.Push(o1, 1);
        queue.Push(o2, 0);
        queue.Push(o3, 1);
        queue.Push(o3, 1);

        Assert.IsTrue(ReferenceEquals(o2, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o2, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o1, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o3, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o3, queue.Pop().Item));

        Assert.IsTrue(ReferenceEquals(o1, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o3, queue.Pop().Item));
        Assert.IsTrue(ReferenceEquals(o2, queue.Pop().Item));
    }

    [TestMethod]
    public void Count_IsCorrect()
    {
        var queue = IntQueueBuilder();
        Assert.AreEqual(0, queue.Count);
        queue.Push(0, 2);
        Assert.AreEqual(1, queue.Count);
        queue.Push(1, 1);
        Assert.AreEqual(2, queue.Count);
        queue.Push(3, 1);
        queue.Push(2, 1);
        Assert.AreEqual(4, queue.Count);
        queue.Pop();
        Assert.AreEqual(3, queue.Count);
        queue.Pop();
        queue.Pop();
        Assert.AreEqual(1, queue.Count);
    }

    [TestMethod]
    public void Count_IsCorrectWithDuplicates()
    {
        var queue = IntQueueBuilder();
        queue.Push(0, 2);
        queue.Push(0, 2);
        queue.Push(0, 1);
        Assert.AreEqual(3, queue.Count);
        queue.Push(1, 1);
        queue.Push(1, 2);
        Assert.AreEqual(5, queue.Count);
        queue.Pop();
        queue.Pop();
        queue.Pop();
        Assert.AreEqual(2, queue.Count);
    }

    [TestMethod]
    public void Peek_RaisesExceptionOnEmptyQueue()
    {
        var queue = IntQueueBuilder();
        Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());
        queue.Push(3, 0);
        queue.Pop();
        Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());
    }

    [TestMethod]
    public void Peek_IsCorrect()
    {
        var queue = IntQueueBuilder();
        queue.Push(3, 2);
        Assert.AreEqual(new ItemAndPriority<int>(3, 2), queue.Peek());
        queue.Push(4, 3);
        Assert.AreEqual(new ItemAndPriority<int>(4, 3), queue.Peek());
        queue.Push(5, 1);
        Assert.AreEqual(new ItemAndPriority<int>(4, 3), queue.Peek());
        queue.Pop();
        Assert.AreEqual(new ItemAndPriority<int>(3, 2), queue.Peek());
        queue.Pop();
        Assert.AreEqual(new ItemAndPriority<int>(5, 1), queue.Peek());
    }

    [TestMethod]
    public void Peek_DoesntChangeTheContentOfTheQueue()
    {
        var queue = IntQueueBuilder();
        queue.Push(3, 2);
        Assert.AreEqual(new ItemAndPriority<int>(3, 2), queue.Peek());
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(new ItemAndPriority<int>(3, 2), queue.Peek());
        Assert.AreEqual(1, queue.Count);
    }

    [TestMethod]
    public void GetEnumerator_RetrievesAllItemsAccordingToPriority()
    {
        var queue = IntQueueBuilder();
        queue.Push(3, 2);
        queue.Push(2, 3);
        queue.Push(10, 2);
        queue.Push(10, 5);
        queue.Push(9, 4);
        queue.Push(9, 6);
        queue.Push(1, 4);

        var expectedSequence = new[] { 9, 10, 9, 1, 2, 3, 10 };
        Assert.IsTrue(expectedSequence.SequenceEqual(queue.ToList()));

        var nonGenericEnumeration = new List<int>();
        var nonGenericEnumerator = ((System.Collections.IEnumerable)queue).GetEnumerator()!;
        while (nonGenericEnumerator.MoveNext())
            nonGenericEnumeration.Add((int)nonGenericEnumerator.Current);

        Assert.IsTrue(expectedSequence.SequenceEqual(nonGenericEnumeration));
    }

    [TestMethod]
    public void GetEnumerator_DoesntAlterQueueWhenIterated()
    {
        var queue = IntQueueBuilder();
        queue.Push(3, 2);
        queue.Push(2, 3);
        queue.Push(10, 2);
        queue.Push(10, 5);
        queue.Push(9, 4);
        queue.Push(9, 6);
        queue.Push(1, 4);

        Assert.AreEqual(7, queue.Count);
        var items = queue.ToList();

        Assert.AreEqual(7, items.Count);
        Assert.AreEqual(7, queue.Count);
    }
}

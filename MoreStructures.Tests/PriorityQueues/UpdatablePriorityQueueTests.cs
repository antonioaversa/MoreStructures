using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

public abstract class UpdatablePriorityQueueTests
{
    protected Func<IUpdatablePriorityQueue<int>> IntUpdatableQueueBuilder { get; }
    protected Func<IUpdatablePriorityQueue<RefType>> RefTypeUpdatableQueueBuilder { get; }
    protected Func<IUpdatablePriorityQueue<ValType>> ValTypeUpdatableQueueBuilder { get; }

    protected UpdatablePriorityQueueTests(
        Func<IUpdatablePriorityQueue<int>> intUpdatableQueueBuilder, 
        Func<IUpdatablePriorityQueue<RefType>> refTypeUpdatableQueueBuilder, 
        Func<IUpdatablePriorityQueue<ValType>> valTypeUpdatableQueueBuilder)
    {
        IntUpdatableQueueBuilder = intUpdatableQueueBuilder;
        RefTypeUpdatableQueueBuilder = refTypeUpdatableQueueBuilder;
        ValTypeUpdatableQueueBuilder = valTypeUpdatableQueueBuilder;
    }

    [TestMethod]
    public void GetPrioritiesOf_FirstIsCorrect()
    {
        var queue = IntUpdatableQueueBuilder();
        Assert.AreEqual(-1, queue.GetPrioritiesOf(3).FirstOrDefault(-1));
        queue.Push(3, 2);
        Assert.AreEqual(2, queue.GetPrioritiesOf(3).FirstOrDefault(-1));
        queue.Push(4, 0);
        Assert.AreEqual(2, queue.GetPrioritiesOf(3).FirstOrDefault(-1));
        Assert.AreEqual(0, queue.GetPrioritiesOf(4).FirstOrDefault(-1));
        queue.Push(3, 3);
        Assert.AreEqual(3, queue.GetPrioritiesOf(3).FirstOrDefault(-1));
        queue.Push(3, 1);
        Assert.AreEqual(3, queue.GetPrioritiesOf(3).FirstOrDefault(-1));
        queue.Pop();
        Assert.AreEqual(2, queue.GetPrioritiesOf(3).FirstOrDefault(-1));
        queue.Pop();
        Assert.AreEqual(1, queue.GetPrioritiesOf(3).FirstOrDefault(-1));
        Assert.AreEqual(0, queue.GetPrioritiesOf(4).FirstOrDefault(-1));
        queue.Pop();
        queue.Pop();
        Assert.AreEqual(-1, queue.GetPrioritiesOf(3).FirstOrDefault(-1));
        Assert.AreEqual(-1, queue.GetPrioritiesOf(4).FirstOrDefault(-1));
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
        Assert.AreEqual(new PrioritizedItem<int>(3, 2, 0), queue.Peek());
        queue.UpdatePriority(3, 3);
        Assert.AreEqual(new PrioritizedItem<int>(3, 3, 3), queue.Peek());
        queue.Pop();
        Assert.AreEqual(new PrioritizedItem<int>(3, 1, 2), queue.Peek());
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
        Assert.IsTrue(ReferenceEquals(o3, queue.Peek().Item));
        Assert.AreEqual(3, queue.Peek().Priority);
        queue.UpdatePriority(o3, 1);
        Assert.IsTrue(ReferenceEquals(o1, queue.Peek().Item));
        Assert.AreEqual(2, queue.Peek().Priority);
    }

    [TestMethod]
    public void UpdatePriority_IsCorrect()
    {
        var queue = IntUpdatableQueueBuilder();
        queue.Push(3, 2);
        queue.UpdatePriority(3, 1);
        Assert.AreEqual(new PrioritizedItem<int>(3, 1, 1), queue.Peek());
        queue.UpdatePriority(3, 4);
        Assert.AreEqual(new PrioritizedItem<int>(3, 4, 2), queue.Peek());
        queue.Push(4, 2);
        Assert.AreEqual(new PrioritizedItem<int>(3, 4, 2), queue.Peek());
        queue.Push(5, 5);
        Assert.AreEqual(new PrioritizedItem<int>(5, 5, 4), queue.Peek());
        queue.UpdatePriority(5, 3);
        Assert.AreEqual(new PrioritizedItem<int>(3, 4, 2), queue.Peek());
        queue.Pop();
        Assert.AreEqual(new PrioritizedItem<int>(5, 3, 5), queue.Peek());
        queue.Pop();
        Assert.AreEqual(new PrioritizedItem<int>(4, 2, 3), queue.Peek());
    }

    [TestMethod]
    public void UpdatePriority_AllPermutations()
    {
        var numbers = Enumerable.Range(0, 5).ToArray();
        var numberOfValues = numbers.Length;
        foreach (var permutation in TestUtilities.GeneratePermutations(numbers))
        {
            var queue = IntUpdatableQueueBuilder();
            foreach (var value in numbers)
                queue.Push(value, (numberOfValues + value + 7) % numberOfValues);

            foreach (var (value, newPriority) in numbers.Zip(permutation))
                queue.UpdatePriority(value, newPriority);

            for (var i = 0; i < numberOfValues; i++)
                Assert.AreEqual(permutation.IndexOf(numberOfValues - 1 - i), queue.Pop().Item);
        }
    }

    [DataRow(
        new[] 
        { 
            43, 36, 46, 37, 34, 23, 43, 23, 48, 41, 31, 49, 40, 38, 4, 30, 14, 22, 47, 15, 37, 38, 46, 0, 26, 
            30, 32, 30, 21, 40, 7, 23, 31, 27, 26, 10, 32, 30, 0, 11, 48, 41, 8, 13, 10, 44, 4, 49, 18, 3, 
        },
        new[]
        {
            48, 19, 8, 13, 1, 9, 29, 22, 34, 34, 14, 44, 32, 27, 35, 32, 39, 43, 47, 26, 12, 13, 10, 18, 18, 22, 
            0, 31, 13, 0, 30, 39, 46, 25, 29, 32, 26, 12, 44, 46, 0, 13, 19, 46, 28, 21, 1, 32, 3, 49,
        },
        new[] 
        {
            23, 28, 1, 20, 39, 24, 34, 16, 40, 24, 16, 28, 42, 4, 38, 13, 6, 43, 43, 10, 22, 20, 27, 4, 2, 27, 22, 
            18, 14, 4, 31, 23, 0, 6, 30, 23, 4, 24, 3, 24, 38, 14, 46, 28, 8, 12, 30, 33, 16, 23,
        })]
    [DataTestMethod]
    public void UpdatePriority_LongChainOfUpdates(int[] values, int[] priorities, int[] updatePriorities)
    {
        var queue = IntUpdatableQueueBuilder();
        var numberOfValues = values.Length;
        for (var i = 0; i < numberOfValues; i++)
        {
            queue.Push(values[i], priorities[i]);
            if (i % 3 == 0) 
                queue.Pop();
            queue.Push(values[numberOfValues - 1 - i], priorities[(numberOfValues - i + 7) % numberOfValues]);
        }

        for (var i = 0; i < values.Length; i++)
            queue.UpdatePriority(values[i], updatePriorities[i]);

        var sortedPriorities = new List<int>(numberOfValues);
        while (queue.Count > 0)
            sortedPriorities.Add(queue.Pop().Priority);
        Assert.IsTrue(sortedPriorities.SequenceEqual(sortedPriorities.OrderByDescending(x => x)));
    }

    [TestMethod]
    public void UpdatePriority_StalePushTimestamps()
    {
        var queue = IntUpdatableQueueBuilder();
        queue.Push(1, 3);
        queue.UpdatePriority(1, 2);
        queue.UpdatePriority(1, 1);
        queue.UpdatePriority(1, 0);
        queue.Remove(1);
        Assert.ThrowsException<InvalidOperationException>(() => queue.UpdatePriority(1, -1));
    }

    [TestMethod]
    public void UpdatePriority_WorksWhenNewPriorityIsTheSame()
    {
        var queue = IntUpdatableQueueBuilder();
        queue.Push(1, 3);
        queue.UpdatePriority(1, 3);
        Assert.AreEqual(3, queue.Peek().Priority);
    }

    [TestMethod]
    public void Remove_IsCorrect()
    {
        var queue = IntUpdatableQueueBuilder();
        Assert.IsNull(queue.Remove(3));
        queue.Push(3, 2);
        Assert.AreEqual(new PrioritizedItem<int>(3, 2, 0), queue.Remove(3));
        queue.Push(3, 2);
        queue.Push(3, 4);
        queue.Push(3, 3);
        Assert.AreEqual(new PrioritizedItem<int>(3, 4, 2), queue.Remove(3));
        Assert.AreEqual(2, queue.Count);
        Assert.AreEqual(new PrioritizedItem<int>(3, 3, 3), queue.Remove(3));
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(new PrioritizedItem<int>(3, 2, 1), queue.Remove(3));
        Assert.IsNull(queue.Remove(3));

        queue.Push(4, 7);
        queue.Push(5, 7);
        Assert.AreEqual(new PrioritizedItem<int>(4, 7, 4), queue.Remove(4));
        Assert.AreEqual(new PrioritizedItem<int>(5, 7, 5), queue.Peek());
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
        Assert.AreEqual(new PrioritizedItem<RefType>(new(2, o1), 4, 1), queue.Remove(new RefType(2, o1)));
        Assert.AreEqual(new PrioritizedItem<RefType>(new(1, o1), 2, 0), queue.Remove(new RefType(1, o1)));
        Assert.AreEqual(new PrioritizedItem<RefType>(new(1, o2), 3, 2), queue.Remove(new RefType(1, o2)));
    }

    [TestMethod]
    public void Remove_WorksWithValueTypes()
    {
        var queue = ValTypeUpdatableQueueBuilder();
        queue.Push(new ValType(1, "a"), 2);
        queue.Push(new ValType(2, "a"), 4);
        queue.Push(new ValType(1, "b"), 3);
        Assert.AreEqual(new PrioritizedItem<ValType>(new(2, "a"), 4, 1), queue.Remove(new ValType(2, "a")));
        Assert.AreEqual(new PrioritizedItem<ValType>(new(1, "a"), 2, 0), queue.Remove(new ValType(1, "a")));
        Assert.AreEqual(new PrioritizedItem<ValType>(new(1, "b"), 3, 2), queue.Remove(new ValType(1, "b")));

    }
}

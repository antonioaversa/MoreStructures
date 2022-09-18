using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Queues;

namespace MoreStructures.Tests.Queues;

public abstract class QueueTests
{
    protected abstract IQueue<T> Build<T>();

    [TestMethod]
    public void Peek_RaisesExceptionOnEmptyQueue()
    {
        var queue = Build<int>();
        Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());
        queue.Enqueue(2);
        queue.Dequeue();
        Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());
        Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());
    }

    [TestMethod]
    public void Dequeue_RaisesExceptionOnEmptyQueue()
    {
        var queue = Build<int>();
        Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
        queue.Enqueue(2);
        queue.Dequeue();
        Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
        Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
    }

    [TestMethod]
    public void Peek_IsCorrect()
    {
        var queue = Build<int>();
        queue.Enqueue(2);
        Assert.AreEqual(2, queue.Peek());
        Assert.AreEqual(2, queue.Peek());
        queue.Enqueue(1);
        Assert.AreEqual(2, queue.Peek());
        queue.Enqueue(3);
        Assert.AreEqual(2, queue.Peek());
        queue.Dequeue();
        Assert.AreEqual(1, queue.Peek());
        queue.Dequeue();
        Assert.AreEqual(3, queue.Peek());
    }

    [TestMethod]
    public void Count_IsCorrect()
    {
        var queue = Build<string>();
        Assert.AreEqual(0, queue.Count);
        for (var i = 1; i <= 10; i++)
        {
            queue.Enqueue(i.ToString());
            Assert.AreEqual(i, queue.Count);
        }

        for (var i = queue.Count; i > 0; i--)
        {
            queue.Dequeue();
            Assert.AreEqual(i - 1, queue.Count);
        }
    }

    [TestMethod]
    public void EnqueueAndDequeue_AreCorrect()
    {
        var queue = Build<double>();
        var numberOfValues = 100;
        var j = 0;
        foreach (var i in Enumerable.Range(0, numberOfValues))
        {
            queue.Enqueue(i);
            if (i % 2 == 0)
                Assert.AreEqual(j++, queue.Dequeue());
            else if (i % 3 == 0)
                Assert.AreEqual(j++, queue.Dequeue());
            else if (i % 5 == 0)
                Assert.AreEqual(j++, queue.Dequeue());
        }

        var numberOfRemainingItems = 50 - 17 - 7;
        Assert.AreEqual(numberOfRemainingItems, queue.Count);

        var remainingItems = new List<double> { };
        while (queue.Count > 0) 
            remainingItems.Add(queue.Dequeue());
        Assert.IsTrue(remainingItems.All(i => 
            i >= numberOfValues - numberOfRemainingItems && 
            i < numberOfValues));
    }
}

using MoreStructures.PriorityQueues;

namespace MoreStructures.Tests.PriorityQueues;

public abstract class MergeablePriorityQueueTests<TPriorityQueue>
    where TPriorityQueue : IMergeablePriorityQueue<int, TPriorityQueue>
{
    protected Func<TPriorityQueue> Builder { get; }

    protected MergeablePriorityQueueTests(Func<TPriorityQueue> builder)
    {
        Builder = builder;
    }

    [TestMethod]
    public void Merge_WithEmptySource()
    {
        var source = Builder();
        var target = Builder();
        target.Push(1, 2);
        target.Push(3, 3);
        var countBeforeMerge = target.Count;
        source.Merge(target);
        var countAfterMerge = source.Count;
        Assert.AreEqual(countBeforeMerge, countAfterMerge);
    }

    [TestMethod]
    public void Merge_WithEmptyTarget()
    {
        var source = Builder();
        var target = Builder();
        source.Push(1, 2);
        source.Push(3, 3);
        var countBeforeMerge = source.Count;
        source.Merge(target);
        var countAfterMerge = source.Count;
        Assert.AreEqual(countBeforeMerge, countAfterMerge);
    }

    [TestMethod]
    public void Merge_ContainsElementsFromBothQueues()
    {
        var sourceList = Enumerable.Range(0, 5).Select(i => 2 * i).ToArray();
        var targetList = Enumerable.Range(0, 5).Select(i => 2 * i + 1).ToArray();
        foreach (var sourcePermutation in TestUtilities.GeneratePermutations(sourceList))
        {
            foreach (var targetPermutation in TestUtilities.GeneratePermutations(targetList))
            {
                var source = Builder();
                foreach (var sourceItem in sourcePermutation)
                    source.Push(sourceItem, targetPermutation[0] + sourceItem);

                var target = Builder();
                foreach (var targetItem in targetPermutation)
                    target.Push(targetItem, sourcePermutation[0] + targetItem);

                var itemsBeforeMerge = source.Concat(target).OrderByDescending(i => i).ToList();
                var countBeforeMerge = source.Count + target.Count;
                source.Merge(target);
                var countAfterMerge = source.Count;
                var itemsAfterMerge = source.OrderByDescending(i => i).ToList();
                Assert.AreEqual(countBeforeMerge, countAfterMerge);
                Assert.IsTrue(itemsAfterMerge.SequenceEqual(itemsBeforeMerge));
            }
        }
    }

    [TestMethod]
    public void Merge_WorksWithManyItems()
    {
        var numberOfItems = 2048;
        var source = Builder();
        for (var i = 0; i < numberOfItems; i += 2)
            source.Push(i, i % 7);
        var target = Builder();
        for (var i = 1; i < numberOfItems; i += 2)
            target.Push(i, i % 5);
        source.Merge(target);
        Assert.AreEqual(numberOfItems, source.Count);
        Assert.IsTrue(Enumerable.Range(0, numberOfItems).SequenceEqual(source.OrderBy(i => i)));
    }

    [TestMethod]
    public void Merge_WorksWithDuplicates()
    {
        var source = Builder();
        source.Push(1, 2);
        source.Push(2, 4);
        source.Push(3, 3);
        source.Push(4, 1);

        var target = Builder();
        target.Push(1, 1);
        target.Push(2, 6);
        target.Push(3, 3);
        target.Push(5, 0);

        var countBeforeMerge = source.Count + target.Count;
        source.Merge(target);
        var countAfterMerge = source.Count;
        Assert.AreEqual(countBeforeMerge, countAfterMerge);
        Assert.AreEqual(2, source.Peek().Item);
        Assert.AreEqual(6, source.Peek().Priority);
        source.Pop();
        Assert.AreEqual(2, source.Peek().Item);
        Assert.AreEqual(4, source.Peek().Priority);
        source.Pop();
        Assert.AreEqual(3, source.Peek().Item);
        Assert.AreEqual(3, source.Peek().Priority);
        source.Pop();
        Assert.AreEqual(3, source.Peek().Item);
        Assert.AreEqual(3, source.Peek().Priority);
    }

    [TestMethod]
    public void Merge_CanBeRepeatedWithMultipleTargets()
    {
        var source = Builder();
        source.Push(1, 2);
        source.Push(2, 4);
        source.Push(3, 3);
        source.Push(4, 1);

        var target1 = Builder();
        target1.Push(1, 1);
        target1.Push(2, 6);
        target1.Push(3, 3);
        target1.Push(5, 0);

        var target2 = Builder();
        target2.Push(2, 1);
        target2.Push(4, 6);
        target2.Push(5, 3);
        target2.Push(1, 0);

        var countBeforeMerge = source.Count + target1.Count + target2.Count;
        var itemsBeforeMerge = source.Concat(target1).Concat(target2).OrderByDescending(i => i).ToList();
        source.Merge(target1);
        source.Merge(target2);
        var countAfterMerge = source.Count;
        var itemsAfterMerge = source.OrderByDescending(i => i).ToList();
        Assert.AreEqual(countBeforeMerge, countAfterMerge);
        Assert.IsTrue(itemsAfterMerge.SequenceEqual(itemsBeforeMerge));
    }

    [TestMethod]
    public void Merge_RemoveAllItemsFromTheTarget()
    {
        var source = Builder();
        source.Push(1, 2);
        source.Push(2, -4);

        var target = Builder();
        target.Push(1, 1);
        target.Push(5, 2);
        target.Push(3, -1);

        source.Merge(target);
        Assert.AreEqual(0, target.Count);
    }

    [TestMethod]
    public void Merge_TargetIsUsableAfterMerge()
    {
        var source = Builder();
        source.Push(1, 2);
        source.Push(2, -4);

        var target = Builder();
        target.Push(1, 1);
        target.Push(5, 2);
        target.Push(3, -1);

        source.Merge(target);

        Assert.ThrowsException<InvalidOperationException>(() => target.Peek());
        Assert.ThrowsException<InvalidOperationException>(() => target.Pop());
        target.Push(4, 2);
        Assert.AreEqual(1, target.Count);
        Assert.AreEqual(4, target.Peek().Item);
        Assert.AreEqual(2, target.Peek().Priority);
        target.Push(2, 3);
        Assert.AreEqual(2, target.Count);
        Assert.AreEqual(2, target.Peek().Item);
        Assert.AreEqual(3, target.Peek().Priority);
        target.Push(4, 4);
        Assert.AreEqual(3, target.Count);
        Assert.AreEqual(4, target.Peek().Item);
        Assert.AreEqual(4, target.Peek().Priority);
        target.Pop();
        Assert.AreEqual(2, target.Count);
        Assert.AreEqual(2, target.Peek().Item);
        Assert.AreEqual(3, target.Peek().Priority);
        target.Pop();
        target.Pop();
        Assert.AreEqual(0, target.Count);
    }

    [TestMethod]
    public void Merge_TargetChangesDontInterfereWithSource()
    {
        var source = Builder();
        source.Push(1, 2);
        source.Push(2, -4);

        var target = Builder();
        target.Push(1, 1);
        target.Push(5, 2);
        target.Push(3, -1);

        source.Merge(target);

        var sourceCountBeforeTargetPush = source.Count;
        target.Push(3, 4);
        var sourceCountAfterTargetPush = source.Count;
        Assert.AreEqual(sourceCountBeforeTargetPush, sourceCountAfterTargetPush);
        target.Pop();
        var sourceCountAfterTargetPop = source.Count;
        Assert.AreEqual(sourceCountBeforeTargetPush, sourceCountAfterTargetPop);
    }

    [TestMethod]
    public void Clear_WipesAllItemsOut()
    {
        var queue = Builder();
        queue.Push(1, 2);
        queue.Push(2, -4);
        queue.Clear();
        Assert.AreEqual(0, queue.Count);

        queue.Clear();
        Assert.AreEqual(0, queue.Count);
    }

    [TestMethod]
    public void Clear_QueueKeepsWorkingAfter()
    {
        var queue = Builder();
        queue.Push(1, 2);
        queue.Push(2, -4);
        queue.Clear();

        queue.Push(1, 1);
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(1, queue.Peek().Item);
        Assert.AreEqual(1, queue.Peek().Priority);
        queue.Push(2, 2);
        Assert.AreEqual(2, queue.Count);
        Assert.AreEqual(2, queue.Peek().Item);
        Assert.AreEqual(2, queue.Peek().Priority);
        var poppedItem1 = queue.Pop();
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(2, poppedItem1.Item);
        Assert.AreEqual(2, poppedItem1.Priority);
        Assert.AreEqual(1, queue.Peek().Item);
        Assert.AreEqual(1, queue.Peek().Priority);
        var poppedItem2 = queue.Pop();
        Assert.AreEqual(0, queue.Count);
        Assert.AreEqual(1, poppedItem2.Item);
        Assert.AreEqual(1, poppedItem2.Priority);
    }
}

